# 🚀 How to Run Your Integration Agent - Complete Guide

## ⚡ Quick Start (2 Minutes)

### Method 1: Using Visual Studio Terminal (Easiest)

```
1. Press Ctrl + ` (backtick) to open terminal in Visual Studio
2. Terminal opens at: C:\Users\Asus\source\repos\SampleProject\
3. Copy and paste this command:

cd SampleProject
$env:JIRA_URL="https://your-domain.atlassian.net"; $env:JIRA_TOKEN="your-token"; $env:GITHUB_TOKEN="your-token"; $env:GITHUB_REPO="owner/repo"; $env:CONFLUENCE_URL="https://your-domain.atlassian.net/wiki"; $env:CONFLUENCE_SPACE="TEST"; $env:CONFLUENCE_PAGE_ID="123456789"; dotnet run
```

That's it! The agent will start running.

---

## 📍 Step-by-Step Instructions

### Step 1: Open Terminal in Visual Studio

**Option A: Keyboard Shortcut**
```
Press: Ctrl + `
```

**Option B: Menu**
```
View → Terminal
```

**Option C: Terminal Menu**
```
Terminal → New Terminal
```

### Step 2: Navigate to Project

The terminal should already be at: `C:\Users\Asus\source\repos\SampleProject\`

If not, type:
```powershell
cd C:\Users\Asus\source\repos\SampleProject\SampleProject
```

### Step 3: Configure Your Credentials

You need to set 7 environment variables. Copy this entire block and paste it:

```powershell
$env:JIRA_URL = "https://your-jira-domain.atlassian.net"
$env:JIRA_TOKEN = "your-actual-jira-token-here"
$env:GITHUB_TOKEN = "your-actual-github-token-here"
$env:GITHUB_REPO = "your-username/your-repo-name"
$env:CONFLUENCE_URL = "https://your-confluence-domain.atlassian.net/wiki"
$env:CONFLUENCE_SPACE = "YOUR_SPACE_KEY"
$env:CONFLUENCE_PAGE_ID = "your-page-id-number"
```

**Replace these values with YOUR actual credentials:**

| Variable | Where to Get It |
|----------|-----------------|
| `JIRA_URL` | Your Jira domain (e.g., `https://mycompany.atlassian.net`) |
| `JIRA_TOKEN` | From Jira → Profile → Security → API Tokens |
| `GITHUB_TOKEN` | From GitHub → Settings → Developer settings → Personal access tokens |
| `GITHUB_REPO` | From your GitHub repo URL (e.g., `ap-phoenix3/SampleProject`) |
| `CONFLUENCE_URL` | Your Confluence domain (e.g., `https://mycompany.atlassian.net/wiki`) |
| `CONFLUENCE_SPACE` | Your Confluence space key (e.g., `TEST`) |
| `CONFLUENCE_PAGE_ID` | Page ID number where you want reports posted |

### Step 4: Verify Environment Variables Are Set

Type this to check:
```powershell
Get-ChildItem env: | Where-Object {$_.Name -like "JIRA*" -or $_.Name -like "GITHUB*" -or $_.Name -like "CONFLUENCE*"}
```

You should see all 7 variables with their values.

### Step 5: Run the Agent

```powershell
dotnet run
```

Press Enter and watch it execute!

---

## 🎯 What You'll See

When the agent runs successfully, you'll see:

