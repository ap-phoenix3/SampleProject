using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SampleProject.Driver
{
    /// <summary>
    /// Helper class for wait operations on web elements
    /// </summary>
    public class WaitHelper
    {
        private readonly IWebDriver _driver;
        private readonly int _explicitWaitTime;

        public WaitHelper(IWebDriver driver, int explicitWaitTime = 20)
        {
            _driver = driver;
            _explicitWaitTime = explicitWaitTime;
        }

        /// <summary>
        /// Waits for an element to be visible
        /// </summary>
        public IWebElement WaitForElementVisible(By locator, int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                return wait.Until(ExpectedConditions.ElementIsVisible(locator));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Element not visible after {timeoutInSeconds} seconds: {locator}", ex);
            }
        }

        /// <summary>
        /// Waits for an element to be clickable
        /// </summary>
        public IWebElement WaitForElementClickable(By locator, int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Element not clickable after {timeoutInSeconds} seconds: {locator}", ex);
            }
        }

        /// <summary>
        /// Waits for an element to be present in DOM
        /// </summary>
        public IWebElement WaitForElementPresent(By locator, int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                var elements = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
                return elements.FirstOrDefault() ?? throw new NoSuchElementException($"Element not found: {locator}");
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Element not present after {timeoutInSeconds} seconds: {locator}", ex);
            }
        }

        /// <summary>
        /// Waits for an element to be invisible
        /// </summary>
        public bool WaitForElementInvisible(By locator, int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                return wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Element still visible after {timeoutInSeconds} seconds: {locator}", ex);
            }
        }

        /// <summary>
        /// Waits for text to be present in element
        /// </summary>
        public bool WaitForTextPresent(By locator, string text, int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                return wait.Until(ExpectedConditions.TextToBePresentInElementLocated(locator, text));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Text '{text}' not found in element after {timeoutInSeconds} seconds: {locator}", ex);
            }
        }

        /// <summary>
        /// Waits for element attribute to have specific value
        /// </summary>
        public bool WaitForElementAttribute(By locator, string attribute, string value, int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                return wait.Until(driver =>
                {
                    var element = driver.FindElement(locator);
                    return element.GetAttribute(attribute) == value;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Element attribute {attribute} did not equal '{value}' after {timeoutInSeconds} seconds: {locator}", ex);
            }
        }

        /// <summary>
        /// Waits for all elements matching locator to be visible
        /// </summary>
        public IList<IWebElement> WaitForAllElementsVisible(By locator, int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                return wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Elements not visible after {timeoutInSeconds} seconds: {locator}", ex);
            }
        }

        /// <summary>
        /// Waits for page to load (document ready state)
        /// </summary>
        public void WaitForPageLoad(int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Page did not load after {timeoutInSeconds} seconds", ex);
            }
        }

        /// <summary>
        /// Waits for jQuery ajax calls to complete (if jQuery is present)
        /// </summary>
        public void WaitForJQueryAjax(int timeoutInSeconds = 0)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds > 0 ? timeoutInSeconds : _explicitWaitTime));
                wait.Until(driver =>
                {
                    try
                    {
                        var result = ((IJavaScriptExecutor)driver).ExecuteScript("return jQuery.active == 0");
                        return result is bool && (bool)result;
                    }
                    catch
                    {
                        // jQuery not available
                        return true;
                    }
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"jQuery ajax calls did not complete after {timeoutInSeconds} seconds", ex);
            }
        }
    }
}
