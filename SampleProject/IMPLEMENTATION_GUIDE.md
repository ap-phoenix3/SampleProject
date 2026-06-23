# BDD Reqnroll Solution - Quick Start Guide

## 📦 What Was Created

A professional-grade BDD test automation framework with the following components:

### 1. **Config Folder** - Configuration Management
```
Config/
├── appsettings.json    # Environment & browser settings
└── ConfigReader.cs     # Singleton pattern config parser
```

**Key Features:**
- Multi-environment support (QA, UAT, Production)
- Browser configuration (Chrome, Edge, Firefox)
- Wait time settings
- Screenshot on failure settings

**Usage:**
```csharp
// Access configuration
string webUrl = ConfigReader.Instance.WebUrl;
string browserType = ConfigReader.Instance.BrowserType;
int timeout = ConfigReader.Instance.Timeout;
```

### 2. **Driver Folder** - WebDriver Management
```
Driver/
├── DriverFactory.cs    # WebDriver factory (Singleton)
└── WaitHelper.cs       # Explicit wait utilities
```

**DriverFactory Features:**
- ✅ Chrome support
- ✅ Edge support
- ✅ Firefox support
- ✅ Thread-safe (Singleton pattern)
- ✅ Automatic window sizing
- ✅ Implicit/Explicit waits configured

**WaitHelper Methods:**
- `WaitForElementVisible()` - Element must be visible
- `WaitForElementClickable()` - Element must be clickable
- `WaitForElementPresent()` - Element in DOM
- `WaitForElementInvisible()` - Element disappears
- `WaitForTextPresent()` - Text in element
- `WaitForPageLoad()` - Full page load
- `WaitForJQueryAjax()` - AJAX completion

### 3. **Pages Folder** - Page Object Model
```
Pages/
├── BasePage.cs         # Common page methods
└── HomePage.cs         # Sample page object
```

**BasePage Provides:**
- Element finding with waits
- Text entry and click operations
- Navigation methods
- Screenshot capture
- JavaScript execution
- Scroll to element

### 4. **StepDefinitions Folder** - Gherkin to Code Mapping

**SeleniumUIStepDefinitions.cs**
- Browser opening/closing
- Navigation
- Form filling
- Search functionality
- Page assertions

**RestSharpAPIStepDefinitions.cs**
- All HTTP methods (GET, POST, PUT, DELETE)
- Request headers and body
- JSON response parsing
- Multiple assertion types
- Response validation

### 5. **Hooks Folder** - Test Lifecycle
```
Hooks/
└── Hooks.cs            # Before/After scenario hooks
```

**Hook Features:**
- Scenario start/end logging
- Environment info display
- Screenshots on failure
- Browser cleanup
- Exception handling

### 6. **Features Folder** - Gherkin Scenarios
```
Features/
├── SeleniumUI.feature       # UI automation tests
└── RestSharpAPI.feature     # API tests
```

---

## 🚀 How to Use

### Change Browser Type
Edit `Config/appsettings.json`:
```json
"Browser": {
  "BrowserType": "Chrome"     // Change to "Edge" or "Firefox"
}
```

### Change Environment
Edit `Config/appsettings.json`:
```json
"Environment": {
  "Current": "QA"             // Change to "UAT" or "Production"
}
```

### Update URLs for Different Environments
Edit `Config/appsettings.json`:
```json
"Environments": {
  "QA": {
    "WebUrl": "https://test-qa.example.com",
    "ApiUrl": "https://api-qa.example.com"
  },
  "UAT": {
    "WebUrl": "https://test-uat.example.com",
    "ApiUrl": "https://api-uat.example.com"
  }
}
```

### Create a New Page Object
```csharp
public class LoginPage : BasePage
{
    private readonly By _emailField = By.Id("email");
    private readonly By _passwordField = By.Id("password");
    private readonly By _loginButton = By.XPath("//button[text()='Login']");

    public void Login(string email, string password)
    {
        EnterText(_emailField, email);
        EnterText(_passwordField, password);
        ClickElement(_loginButton);
    }
}
```

### Create a New Feature File
```gherkin
Feature: User Login
  Scenario: Successful login
    Given I open the browser
    When I navigate to the login page
    And I login with "user@example.com" and "password123"
    Then I should be logged in successfully
    And I close the browser
```

### Add Step Definitions
```csharp
[When("I login with \"(.*)\" and \"(.*)\"")]
public void WhenILoginWith(string email, string password)
{
    var loginPage = new LoginPage();
    loginPage.Login(email, password);
}
```

---

## 🧪 API Testing Examples

### GET Request
```gherkin
When I send a GET request to "/users"
Then the response status code should be 200
And the response should contain a list of users
```

### POST Request
```gherkin
When I send a POST request to "/posts" with the following data:
  | Field | Value |
  | userId | 1 |
  | title | My Post |
  | body | Content here |
Then the response status code should be 201
```

### PUT Request
```gherkin
When I send a PUT request to "/posts/1" with the following data:
  | Field | Value |
  | title | Updated Title |
  | body | Updated Content |
Then the response status code should be 200
```

