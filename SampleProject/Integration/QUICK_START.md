# 🤖 Integration Agent - Quick Start Guide

## What Does It Do?

The Integration Agent automates your entire test workflow:

```
Jira User Story
       ↓
Generate Test Cases
       ↓
Add to Solution
       ↓
Run Tests
       ↓
Collect Results
       ↓
Post to Confluence
       ↓
Create GitHub PR
```

---

## 🔑 Your Credentials

⚠️ **SECURITY WARNING**: Never commit actual tokens to GitHub!

Set up environment variables instead:

```powershell
# In PowerShell (Windows)
$env:JIRA_URL = "https://your-domain.atlassian.net"
$env:JIRA_TOKEN = "your-jira-token-here"
$env:GITHUB_TOKEN = "your-github-token-here"
$env:GITHUB_REPO = "your-username/your-repo"
$env:CONFLUENCE_URL = "https://your-domain.atlassian.net/wiki"
$env:CONFLUENCE_SPACE = "YOUR_SPACE"
$env:CONFLUENCE_PAGE_ID = "your-page-id"
```

See `.env.example` for all available environment variables.

---

## ⚙️ Basic Setup

### Step 1: Configure the Agent

```csharp
var config = IntegrationAgentRunner.CreateConfig(
    jiraBaseUrl: "https://your-jira-domain.atlassian.net",
    jiraToken: "YOUR_JIRA_TOKEN_HERE",
    githubToken: "YOUR_GITHUB_TOKEN_HERE",
    githubRepo: "your-username/your-repo",
    confluenceBaseUrl: "https://your-confluence-domain.atlassian.net/wiki",
    confluenceSpaceKey: "TEST",
    confluencePageId: "123456789",
    jiraProjectKey: "TEST"
);
```

### Step 2: Create Runner

```csharp
var runner = new IntegrationAgentRunner(config);
```

### Step 3: Execute Workflow

```csharp
var success = await runner.RunWorkflowAsync("TEST");
```

---

## 📋 Full Example

### Console Application

Create a new file: `SampleProject/Agent/RunAgent.cs`

```csharp
using System;
using System.Threading.Tasks;
using SampleProject.Integration;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("🤖 Starting Integration Agent...\n");

        try
        {
            // Configure - IMPORTANT: Use environment variables!
            var config = IntegrationAgentRunner.CreateConfig(
                jiraBaseUrl: Environment.GetEnvironmentVariable("JIRA_URL") ?? "https://your-jira-domain.atlassian.net",
                jiraToken: Environment.GetEnvironmentVariable("JIRA_TOKEN") ?? "YOUR_TOKEN_HERE",
                githubToken: Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? "YOUR_TOKEN_HERE",
                githubRepo: Environment.GetEnvironmentVariable("GITHUB_REPO") ?? "your-username/your-repo",
                confluenceBaseUrl: Environment.GetEnvironmentVariable("CONFLUENCE_URL") ?? "https://your-confluence-domain.atlassian.net/wiki",
                confluenceSpaceKey: Environment.GetEnvironmentVariable("CONFLUENCE_SPACE") ?? "TEST",
                confluencePageId: Environment.GetEnvironmentVariable("CONFLUENCE_PAGE_ID") ?? "123456789",
                jiraProjectKey: "TEST"
            );

            // Run
            var runner = new IntegrationAgentRunner(config);
            var success = await runner.RunWorkflowAsync("TEST");

            // Exit with status
            Environment.Exit(success ? 0 : 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Environment.Exit(-1);
        }
    }
}
```

Run it:
```powershell
dotnet run
```

---

## 🧪 Test Example

```csharp
[Test]
public async Task TestIntegrationWorkflow()
{
    // Setup
    var config = IntegrationAgentRunner.CreateConfig(
        jiraBaseUrl: "https://your-jira-domain.atlassian.net",
        jiraToken: "YOUR_JIRA_TOKEN",
        githubToken: "YOUR_GITHUB_TOKEN",
        githubRepo: "owner/repo",
        confluenceBaseUrl: "https://your-confluence-domain.atlassian.net/wiki",
        confluenceSpaceKey: "TEST",
        confluencePageId: "123456789"
    );

    // Execute
    var runner = new IntegrationAgentRunner(config);
    var success = await runner.RunWorkflowAsync("TEST");

    // Verify
    Assert.That(success, Is.True, "Workflow should complete successfully");
}
```

---

## 📊 What You Get

### 1. Generated Test Files

**Feature file:** `Features/TEST1_UserRegistration.feature`
```gherkin
Feature: User Registration Feature
  Story: TEST-1

  Scenario: Verify User Registration Feature
    Given I open the browser
    When I navigate to the home page
    Then the home page should be loaded
```

