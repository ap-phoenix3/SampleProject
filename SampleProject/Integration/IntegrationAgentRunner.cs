using System;
using System.Threading.Tasks;
using SampleProject.Integration.Agents;
using SampleProject.Integration.Configuration;

namespace SampleProject.Integration
{
    /// <summary>
    /// Integration Agent Runner - Entry point for the Jira-GitHub-Confluence workflow
    /// </summary>
    public class IntegrationAgentRunner
    {
        private readonly JiraGithubConfluenceAgent _agent;
        private readonly IntegrationAgentConfig _config;

        public IntegrationAgentRunner(IntegrationAgentConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _agent = new JiraGithubConfluenceAgent(
                jiraBaseUrl: _config.Jira.BaseUrl,
                jiraToken: _config.Jira.Token,
                githubToken: _config.GitHub.Token,
                githubRepo: _config.GitHub.Repository,
                githubBranch: _config.GitHub.BaseBranch,
                confluenceBaseUrl: _config.Confluence.BaseUrl,
                confluenceSpaceKey: _config.Confluence.SpaceKey,
                confluencePageId: _config.Confluence.PageId
            );
        }

        /// <summary>
        /// Runs the complete integration workflow
        /// </summary>
        public async Task<bool> RunWorkflowAsync(string jiraProjectKey, string customJql = null)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  Jira → Test Generation → Test Execution → GitHub → Confluence ");
            Console.WriteLine("║                    INTEGRATION WORKFLOW                      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            try
            {
                var report = await _agent.ExecuteWorkflowAsync(jiraProjectKey, customJql);

                if (report.Successful)
                {
                    Console.WriteLine("✨ Workflow completed successfully!");
                    Console.WriteLine($"\n📊 Final Results:");
                    Console.WriteLine($"   • Total Tests Run: {report.TestsRun}");
                    Console.WriteLine($"   • Passed: {report.TestsPassed} ✅");
                    Console.WriteLine($"   • Failed: {report.TestsFailed} ❌");
                    Console.WriteLine($"   • Success Rate: {(report.TestsRun > 0 ? (report.TestsPassed * 100.0 / report.TestsRun) : 0):F2}%");
                    Console.WriteLine($"\n🔗 Reports:");
                    Console.WriteLine($"   • Confluence: {report.ConfluencePageUrl}");
                    Console.WriteLine($"   • GitHub PR: {report.GitHubPRUrl}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"⚠️  Workflow completed with errors: {report.ErrorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Workflow failed with exception: {ex.Message}");
                Console.WriteLine($"\nStack Trace:\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Creates a configuration from credentials
        /// </summary>
        public static IntegrationAgentConfig CreateConfig(
            string jiraBaseUrl,
            string jiraToken,
            string githubToken,
            string githubRepo,
            string confluenceBaseUrl,
            string confluenceSpaceKey,
            string confluencePageId,
            string jiraProjectKey = "TEST")
        {
            return new IntegrationAgentConfig
            {
                Jira = new JiraConfig
                {
                    BaseUrl = jiraBaseUrl,
                    Token = jiraToken,
                    ProjectKey = jiraProjectKey
                },
                GitHub = new GitHubConfig
                {
                    Token = githubToken,
                    Repository = githubRepo
                },
                Confluence = new ConfluenceConfig
                {
                    BaseUrl = confluenceBaseUrl,
                    SpaceKey = confluenceSpaceKey,
                    PageId = confluencePageId
                },
                Agent = new AgentSettings
                {
                    AutoRunTests = true,
                    AutoCreatePR = true,
                    AutoPostConfluence = true,
                    VerboseLogging = true
                }
            };
        }
    }

    /// <summary>
    /// Console application for running the integration workflow
    /// </summary>
    public class IntegrationAgentProgram
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("🚀 Starting Integration Agent...\n");

            try
            {
                // ⚠️  IMPORTANT: Use environment variables for tokens, never hardcode them!
                // Example: Set environment variables:
                // $env:JIRA_URL = "https://your-domain.atlassian.net"
                // $env:JIRA_TOKEN = "Your Jira token here"
                // $env:GITHUB_TOKEN = "Your GitHub token here"
                // $env:GITHUB_REPO = "owner/repo"
                // $env:CONFLUENCE_URL = "https://your-domain.atlassian.net/wiki"
                // $env:CONFLUENCE_SPACE = "TEST"
                // $env:CONFLUENCE_PAGE_ID = "123456789"

                var config = new IntegrationAgentConfig
                {
                    Jira = new JiraConfig
                    {
                        BaseUrl = Environment.GetEnvironmentVariable("JIRA_URL") ?? "https://your-domain.atlassian.net",
                        Token = Environment.GetEnvironmentVariable("JIRA_TOKEN") ?? "YOUR_JIRA_TOKEN_HERE",
                        ProjectKey = "TEST"
                    },
                    GitHub = new GitHubConfig
                    {
                        Token = Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? "YOUR_GITHUB_TOKEN_HERE",
                        Repository = Environment.GetEnvironmentVariable("GITHUB_REPO") ?? "owner/repo"
                    },
                    Confluence = new ConfluenceConfig
                    {
                        BaseUrl = Environment.GetEnvironmentVariable("CONFLUENCE_URL") ?? "https://your-domain.atlassian.net/wiki",
                        SpaceKey = Environment.GetEnvironmentVariable("CONFLUENCE_SPACE") ?? "TEST",
                        PageId = Environment.GetEnvironmentVariable("CONFLUENCE_PAGE_ID") ?? "123456789"
                    },
                    Agent = new AgentSettings
                    {
                        AutoRunTests = true,
                        AutoCreatePR = true,
                        AutoPostConfluence = true,
                        VerboseLogging = true
                    }
                };

                var runner = new IntegrationAgentRunner(config);
                var success = await runner.RunWorkflowAsync("TEST");

                Environment.Exit(success ? 0 : 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fatal Error: {ex.Message}");
                Console.WriteLine($"\nStack Trace:\n{ex.StackTrace}");
                Environment.Exit(-1);
            }
        }
    }
}
