# 🤖 Jira-GitHub-Confluence Integration Agent

## Overview

The Integration Agent is an automated system that:

1. 📋 **Reads user stories from Jira**
2. 🧪 **Generates test cases** based on user story descriptions
3. 📁 **Adds tests to the solution** (feature files and step definitions)
4. ▶️ **Runs all tests** and collects results
5. 📊 **Generates execution reports** with detailed statistics
6. 🔗 **Posts reports to Confluence** for visibility
7. 🔀 **Creates GitHub pull requests** with test changes

---

## 🚀 Quick Start

### Step 1: Configuration

Create an `IntegrationAgentConfig` with your credentials:

```csharp
var config = IntegrationAgentRunner.CreateConfig(
    jiraBaseUrl: "https://your-domain.atlassian.net/",
    jiraToken: "YOUR_JIRA_TOKEN", // Never hardcode tokens!
    githubToken: "YOUR_GITHUB_TOKEN", // Use environment variables
    githubRepo: "owner/repo",
    confluenceBaseUrl: "https://your-domain.atlassian.net/wiki",
    confluenceSpaceKey: "YOUR_SPACE",
    confluencePageId: "YOUR_PAGE_ID",
    jiraProjectKey: "YOUR_PROJECT"
);
```

### Step 2: Run the Workflow

```csharp
var runner = new IntegrationAgentRunner(config);
var success = await runner.RunWorkflowAsync("TEST");
```

### Step 3: Check Results

- ✅ Tests added to `SampleProject/Features/` and `SampleProject/StepDefinitions/`
- ✅ Test results displayed in console
- ✅ Report posted to Confluence
- ✅ Pull request created on GitHub

---

## 📋 Workflow Steps Explained

### Step 1: Fetch User Stories from Jira

```
GET /rest/api/3/search?jql=project=TEST AND type=Story
```

**Retrieves:**
- Story Key (e.g., TEST-1, TEST-2)
- Story Summary (title)
- Story Description (acceptance criteria)

**Console Output:**
```
📋 Step 1: Fetching user stories from Jira...
  📌 TEST-1: User Registration Feature
  📌 TEST-2: Login Validation
  📌 TEST-3: Password Reset
✅ Found 3 user stories
```

---

### Step 2: Generate Test Cases

For each user story, the agent creates:

**Feature File Example:**
```gherkin
Feature: User Registration Feature
  Story: TEST-1

  As a user
  I want to register a new account
  So that I can access the application

  Background:
    Given I open the browser

  Scenario: Verify User Registration Feature
    When I navigate to the home page
    Then the home page should be loaded
    And I close the browser
```

**Step Definitions Example:**
```csharp
[Binding]
public class TEST1StepDefinitions
{
    [Given("I open the browser")]
    public void GivenIOpenTheBrowser()
    {
        // Implementation
    }

    [When("I navigate to the home page")]
    public void WhenINavigateToTheHomePage()
    {
        var homePage = new HomePage();
        homePage.GoToHomePage();
    }

    [Then("the home page should be loaded")]
    public void ThenTheHomePageShouldBeLoaded()
    {
        var homePage = new HomePage();
        Assert.That(homePage.IsHomePageLoaded(), Is.True);
    }
}
```

---

### Step 3: Add Tests to Solution

Tests are automatically added to:

- **Feature Files:** `SampleProject/Features/`
- **Step Definitions:** `SampleProject/StepDefinitions/`

**Console Output:**
```
📁 Step 3: Adding test cases to solution...
  ✅ Added TEST-1 to solution
  ✅ Added TEST-2 to solution
  ✅ Added TEST-3 to solution
✅ Added 3 test cases to solution
```

---

### Step 4: Run Tests

Executes: `dotnet test` in the project directory

**Console Output:**
```
▶️  Step 4: Running test cases...
✅ Test Results: 5 Passed, 1 Failed
```

---

### Step 5: Generate Execution Report

Creates a comprehensive report with:

- Total tests run
- Pass/fail counts
- Pass rate percentage
- Execution time
- Timestamps
- Links to Confluence and GitHub

---

### Step 6: Post to Confluence

Posts formatted report to Confluence page:

```
https://your-domain.atlassian.net/wiki/spaces/TEST/pages/123456789
```

**Report Contains:**
- Status badge (Success/Partial Success)
- Summary statistics
- Test counts and pass rate
- Execution timestamps
- GitHub PR link

