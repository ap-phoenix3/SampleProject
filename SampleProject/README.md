# BDD Reqnroll Test Suite - Professional Automation Framework

A comprehensive Behavior-Driven Development (BDD) test automation framework built with Reqnroll, Selenium, and RestSharp for testing web applications and REST APIs.

## 📁 Project Structure

```
SampleProject/
├── Config/
│   ├── appsettings.json          # Environment & browser configuration
│   └── ConfigReader.cs           # Configuration parser (Singleton pattern)
│
├── Driver/
│   ├── DriverFactory.cs          # WebDriver factory (Chrome, Edge, Firefox)
│   └── WaitHelper.cs             # Explicit waits for element interactions
│
├── Pages/
│   ├── BasePage.cs               # Base page object with common methods
│   └── HomePage.cs               # Home page implementation
│
├── StepDefinitions/
│   ├── SeleniumUIStepDefinitions.cs    # UI automation steps
│   └── RestSharpAPIStepDefinitions.cs  # API testing steps
│
├── Hooks/
│   └── Hooks.cs                  # BeforeScenario & AfterScenario hooks
│
├── Features/
│   ├── SeleniumUI.feature        # UI automation scenarios
│   └── RestSharpAPI.feature      # API testing scenarios
│
└── UnitTest1.cs                  # Main test class
```

## 🎯 Key Features

### 1. Configuration Management
- **Environment Support**: QA, UAT, Production
- **Browser Configuration**: Chrome, Edge, Firefox
- **Dynamic Settings**: URLs, timeouts, headless mode
- **Centralized Config**: appsettings.json

**Example Configuration (appsettings.json):**
```json
{
  "Environment": {
    "Current": "QA",
    "Environments": {
      "QA": {
        "WebUrl": "https://testautomationpractice.blogspot.com/",
        "ApiUrl": "https://jsonplaceholder.typicode.com",
        "Timeout": 30
      }
    }
  },
  "Browser": {
    "BrowserType": "Chrome",
    "Headless": false,
    "ImplicitWait": 10,
    "ExplicitWait": 20
  }
}
```

### 2. Driver Management (DriverFactory)

**Features:**
- Singleton pattern for driver management
- Support for multiple browsers (Chrome, Edge, Firefox)
- Automatic window sizing
- Implicit/Explicit wait configuration
- Thread-safe driver initialization

**Usage:**
```csharp
// Get driver instance
IWebDriver driver = DriverFactory.GetDriver();

// Close driver
DriverFactory.CloseDriver();

// Check if driver is initialized
bool isInitialized = DriverFactory.IsDriverInitialized;
```

### 3. Wait Helpers (WaitHelper)

**Available Wait Methods:**
- `WaitForElementVisible()` - Wait for element visibility
- `WaitForElementClickable()` - Wait for element to be clickable
- `WaitForElementPresent()` - Wait for element in DOM
- `WaitForElementInvisible()` - Wait for element to disappear
- `WaitForTextPresent()` - Wait for specific text in element
- `WaitForPageLoad()` - Wait for page to fully load
- `WaitForJQueryAjax()` - Wait for AJAX calls to complete

**Usage:**
```csharp
var waitHelper = new WaitHelper(driver, timeoutSeconds: 20);
var element = waitHelper.WaitForElementClickable(By.Id("submitBtn"));
```

### 4. Page Object Model (BasePage)

**Base Page Methods:**
- Navigation: `Navigate()`, `RefreshPage()`, `GetCurrentUrl()`
- Elements: `FindElement()`, `FindClickableElement()`, `IsElementDisplayed()`
- Interactions: `ClickElement()`, `EnterText()`, `GetText()`
- Screenshots: `TakeScreenshot()`
- JavaScript: `ExecuteJavaScript()`, `ScrollToElement()`

**Custom Page Example:**
```csharp
public class HomePage : BasePage
{
    private readonly By _searchBox = By.Id("s");

    public void Search(string term)
    {
        EnterText(_searchBox, term);
    }
}
```

### 5. Selenium UI Testing

**Available Steps:**
- `Given I open the browser` - Initialize WebDriver
- `When I navigate to <url>` - Navigate to URL
- `When I fill the form with...` - Fill form fields
- `Then search results should be displayed` - Verify search results
- `Then I close the browser` - Close WebDriver

**Example Feature:**
```gherkin
Feature: Selenium UI Testing
  Scenario: Search functionality
    Given I open the browser
    When I navigate to the home page
    And I search for "Selenium"
    Then search results should be displayed
    And I close the browser
```

### 6. RestSharp API Testing - All HTTP Methods

#### GET Requests
```gherkin
When I send a GET request to "/users"
When I send a GET request to "/users/1" with headers:
  | Header | Value |
  | Authorization | Bearer token |
```

#### POST Requests
```gherkin
When I send a POST request to "/posts" with the following data:
  | Field | Value |
  | userId | 1 |
  | title | New Post |
  | body | Content |

When I send a POST request to "/posts" with JSON body:
  {"userId": 1, "title": "Post", "body": "Content"}
```

#### PUT Requests
```gherkin
When I send a PUT request to "/posts/1" with the following data:
  | Field | Value |
  | id | 1 |
  | title | Updated |

When I send a PUT request to "/posts/1" with JSON body:
  {"id": 1, "title": "Updated"}
```

#### DELETE Requests
```gherkin
When I send a DELETE request to "/posts/1"

When I send a DELETE request to "/posts/1" with headers:
  | Header | Value |
  | Authorization | Bearer token |
```

### 7. Response Assertions

