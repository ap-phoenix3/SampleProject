# ⚡ Quick Reference - Run Your Agent in 30 Seconds

## 🎯 The Absolute Fastest Way

### Open Terminal in VS
```
Ctrl + `
```

### Type This (All In One)
```powershell
cd SampleProject; $env:JIRA_URL="https://your-jira"; $env:JIRA_TOKEN="your-token"; $env:GITHUB_TOKEN="your-token"; $env:GITHUB_REPO="owner/repo"; $env:CONFLUENCE_URL="https://your-confluence"; $env:CONFLUENCE_SPACE="SPACE"; $env:CONFLUENCE_PAGE_ID="12345"; dotnet run
```

**Replace the placeholders with YOUR actual values** ⬆️

---

## 🔑 Where to Get Your Credentials

| What | Where |
|-----|-------|
| `JIRA_URL` | Your Jira domain (e.g., `https://mycompany.atlassian.net`) |
| `JIRA_TOKEN` | https://id.atlassian.com/manage-profile/security/api-tokens → Create API token |
| `GITHUB_TOKEN` | https://github.com/settings/tokens → Generate new token (need `repo` + `workflow` scopes) |
| `GITHUB_REPO` | From your repo URL: `owner/repo` |
| `CONFLUENCE_URL` | Your Confluence domain + `/wiki` (e.g., `https://mycompany.atlassian.net/wiki`) |
| `CONFLUENCE_SPACE` | Your space key (e.g., `BDD`, `TEST`, `MYSPACE`) |
| `CONFLUENCE_PAGE_ID` | Page number from Confluence URL |

---

## 📋 Step-by-Step (If One-Liner Is Too Much)

```powershell
# 1. Navigate to project
cd SampleProject

# 2. Set each credential (copy-paste each line)
$env:JIRA_URL = "https://your-jira-domain.atlassian.net"
$env:JIRA_TOKEN = "your-jira-token"
$env:GITHUB_TOKEN = "your-github-token"
$env:GITHUB_REPO = "your-username/your-repo"
$env:CONFLUENCE_URL = "https://your-confluence-domain.atlassian.net/wiki"
$env:CONFLUENCE_SPACE = "YOUR_SPACE_KEY"
$env:CONFLUENCE_PAGE_ID = "your-page-id"

# 3. Run the agent
dotnet run
```

---

## ✅ You'll See This When It Works

```
🚀 Starting Integration Agent...

╔═══════════════════════════════════════════════════════════╗
║ Jira → Test Generation → Test Execution → GitHub → Confluence
╚═══════════════════════════════════════════════════════════╝

📋 Step 1: Fetching user stories from Jira...
✅ Found X user stories

🧪 Step 2: Generating test cases...
✅ Generated X test cases

📁 Step 3: Adding test cases to solution...
✅ Added X test cases

▶️  Step 4: Running test cases...
✅ Test Results: X Passed, Y Failed

📊 Step 5: Generating execution report...
✅ Report generated with status: SUCCESS

🔗 Step 6: Posting report to Confluence...
✅ Report posted to Confluence: https://...

🔀 Step 7: Creating GitHub pull request...
✅ Pull request created: https://github.com/.../pull/123

═══════════════════════════════════════════════════════════
               EXECUTION SUMMARY
═══════════════════════════════════════════════════════════
Status: SUCCESS ✅
Tests Run: 4
Tests Passed: 4 ✅
Tests Failed: 0 ❌
Pass Rate: 100%
Duration: 45.32s
═══════════════════════════════════════════════════════════
```

---

## ❌ Troubleshooting

| Problem | Solution |
|---------|----------|
| `dotnet: not found` | Install .NET 10 from https://dotnet.microsoft.com/download |
| `Can't find SampleProject` | Type: `cd SampleProject` |
| Connection fails | Check your credentials - copy them exactly |
| Agent hangs | Press Ctrl+C to stop, then try again |
| Environment variables empty | Set them again - they only persist for current session |

---

## 🎯 What Happens Next

✅ Agent reads your Jira stories
✅ Generates test files (`.feature` and `.cs`)
✅ Adds them to your `Features/` and `StepDefinitions/` folders
✅ Runs all tests
✅ Posts detailed report to Confluence
✅ Creates GitHub PR with generated tests

**All automatically!** 🚀

---

## 📚 Full Guides Available

- `HOW_TO_RUN_AGENT.md` - Detailed step-by-step
- `VISUAL_RUN_GUIDE.md` - Screen-by-screen with diagrams
- `WHY_NO_CHAT_AGENT.md` - Understanding the agent types
- `Integration/QUICK_START.md` - 5-minute setup
- `Integration/AGENT_DOCUMENTATION.md` - Complete reference

---

## 🎉 Ready?

1. Get your 7 credentials
2. Open terminal in VS (Ctrl + `)
3. Paste the command above
4. Watch the magic! ✨

**Go go go!** 🚀