```
🚀 Starting Integration Agent...

╔════════════════════════════════════════════════════════════╗
║  Jira → Test Generation → Test Execution → GitHub → Confluence
║                    INTEGRATION WORKFLOW
╚════════════════════════════════════════════════════════════╝

📋 Step 1: Fetching user stories from Jira...
  📌 PROJ-1: Your first story
  📌 PROJ-2: Your second story
✅ Found 2 user stories

🧪 Step 2: Generating test cases from user stories...
  🧪 Generated test for PROJ-1
  🧪 Generated test for PROJ-2
✅ Generated 2 test cases

📁 Step 3: Adding test cases to solution...
  ✅ Added PROJ-1 to solution
  ✅ Added PROJ-2 to solution
✅ Added 2 test cases to solution

▶️  Step 4: Running test cases...
✅ Test Results: 4 Passed, 0 Failed

📊 Step 5: Generating execution report...
✅ Report generated with status: SUCCESS

🔗 Step 6: Posting report to Confluence...
✅ Report posted to Confluence: https://...

🔀 Step 7: Creating GitHub pull request...
✅ Pull request created: https://github.com/.../pull/123

🎉 Workflow completed successfully!

═══════════════════════════════════════════════════════════
               EXECUTION SUMMARY
═══════════════════════════════════════════════════════════
Project: PROJ
Status: SUCCESS
User Stories: 2
Tests Generated: 2
Tests Added: 2
Tests Run: 4
Tests Passed: 4 ✅
Tests Failed: 0 ❌
Pass Rate: 100%
Duration: 45.32s
Confluence Report: https://your-confluence.atlassian.net/...
GitHub PR: https://github.com/your-username/your-repo/pull/123
═══════════════════════════════════════════════════════════
```

---

## 🔧 Complete One-Line Command

If you want to run it all at once, copy this entire command:

```powershell
$env:JIRA_URL="https://your-jira-domain.atlassian.net"; $env:JIRA_TOKEN="your-jira-token"; $env:GITHUB_TOKEN="your-github-token"; $env:GITHUB_REPO="your-username/your-repo"; $env:CONFLUENCE_URL="https://your-confluence-domain.atlassian.net/wiki"; $env:CONFLUENCE_SPACE="YOUR_SPACE"; $env:CONFLUENCE_PAGE_ID="your-page-id"; dotnet run
```

Just replace the credential values and paste it into the terminal.

---

## 📋 Getting Your Credentials

### Get Jira Token

1. Go to: https://id.atlassian.com/manage-profile/security/api-tokens
2. Click "Create API token"
3. Give it a name (e.g., "Integration Agent")
4. Copy the token
5. Set as: `$env:JIRA_TOKEN = "copied-token-here"`

### Get GitHub Token