**Available Assertions:**
- `the response status code should be <code>`
- `the response status code should be one of: <codes>`
- `the response should be valid JSON`
- `the response body should contain "<text>"`
- `the response should have header "<name>"`
- `the response should have a property "<name>" with value "<value>"`
- `the response should contain a list of users`
- `the response should contain user with id <id>`

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- Visual Studio Community 2026 or later
- Chrome/Edge/Firefox browser
- Internet connection (for API testing)

### Installation

1. Clone or open the solution
2. Build the project:
   ```powershell
   dotnet build
   ```
3. NuGet packages will be automatically restored

### Running Tests

#### Visual Studio Test Explorer
1. Open Test Explorer: `Ctrl+E, T`
2. Click "Run All Tests"
3. Or run specific tests

#### Command Line
```powershell
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test
dotnet test --filter "SeleniumUI"
```

## 🔧 Configuration

### appsettings.json

```json
{
  "Environment": {
    "Current": "QA",
    "Environments": {
      "QA": {
        "WebUrl": "https://testautomationpractice.blogspot.com/",
        "ApiUrl": "https://jsonplaceholder.typicode.com",
        "Timeout": 30
      },
      "UAT": {
        "WebUrl": "https://uat.example.com/",
        "ApiUrl": "https://api-uat.example.com",
        "Timeout": 30
      }
    }
  },
  "Browser": {
    "BrowserType": "Chrome",    // Chrome, Edge, Firefox
    "Headless": false,
    "WindowSize": "1920,1080",
    "ImplicitWait": 10,
    "ExplicitWait": 20,
    "ScreenshotOnFailure": true,
    "ScreenshotPath": "./Screenshots/"
  },
  "Browser.Chrome": {
    "Arguments": [
      "--start-maximized",
      "--disable-blink-features=AutomationControlled",
      "--disable-dev-shm-usage"
    ]
  }
}
```

### Switching Browser
Edit `appsettings.json`:
```json
"Browser": {
  "BrowserType": "Chrome"  // Change to "Edge" or "Firefox"
}
```

### Switching Environment
Edit `appsettings.json`:
```json
"Environment": {
  "Current": "QA"  // Change to "UAT" or "Production"
}
```

## 📝 Writing Tests

### Create a Feature File

```gherkin
Feature: My Feature
  Background:
    Given I open the browser

  Scenario: My Scenario
    When I navigate to the home page
    Then the home page should be loaded
    And I close the browser
```

### Add Step Definition

```csharp
[Binding]
public class MyStepDefinitions
{
    [When("I do something")]
    public void WhenIDoSomething()
    {
        // Implementation
    }
}
```

### Create Page Object

```csharp
public class MyPage : BasePage
{
    private readonly By _element = By.Id("elementId");

    public void MyAction()
    {
        ClickElement(_element);
    }
}
```

## 🔍 Hooks

**BeforeScenario:**
- Logs scenario start
- Displays environment info
- Prepares test data

**AfterScenario:**
- Takes screenshot on failure (if configured)
- Closes browser
- Logs scenario completion

```csharp
[BeforeScenario]
public void BeforeScenario()
{
    // Setup code
}

[AfterScenario]
public void AfterScenario()
{
    // Cleanup code
}
```

## 📦 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Reqnroll | 2.0.2 | BDD Framework |
| Reqnroll.NUnit | 2.0.2 | NUnit Integration |
| Selenium.WebDriver | 4.25.0 | Browser Automation |
| Selenium.Support | 4.25.0 | Wait Helpers |
| RestSharp | 107.3.0 | HTTP Client |
| NUnit | 4.3.2 | Test Framework |
| Microsoft.Extensions.Configuration | 10.0.0 | Config Management |
| Microsoft.Extensions.Configuration.Json | 10.0.0 | JSON Config |

## 💡 Best Practices

1. **Use Page Objects** for maintainability
2. **Configure Waits Properly** to avoid flakiness
3. **Use Appropriate Assertions** for clear failure messages
4. **Keep Scenarios Independent** - no test interdependencies
5. **Use Data Driven Tests** with Tables in Gherkin
6. **Handle Exceptions Gracefully** with meaningful messages
7. **Take Screenshots on Failure** for debugging
8. **Use Environment Configs** for different environments
9. **Keep Step Definitions Simple** - delegate to page objects
10. **Run Tests in Headless Mode** for CI/CD pipelines

## 🐛 Troubleshooting

### WebDriver Timeouts
- Increase timeout in appsettings.json
- Check element locators
- Verify page load conditions

### Element Not Found
- Check XPath/CSS selectors
- Increase wait times
- Use browser DevTools to inspect elements

### API Connection Issues
- Verify API endpoint is accessible
- Check request/response format
- Validate authentication headers

### Configuration Not Loading
- Ensure appsettings.json is in Config folder
- Verify JSON syntax
- Check project build output directory

## 📚 Resources

- [Reqnroll Documentation](https://reqnroll.net/)
- [Selenium Documentation](https://www.selenium.dev/documentation/)
- [RestSharp Documentation](https://restsharp.dev/)
- [NUnit Documentation](https://docs.nunit.org/)
- [Gherkin Syntax](https://cucumber.io/docs/gherkin/)

## 📄 License

This is a sample BDD test project for learning and practice purposes.

## ✨ Recent Updates

- ✅ Configuration Management (appsettings.json)
- ✅ DriverFactory with multiple browser support
- ✅ WaitHelper for reliable element interactions
- ✅ Page Object Model implementation
- ✅ All HTTP methods for RestSharp (GET, POST, PUT, DELETE)
- ✅ Enhanced error handling and logging
- ✅ Screenshot on failure functionality
- ✅ Comprehensive hooks implementation
