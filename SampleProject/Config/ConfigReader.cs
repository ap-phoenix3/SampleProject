using Microsoft.Extensions.Configuration;
using System;

namespace SampleProject.Config
{
    /// <summary>
    /// Reads and manages configuration from appsettings.json
    /// </summary>
    public class ConfigReader
    {
        private static ConfigReader? _instance;
        private readonly IConfiguration _configuration;
        private static readonly object _lockObject = new object();

        private ConfigReader()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "appsettings.json");

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Configuration file not found at: {configPath}");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(Path.Combine("Config", "appsettings.json"), optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        /// <summary>
        /// Gets the singleton instance of ConfigReader
        /// </summary>
        public static ConfigReader Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConfigReader();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the current environment (QA, UAT, Production)
        /// </summary>
        public string CurrentEnvironment => _configuration["Environment:Current"] ?? "QA";

        /// <summary>
        /// Gets the web URL for the current environment
        /// </summary>
        public string WebUrl
        {
            get
            {
                var env = CurrentEnvironment;
                var url = _configuration[$"Environment:Environments:{env}:WebUrl"];
                if (string.IsNullOrEmpty(url))
                    throw new InvalidOperationException($"WebUrl not configured for environment: {env}");
                return url;
            }
        }

        /// <summary>
        /// Gets the API URL for the current environment
        /// </summary>
        public string ApiUrl
        {
            get
            {
                var env = CurrentEnvironment;
                var url = _configuration[$"Environment:Environments:{env}:ApiUrl"];
                if (string.IsNullOrEmpty(url))
                    throw new InvalidOperationException($"ApiUrl not configured for environment: {env}");
                return url;
            }
        }

        /// <summary>
        /// Gets the timeout for the current environment
        /// </summary>
        public int Timeout
        {
            get
            {
                var env = CurrentEnvironment;
                var timeout = _configuration[$"Environment:Environments:{env}:Timeout"];
                return int.TryParse(timeout, out var result) ? result : 30;
            }
        }

        /// <summary>
        /// Gets the browser type (Chrome, Edge, Firefox)
        /// </summary>
        public string BrowserType => _configuration["Browser:BrowserType"] ?? "Chrome";

        /// <summary>
        /// Gets whether to run browser in headless mode
        /// </summary>
        public bool Headless => bool.TryParse(_configuration["Browser:Headless"], out var result) ? result : false;

        /// <summary>
        /// Gets the window size (e.g., "1920,1080")
        /// </summary>
        public string WindowSize => _configuration["Browser:WindowSize"] ?? "1920,1080";

        /// <summary>
        /// Gets the implicit wait time in seconds
        /// </summary>
        public int ImplicitWait => int.TryParse(_configuration["Browser:ImplicitWait"], out var result) ? result : 10;

        /// <summary>
        /// Gets the explicit wait time in seconds
        /// </summary>
        public int ExplicitWait => int.TryParse(_configuration["Browser:ExplicitWait"], out var result) ? result : 20;

        /// <summary>
        /// Gets whether to take screenshots on failure
        /// </summary>
        public bool ScreenshotOnFailure => bool.TryParse(_configuration["Browser:ScreenshotOnFailure"], out var result) ? result : true;

        /// <summary>
        /// Gets the screenshot path
        /// </summary>
        public string ScreenshotPath => _configuration["Browser:ScreenshotPath"] ?? "./Screenshots/";

        /// <summary>
        /// Gets browser-specific arguments
        /// </summary>
        public string[] GetBrowserArguments()
        {
            var browserType = BrowserType;
            var key = $"Browser.{browserType}:Arguments";
            var section = _configuration.GetSection(key);

            if (section.Exists())
            {
                var arguments = new List<string>();
                foreach (var item in section.GetChildren())
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        arguments.Add(item.Value);
                    }
                }
                return arguments.ToArray();
            }

            return Array.Empty<string>();
        }

        /// <summary>
        /// Gets browser binary path if configured
        /// </summary>
        public string? GetBrowserBinaryPath()
        {
            var browserType = BrowserType;
            var path = _configuration[$"Browser.{browserType}:BinaryPath"];
            return string.IsNullOrEmpty(path) ? null : path;
        }

        /// <summary>
        /// Resets the singleton instance (useful for testing)
        /// </summary>
        public static void Reset()
        {
            lock (_lockObject)
            {
                _instance = null;
            }
        }
    }
}
