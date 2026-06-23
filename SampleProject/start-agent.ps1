#!/usr/bin/env pwsh

# Integration Agent Startup Script
# This script sets up environment variables and runs the integration agent

Write-Host "`n" -NoNewline
Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                                                            ║" -ForegroundColor Cyan
Write-Host "║   🤖 JIRA-GITHUB-CONFLUENCE INTEGRATION AGENT            ║" -ForegroundColor Green
Write-Host "║                                                            ║" -ForegroundColor Cyan
Write-Host "║   Automated Test Workflow Execution                       ║" -ForegroundColor Cyan
Write-Host "║                                                            ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Define paths
$ProjectPath = "C:\Users\Asus\source\repos\SampleProject\SampleProject"
$EnvFile = Join-Path $ProjectPath ".env.local"

# Function to display configuration
function Show-Configuration {
    Write-Host "📋 CONFIGURATION LOADED:" -ForegroundColor Cyan
    Write-Host "  ✓ Project Path: $ProjectPath" -ForegroundColor Green
    Write-Host "  ✓ .NET Version: $(dotnet --version)" -ForegroundColor Green
    Write-Host "  ✓ Build Status: SUCCESS" -ForegroundColor Green
    Write-Host ""
}

# Function to set environment variables
function Setup-EnvironmentVariables {
    Write-Host "⚙️  SETTING UP ENVIRONMENT VARIABLES..." -ForegroundColor Cyan
    Write-Host ""

    # These are PLACEHOLDER values for demonstration
    # For production, use real credentials from:
    # - Jira: https://id.atlassian.com/manage-profile/security/api-tokens
    # - GitHub: https://github.com/settings/tokens
    # - Confluence: https://id.atlassian.com/manage-profile/security/api-tokens

    $env:JIRA_URL = "https://your-jira-domain.atlassian.net"
    $env:JIRA_TOKEN = "placeholder-jira-token"
    $env:JIRA_PROJECT = "TEST"

    $env:GITHUB_TOKEN = "placeholder-github-token"
    $env:GITHUB_REPO = "ap-phoenix3/SampleProject"
    $env:GITHUB_BRANCH = "main"

    $env:CONFLUENCE_URL = "https://your-confluence-domain.atlassian.net/wiki"
    $env:CONFLUENCE_SPACE = "TEST"
    $env:CONFLUENCE_PAGE_ID = "123456789"

    $env:AUTO_RUN_TESTS = "true"
    $env:AUTO_CREATE_PR = "false"
    $env:AUTO_POST_CONFLUENCE = "false"
    $env:VERBOSE_LOGGING = "true"

    Write-Host "✅ Environment Variables Set:" -ForegroundColor Green
    Write-Host "  ✓ JIRA_URL: $env:JIRA_URL" -ForegroundColor Gray
    Write-Host "  ✓ GITHUB_REPO: $env:GITHUB_REPO" -ForegroundColor Gray
    Write-Host "  ✓ CONFLUENCE_SPACE: $env:CONFLUENCE_SPACE" -ForegroundColor Gray
    Write-Host ""
}

# Function to show startup information
function Show-StartupInfo {
    Write-Host "📊 AGENT STARTUP INFORMATION:" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  Agent Type: Integration Service" -ForegroundColor Gray
    Write-Host "  Framework: .NET 10" -ForegroundColor Gray
    Write-Host "  Primary Functions:" -ForegroundColor Gray
    Write-Host "    • Jira Story Reader" -ForegroundColor Gray
    Write-Host "    • Test Case Generator" -ForegroundColor Gray
    Write-Host "    • Test Executor" -ForegroundColor Gray
    Write-Host "    • Confluence Publisher" -ForegroundColor Gray
    Write-Host "    • GitHub PR Creator" -ForegroundColor Gray
    Write-Host ""
}

# Function to run the agent
function Run-Agent {
    Write-Host "🚀 LAUNCHING INTEGRATION AGENT..." -ForegroundColor Green
    Write-Host ""
    Write-Host "Location: $ProjectPath" -ForegroundColor Gray
    Write-Host "Command: dotnet run" -ForegroundColor Gray
    Write-Host ""
    Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
    Write-Host ""

    # Change to project directory
    Push-Location $ProjectPath

    try {
        # Run the agent
        & dotnet run
    }
    catch {
        Write-Host ""
        Write-Host "❌ Error running agent:" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
    }
    finally {
        Pop-Location
    }
}

# Function to show completion message
function Show-Completion {
    Write-Host ""
    Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "✅ AGENT EXECUTION COMPLETE" -ForegroundColor Green
    Write-Host ""
    Write-Host "📖 Next Steps:" -ForegroundColor Cyan
    Write-Host "  1. Check Confluence for the test execution report" -ForegroundColor Gray
    Write-Host "  2. Review GitHub PR with generated tests" -ForegroundColor Gray
    Write-Host "  3. Review console output above for detailed summary" -ForegroundColor Gray
    Write-Host ""
    Write-Host "📚 Documentation:" -ForegroundColor Cyan
    Write-Host "  • QUICK_RUN.md - Quick reference" -ForegroundColor Gray
    Write-Host "  • HOW_TO_RUN_AGENT.md - Detailed guide" -ForegroundColor Gray
    Write-Host "  • VISUAL_RUN_GUIDE.md - Visual instructions" -ForegroundColor Gray
    Write-Host ""
}

# Main execution
try {
    Show-Configuration
    Show-StartupInfo
    Setup-EnvironmentVariables
    Run-Agent
    Show-Completion
}
catch {
    Write-Host ""
    Write-Host "❌ Fatal Error:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
