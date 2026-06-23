# 🤖 Jira-GitHub-Confluence Integration Agent

## 🎯 Complete Automation Solution

Your automation agent is **READY** to:

1. 📋 **Read user stories** from your Jira project
2. 🧪 **Generate test cases** automatically
3. ▶️ **Run tests** with a single command
4. 📊 **Collect metrics** and generate reports
5. 🔗 **Post to Confluence** for team visibility
6. 🔀 **Create GitHub PRs** with test changes
7. 📧 **Send notifications** with results

---

## 🚀 Quick Start (5 Minutes)

### 1. Configure Your Credentials

Edit `SampleProject/Integration/IntegrationAgentRunner.cs` around line 45:

```csharp
var config = IntegrationAgentRunner.CreateConfig(
    jiraBaseUrl: "https://YOUR-DOMAIN.atlassian.net",
    jiraToken: "YOUR_JIRA_TOKEN",
    githubToken: "YOUR_GITHUB_TOKEN",
    githubRepo: "your-username/your-repo-name",
    confluenceBaseUrl: "https://YOUR-DOMAIN.atlassian.net/wiki",
    confluenceSpaceKey: "YOUR_SPACE_KEY",
    confluencePageId: "YOUR_PAGE_ID",
    jiraProjectKey: "YOUR_PROJECT_KEY"
);
```

**IMPORTANT:** Use environment variables, not hardcoded tokens!

### 2. Build & Run

```powershell
cd SampleProject
dotnet build
dotnet run
```

### 3. Check Results

✅ See console output with real-time progress
✅ Find report on your Confluence page
✅ Review PR on GitHub

---

## 📋 What Happens

### Automatic Flow

```
┌─────────────────┐
│  Jira Stories   │
│   TEST-1        │
│   TEST-2        │
│   TEST-3        │
└────────┬────────┘
         │
         ▼
┌─────────────────────────┐
│ Generate Test Files     │
│ .feature files          │
│ Step definitions        │
└────────┬────────────────┘
         │
         ▼
┌─────────────────────────┐
│ Add to Solution         │
│ Update project files    │
│ Build solution          │
└────────┬────────────────┘
         │
         ▼
┌─────────────────────────┐
│ Run Tests               │
│ dotnet test             │
│ Collect results         │
└────────┬────────────────┘
         │
         ▼
┌─────────────────────────┐
│ Generate Report         │
│ Metrics & statistics    │
│ Pass/fail breakdown     │
└────────┬────────────────┘
         │
    ┌────┴────┐
    │          │
    ▼          ▼
┌────────┐  ┌─────────┐
│Confluence│ │ GitHub  │
│Report   │ │ Pull    │
│Post     │ │Request  │
└────────┘  └─────────┘
```

---

## 🔍 Detailed Workflow

### Step 1: Fetch from Jira ✅

Queries your Jira project for user stories:

```
GET /rest/api/3/search?jql=project=TEST AND type=Story
```

**Retrieves:**
- Story ID (e.g., TEST-1)
- Title/Summary
- Description/Acceptance Criteria
- Status and other metadata

### Step 2: Generate Tests ✅

Creates test files from each story:

**Feature File:**
```gherkin
Feature: User Registration Feature
  Story: TEST-1

  Scenario: Verify User Registration
    Given I open the browser
    When I navigate to the home page
    Then the home page should be loaded
```

**Step Definitions:**
```csharp
[Binding]
public class TEST1StepDefinitions
{
    [Given("I open the browser")]
    public void GivenIOpenTheBrowser() { ... }
}
```

### Step 3: Add to Solution ✅

Files automatically saved to:
- `Features/` directory
- `StepDefinitions/` directory

### Step 4: Run Tests ✅

Executes: `dotnet test --logger "console;verbosity=detailed"`

Collects:
- Total tests
- Passed count
- Failed count
- Execution time

### Step 5: Generate Report ✅

Creates comprehensive metrics:
- Pass rate percentage
- Execution duration
- Timestamps
- Links to results

### Step 6: Post to Confluence ✅

Uploads formatted report to your Confluence page:

```
Project: TEST
Status: SUCCESS
Tests Run: 10
Tests Passed: 9 ✅
Tests Failed: 1 ❌
Pass Rate: 90%
```

### Step 7: Create GitHub PR ✅

Automatic pull request with:
- All new test files
- Complete statistics in description
- Confluence report link
- Ready for code review

---

## 📚 Files Created

### Agent Code
- `Integration/Agents/JiraGithubConfluenceAgent.cs` - Main logic
- `Integration/Configuration/IntegrationAgentConfig.cs` - Settings
- `Integration/IntegrationAgentRunner.cs` - Runner/Entry point

