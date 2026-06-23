# Code Examples and Patterns

## 🎯 Complete Example: Login Feature

### Feature File: `LoginTests.feature`
```gherkin
Feature: User Authentication
  As a user
  I want to log in to the application
  So that I can access my account

  Background:
    Given I open the browser
    And I navigate to the login page

  Scenario: Successful login with valid credentials
    When I enter email "john@example.com"
    And I enter password "SecurePassword123"
    And I click the login button
    Then I should be logged in successfully
    And the dashboard should be displayed
    And I close the browser

  Scenario: Login fails with invalid credentials
    When I enter email "john@example.com"
    And I enter password "WrongPassword"
    And I click the login button
    Then an error message should appear
    And the message should be "Invalid credentials"
    And I close the browser

  Scenario: Email is required
    When I leave email empty
    And I enter password "AnyPassword"
    And I click the login button
    Then a validation error should appear
    And I close the browser
```

### Page Object: `LoginPage.cs`
```csharp
using OpenQA.Selenium;
using SampleProject.Pages;

namespace SampleProject.Pages
{
    public class LoginPage : BasePage
    {
        // Locators
        private readonly By _emailInput = By.Id("email");
        private readonly By _passwordInput = By.Id("password");
        private readonly By _loginButton = By.XPath("//button[@type='submit'][contains(text(), 'Login')]");
        private readonly By _errorMessage = By.ClassName("error-message");
        private readonly By _validationError = By.ClassName("validation-error");

        public void GoToLoginPage()
        {
            Navigate(ConfigReader.Instance.WebUrl + "/login");
            WaitForPageLoad();
        }

        public void EnterEmail(string email)
        {
            EnterText(_emailInput, email);
        }

        public void EnterPassword(string password)
        {
            EnterText(_passwordInput, password);
        }

        public void ClickLogin()
        {
            ClickElement(_loginButton);
            System.Threading.Thread.Sleep(1000); // Wait for page transition
        }

        public string GetErrorMessage()
        {
            return GetText(_errorMessage);
        }

        public bool IsValidationErrorDisplayed()
        {
            return IsElementDisplayed(_validationError);
        }

        public bool IsLoginButtonDisplayed()
        {
            return IsElementDisplayed(_loginButton);
        }

        public bool IsEmailFieldDisplayed()
        {
            return IsElementDisplayed(_emailInput);
        }
    }
}
```

### Step Definitions: `LoginStepDefinitions.cs`
```csharp
using Reqnroll;
using SampleProject.Pages;

namespace SampleProject.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions
    {
        private LoginPage _loginPage;

        [Given("I navigate to the login page")]
        public void GivenINavigateToLoginPage()
        {
            _loginPage = new LoginPage();
            _loginPage.GoToLoginPage();
        }

        [When("I enter email \"(.*)\"")]
        public void WhenIEnterEmail(string email)
        {
            _loginPage.EnterEmail(email);
        }

        [When("I enter password \"(.*)\"")]
        public void WhenIEnterPassword(string password)
        {
            _loginPage.EnterPassword(password);
        }

        [When("I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            _loginPage.ClickLogin();
        }

        [When("I leave email empty")]
        public void WhenILeaveEmailEmpty()
        {
            // Email field is already empty, just verify
            Assert.That(_loginPage.IsEmailFieldDisplayed(), Is.True);
        }

        [Then("I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            var dashboardPage = new DashboardPage();
            Assert.That(dashboardPage.IsUserLoggedIn(), Is.True);
        }

        [Then("the dashboard should be displayed")]
        public void ThenTheDashboardShouldBeDisplayed()
        {
            var dashboardPage = new DashboardPage();
            Assert.That(dashboardPage.IsWelcomeMessageDisplayed(), Is.True);
        }

        [Then("an error message should appear")]
        public void ThenAnErrorMessageShouldAppear()
        {
            Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True);
        }

        [Then("the message should be \"(.*)\"")]
        public void ThenTheMessageShouldBe(string expectedMessage)
        {
            var actualMessage = _loginPage.GetErrorMessage();
            Assert.That(actualMessage, Does.Contain(expectedMessage));
        }

        [Then("a validation error should appear")]
        public void ThenAValidationErrorShouldAppear()
        {
            Assert.That(_loginPage.IsValidationErrorDisplayed(), Is.True);
        }
    }
}
```

