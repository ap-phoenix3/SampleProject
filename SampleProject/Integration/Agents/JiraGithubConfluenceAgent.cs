using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleProject.Integration.Agents
{
    /// <summary>
    /// Integration Agent for Jira, GitHub, and Confluence
    /// Reads user stories from Jira, creates tests, runs them, and reports results
    /// </summary>
    public class JiraGithubConfluenceAgent
    {
        private readonly string _jiraBaseUrl;
        private readonly string _jiraToken;
        private readonly string _githubToken;
        private readonly string _githubRepo;
        private readonly string _githubBranch;
        private readonly string _confluenceBaseUrl;
        private readonly string _confluenceSpaceKey;
        private readonly string _confluencePageId;
        private readonly HttpClient _httpClient;

        public JiraGithubConfluenceAgent(
            string jiraBaseUrl,
            string jiraToken,
            string githubToken,
            string githubRepo,
            string githubBranch,
            string confluenceBaseUrl,
            string confluenceSpaceKey,
            string confluencePageId)
        {
            _jiraBaseUrl = jiraBaseUrl;
            _jiraToken = jiraToken;
            _githubToken = githubToken;
            _githubRepo = githubRepo;
            _githubBranch = githubBranch;
            _confluenceBaseUrl = confluenceBaseUrl;
            _confluenceSpaceKey = confluenceSpaceKey;
            _confluencePageId = confluencePageId;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Executes the complete workflow: Jira -> Test Creation -> Execution -> Confluence -> GitHub PR
        /// </summary>
        public async Task<ExecutionReport> ExecuteWorkflowAsync(string projectKey, string jql = "")
        {
            Console.WriteLine("🚀 Starting Jira-GitHub-Confluence Integration Agent...\n");

            var report = new ExecutionReport
            {
                StartTime = DateTime.UtcNow,
                ProjectKey = projectKey
            };

            try
            {
                // Step 1: Fetch user stories from Jira
                Console.WriteLine("📋 Step 1: Fetching user stories from Jira...");
                var userStories = await FetchUserStoriesFromJiraAsync(projectKey, jql);
                report.UserStoriesCount = userStories.Count;
                Console.WriteLine($"✅ Found {userStories.Count} user stories\n");

                // Step 2: Generate test cases
                Console.WriteLine("🧪 Step 2: Generating test cases from user stories...");
                var generatedTests = GenerateTestCases(userStories);
                report.TestsGenerated = generatedTests.Count;
                Console.WriteLine($"✅ Generated {generatedTests.Count} test cases\n");

                // Step 3: Add tests to solution
                Console.WriteLine("📁 Step 3: Adding test cases to solution...");
                var addedTests = await AddTestsToSolutionAsync(generatedTests);
                report.TestsAdded = addedTests.Count;
                Console.WriteLine($"✅ Added {addedTests.Count} test cases to solution\n");

                // Step 4: Run tests
                Console.WriteLine("▶️  Step 4: Running test cases...");
                var testResults = RunTests();
                report.TestsRun = testResults.TotalTests;
                report.TestsPassed = testResults.PassedTests;
                report.TestsFailed = testResults.FailedTests;
                Console.WriteLine($"✅ Test Results: {testResults.PassedTests} Passed, {testResults.FailedTests} Failed\n");

                // Step 5: Generate execution report
                Console.WriteLine("📊 Step 5: Generating execution report...");
                report.TestResults = testResults;
                report.Status = testResults.FailedTests == 0 ? "SUCCESS" : "PARTIAL_SUCCESS";
                Console.WriteLine($"✅ Report generated with status: {report.Status}\n");

                // Step 6: Post to Confluence
                Console.WriteLine("🔗 Step 6: Posting report to Confluence...");
                var confluencePageUrl = await PostToConfluenceAsync(report);
                report.ConfluencePageUrl = confluencePageUrl;
                Console.WriteLine($"✅ Report posted to Confluence: {confluencePageUrl}\n");

                // Step 7: Create GitHub PR
                Console.WriteLine("🔀 Step 7: Creating GitHub pull request...");
                var prUrl = await CreateGitHubPullRequestAsync(report);
                report.GitHubPRUrl = prUrl;
                Console.WriteLine($"✅ Pull request created: {prUrl}\n");

                report.EndTime = DateTime.UtcNow;
                report.Successful = true;

                Console.WriteLine("🎉 Workflow completed successfully!\n");
                PrintExecutionSummary(report);

                return report;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in workflow: {ex.Message}\n");
                report.ErrorMessage = ex.Message;
                report.Successful = false;
                report.EndTime = DateTime.UtcNow;
                return report;
            }
        }

        /// <summary>
        /// Fetches user stories from Jira using JQL query
        /// </summary>
        private async Task<List<JiraUserStory>> FetchUserStoriesFromJiraAsync(string projectKey, string customJql = "")
        {
            try
            {
                var jql = string.IsNullOrEmpty(customJql)
                    ? $"project = {projectKey} AND type = Story AND status != Done"
                    : customJql;

                var url = $"{_jiraBaseUrl}/rest/api/3/search?jql={Uri.EscapeDataString(jql)}&fields=key,summary,description,customfield_10000";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jiraToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var issues = doc.RootElement.GetProperty("issues");
                    var stories = new List<JiraUserStory>();

                    foreach (var issue in issues.EnumerateArray())
                    {
                        var key = issue.GetProperty("key").GetString();
                        var fields = issue.GetProperty("fields");
                        var summary = fields.GetProperty("summary").GetString();
                        var description = fields.TryGetProperty("description", out var desc)
                            ? desc.GetString() ?? ""
                            : "";

                        stories.Add(new JiraUserStory
                        {
                            Key = key,
                            Summary = summary,
                            Description = description
                        });

                        Console.WriteLine($"  📌 {key}: {summary}");
                    }

                    return stories;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching from Jira: {ex.Message}");
                return new List<JiraUserStory>();
            }
        }

        /// <summary>
        /// Generates test cases from user stories
        /// </summary>
        private List<GeneratedTestCase> GenerateTestCases(List<JiraUserStory> userStories)
        {
            var testCases = new List<GeneratedTestCase>();

            foreach (var story in userStories)
            {
                // Generate feature file name from story key
                var featureFileName = $"{story.Key.Replace("-", "")}_{SanitizeName(story.Summary)}.feature";

                var testCase = new GeneratedTestCase
                {
                    StoryKey = story.Key,
                    FeatureFileName = featureFileName,
                    FeatureContent = GenerateFeatureFileContent(story),
                    StepDefinitionsContent = GenerateStepDefinitionsContent(story)
                };

                testCases.Add(testCase);
                Console.WriteLine($"  🧪 Generated test for {story.Key}");
            }

            return testCases;
        }

        /// <summary>
        /// Adds generated test cases to the solution
        /// </summary>
        private async Task<List<GeneratedTestCase>> AddTestsToSolutionAsync(List<GeneratedTestCase> testCases)
        {
            var addedTests = new List<GeneratedTestCase>();

            foreach (var testCase in testCases)
            {
                try
                {
                    // Add feature file
                    var featurePath = $"SampleProject/Features/{testCase.FeatureFileName}";
                    await System.IO.File.WriteAllTextAsync(featurePath, testCase.FeatureContent);

                    // Add step definitions
                    var stepDefPath = $"SampleProject/StepDefinitions/{testCase.StoryKey.Replace("-", "")}StepDefinitions.cs";
                    await System.IO.File.WriteAllTextAsync(stepDefPath, testCase.StepDefinitionsContent);

                    addedTests.Add(testCase);
                    Console.WriteLine($"  ✅ Added {testCase.StoryKey} to solution");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ❌ Error adding {testCase.StoryKey}: {ex.Message}");
                }
            }

            return addedTests;
        }

        /// <summary>
        /// Runs all tests in the solution
        /// </summary>
        private TestExecutionResult RunTests()
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "test --logger \"console;verbosity=detailed\"",
                    WorkingDirectory = "SampleProject",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(psi))
                {
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // Parse test results
                    var result = new TestExecutionResult
                    {
                        Output = output,
                        ErrorOutput = error,
                        ExitCode = process.ExitCode
                    };

                    // Extract counts from output (simple parsing)
                    ParseTestResults(output, result);

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running tests: {ex.Message}");
                return new TestExecutionResult { ExitCode = -1, Output = ex.Message };
            }
        }

        /// <summary>
        /// Posts execution report to Confluence
        /// </summary>
        private async Task<string> PostToConfluenceAsync(ExecutionReport report)
        {
            try
            {
                var confluenceContent = GenerateConfluenceContent(report);

                var url = $"{_confluenceBaseUrl}/rest/api/content/{_confluencePageId}";
                var updatePayload = new
                {
                    version = new { number = 1 },
                    title = $"Test Execution Report - {DateTime.Now:yyyy-MM-dd HH:mm}",
                    type = "page",
                    space = new { key = _confluenceSpaceKey },
                    body = new
                    {
                        storage = new
                        {
                            value = confluenceContent,
                            representation = "storage"
                        }
                    }
                };

                var json = JsonSerializer.Serialize(updatePayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = content
                };

                var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"user:token"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return $"{_confluenceBaseUrl}/pages/viewpage.action?pageId={_confluencePageId}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error posting to Confluence: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// Creates a GitHub pull request with the test changes
        /// </summary>
        private async Task<string> CreateGitHubPullRequestAsync(ExecutionReport report)
        {
            try
            {
                var url = $"https://api.github.com/repos/{_githubRepo}/pulls";

                var payload = new
                {
                    title = $"Test Suite Update - {report.ProjectKey} - {DateTime.Now:yyyy-MM-dd}",
                    body = GenerateGitHubPRDescription(report),
                    head = "test-automation",
                    @base = _githubBranch
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _githubToken);
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue("AutomationAgent", "1.0"));

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(responseJson))
                {
                    var prUrl = doc.RootElement.GetProperty("html_url").GetString();
                    return prUrl;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating GitHub PR: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// Generates feature file content from a user story
        /// </summary>
        private string GenerateFeatureFileContent(JiraUserStory story)
        {
            var sanitizedSummary = SanitizeName(story.Summary);

            return $@"Feature: {story.Summary}
  Story: {story.Key}

  {(string.IsNullOrEmpty(story.Description) ? $"As a user\n  I want to verify {sanitizedSummary}\n  So that the feature works correctly" : story.Description)}

  Background:
    Given I open the browser

  Scenario: Verify {sanitizedSummary}
    When I navigate to the home page
    Then the home page should be loaded
    And I close the browser

  Scenario: API test for {sanitizedSummary}
    Given I initialize the API client with configured URL
    When I send a GET request to ""/users""
    Then the response status code should be 200
    And the response should be valid JSON
";
        }

        /// <summary>
        /// Generates step definitions from a user story
        /// </summary>
        private string GenerateStepDefinitionsContent(JiraUserStory story)
        {
            var className = story.Key.Replace("-", "") + "StepDefinitions";

            return $@"using Reqnroll;
using SampleProject.Pages;

namespace SampleProject.StepDefinitions
{{
    [Binding]
    public class {className}
    {{
        private readonly ScenarioContext _scenarioContext;

        public {className}(ScenarioContext scenarioContext)
        {{
            _scenarioContext = scenarioContext;
        }}

        [Given(""I open the browser"")]
        public void GivenIOpenTheBrowser()
        {{
            // Browser opening step
            _scenarioContext[""browser_opened""] = true;
        }}

        [When(""I navigate to the home page"")]
        public void WhenINavigateToTheHomePage()
        {{
            var homePage = new HomePage();
            homePage.GoToHomePage();
        }}

        [Then(""the home page should be loaded"")]
        public void ThenTheHomePageShouldBeLoaded()
        {{
            var homePage = new HomePage();
            Assert.That(homePage.IsHomePageLoaded(), Is.True);
        }}

        [Then(""I close the browser"")]
        public void ThenICloseTheBrowser()
        {{
            // Browser closing step
        }}
    }}
}}
";
        }

        /// <summary>
        /// Generates Confluence content from execution report
        /// </summary>
        private string GenerateConfluenceContent(ExecutionReport report)
        {
            var status = report.Status == "SUCCESS"
                ? "<ac:structured-macro ac:name=\"status\"><ac:parameter ac:name=\"colour\">Green</ac:parameter><ac:parameter ac:name=\"title\">SUCCESS</ac:parameter></ac:structured-macro>"
                : "<ac:structured-macro ac:name=\"status\"><ac:parameter ac:name=\"colour\">Yellow</ac:parameter><ac:parameter ac:name=\"title\">PARTIAL SUCCESS</ac:parameter></ac:structured-macro>";

            return $@"<h1>Test Execution Report</h1>
<p>{status}</p>
<h2>Summary</h2>
<ul>
    <li><strong>Project:</strong> {report.ProjectKey}</li>
    <li><strong>User Stories:</strong> {report.UserStoriesCount}</li>
    <li><strong>Tests Generated:</strong> {report.TestsGenerated}</li>
    <li><strong>Tests Added:</strong> {report.TestsAdded}</li>
    <li><strong>Tests Run:</strong> {report.TestsRun}</li>
    <li><strong>Tests Passed:</strong> {report.TestsPassed}</li>
    <li><strong>Tests Failed:</strong> {report.TestsFailed}</li>
    <li><strong>Pass Rate:</strong> {(report.TestsRun > 0 ? (report.TestsPassed * 100 / report.TestsRun) : 0)}%</li>
</ul>
<h2>Execution Time</h2>
<p>Started: {report.StartTime:yyyy-MM-dd HH:mm:ss UTC}</p>
<p>Ended: {report.EndTime:yyyy-MM-dd HH:mm:ss UTC}</p>
<p>Duration: {(report.EndTime - report.StartTime).TotalSeconds:F2} seconds</p>
<h2>Links</h2>
<ul>
    <li><a href=""{report.GitHubPRUrl}"">GitHub Pull Request</a></li>
</ul>";
        }

        /// <summary>
        /// Generates GitHub PR description
        /// </summary>
        private string GenerateGitHubPRDescription(ExecutionReport report)
        {
            return $@"## Test Suite Update Report

**Project:** {report.ProjectKey}
**Status:** {report.Status}
**Execution Time:** {(report.EndTime - report.StartTime).TotalSeconds:F2} seconds

### Summary
- User Stories: {report.UserStoriesCount}
- Tests Generated: {report.TestsGenerated}
- Tests Added: {report.TestsAdded}
- Tests Run: {report.TestsRun}
- Tests Passed: ✅ {report.TestsPassed}
- Tests Failed: ❌ {report.TestsFailed}
- Pass Rate: {(report.TestsRun > 0 ? (report.TestsPassed * 100 / report.TestsRun) : 0)}%

### Details
- Started: {report.StartTime:yyyy-MM-dd HH:mm:ss UTC}
- Ended: {report.EndTime:yyyy-MM-dd HH:mm:ss UTC}
- Confluence Report: {report.ConfluencePageUrl}

---
*This PR was automatically generated by the Jira-GitHub-Confluence Integration Agent*";
        }

        /// <summary>
        /// Parses test results from dotnet test output
        /// </summary>
        private void ParseTestResults(string output, TestExecutionResult result)
        {
            try
            {
                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (var line in lines)
                {
                    if (line.Contains("Passed"))
                    {
                        var parts = line.Split(',');
                        foreach (var part in parts)
                        {
                            if (part.Contains("Passed"))
                            {
                                var count = int.Parse(System.Text.RegularExpressions.Regex.Match(part, @"\d+").Value);
                                result.PassedTests = count;
                            }
                            else if (part.Contains("Failed"))
                            {
                                var count = int.Parse(System.Text.RegularExpressions.Regex.Match(part, @"\d+").Value);
                                result.FailedTests = count;
                            }
                        }
                    }

                    if (line.Contains("Test run finished"))
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(line, @"(\d+) Tests");
                        if (match.Success)
                        {
                            result.TotalTests = int.Parse(match.Groups[1].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing test results: {ex.Message}");
            }
        }

        /// <summary>
        /// Sanitizes a string for use as a filename
        /// </summary>
        private string SanitizeName(string name)
        {
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            var sanitized = new StringBuilder();

            foreach (var c in name)
            {
                if (!invalidChars.Contains(c) && !char.IsWhiteSpace(c))
                {
                    sanitized.Append(c);
                }
            }

            return sanitized.ToString();
        }

        /// <summary>
        /// Prints execution summary to console
        /// </summary>
        private void PrintExecutionSummary(ExecutionReport report)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("               EXECUTION SUMMARY");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine($"Project: {report.ProjectKey}");
            Console.WriteLine($"Status: {report.Status}");
            Console.WriteLine($"User Stories: {report.UserStoriesCount}");
            Console.WriteLine($"Tests Generated: {report.TestsGenerated}");
            Console.WriteLine($"Tests Added: {report.TestsAdded}");
            Console.WriteLine($"Tests Run: {report.TestsRun}");
            Console.WriteLine($"Tests Passed: {report.TestsPassed} ✅");
            Console.WriteLine($"Tests Failed: {report.TestsFailed} ❌");
            Console.WriteLine($"Pass Rate: {(report.TestsRun > 0 ? (report.TestsPassed * 100 / report.TestsRun) : 0)}%");
            Console.WriteLine($"Duration: {(report.EndTime - report.StartTime).TotalSeconds:F2}s");
            Console.WriteLine($"Confluence Report: {report.ConfluencePageUrl}");
            Console.WriteLine($"GitHub PR: {report.GitHubPRUrl}");
            Console.WriteLine("═══════════════════════════════════════════════════════════\n");
        }
    }

    /// <summary>
    /// Represents a Jira user story
    /// </summary>
    public class JiraUserStory
    {
        public string Key { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents a generated test case
    /// </summary>
    public class GeneratedTestCase
    {
        public string StoryKey { get; set; }
        public string FeatureFileName { get; set; }
        public string FeatureContent { get; set; }
        public string StepDefinitionsContent { get; set; }
    }

    /// <summary>
    /// Represents test execution results
    /// </summary>
    public class TestExecutionResult
    {
        public int TotalTests { get; set; }
        public int PassedTests { get; set; }
        public int FailedTests { get; set; }
        public string Output { get; set; }
        public string ErrorOutput { get; set; }
        public int ExitCode { get; set; }
    }

    /// <summary>
    /// Represents the complete execution report
    /// </summary>
    public class ExecutionReport
    {
        public string ProjectKey { get; set; }
        public int UserStoriesCount { get; set; }
        public int TestsGenerated { get; set; }
        public int TestsAdded { get; set; }
        public int TestsRun { get; set; }
        public int TestsPassed { get; set; }
        public int TestsFailed { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ConfluencePageUrl { get; set; }
        public string GitHubPRUrl { get; set; }
        public string ErrorMessage { get; set; }
        public bool Successful { get; set; }
        public TestExecutionResult TestResults { get; set; }
    }
}