### Documentation
- `Integration/AGENT_DOCUMENTATION.md` - Full guide (50+ pages)
- `Integration/QUICK_START.md` - Quick reference
- `Integration/SETUP_SUMMARY.md` - Setup details
- `Integration/README.md` - This file

---

## 🔐 Your Credentials

⚠️ **SECURITY FIRST**: Never hardcode or commit tokens!

**Set up environment variables:**

```powershell
# PowerShell (Windows)
$env:JIRA_URL = "https://your-domain.atlassian.net"
$env:JIRA_TOKEN = "your-token-here"
$env:GITHUB_TOKEN = "your-token-here"
$env:GITHUB_REPO = "owner/repo"
$env:CONFLUENCE_URL = "https://your-domain.atlassian.net/wiki"
$env:CONFLUENCE_SPACE = "TEST"
$env:CONFLUENCE_PAGE_ID = "123456789"

# Bash/Linux
export JIRA_URL="https://your-domain.atlassian.net"
export JIRA_TOKEN="your-token-here"
# ... etc
```

**Reference:** See `.env.example` for all available variables

---

## ⚙️ Configuration

### Minimal Setup

```csharp
var config = IntegrationAgentRunner.CreateConfig(
    jiraBaseUrl: "https://your-jira-domain.atlassian.net",
    jiraToken: "YOUR_TOKEN",
    githubToken: "ghp_...",
    githubRepo: "owner/repo",
    confluenceBaseUrl: "https://your-confluence-domain.atlassian.net/wiki",
    confluenceSpaceKey: "TEST",
    confluencePageId: "123456789"
);
```

### Full Configuration

```csharp
var config = new IntegrationAgentConfig
{
    Jira = new JiraConfig
    {
        BaseUrl = "https://your-domain.atlassian.net",
        Token = "YOUR_TOKEN",
        ProjectKey = "TEST",
        DefaultJQL = "type = Story AND status != Done"
    },
    GitHub = new GitHubConfig
    {
        Token = "ghp_...",
        Repository = "owner/repo",
        BaseBranch = "main",
        FeatureBranch = "test-automation"
    },
    Confluence = new ConfluenceConfig
    {
        BaseUrl = "https://your-domain.atlassian.net/wiki",
        SpaceKey = "TEST",
        PageId = "123456789",
        Token = "YOUR_TOKEN"
    },
    Agent = new AgentSettings
    {
        AutoRunTests = true,
        AutoCreatePR = true,
        AutoPostConfluence = true,
        VerboseLogging = true
    }
};
```

---

## 🎯 Usage Examples

### Run Workflow

```csharp
var runner = new IntegrationAgentRunner(config);
var success = await runner.RunWorkflowAsync("TEST");
```

### Run with Custom Filter

```csharp
await runner.RunWorkflowAsync("TEST", 
    customJql: "type = Story AND priority = High");
```

### Run from Console

```powershell
cd SampleProject
dotnet run
```

### Run from Unit Test

```csharp
[Test]
public async Task TestAutomationWorkflow()
{
    var runner = new IntegrationAgentRunner(config);
    var success = await runner.RunWorkflowAsync("TEST");
    Assert.That(success, Is.True);
}
```

---

## 📊 Sample Output

### Console Report

```
╔════════════════════════════════════════════════════════════╗
║  Jira → Test Generation → Test Execution → GitHub → Confluence
║                    INTEGRATION WORKFLOW
╚════════════════════════════════════════════════════════════╝

📋 Step 1: Fetching user stories from Jira...
  📌 TEST-1: User Registration Feature
  📌 TEST-2: Login Validation
  📌 TEST-3: Password Reset
✅ Found 3 user stories

🧪 Step 2: Generating test cases from user stories...
  🧪 Generated test for TEST-1
  🧪 Generated test for TEST-2
  🧪 Generated test for TEST-3
✅ Generated 3 test cases

📁 Step 3: Adding test cases to solution...
  ✅ Added TEST-1 to solution
  ✅ Added TEST-2 to solution
  ✅ Added TEST-3 to solution
✅ Added 3 test cases to solution

▶️  Step 4: Running test cases...
✅ Test Results: 6 Passed, 1 Failed

📊 Step 5: Generating execution report...
✅ Report generated with status: PARTIAL_SUCCESS

🔗 Step 6: Posting report to Confluence...
✅ Report posted to Confluence: https://your-domain.atlassian.net/wiki/...

🔀 Step 7: Creating GitHub pull request...
✅ Pull request created: https://github.com/your-username/your-repo/pull/42

🎉 Workflow completed successfully!

═══════════════════════════════════════════════════════════
               EXECUTION SUMMARY
═══════════════════════════════════════════════════════════
Project: TEST
Status: PARTIAL_SUCCESS
User Stories: 3
Tests Generated: 3
Tests Added: 3
Tests Run: 7
Tests Passed: 6 ✅
Tests Failed: 1 ❌
Pass Rate: 85.71%
Duration: 52.34s
Confluence Report: https://your-domain.atlassian.net/wiki/...
GitHub PR: https://github.com/your-username/your-repo/pull/42
═══════════════════════════════════════════════════════════
```