---

## 📡 Complete Example: API Testing

### Feature File: `UserAPI.feature`
```gherkin
Feature: User API Testing
  As a developer
  I want to test user endpoints
  So that I can verify API functionality

  Background:
    Given I initialize the API client with configured URL

  Scenario: Get all users
    When I send a GET request to "/users"
    Then the response status code should be 200
    And the response should contain a list of users
    And the response should have at least 1 user

  Scenario: Get specific user
    When I send a GET request to "/users/1"
    Then the response status code should be 200
    And the response should contain user with id 1
    And the user name should be "Leanne Graham"

  Scenario: Create new user
    When I send a POST request to "/users" with the following data:
      | Field | Value |
      | name | Jane Doe |
      | email | jane@example.com |
      | username | janedoe |
    Then the response status code should be 201
    And the response should have a property "id" with value greater than 0

  Scenario: Update user
    When I send a PUT request to "/users/1" with the following data:
      | Field | Value |
      | id | 1 |
      | name | Updated Name |
      | email | updated@example.com |
    Then the response status code should be 200
    And the response should have a property "name" with value "Updated Name"

  Scenario: Delete user
    When I send a DELETE request to "/users/1"
    Then the response status code should be one of: 200,204
```

### Enhanced Step Definitions: `ApiStepDefinitions.cs`
```csharp
using RestSharp;
using Reqnroll;
using System.Text.Json;

namespace SampleProject.StepDefinitions
{
    [Binding]
    public class ApiStepDefinitions
    {
        private RestClient _client;
        private RestResponse _response;
        private readonly ScenarioContext _scenarioContext;

        public ApiStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("I initialize the API client with base URL \"(.*)\"")]
        public void InitializeApiClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
            _scenarioContext["apiClient"] = _client;
            Console.WriteLine($"API client initialized: {baseUrl}");
        }

        // GET Methods
        [When("I send a GET request to \"(.*)\"")]
        public void SendGetRequest(string endpoint)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];
            var request = new RestRequest(endpoint, Method.Get);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            LogRequest("GET", endpoint);
        }

        // POST Methods
        [When("I send a POST request to \"(.*)\" with the following data:")]
        public void SendPostRequest(string endpoint, Table table)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];
            var request = new RestRequest(endpoint, Method.Post);
            var body = BuildRequestBody(table);
            request.AddJsonBody(body);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            LogRequest("POST", endpoint);
        }

        // PUT Methods
        [When("I send a PUT request to \"(.*)\" with the following data:")]
        public void SendPutRequest(string endpoint, Table table)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];
            var request = new RestRequest(endpoint, Method.Put);
            var body = BuildRequestBody(table);
            request.AddJsonBody(body);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            LogRequest("PUT", endpoint);
        }

        // DELETE Methods
        [When("I send a DELETE request to \"(.*)\"")]
        public void SendDeleteRequest(string endpoint)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];
            var request = new RestRequest(endpoint, Method.Delete);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            LogRequest("DELETE", endpoint);
        }

        // Assertions
        [Then("the response status code should be (.*)")]
        public void VerifyStatusCode(int expectedCode)
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];
            Assert.That((int)_response.StatusCode, Is.EqualTo(expectedCode),
                $"Expected {expectedCode}, got {(int)_response.StatusCode}");
        }

        [Then("the response should have a property \"(.*)\" with value greater than (.*)")]
        public void VerifyPropertyGreaterThan(string property, int value)
        {
            using (JsonDocument doc = JsonDocument.Parse(_response.Content))
            {
                var root = doc.RootElement;
                if (root.TryGetProperty(property, out var prop))
                {
                    var propValue = prop.GetInt32();
                    Assert.That(propValue, Is.GreaterThan(value));
                }
            }
        }

        // Helper Methods
        private Dictionary<string, object> BuildRequestBody(Table table)
        {
            var body = new Dictionary<string, object>();
            foreach (var row in table.Rows)
            {
                body[row["Field"]] = ParseValue(row["Value"]);
            }
            return body;
        }

        private object ParseValue(string value)
        {
            if (int.TryParse(value, out var intValue)) return intValue;
            if (bool.TryParse(value, out var boolValue)) return boolValue;
            return value;
        }

        private void LogRequest(string method, string endpoint)
        {
            Console.WriteLine($"[API] {method} {endpoint} => {(int)_response.StatusCode}");
        }
    }
}
```