---

### Step 7: Create GitHub Pull Request

Creates PR with:

- **Title:** Test Suite Update - {PROJECT} - {DATE}
- **Branch:** `test-automation` → `main` (or configured base branch)
- **Description:** Complete execution summary
- **Files Changed:** All newly added test files

---

## 🔐 Authentication Setup

### Jira Token

1. Go to https://id.atlassian.com/manage-profile/security/api-tokens
2. Click "Create API token"
3. Copy the token and save it securely
4. **Never commit tokens to version control**
5. Use environment variables or secret management tools

### GitHub Token

1. Go to GitHub Settings → Developer settings → Personal access tokens
2. Click "Generate new token"
3. Select scopes: `repo`, `workflow`
4. Copy and save it securely
5. **Never commit tokens to version control**
6. Use environment variables or GitHub Secrets

### Confluence Token

1. Go to your Atlassian account settings
2. Generate an API token
3. Save it securely
4. **Never commit tokens to version control**

### Confluence Token

1. Go to https://id.atlassian.com/manage-profile/security/api-tokens
2. Create an API token
3. Use with your Confluence email

---

## 📊 Generated Report Example

### Console Output

```
═══════════════════════════════════════════════════════════
               EXECUTION SUMMARY
═══════════════════════════════════════════════════════════
Project: TEST
Status: SUCCESS
User Stories: 3
Tests Generated: 3
Tests Added: 3
Tests Run: 8
Tests Passed: 7 ✅
Tests Failed: 1 ❌
Pass Rate: 87.5%
Duration: 45.32s
Confluence Report: https://your-domain.atlassian.net/wiki/...
GitHub PR: https://github.com/your-username/your-repo/pull/123
═══════════════════════════════════════════════════════════
```

### Confluence Report

```
Test Execution Report
[SUCCESS]

Summary
• Project: TEST
• User Stories: 3
• Tests Generated: 3
• Tests Added: 3
• Tests Run: 8
• Tests Passed: 7
• Tests Failed: 1
• Pass Rate: 87.5%

Execution Time
Started: 2024-01-15 14:30:00 UTC
Ended: 2024-01-15 14:30:45 UTC
Duration: 45.32 seconds

Links
• GitHub Pull Request
```

---

## 🔧 Configuration Options

### Agent Settings

```csharp
var config = new IntegrationAgentConfig
{
    Agent = new AgentSettings
    {
        AutoRunTests = true,                // Run tests after generation
        AutoCreatePR = true,                // Create GitHub PR
        AutoPostConfluence = true,          // Post to Confluence
        VerboseLogging = true,              // Detailed console output
        TestExecutionTimeout = 300,         // 5 minutes
        MaxTestCasesPerStory = 5            // Max cases per story
    }
};
```

### Jira Settings

```csharp
config.Jira = new JiraConfig
{
    BaseUrl = "https://your-domain.atlassian.net",
    Token = "YOUR_TOKEN",
    ProjectKey = "TEST",
    DefaultJQL = "type = Story AND status != Done"
};
```

### GitHub Settings

```csharp
config.GitHub = new GitHubConfig
{
    Token = "ghp_...",
    Repository = "owner/repo",
    BaseBranch = "main",
    FeatureBranch = "test-automation"
};
```

### Confluence Settings

```csharp
config.Confluence = new ConfluenceConfig
{
    BaseUrl = "https://your-domain.atlassian.net/wiki",
    SpaceKey = "TEST",
    PageId = "123456789",
    Token = "YOUR_TOKEN"
};
```

---

## 🚀 Running the Agent

### Via Console Application

```powershell
cd SampleProject
dotnet run --project Integration/IntegrationAgentRunner.cs
```

### Via Unit Test

```csharp
[Test]
public async Task RunIntegrationWorkflow()
{
    var config = IntegrationAgentRunner.CreateConfig(
        jiraBaseUrl: "https://your-domain.atlassian.net",
        jiraToken: "YOUR_TOKEN",
        githubToken: "YOUR_TOKEN",
        githubRepo: "owner/repo",
        confluenceBaseUrl: "https://your-domain.atlassian.net/wiki",
        confluenceSpaceKey: "TEST",
        confluencePageId: "123456789"
    );

    var runner = new IntegrationAgentRunner(config);
    var success = await runner.RunWorkflowAsync("TEST");

    Assert.That(success, Is.True);
}
```

