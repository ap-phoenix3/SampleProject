Feature: RestSharp API Testing
  As a tester
  I want to test REST API endpoints
  So that I can verify the API functionality

  Background:
    Given I initialize the API client with configured URL

  Scenario: Get list of all users
    When I send a GET request to "/users"
    Then the response status code should be 200
    And the response should contain a list of users
    And the response should have at least 1 user

  Scenario: Get specific user by ID
    When I send a GET request to "/users/1"
    Then the response status code should be 200
    And the response should contain user with id 1
    And the user name should be "Leanne Graham"

  Scenario: Create a new post
    When I send a POST request to "/posts" with the following data:
      | Field | Value |
      | userId | 1 |
      | title | Test Post |
      | body | This is a test post |
    Then the response status code should be 201
    And the response should contain the created post

  Scenario: Update an existing post
    When I send a PUT request to "/posts/1" with the following data:
      | Field | Value |
      | id | 1 |
      | userId | 1 |
      | title | Updated Post |
      | body | Updated content |
    Then the response status code should be 200
    And the response should contain the updated post

  Scenario: Delete a post
    When I send a DELETE request to "/posts/1"
    Then the response status code should be one of: 200,204

  Scenario: Verify response is valid JSON
    When I send a GET request to "/users/1"
    Then the response status code should be 200
    And the response should be valid JSON

  Scenario: Create and verify post with headers
    When I send a POST request to "/posts" with headers:
      | Header | Value |
      | Content-Type | application/json |
    And I send a POST request to "/posts" with the following data:
      | Field | Value |
      | userId | 1 |
      | title | Post with Headers |
      | body | Content with specific headers |
    Then the response status code should be 201