---

## ⚙️ Configuration Management Examples

### Production Configuration
```json
{
  "Environment": {
    "Current": "Production",
    "Environments": {
      "Production": {
        "WebUrl": "https://www.example.com",
        "ApiUrl": "https://api.example.com",
        "Timeout": 60
      }
    }
  },
  "Browser": {
    "BrowserType": "Chrome",
    "Headless": true,
    "ImplicitWait": 15,
    "ExplicitWait": 30,
    "ScreenshotOnFailure": true
  },
  "Browser.Chrome": {
    "Arguments": [
      "--headless=new",
      "--disable-gpu",
      "--window-size=1920,1080",
      "--no-sandbox",
      "--disable-dev-shm-usage"
    ]
  }
}
```

### Local Development Configuration
```json
{
  "Environment": {
    "Current": "QA",
    "Environments": {
      "QA": {
        "WebUrl": "https://localhost:3000",
        "ApiUrl": "http://localhost:5000",
        "Timeout": 30
      }
    }
  },
  "Browser": {
    "BrowserType": "Chrome",
    "Headless": false,
    "WindowSize": "1920,1080",
    "ImplicitWait": 10,
    "ExplicitWait": 20,
    "ScreenshotOnFailure": true,
    "ScreenshotPath": "./Screenshots/"
  }
}
```

---

## 🎨 Advanced Wait Patterns

### WaitHelper Usage Examples
```csharp
// Wait for element to be visible
var element = waitHelper.WaitForElementVisible(By.Id("submit-btn"));

// Wait for element to be clickable before clicking
var button = waitHelper.WaitForElementClickable(By.XPath("//button[@id='action']"));
button.Click();

// Wait for specific text
waitHelper.WaitForTextPresent(By.Id("message"), "Success");

// Wait for page load
waitHelper.WaitForPageLoad();

// Wait for jQuery AJAX
waitHelper.WaitForJQueryAjax();

// Wait for element to disappear (loading spinner)
waitHelper.WaitForElementInvisible(By.ClassName("spinner"));

// Get all visible elements
var elements = waitHelper.WaitForAllElementsVisible(By.ClassName("item"));
```

---

## 📊 Data-Driven Testing Example

### Feature File with Data Tables
```gherkin
Feature: Login with Multiple Users

  Scenario Outline: Login with different credentials
    Given I open the browser
    When I navigate to the login page
    And I enter email "<email>"
    And I enter password "<password>"
    And I click the login button
    Then the result should be "<result>"
    And I close the browser

    Examples:
      | email | password | result |
      | user1@example.com | Pass123! | success |
      | user2@example.com | Pass456! | success |
      | invalid@example.com | WrongPass | error |
      | user3@example.com | | error |
```

---

## 🔐 Secure Configuration with Environment Variables

```csharp
public class ConfigReader
{
    public string ApiKey
    {
        get
        {
            // Try to get from environment first (CI/CD)
            var envKey = Environment.GetEnvironmentVariable("API_KEY");
            if (!string.IsNullOrEmpty(envKey))
                return envKey;

            // Fall back to config file
            return _configuration["ApiKey"];
        }
    }
}
```

---

## ✨ Best Practices Summary

| Practice | Benefit |
|----------|---------|
| Use Page Objects | Maintainability |
| Configure everything | Flexibility |
| Explicit waits | Reliability |
| Meaningful assertions | Clear failures |
| Data-driven tests | Coverage |
| Screenshots on fail | Debugging |
| Hook cleanup | Resource management |
| Logging | Troubleshooting |