### Confluence Report

```
Test Execution Report
[SUCCESS - Green badge]

Summary
• Project: TEST
• User Stories: 3
• Tests Generated: 3
• Tests Added: 3
• Tests Run: 7
• Tests Passed: 6
• Tests Failed: 1
• Pass Rate: 85.71%

Execution Time
Started: 2024-01-15 14:30:00 UTC
Ended: 2024-01-15 14:31:32 UTC
Duration: 52.34 seconds

Links
• GitHub Pull Request: https://github.com/...
```

### GitHub Pull Request

```
Title: Test Suite Update - TEST - 2024-01-15

## Test Suite Update Report

**Project:** TEST
**Status:** PARTIAL_SUCCESS
**Execution Time:** 52.34 seconds

### Summary
- User Stories: 3
- Tests Generated: 3
- Tests Added: 3
- Tests Run: 7
- Tests Passed: ✅ 6
- Tests Failed: ❌ 1
- Pass Rate: 85.71%

### Details
- Started: 2024-01-15 14:30:00 UTC
- Ended: 2024-01-15 14:31:32 UTC
- Confluence Report: https://...

---
*This PR was automatically generated by the Jira-GitHub-Confluence Integration Agent*
```

---

## 🔄 Automation Scenarios

### Daily Automated Tests

```powershell
# Windows Task Scheduler
# Run daily at 8 AM
dotnet run --project SampleProject/
```

### Triggered by Jira Status Change

```csharp
// Webhook triggered when story status = "Ready for Testing"
[HttpPost("/api/jira-webhook")]
public async Task HandleWebhook()
{
    await runner.RunWorkflowAsync("TEST");
}
```

### Sprint Planning

```csharp
// Before sprint starts
await runner.RunWorkflowAsync("TEST", 
    "sprint = 'Current Sprint' AND type = Story");
```

### Continuous Integration

```yaml
# GitHub Actions workflow
name: Test Generation
on: [schedule: "0 9 * * *"]
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Run Integration Agent
        run: dotnet run
```

---

## 📈 Benefits

✅ **Automation** - No manual test creation
✅ **Visibility** - Real-time Confluence reports
✅ **Integration** - Automatic GitHub PRs
✅ **Metrics** - Detailed pass/fail tracking
✅ **Speed** - Generate tests in minutes
✅ **Quality** - Consistent test structure
✅ **Traceability** - Link tests to stories
✅ **Collaboration** - Team can review PRs

---

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| Jira connection fails | Check token, URL, network access |
| GitHub PR not created | Verify token has `repo` scope |
| Confluence post fails | Check page ID, space key, token |
| Tests not found | Verify feature file syntax |
| Build fails | Run `dotnet build` for details |

---

## 📚 Documentation

| File | Content |
|------|---------|
| `AGENT_DOCUMENTATION.md` | Complete 50+ page guide |
| `QUICK_START.md` | 5-minute setup |
| `SETUP_SUMMARY.md` | Configuration details |
| Agent code | Fully commented |

---

## ✨ Key Features

- ✅ Automatic test generation from Jira stories
- ✅ BDD Gherkin scenario creation
- ✅ Step definition generation
- ✅ Automatic test execution
- ✅ Metrics collection (pass/fail/time)
- ✅ Confluence report posting
- ✅ GitHub pull request creation
- ✅ Error handling and reporting
- ✅ Async operations
- ✅ Configurable settings
- ✅ Extensible architecture
- ✅ Comprehensive logging

---

## 🎉 Ready to Go!

Your agent is:
✅ Fully implemented
✅ Well documented
✅ Properly configured
✅ Ready to run

**Start now:**

```powershell
cd SampleProject
dotnet build
dotnet run
```

---

## 🤝 Support

1. Check `AGENT_DOCUMENTATION.md` for detailed help
2. Review code comments for implementation
3. Check Confluence reports for execution info
4. Review GitHub PRs for test details

---

## 📞 Next Steps

1. Update configuration with your details
2. Test with: `dotnet run`
3. Review Confluence report
4. Check GitHub PR
5. Merge PR when ready
6. Schedule for automation

---

**🚀 Welcome to Automated Testing at Scale!**

Your testing workflow is now:
- Fully automated
- Fully visible
- Fully integrated
- Fully reportable

*Happy Testing!* 🎉