**Step definitions:** `StepDefinitions/TEST1StepDefinitions.cs`
```csharp
[Binding]
public class TEST1StepDefinitions
{
    [Given("I open the browser")]
    public void GivenIOpenTheBrowser() { ... }

    [When("I navigate to the home page")]
    public void WhenINavigateToTheHomePage() { ... }
}
```

### 2. Console Report

```
═══════════════════════════════════════════════════════════
               EXECUTION SUMMARY
═══════════════════════════════════════════════════════════
Project: TEST
Status: SUCCESS
User Stories: 5
Tests Generated: 5
Tests Added: 5
Tests Run: 10
Tests Passed: 9 ✅
Tests Failed: 1 ❌
Pass Rate: 90%
Duration: 65.42s
Confluence Report: https://...confluence...
GitHub PR: https://github.com/.../pull/123
═══════════════════════════════════════════════════════════
```

### 3. Confluence Report

Beautiful formatted report posted to your Confluence page with:
- Test statistics
- Pass/fail breakdown
- Execution time
- GitHub PR link

### 4. GitHub Pull Request

Pull request created with:
- All new test files
- Detailed description
- Test statistics in PR body
- Ready for review and merge

---

## 🔄 Automation Scenarios

### Scenario 1: Run Daily Tests

Schedule via Windows Task Scheduler or cron:

```powershell
# Run every morning at 8 AM
dotnet run --project SampleProject/Integration/
```

### Scenario 2: Triggered by Jira Workflow

When story status changes to "Ready for Testing":

```csharp
// Webhook receives event
// Triggers: await runner.RunWorkflowAsync("TEST", 
//   "status = 'Ready for Testing'");
```

### Scenario 3: Sprint Planning

Before sprint starts:

```csharp
await runner.RunWorkflowAsync("TEST", 
  "sprint = 'Current Sprint'");
```

---

## 🎯 What Happens Step-by-Step

### 1️⃣ Fetch from Jira
- Queries your Jira project
- Gets all user stories
- Extracts summaries and descriptions

### 2️⃣ Generate Tests
- Creates .feature files from stories
- Creates step definitions
- Generates Gherkin scenarios

### 3️⃣ Add to Solution
- Writes files to Features folder
- Writes files to StepDefinitions folder
- Ready for testing

### 4️⃣ Run Tests
- Executes `dotnet test`
- Collects pass/fail results
- Captures execution time

### 5️⃣ Generate Report
- Compiles all metrics
- Calculates pass rate
- Formats results

### 6️⃣ Post to Confluence
- Uploads formatted report
- Updates your project page
- Anyone can view results

### 7️⃣ Create GitHub PR
- Commits test files
- Creates pull request
- Links to Confluence report
- Ready for code review

---

## 🔧 Configuration File

Instead of hardcoding, use `Config/appsettings.json`:

```json
{
  "Integration": {
    "Jira": {
      "BaseUrl": "https://your-jira-domain.atlassian.net",
      "Token": "YOUR_TOKEN",
      "ProjectKey": "TEST"
    },
    "GitHub": {
      "Token": "YOUR_TOKEN",
      "Repository": "owner/repo",
      "BaseBranch": "main"
    },
    "Confluence": {
      "BaseUrl": "https://your-confluence-domain.atlassian.net/wiki",
      "SpaceKey": "TEST",
      "PageId": "123456789"
    }
  }
}
```

Load it:

```csharp
var jiraToken = ConfigReader.Instance.GetValue("Integration:Jira:Token");
```

---

## ✅ Success Checklist

- [ ] Jira credentials configured
- [ ] GitHub token has repo access
- [ ] Confluence page created
- [ ] Agent configuration updated
- [ ] First test run completed
- [ ] Results visible on Confluence
- [ ] Pull request created on GitHub
- [ ] Team can see reports

---

## 🐛 Troubleshooting

### Tests not running?
```powershell
cd SampleProject
dotnet test
```

### Jira connection fails?
- Check URL format
- Verify token
- Ensure network access

### GitHub PR not created?
- Check token has `repo` scope
- Verify repository name
- Ensure branch exists

### Confluence report missing?
- Verify page ID
- Check space key
- Confirm token permissions

---

## 📚 More Information

- See `AGENT_DOCUMENTATION.md` for detailed guide
- Check `Integration/Agents/` for agent code
- Review `Integration/Configuration/` for settings

---

## 🚀 Ready to Go!

Your agent is configured and ready to:
1. ✅ Read Jira stories
2. ✅ Generate tests
3. ✅ Run tests
4. ✅ Report results
5. ✅ Update Confluence
6. ✅ Create GitHub PRs

**Start the workflow:**

```powershell
cd SampleProject
dotnet run
```

🎉 **Let the automation begin!**
