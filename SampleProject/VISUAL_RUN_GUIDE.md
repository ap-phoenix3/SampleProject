# 🎬 Visual Step-by-Step Guide to Run Your Agent

## Screen-by-Screen Instructions

### SCREEN 1: Visual Studio

```
┌─────────────────────────────────────────────────────┐
│  Visual Studio Community 2026                       │
├─────────────────────────────────────────────────────┤
│                                                      │
│  File  Edit  View  Project  Build  Debug  Tools     │
│                                                      │
│  [Project Explorer showing SampleProject]           │
│                                                      │
│                                                      │
│  👇 ACTION: Press Ctrl + `                          │
│  (This opens the terminal)                          │
│                                                      │
└─────────────────────────────────────────────────────┘
```

### SCREEN 2: Terminal Opens

```
┌─────────────────────────────────────────────────────┐
│  Visual Studio - Terminal Opened                    │
├─────────────────────────────────────────────────────┤
│                                                      │
│  PS C:\Users\Asus\source\repos\SampleProject>       │
│                                                      │
│  👇 ACTION: Type this:                              │
│  cd SampleProject                                   │
│  (Press Enter)                                      │
│                                                      │
└─────────────────────────────────────────────────────┘
```

### SCREEN 3: Navigate to Project

```
┌─────────────────────────────────────────────────────┐
│  Terminal with Project Path                         │
├─────────────────────────────────────────────────────┤
│                                                      │
│  PS C:\Users\Asus\source\repos\SampleProject>       │
│  cd SampleProject                                   │
│                                                      │
│  PS C:\Users\Asus\source\repos\SampleProject\       │
│  SampleProject>                                     │
│                                                      │
│  👇 ACTION: Paste this entire command:              │
│  (Copy the credentials command below)               │
│                                                      │
└─────────────────────────────────────────────────────┘
```

### SCREEN 4: Set Credentials

```
┌─────────────────────────────────────────────────────┐
│  Setting Environment Variables                      │
├─────────────────────────────────────────────────────┤
│                                                      │
│  PS SampleProject> $env:JIRA_URL="https://your..."  │
│                                                      │
│  PS SampleProject> $env:JIRA_TOKEN="your-token"     │
│                                                      │
│  PS SampleProject> $env:GITHUB_TOKEN="your-token"   │
│                                                      │
│  PS SampleProject> $env:GITHUB_REPO="owner/repo"    │
│                                                      │
│  PS SampleProject> $env:CONFLUENCE_URL="https://..." │
│                                                      │
│  PS SampleProject> $env:CONFLUENCE_SPACE="TEST"     │
│                                                      │
│  PS SampleProject> $env:CONFLUENCE_PAGE_ID="123"    │
│                                                      │
│  👇 ACTION: Press Enter after each line             │
│  OR paste entire block at once                      │
│                                                      │
└─────────────────────────────────────────────────────┘
```

### SCREEN 5: Run the Agent

```
┌─────────────────────────────────────────────────────┐
│  Running the Agent                                  │
├─────────────────────────────────────────────────────┤
│                                                      │
│  PS SampleProject> dotnet run                        │
│                                                      │
│  👇 ACTION: Press Enter                             │
│                                                      │
│  🚀 Starting Integration Agent...                   │
│                                                      │
│  Watch the progress appear below...                 │
│                                                      │
└─────────────────────────────────────────────────────┘
```

### SCREEN 6: Agent Executing

```
┌─────────────────────────────────────────────────────┐
│  Agent Execution in Progress                        │
├─────────────────────────────────────────────────────┤
│                                                      │
│  🚀 Starting Integration Agent...                   │
│                                                      │
│  ╔═══════════════════════════════════════════════╗  │
│  ║ Jira → Test Gen → Test Exec → GitHub →        ║  │
│  ║            Confluence                         ║  │
│  ╚═══════════════════════════════════════════════╝  │
│                                                      │
│  📋 Step 1: Fetching user stories from Jira...     │
│    📌 TEST-1: Story Title                           │
│    📌 TEST-2: Another Story                         │
│  ✅ Found 2 user stories                            │
│                                                      │
│  🧪 Step 2: Generating test cases...               │
│    🧪 Generated test for TEST-1                     │
│    🧪 Generated test for TEST-2                     │
│  ✅ Generated 2 test cases                          │
│                                                      │
│  📁 Step 3: Adding test cases to solution...        │
│  ✅ Added 2 test cases to solution                  │
│                                                      │
│  ▶️  Step 4: Running test cases...                  │
│  ✅ Test Results: 4 Passed, 0 Failed               │
│                                                      │
│  📊 Step 5: Generating execution report...          │
│  ✅ Report generated with status: SUCCESS           │
│                                                      │
│  🔗 Step 6: Posting report to Confluence...         │
│  ✅ Report posted to Confluence: https://...        │
│                                                      │
│  🔀 Step 7: Creating GitHub pull request...         │
│  ✅ Pull request created: https://github.com/...    │
│                                                      │
│  🎉 Workflow completed successfully!                │
│                                                      │
└─────────────────────────────────────────────────────┘
```

### SCREEN 7: Final Summary

```
┌─────────────────────────────────────────────────────┐
│  Agent Execution Complete - Summary                 │
├─────────────────────────────────────────────────────┤
│                                                      │
│  ═══════════════════════════════════════════════    │
│              EXECUTION SUMMARY                      │
│  ═══════════════════════════════════════════════    │
│  Project: TEST                                      │
│  Status: SUCCESS ✅                                 │
│  User Stories: 2                                    │
│  Tests Generated: 2                                 │
│  Tests Added: 2                                     │
│  Tests Run: 4                                       │
│  Tests Passed: 4 ✅                                 │
│  Tests Failed: 0 ❌                                 │
│  Pass Rate: 100%                                    │
│  Duration: 45.32s                                   │
│  Confluence Report: https://...                     │
│  GitHub PR: https://github.com/.../pull/123         │
│  ═══════════════════════════════════════════════    │
│                                                      │
│  ✅ Agent completed successfully!                   │
│                                                      │
│  👉 Check your:                                     │
│     • Confluence page for detailed report            │
│     • GitHub PR with generated tests                 │
│     • Console output above                           │
│                                                      │
└─────────────────────────────────────────────────────┘
```

---

## 🎯 The Command You Need to Copy and Paste

### Replace these VALUES with your actual credentials:

```powershell
$env:JIRA_URL="https://your-jira-domain.atlassian.net"; `
$env:JIRA_TOKEN="your-actual-jira-api-token"; `
$env:GITHUB_TOKEN="your-actual-github-personal-access-token"; `
$env:GITHUB_REPO="your-github-username/your-repo-name"; `
$env:CONFLUENCE_URL="https://your-confluence-domain.atlassian.net/wiki"; `
$env:CONFLUENCE_SPACE="YOUR_SPACE_KEY"; `
$env:CONFLUENCE_PAGE_ID="your-page-id"; `
dotnet run
```

