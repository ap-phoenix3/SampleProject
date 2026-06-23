using Reqnroll;
using SampleProject.Driver;
using SampleProject.Config;

namespace SampleProject.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Console.WriteLine($"========================================");
            Console.WriteLine($"Starting scenario: {_scenarioContext.ScenarioInfo.Title}");
            Console.WriteLine($"Environment: {ConfigReader.Instance.CurrentEnvironment}");
            Console.WriteLine($"Browser: {ConfigReader.Instance.BrowserType}");
            Console.WriteLine($"========================================");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine($"========================================");
            Console.WriteLine($"Finishing scenario: {_scenarioContext.ScenarioInfo.Title}");
            Console.WriteLine($"Scenario Status: {_scenarioContext.ScenarioExecutionStatus}");
            Console.WriteLine($"========================================");

            // Cleanup: Close browser if initialized
            if (DriverFactory.IsDriverInitialized)
            {
                try
                {
                    // Take screenshot on failure if configured
                    if (_scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError 
                        && ConfigReader.Instance.ScreenshotOnFailure)
                    {
                        var screenshotPath = Path.Combine(ConfigReader.Instance.ScreenshotPath);
                        if (!Directory.Exists(screenshotPath))
                            Directory.CreateDirectory(screenshotPath);

                        var driver = DriverFactory.GetDriver();
                        var screenshot = ((OpenQA.Selenium.ITakesScreenshot)driver).GetScreenshot();
                        var fileName = Path.Combine(screenshotPath, 
                            $"failure_{_scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                        screenshot.SaveAsFile(fileName);
                        Console.WriteLine($"Screenshot saved on failure: {fileName}");
                    }

                    DriverFactory.CloseDriver();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during cleanup: {ex.Message}");
                }
            }
        }

        [BeforeScenario(Order = 1)]
        public void LogEnvironmentInfo()
        {
            var config = ConfigReader.Instance;
            Console.WriteLine($"Configuration Details:");
            Console.WriteLine($"  Web URL: {config.WebUrl}");
            Console.WriteLine($"  API URL: {config.ApiUrl}");
            Console.WriteLine($"  Implicit Wait: {config.ImplicitWait}s");
            Console.WriteLine($"  Explicit Wait: {config.ExplicitWait}s");
            Console.WriteLine($"  Headless: {config.Headless}");
        }
    }
}
