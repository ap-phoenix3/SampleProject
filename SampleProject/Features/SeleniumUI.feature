Feature: Selenium UI Testing
  As a user
  I want to test web automation scenarios
  So that I can verify the UI functionality

  Background:
    Given I open the browser

  Scenario: Navigate to test automation practice website
    When I navigate to the home page
    Then the home page should be loaded
    And I close the browser

  Scenario: Verify page title
    When I navigate to the home page
    Then the page title should contain "Test Automation"
    And I close the browser

  Scenario: Fill and submit a form
    When I navigate to the home page
    And I fill the form with the following details:
      | Field | Value |
      | Name  | John Doe |
      | Email | john@example.com |
    Then the form should be submitted successfully
    And I close the browser

  Scenario: Search functionality
    When I navigate to the home page
    And I search for "Selenium"
    Then search results should be displayed
    And I close the browser