1. Go to: https://github.com/settings/tokens
2. Click "Generate new token (classic)"
3. Name: "Integration Agent"
4. Select scopes: `repo`, `workflow`
5. Click "Generate token"
6. Copy immediately (you won't see it again)
7. Set as: `$env:GITHUB_TOKEN = "ghp_..."`

### Get Your Jira URL

Just your domain name, like:
- `https://mycompany.atlassian.net`
- `https://anuragpanda.atlassian.net`

### Get Your GitHub Repo

From your repo URL:
- URL: `https://github.com/ap-phoenix3/SampleProject`
- Repo: `ap-phoenix3/SampleProject`

### Get Confluence Details

1. Go to your Confluence space
2. Look at the URL: `https://domain.atlassian.net/wiki/spaces/TEST/pages/123456789`
3. Space Key: `TEST`
4. Page ID: `123456789`

---

## ❌ Troubleshooting

### Terminal Won't Open
- Try: `Ctrl + `` (backtick key)
- Or go to: View → Terminal

### Project Not Found
- Make sure you're in: `C:\Users\Asus\source\repos\SampleProject\SampleProject`
- Type: `pwd` to see current directory
- Type: `cd SampleProject` to navigate

### dotnet not found
```powershell
# Install .NET 10
# Download from: https://dotnet.microsoft.com/en-us/download

# Or verify installation:
dotnet --version
```

### Environment Variables Not Working
```powershell
# Verify they're set:
echo $env:JIRA_TOKEN

# If empty, set them again (they only persist for current session)
$env:JIRA_TOKEN = "your-token"
```

### Agent Fails to Connect
- ✅ Check all credentials are correct
- ✅ Test Jira connection manually:
```powershell
$headers = @{
    "Authorization" = "Bearer $env:JIRA_TOKEN"
    "Accept" = "application/json"
}
Invoke-RestMethod -Uri "$env:JIRA_URL/rest/api/3/myself" -Headers $headers
```

### Agent Hangs
- ✅ Wait 30 seconds (API calls can be slow)
- ✅ Press Ctrl+C to stop
- ✅ Check network connectivity
- ✅ Verify credentials

---

## 🎯 Full Example Walkthrough

### Starting Point
```
Open Visual Studio
```

### Step 1: Open Terminal
```
Press: Ctrl + `
```

### Step 2: See this in terminal
```
PS C:\Users\Asus\source\repos\SampleProject>
```

### Step 3: Navigate to project
```powershell
cd SampleProject
PS C:\Users\Asus\source\repos\SampleProject\SampleProject>
```

### Step 4: Set your actual credentials
```powershell
$env:JIRA_URL = "https://anuragpanda.atlassian.net"
$env:JIRA_TOKEN = "ATATT3xFfGF0fswUi5pP4D8ajcOD..."
$env:GITHUB_TOKEN = "ghp_NqFzloSavZpD9eurj1gNC0P4..."
$env:GITHUB_REPO = "ap-phoenix3/SampleProject"
$env:CONFLUENCE_URL = "https://anuragpanda.atlassian.net/wiki"
$env:CONFLUENCE_SPACE = "BDD"
$env:CONFLUENCE_PAGE_ID = "327792"
```

### Step 5: Run the agent
```powershell
dotnet run
```

### Step 6: Watch it execute
```
🚀 Starting Integration Agent...
📋 Step 1: Fetching user stories from Jira...
... (continues)
```

### Step 7: View results
- 📊 Console shows execution summary
- 📖 Confluence page gets updated
- 🔀 GitHub PR gets created

---

## 🔄 Running Again

The next time you want to run it:

```powershell
# Terminal is already open and at correct path
# Just set credentials again:
$env:JIRA_TOKEN = "your-token"; $env:GITHUB_TOKEN = "your-token"; $env:JIRA_URL = "your-url"; $env:GITHUB_REPO = "your-repo"; $env:CONFLUENCE_URL = "your-confluence"; $env:CONFLUENCE_SPACE = "your-space"; $env:CONFLUENCE_PAGE_ID = "your-page-id"; dotnet run
```

---

## 💾 Save Your Configuration

### Create a PowerShell Script

Create file: `run-agent.ps1`

```powershell
# Set your credentials
$env:JIRA_URL = "https://anuragpanda.atlassian.net"
$env:JIRA_TOKEN = "your-actual-token-here"
$env:GITHUB_TOKEN = "your-actual-token-here"
$env:GITHUB_REPO = "ap-phoenix3/SampleProject"
$env:CONFLUENCE_URL = "https://anuragpanda.atlassian.net/wiki"
$env:CONFLUENCE_SPACE = "BDD"
$env:CONFLUENCE_PAGE_ID = "327792"

Write-Host "🤖 Starting Integration Agent..." -ForegroundColor Green
Write-Host ""

cd C:\Users\Asus\source\repos\SampleProject\SampleProject
dotnet run

Write-Host ""
Write-Host "✅ Agent execution complete!" -ForegroundColor Green
```

### Run it anytime with:
```powershell
.\run-agent.ps1
```

---

## 🎉 You're Ready!

Now you can:

✅ Run the agent from VS terminal
✅ Automatically read Jira stories
✅ Generate tests
✅ Execute tests
✅ Post to Confluence
✅ Create GitHub PRs

**All with one command!** 🚀

---

## 📚 Next Steps

1. **Run the agent** using the steps above
2. **Check Confluence** for test execution report
3. **Review GitHub PR** with generated tests
4. **View test results** in console
5. **Customize** for your needs

---

**Ready? Let's go! 🎯**

Type in the terminal and watch the magic happen! ✨
