using OpenQA.Selenium;
using SampleProject.Driver;

namespace SampleProject.Pages
{
    /// <summary>
    /// Base class for all page objects
    /// </summary>
    public class BasePage
    {
        protected IWebDriver _driver;
        protected WaitHelper _waitHelper;

        public BasePage()
        {
            _driver = DriverFactory.GetDriver();
            _waitHelper = new WaitHelper(_driver);
        }

        /// <summary>
        /// Navigate to a specific URL
        /// </summary>
        public void Navigate(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Get the current page title
        /// </summary>
        public string GetPageTitle()
        {
            return _driver.Title;
        }

        /// <summary>
        /// Get the current URL
        /// </summary>
        public string GetCurrentUrl()
        {
            return _driver.Url;
        }

        /// <summary>
        /// Refresh the page
        /// </summary>
        public void RefreshPage()
        {
            _driver.Navigate().Refresh();
        }

        /// <summary>
        /// Wait for page to load
        /// </summary>
        public void WaitForPageLoad()
        {
            _waitHelper.WaitForPageLoad();
        }

        /// <summary>
        /// Find element and wait for it to be visible
        /// </summary>
        protected IWebElement FindElement(By locator)
        {
            return _waitHelper.WaitForElementVisible(locator);
        }

        /// <summary>
        /// Find element and wait for it to be clickable
        /// </summary>
        protected IWebElement FindClickableElement(By locator)
        {
            return _waitHelper.WaitForElementClickable(locator);
        }

        /// <summary>
        /// Click on an element
        /// </summary>
        protected void ClickElement(By locator)
        {
            var element = FindClickableElement(locator);
            element.Click();
        }

        /// <summary>
        /// Enter text in a field
        /// </summary>
        protected void EnterText(By locator, string text)
        {
            var element = FindElement(locator);
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        /// Get text from an element
        /// </summary>
        protected string GetText(By locator)
        {
            var element = FindElement(locator);
            return element.Text;
        }

        /// <summary>
        /// Get attribute value from an element
        /// </summary>
        protected string GetAttribute(By locator, string attributeName)
        {
            var element = FindElement(locator);
            return element.GetAttribute(attributeName);
        }

        /// <summary>
        /// Check if element is displayed
        /// </summary>
        protected bool IsElementDisplayed(By locator)
        {
            try
            {
                return FindElement(locator).Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if element is enabled
        /// </summary>
        protected bool IsElementEnabled(By locator)
        {
            try
            {
                return FindElement(locator).Enabled;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Wait for element to be invisible
        /// </summary>
        protected bool WaitForElementToDisappear(By locator)
        {
            return _waitHelper.WaitForElementInvisible(locator);
        }

        /// <summary>
        /// Take screenshot
        /// </summary>
        public void TakeScreenshot(string fileName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");

                if (!Directory.Exists(screenshotPath))
                    Directory.CreateDirectory(screenshotPath);

                var filePath = Path.Combine(screenshotPath, $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(filePath);
                Console.WriteLine($"Screenshot saved: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error taking screenshot: {ex.Message}");
            }
        }

        /// <summary>
        /// Execute JavaScript
        /// </summary>
        protected object ExecuteJavaScript(string script, params object[] args)
        {
            var jsExecutor = (IJavaScriptExecutor)_driver;
            return jsExecutor.ExecuteScript(script, args);
        }

        /// <summary>
        /// Scroll element into view
        /// </summary>
        protected void ScrollToElement(By locator)
        {
            var element = _driver.FindElement(locator);
            ExecuteJavaScript("arguments[0].scrollIntoView(true);", element);
        }
    }
}
