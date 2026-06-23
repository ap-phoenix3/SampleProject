using System;

namespace SampleProject.Integration.Configuration
{
    /// <summary>
    /// Configuration for the Jira-GitHub-Confluence Integration Agent
    /// </summary>
    public class IntegrationAgentConfig
    {
        /// <summary>
        /// Jira configuration
        /// </summary>
        public JiraConfig Jira { get; set; }

        /// <summary>
        /// GitHub configuration
        /// </summary>
        public GitHubConfig GitHub { get; set; }

        /// <summary>
        /// Confluence configuration
        /// </summary>
        public ConfluenceConfig Confluence { get; set; }

        /// <summary>
        /// Agent configuration
        /// </summary>
        public AgentSettings Agent { get; set; }

        public IntegrationAgentConfig()
        {
            Jira = new JiraConfig();
            GitHub = new GitHubConfig();
            Confluence = new ConfluenceConfig();
            Agent = new AgentSettings();
        }
    }

    /// <summary>
    /// Jira configuration
    /// </summary>
    public class JiraConfig
    {
        /// <summary>
        /// Jira base URL (e.g., https://your-domain.atlassian.net)
        /// </summary>
        public string BaseUrl { get; set; } = "https://your-domain.atlassian.net";

        /// <summary>
        /// Jira API token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Email associated with Jira account
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Default JQL query to fetch issues
        /// </summary>
        public string DefaultJQL { get; set; } = "project = TEST AND type = Story AND status != Done";

        /// <summary>
        /// Jira project key
        /// </summary>
        public string ProjectKey { get; set; } = "TEST";
    }

    /// <summary>
    /// GitHub configuration
    /// </summary>
    public class GitHubConfig
    {
        /// <summary>
        /// GitHub personal access token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// GitHub repository (format: owner/repo)
        /// </summary>
        public string Repository { get; set; } = "owner/automation-tests";

        /// <summary>
        /// Base branch for pull requests
        /// </summary>
        public string BaseBranch { get; set; } = "main";

        /// <summary>
        /// Feature branch for test changes
        /// </summary>
        public string FeatureBranch { get; set; } = "test-automation";

        /// <summary>
        /// GitHub API URL
        /// </summary>
        public string ApiBaseUrl { get; set; } = "https://api.github.com";
    }

    /// <summary>
    /// Confluence configuration
    /// </summary>
    public class ConfluenceConfig
    {
        /// <summary>
        /// Confluence base URL (e.g., https://your-domain.atlassian.net/wiki)
        /// </summary>
        public string BaseUrl { get; set; } = "https://your-domain.atlassian.net/wiki";

        /// <summary>
        /// Confluence space key
        /// </summary>
        public string SpaceKey { get; set; } = "TEST";

        /// <summary>
        /// Confluence page ID where reports will be posted
        /// </summary>
        public string PageId { get; set; }

        /// <summary>
        /// Email associated with Confluence account
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// API token for Confluence
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// Agent settings
    /// </summary>
    public class AgentSettings
    {
        /// <summary>
        /// Whether to automatically run tests after generation
        /// </summary>
        public bool AutoRunTests { get; set; } = true;

        /// <summary>
        /// Whether to automatically create GitHub PR
        /// </summary>
        public bool AutoCreatePR { get; set; } = true;

        /// <summary>
        /// Whether to automatically post to Confluence
        /// </summary>
        public bool AutoPostConfluence { get; set; } = true;

        /// <summary>
        /// Enable detailed logging
        /// </summary>
        public bool VerboseLogging { get; set; } = true;

        /// <summary>
        /// Test execution timeout in seconds
        /// </summary>
        public int TestExecutionTimeout { get; set; } = 300;

        /// <summary>
        /// Maximum number of test cases to generate per user story
        /// </summary>
        public int MaxTestCasesPerStory { get; set; } = 5;
    }
}
