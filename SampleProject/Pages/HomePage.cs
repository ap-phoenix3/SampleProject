using OpenQA.Selenium;
using SampleProject.Config;

namespace SampleProject.Pages
{
    /// <summary>
    /// Home page of the test automation practice website
    /// </summary>
    public class HomePage : BasePage
    {
        private readonly By _pageHeading = By.XPath("//h1[contains(text(), 'Test Automation')]");
        private readonly By _searchBox = By.Id("s");
        private readonly By _searchButton = By.XPath("//button[@type='submit']");

        public HomePage()
        {
        }

        /// <summary>
        /// Navigate to the home page
        /// </summary>
        public void GoToHomePage()
        {
            Navigate(ConfigReader.Instance.WebUrl);
            WaitForPageLoad();
        }

        /// <summary>
        /// Get page title
        /// </summary>
        public string GetHomePageTitle()
        {
            return GetPageTitle();
        }

        /// <summary>
        /// Verify page is loaded
        /// </summary>
        public bool IsHomePageLoaded()
        {
            return IsElementDisplayed(_pageHeading);
        }

        /// <summary>
        /// Search for a term
        /// </summary>
        public void Search(string searchTerm)
        {
            EnterText(_searchBox, searchTerm);
            ClickElement(_searchButton);
            System.Threading.Thread.Sleep(2000);
        }

        /// <summary>
        /// Get search box placeholder text
        /// </summary>
        public string GetSearchPlaceholder()
        {
            return GetAttribute(_searchBox, "placeholder");
        }
    }
}