### Via Program.cs in a Standalone App

```csharp
static async Task Main(string[] args)
{
    var config = IntegrationAgentRunner.CreateConfig(
        jiraBaseUrl: Environment.GetEnvironmentVariable("JIRA_URL"),
        jiraToken: Environment.GetEnvironmentVariable("JIRA_TOKEN"),
        githubToken: Environment.GetEnvironmentVariable("GITHUB_TOKEN"),
        githubRepo: Environment.GetEnvironmentVariable("GITHUB_REPO"),
        confluenceBaseUrl: Environment.GetEnvironmentVariable("CONFLUENCE_URL"),
        confluenceSpaceKey: Environment.GetEnvironmentVariable("CONFLUENCE_SPACE"),
        confluencePageId: Environment.GetEnvironmentVariable("CONFLUENCE_PAGE_ID")
    );

    var runner = new IntegrationAgentRunner(config);
    var success = await runner.RunWorkflowAsync("TEST");

    return success ? 0 : 1;
}
```

---

## 📝 Example Scenarios

### Scenario 1: Nightly Automated Test Suite

```csharp
// Run every night at 2 AM
var runner = new IntegrationAgentRunner(config);
await runner.RunWorkflowAsync("TEST");

// Results automatically posted to Confluence
// PR automatically created for review
```

### Scenario 2: Triggered by Jira Webhook

```csharp
// When story moves to "Ready for Testing"
// Webhook triggers:
var runner = new IntegrationAgentRunner(config);
await runner.RunWorkflowAsync("TEST", 
    customJql: "type = Story AND status = 'Ready for Testing'");
```

### Scenario 3: Sprint Planning

```csharp
// Generate tests for sprint stories
var runner = new IntegrationAgentRunner(config);
await runner.RunWorkflowAsync("TEST", 
    customJql: "sprint = 'Current Sprint' AND type = Story");
```

---

## 🔍 Debugging

### Enable Verbose Logging

```csharp
config.Agent.VerboseLogging = true;
```

### Check Console Output

All steps print detailed information:

```
📋 Step 1: Fetching user stories from Jira...
  📌 TEST-1: ...
  ✅ Found 3 user stories

🧪 Step 2: Generating test cases...
  🧪 Generated test for TEST-1
  ✅ Generated 3 test cases

... (continues for all steps)
```

### Check Generated Files

- **Features:** `SampleProject/Features/TEST*.feature`
- **Step Defs:** `SampleProject/StepDefinitions/TEST*StepDefinitions.cs`

### Verify Test Results

```powershell
cd SampleProject
dotnet test --logger "console;verbosity=detailed"
```

---

## 🎯 Best Practices

1. **Use meaningful Jira descriptions** - Better descriptions = better tests
2. **Review generated tests** - Check feature files for accuracy
3. **Update step definitions** - Customize generic steps for your app
4. **Monitor Confluence reports** - Track trends over time
5. **Review PRs before merge** - Ensure quality before integration
6. **Use appropriate JQL queries** - Filter stories efficiently
7. **Set reasonable timeouts** - Prevent hanging tests
8. **Rotate tokens regularly** - Security best practice

---

## 📊 Metrics Tracked

- Total user stories processed
- Test cases generated
- Test cases added to solution
- Total tests run
- Tests passed/failed
- Pass rate percentage
- Execution duration
- Timestamp of execution

---

## ❌ Troubleshooting

| Issue | Solution |
|-------|----------|
| **Jira authentication fails** | Verify token and URL format |
| **GitHub PR not created** | Check token has `repo` scope |
| **Confluence post fails** | Verify page ID and space key |
| **Tests not found** | Check feature file syntax |
| **Connection timeout** | Increase timeout in settings |

---

## 📚 Related Documentation

- [Jira API Documentation](https://developer.atlassian.com/cloud/jira/rest/)
- [GitHub API Documentation](https://docs.github.com/en/rest)
- [Confluence API Documentation](https://developer.atlassian.com/cloud/confluence/rest/)
- [BDD Reqnroll Documentation](https://reqnroll.net/)

---

## 🎉 Success Indicators

✅ User stories fetched from Jira
✅ Test files generated
✅ Tests added to solution
✅ Tests executed successfully
✅ Report posted to Confluence
✅ GitHub PR created
✅ All steps completed within timeout

---

**Ready to automate your testing workflow!** 🚀
