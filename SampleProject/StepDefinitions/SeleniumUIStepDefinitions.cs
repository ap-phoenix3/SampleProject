using OpenQA.Selenium;
using Reqnroll;
using SampleProject.Driver;
using SampleProject.Pages;
using SampleProject.Config;

namespace SampleProject.StepDefinitions
{
    [Binding]
    public class SeleniumUIStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private HomePage? _homePage;

        public SeleniumUIStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("I open the browser")]
        public void GivenIOpenTheBrowser()
        {
            var driver = DriverFactory.GetDriver();
            _scenarioContext["driver"] = driver;
            Console.WriteLine($"Browser opened: {ConfigReader.Instance.BrowserType}");
        }

        [When("I navigate to \"(.*)\"")]
        public void WhenINavigateTo(string url)
        {
            var driver = (IWebDriver)_scenarioContext["driver"];
            driver.Navigate().GoToUrl(url);
            System.Threading.Thread.Sleep(1000);
        }

        [When("I navigate to the home page")]
        public void WhenINavigateToTheHomePage()
        {
            _homePage = new HomePage();
            _homePage.GoToHomePage();
        }

        [Then("the page title should contain \"(.*)\"")]
        public void ThenThePageTitleShouldContain(string expectedTitle)
        {
            var driver = (IWebDriver)_scenarioContext["driver"];
            Assert.That(driver.Title, Does.Contain(expectedTitle),
                $"Expected title to contain '{expectedTitle}', but got '{driver.Title}'");
        }

        [Then("the home page should be loaded")]
        public void ThenTheHomePageShouldBeLoaded()
        {
            _homePage ??= new HomePage();
            Assert.That(_homePage.IsHomePageLoaded(), Is.True, "Home page is not loaded");
        }

        [When("I fill the form with the following details:")]
        public void WhenIFillTheFormWithFollowingDetails(Table table)
        {
            var driver = (IWebDriver)_scenarioContext["driver"];
            var waitHelper = new WaitHelper(driver);

            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var value = row["Value"];

                try
                {
                    var element = waitHelper.WaitForElementVisible(By.Id(field));
                    element.SendKeys(value);
                }
                catch
                {
                    try
                    {
                        var element = waitHelper.WaitForElementVisible(By.Name(field));
                        element.SendKeys(value);
                    }
                    catch
                    {
                        throw new Exception($"Element with field '{field}' not found");
                    }
                }
            }
        }

        [Then("the form should be submitted successfully")]
        public void ThenTheFormShouldBeSubmittedSuccessfully()
        {
            var driver = (IWebDriver)_scenarioContext["driver"];
            var waitHelper = new WaitHelper(driver);

            try
            {
                var submitButton = waitHelper.WaitForElementClickable(By.XPath("//button[@type='submit']"));
                submitButton.Click();

                System.Threading.Thread.Sleep(2000);
                Assert.Pass("Form submitted successfully");
            }
            catch (Exception ex)
            {
                throw new Exception("Submit button not found or submission failed", ex);
            }
        }

        [When("I search for \"(.*)\"")]
        public void WhenISearchFor(string searchTerm)
        {
            _homePage ??= new HomePage();
            _homePage.Search(searchTerm);
        }

        [Then("search results should be displayed")]
        public void ThenSearchResultsShouldBeDisplayed()
        {
            var driver = (IWebDriver)_scenarioContext["driver"];

            try
            {
                var results = driver.FindElements(By.ClassName("post"));
                Assert.That(results.Count, Is.GreaterThan(0), "No search results found");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not verify search results", ex);
            }
        }

        [Then("I close the browser")]
        public void ThenICloseTheBrowser()
        {
            DriverFactory.CloseDriver();
            Console.WriteLine("Browser closed");
        }
    }
}