---

## 📝 Getting Your Actual Credentials

### 1. JIRA_URL
Your Jira domain URL
- Format: `https://[company].atlassian.net`
- Example: `https://anuragpanda.atlassian.net`
- ✅ No trailing slash

### 2. JIRA_TOKEN
Your Jira API Token
- Go to: https://id.atlassian.com/manage-profile/security/api-tokens
- Click: "Create API token"
- Copy the entire token
- Format: `ATATT3xFfGF0fswUi...` (very long string)
- ✅ Keep it secret!

### 3. GITHUB_TOKEN
Your GitHub Personal Access Token
- Go to: https://github.com/settings/tokens
- Click: "Generate new token (classic)"
- Scopes: Select `repo` and `workflow`
- Copy immediately
- Format: `ghp_NqFzloSavZpD9eurj1g...` (starts with ghp_)
- ✅ Can't see it again after creating!

### 4. GITHUB_REPO
Your GitHub Repository
- From your repo URL: `https://github.com/ap-phoenix3/SampleProject`
- Value: `ap-phoenix3/SampleProject`
- Format: `username/reponame`
- ✅ No https:// or .git

### 5. CONFLUENCE_URL
Your Confluence domain
- Format: `https://[company].atlassian.net/wiki`
- Example: `https://anuragpanda.atlassian.net/wiki`
- ✅ Must have /wiki at the end

### 6. CONFLUENCE_SPACE
Your Confluence Space Key
- From URL: `https://domain.atlassian.net/wiki/spaces/BDD/overview`
- Key: `BDD`
- ✅ Usually 2-10 characters, UPPERCASE

### 7. CONFLUENCE_PAGE_ID
Your Confluence Page ID
- From URL: `https://domain.atlassian.net/wiki/spaces/BDD/pages/327792`
- Page ID: `327792`
- ✅ Just the number at the end

---

## ✅ Complete Example

Here's what it looks like with SAMPLE values (replace with YOUR actual credentials):

```powershell
$env:JIRA_URL="https://mycompany.atlassian.net"; `
$env:JIRA_TOKEN="your-jira-api-token-here"; `
$env:GITHUB_TOKEN="your-github-personal-access-token-here"; `
$env:GITHUB_REPO="ap-phoenix3/SampleProject"; `
$env:CONFLUENCE_URL="https://mycompany.atlassian.net/wiki"; `
$env:CONFLUENCE_SPACE="BDD"; `
$env:CONFLUENCE_PAGE_ID="327792"; `
dotnet run
```

⚠️ **NEVER use real tokens in examples or documentation!**

---

## 🎬 Your Exact Steps

1. **Open Visual Studio**
2. **Press Ctrl + `** (opens terminal)
3. **Type:** `cd SampleProject` → Press Enter
4. **Paste:** The entire credentials command above (with YOUR values)
5. **Press:** Enter
6. **Watch:** The agent run and complete!
7. **Check:**
   - Console for summary
   - Confluence for detailed report
   - GitHub for PR

---

## ✨ That's It!

Your agent will:
✅ Read your Jira stories
✅ Generate tests
✅ Run tests
✅ Post results to Confluence
✅ Create a GitHub PR

**All automatically!** 🚀

Go ahead and run it now! 🎉
