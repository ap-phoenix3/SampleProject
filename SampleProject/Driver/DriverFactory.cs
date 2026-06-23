using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SampleProject.Config;
using System;

namespace SampleProject.Driver
{
    /// <summary>
    /// Factory class to create and manage WebDriver instances
    /// </summary>
    public class DriverFactory
    {
        private static IWebDriver? _driver;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Gets or creates a WebDriver instance based on configuration
        /// </summary>
        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                lock (_lockObject)
                {
                    if (_driver == null)
                    {
                        _driver = CreateDriver();
                    }
                }
            }
            return _driver;
        }

        /// <summary>
        /// Creates a new WebDriver instance
        /// </summary>
        private static IWebDriver CreateDriver()
        {
            var config = ConfigReader.Instance;
            var browserType = config.BrowserType.ToLower();

            return browserType switch
            {
                "chrome" => CreateChromeDriver(),
                "edge" => CreateEdgeDriver(),
                "firefox" => CreateFirefoxDriver(),
                _ => throw new NotSupportedException($"Browser type '{browserType}' is not supported")
            };
        }

        /// <summary>
        /// Creates a Chrome WebDriver instance
        /// </summary>
        private static IWebDriver CreateChromeDriver()
        {
            var config = ConfigReader.Instance;
            var options = new ChromeOptions();

            // Add arguments
            var arguments = config.GetBrowserArguments();
            foreach (var arg in arguments)
            {
                options.AddArgument(arg);
            }

            // Set headless mode if configured
            if (config.Headless)
            {
                options.AddArgument("--headless=new");
            }

            // Set binary path if configured
            var binaryPath = config.GetBrowserBinaryPath();
            if (!string.IsNullOrEmpty(binaryPath))
            {
                options.BinaryLocation = binaryPath;
            }

            // Disable notifications
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");

            // Create driver with options
            var driver = new ChromeDriver(options);

            // Set window size
            var windowSize = config.WindowSize.Split(',');
            if (windowSize.Length == 2 && 
                int.TryParse(windowSize[0], out var width) && 
                int.TryParse(windowSize[1], out var height))
            {
                driver.Manage().Window.Size = new System.Drawing.Size(width, height);
            }

            // Set timeouts
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(config.ImplicitWait);

            Console.WriteLine($"Chrome driver initialized with configuration: {config.CurrentEnvironment}");
            return driver;
        }

        /// <summary>
        /// Creates an Edge WebDriver instance
        /// </summary>
        private static IWebDriver CreateEdgeDriver()
        {
            var config = ConfigReader.Instance;
            var options = new EdgeOptions();

            // Add arguments
            var arguments = config.GetBrowserArguments();
            foreach (var arg in arguments)
            {
                options.AddArgument(arg);
            }

            // Set headless mode if configured
            if (config.Headless)
            {
                options.AddArgument("--headless=new");
            }

            // Set binary path if configured
            var binaryPath = config.GetBrowserBinaryPath();
            if (!string.IsNullOrEmpty(binaryPath))
            {
                options.BinaryLocation = binaryPath;
            }

            // Disable notifications
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");

            var driver = new EdgeDriver(options);

            // Set window size
            var windowSize = config.WindowSize.Split(',');
            if (windowSize.Length == 2 && 
                int.TryParse(windowSize[0], out var width) && 
                int.TryParse(windowSize[1], out var height))
            {
                driver.Manage().Window.Size = new System.Drawing.Size(width, height);
            }

            // Set timeouts
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(config.ImplicitWait);

            Console.WriteLine($"Edge driver initialized with configuration: {config.CurrentEnvironment}");
            return driver;
        }

        /// <summary>
        /// Creates a Firefox WebDriver instance
        /// </summary>
        private static IWebDriver CreateFirefoxDriver()
        {
            var config = ConfigReader.Instance;
            var options = new FirefoxOptions();

            // Set headless mode if configured
            if (config.Headless)
            {
                options.AddArgument("--headless");
            }

            // Set binary path if configured
            var binaryPath = config.GetBrowserBinaryPath();
            if (!string.IsNullOrEmpty(binaryPath))
            {
                options.BinaryLocation = binaryPath;
            }

            var driver = new FirefoxDriver(options);

            // Set window size
            var windowSize = config.WindowSize.Split(',');
            if (windowSize.Length == 2 && 
                int.TryParse(windowSize[0], out var width) && 
                int.TryParse(windowSize[1], out var height))
            {
                driver.Manage().Window.Size = new System.Drawing.Size(width, height);
            }

            // Set timeouts
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(config.ImplicitWait);

            Console.WriteLine($"Firefox driver initialized with configuration: {config.CurrentEnvironment}");
            return driver;
        }

        /// <summary>
        /// Closes and disposes the WebDriver instance
        /// </summary>
        public static void CloseDriver()
        {
            if (_driver != null)
            {
                lock (_lockObject)
                {
                    if (_driver != null)
                    {
                        try
                        {
                            _driver.Quit();
                            Console.WriteLine("Driver closed successfully");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error closing driver: {ex.Message}");
                        }
                        finally
                        {
                            _driver.Dispose();
                            _driver = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if driver is currently initialized
        /// </summary>
        public static bool IsDriverInitialized => _driver != null;

        /// <summary>
        /// Resets driver (closes and clears reference)
        /// </summary>
        public static void ResetDriver()
        {
            CloseDriver();
            lock (_lockObject)
            {
                _driver = null;
            }
        }
    }
}