### DELETE Request
```gherkin
When I send a DELETE request to "/posts/1"
Then the response status code should be 204
```

---

## 📝 Configuration Examples

### QA Environment with Chrome (Headless)
```json
{
  "Environment": {
    "Current": "QA"
  },
  "Browser": {
    "BrowserType": "Chrome",
    "Headless": true,
    "ImplicitWait": 10,
    "ExplicitWait": 20
  }
}
```

### UAT Environment with Edge (Full Browser)
```json
{
  "Environment": {
    "Current": "UAT"
  },
  "Browser": {
    "BrowserType": "Edge",
    "Headless": false,
    "WindowSize": "1920,1080",
    "ImplicitWait": 10,
    "ExplicitWait": 20
  }
}
```

### Local Development with Firefox (Debugging)
```json
{
  "Environment": {
    "Current": "QA"
  },
  "Browser": {
    "BrowserType": "Firefox",
    "Headless": false,
    "ScreenshotOnFailure": true,
    "ScreenshotPath": "./Screenshots/"
  }
}
```

---

## 🔍 Folder Structure Summary

```
SampleProject/
│
├── Config/                           # Configuration
│   ├── appsettings.json
│   └── ConfigReader.cs               # Singleton pattern
│
├── Driver/                           # WebDriver Management
│   ├── DriverFactory.cs              # Factory pattern, multi-browser
│   └── WaitHelper.cs                 # Wait strategies
│
├── Pages/                            # Page Object Model
│   ├── BasePage.cs                   # Abstract base class
│   └── HomePage.cs                   # Sample implementation
│
├── StepDefinitions/                  # Gherkin Mapping
│   ├── SeleniumUIStepDefinitions.cs  # UI steps
│   └── RestSharpAPIStepDefinitions.cs # API steps
│
├── Hooks/                            # Test Lifecycle
│   └── Hooks.cs                      # Setup/Teardown
│
├── Features/                         # Gherkin Scenarios
│   ├── SeleniumUI.feature
│   └── RestSharpAPI.feature
│
├── reqnroll.config                   # Reqnroll config
├── SampleProject.csproj              # Project file
├── UnitTest1.cs                      # Main test class
└── README.md                         # Full documentation
```

---

## 🧬 Design Patterns Used

| Pattern | Class | Purpose |
|---------|-------|---------|
| **Singleton** | ConfigReader, DriverFactory | Single instance management |
| **Factory** | DriverFactory | Browser creation |
| **Page Object** | BasePage, HomePage | UI element encapsulation |
| **Wait** | WaitHelper | Reliable element interaction |
| **Hook** | Hooks | Test lifecycle management |

---

## 📚 Key Files Reference

| File | Purpose | Key Methods |
|------|---------|-------------|
| `ConfigReader.cs` | Read config | `.Instance`, `.WebUrl`, `.BrowserType` |
| `DriverFactory.cs` | Get/Close driver | `.GetDriver()`, `.CloseDriver()` |
| `WaitHelper.cs` | Wait for elements | `.WaitForElementClickable()`, etc |
| `BasePage.cs` | Page operations | `.FindElement()`, `.ClickElement()`, etc |
| `Hooks.cs` | Test setup/teardown | `@BeforeScenario`, `@AfterScenario` |

---

## ✅ What's Configured

- ✅ Multi-browser support (Chrome, Edge, Firefox)
- ✅ Multi-environment support (QA, UAT, Production)
- ✅ Configurable wait times
- ✅ Screenshot on failure
- ✅ Thread-safe driver management
- ✅ Comprehensive wait strategies
- ✅ Page Object Model
- ✅ Full CRUD API testing (GET, POST, PUT, DELETE)
- ✅ JSON response parsing
- ✅ Detailed logging
- ✅ Professional error handling

---

## 🎯 Next Steps

1. **Update appsettings.json** with your environment URLs
2. **Create page objects** for your application pages
3. **Write feature files** describing your test scenarios
4. **Implement step definitions** mapping Gherkin to code
5. **Run tests** via Test Explorer or CLI
6. **Analyze results** and adjust waits/locators as needed

---

## 💡 Pro Tips

1. **Always use Page Objects** for maintainability
2. **Set appropriate wait times** to avoid flakiness
3. **Use Data Tables** for multiple test data scenarios
4. **Tag scenarios** (@smoke, @critical) for filtering
5. **Keep waits explicit** over implicit for better control
6. **Use configuration** instead of hardcoding values
7. **Take screenshots on failure** for debugging
8. **Run tests in headless** mode for CI/CD
9. **Validate responses** before making assertions
10. **Clean up resources** in AfterScenario hooks

---

## 🔧 Troubleshooting

| Issue | Solution |
|-------|----------|
| Element not found | Check locator, increase wait time |
| API timeout | Verify endpoint, check connectivity |
| Browser won't close | Check Hooks cleanup code |
| Config not loading | Verify JSON syntax, build output |
| Wait times too short | Adjust in appsettings.json |

---

**Build Status: ✅ SUCCESS**

All components are properly configured and ready for use!
