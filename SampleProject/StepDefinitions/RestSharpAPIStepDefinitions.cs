using RestSharp;
using Reqnroll;
using System.Text.Json;
using SampleProject.Config;

namespace SampleProject.StepDefinitions
{
    [Binding]
    public class RestSharpAPIStepDefinitions
    {
        private RestClient? _client;
        private RestResponse? _response;
        private readonly ScenarioContext _scenarioContext;

        public RestSharpAPIStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("I initialize the API client with base URL \"(.*)\"")]
        public void GivenIInitializeTheAPIClientWithBaseURL(string baseUrl)
        {
            _client = new RestClient(baseUrl);
            _scenarioContext["apiClient"] = _client;
            Console.WriteLine($"API client initialized with URL: {baseUrl}");
        }

        [Given("I initialize the API client with configured URL")]
        public void GivenIInitializeTheAPIClientWithConfiguredURL()
        {
            var apiUrl = ConfigReader.Instance.ApiUrl;
            _client = new RestClient(apiUrl);
            _scenarioContext["apiClient"] = _client;
            Console.WriteLine($"API client initialized with configured URL: {apiUrl}");
        }

        #region GET Requests

        [When("I send a GET request to \"(.*)\"")]
        public void WhenISendAGETRequestTo(string endpoint)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];

            var request = new RestRequest(endpoint, Method.Get);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            Console.WriteLine($"GET {endpoint} - Status: {(int)_response.StatusCode}");
        }

        [When("I send a GET request to \"(.*)\" with headers:")]
        public void WhenISendAGETRequestToWithHeaders(string endpoint, Table table)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];

            var request = new RestRequest(endpoint, Method.Get);

            foreach (var row in table.Rows)
            {
                var headerName = row["Header"];
                var headerValue = row["Value"];
                request.AddHeader(headerName, headerValue);
            }

            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            Console.WriteLine($"GET {endpoint} with headers - Status: {(int)_response.StatusCode}");
        }

        #endregion

        #region POST Requests

        [When("I send a POST request to \"(.*)\" with the following data:")]
        public void WhenISendAPOSTRequestToWithData(string endpoint, Table table)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];

            var request = new RestRequest(endpoint, Method.Post);
            var body = new Dictionary<string, object>();

            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var value = row["Value"];

                body[field] = ParseValue(value);
            }

            request.AddJsonBody(body);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            _scenarioContext["lastRequestBody"] = body;
            Console.WriteLine($"POST {endpoint} - Status: {(int)_response.StatusCode}");
        }

        [When("I send a POST request to \"(.*)\" with JSON body:")]
        public void WhenISendAPOSTRequestToWithJSONBody(string endpoint, string jsonBody)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];

            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(jsonBody);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            Console.WriteLine($"POST {endpoint} with JSON - Status: {(int)_response.StatusCode}");
        }

        #endregion

        #region PUT Requests

        [When("I send a PUT request to \"(.*)\" with the following data:")]
        public void WhenISendAPUTRequestToWithData(string endpoint, Table table)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];

            var request = new RestRequest(endpoint, Method.Put);
            var body = new Dictionary<string, object>();

            foreach (var row in table.Rows)
            {
                var field = row["Field"];
                var value = row["Value"];

                body[field] = ParseValue(value);
            }

            request.AddJsonBody(body);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            _scenarioContext["lastRequestBody"] = body;
            Console.WriteLine($"PUT {endpoint} - Status: {(int)_response.StatusCode}");
        }

        [When("I send a PUT request to \"(.*)\" with JSON body:")]
        public void WhenISendAPUTRequestToWithJSONBody(string endpoint, string jsonBody)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];

            var request = new RestRequest(endpoint, Method.Put);
            request.AddJsonBody(jsonBody);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            Console.WriteLine($"PUT {endpoint} with JSON - Status: {(int)_response.StatusCode}");
        }

        #endregion

        #region DELETE Requests

        [When("I send a DELETE request to \"(.*)\"")]
        public void WhenISendADELETERequestTo(string endpoint)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];

            var request = new RestRequest(endpoint, Method.Delete);
            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            Console.WriteLine($"DELETE {endpoint} - Status: {(int)_response.StatusCode}");
        }

        [When("I send a DELETE request to \"(.*)\" with headers:")]
        public void WhenISendADELETERequestToWithHeaders(string endpoint, Table table)
        {
            _client ??= (RestClient)_scenarioContext["apiClient"];

            var request = new RestRequest(endpoint, Method.Delete);

            foreach (var row in table.Rows)
            {
                var headerName = row["Header"];
                var headerValue = row["Value"];
                request.AddHeader(headerName, headerValue);
            }

            _response = _client.ExecuteAsync(request).Result;
            _scenarioContext["lastResponse"] = _response;
            Console.WriteLine($"DELETE {endpoint} with headers - Status: {(int)_response.StatusCode}");
        }

        #endregion

        #region Response Assertions

        [Then("the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            Assert.That((int)_response.StatusCode, Is.EqualTo(expectedStatusCode),
                $"Expected status code {expectedStatusCode}, but got {(int)_response.StatusCode}. Response: {_response.Content}");
        }

        [Then("the response status code should be one of: (.*)")]
        public void ThenTheResponseStatusCodeShouldBeOneOf(string statusCodes)
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];
            var expectedCodes = statusCodes.Split(',').Select(s => int.Parse(s.Trim())).ToArray();

            Assert.That((int)_response.StatusCode, Is.AnyOf(expectedCodes),
                $"Expected status code to be one of {string.Join(", ", expectedCodes)}, but got {(int)_response.StatusCode}");
        }

        [Then("the response should be valid JSON")]
        public void ThenTheResponseShouldBeValidJSON()
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            try
            {
                using (JsonDocument.Parse(_response.Content!))
                {
                    Assert.Pass("Response is valid JSON");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Response is not valid JSON: {ex.Message}", ex);
            }
        }

        [Then("the response should contain a list of users")]
        public void ThenTheResponseShouldContainAListOfUsers()
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(_response.Content!))
                {
                    var root = doc.RootElement;
                    Assert.That(root.ValueKind, Is.EqualTo(JsonValueKind.Array),
                        "Response should be an array of users");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse response as JSON array: {ex.Message}", ex);
            }
        }

        [Then("the response should have at least (\\d+) user")]
        public void ThenTheResponseShouldHaveAtLeastUser(int minimumCount)
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            using (JsonDocument doc = JsonDocument.Parse(_response.Content!))
            {
                var root = doc.RootElement;
                var userCount = root.GetArrayLength();
                Assert.That(userCount, Is.GreaterThanOrEqualTo(minimumCount),
                    $"Expected at least {minimumCount} users, but got {userCount}");
            }
        }

        [Then("the response should contain user with id (.*)")]
        public void ThenTheResponseShouldContainUserWithId(int userId)
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            using (JsonDocument doc = JsonDocument.Parse(_response.Content!))
            {
                var root = doc.RootElement;

                if (root.TryGetProperty("id", out var idElement))
                {
                    var id = idElement.GetInt32();
                    Assert.That(id, Is.EqualTo(userId), $"Expected user id {userId}, but got {id}");
                    _scenarioContext["currentUser"] = root;
                }
                else
                {
                    throw new Exception($"User id property not found in response");
                }
            }
        }

        [Then("the user name should be \"(.*)\"")]
        public void ThenTheUserNameShouldBe(string expectedName)
        {
            var userElement = (JsonElement)_scenarioContext["currentUser"];

            if (userElement.TryGetProperty("name", out var nameElement))
            {
                var name = nameElement.GetString();
                Assert.That(name, Is.EqualTo(expectedName),
                    $"Expected user name '{expectedName}', but got '{name}'");
            }
            else
            {
                throw new Exception("User name property not found in response");
            }
        }

        [Then("the response should contain the created post")]
        public void ThenTheResponseShouldContainTheCreatedPost()
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            using (JsonDocument doc = JsonDocument.Parse(_response.Content!))
            {
                var root = doc.RootElement;

                Assert.That(root.TryGetProperty("id", out _), Is.True,
                    "Created post should have an id");
                Assert.That(root.TryGetProperty("title", out _), Is.True,
                    "Created post should have a title");
                Assert.That(root.TryGetProperty("body", out _), Is.True,
                    "Created post should have a body");
            }
        }

        [Then("the response should contain the updated post")]
        public void ThenTheResponseShouldContainTheUpdatedPost()
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            using (JsonDocument doc = JsonDocument.Parse(_response.Content!))
            {
                var root = doc.RootElement;

                Assert.That(root.TryGetProperty("id", out _), Is.True,
                    "Updated post should have an id");
                Assert.That(root.TryGetProperty("title", out _), Is.True,
                    "Updated post should have a title");
            }
        }

        [Then("the response body should contain \"(.*)\"")]
        public void ThenTheResponseBodyShouldContain(string expectedText)
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            Assert.That(_response.Content, Does.Contain(expectedText),
                $"Response body should contain '{expectedText}'");
        }

        [Then("the response should have header \"(.*)\"")]
        public void ThenTheResponseShouldHaveHeader(string headerName)
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            var header = _response.Headers?.FirstOrDefault(h => h.Name?.Equals(headerName, StringComparison.OrdinalIgnoreCase) == true);
            Assert.That(header, Is.Not.Null, $"Response should have header '{headerName}'");
        }

        [Then("the response should have a property \"(.*)\" with value \"(.*)\"")]
        public void ThenTheResponseShouldHavePropertyWithValue(string propertyName, string expectedValue)
        {
            _response ??= (RestResponse)_scenarioContext["lastResponse"];

            using (JsonDocument doc = JsonDocument.Parse(_response.Content!))
            {
                var root = doc.RootElement;

                if (root.TryGetProperty(propertyName, out var property))
                {
                    var value = property.GetString();
                    Assert.That(value, Is.EqualTo(expectedValue),
                        $"Property '{propertyName}' should have value '{expectedValue}', but got '{value}'");
                }
                else
                {
                    throw new Exception($"Property '{propertyName}' not found in response");
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Parses a string value to appropriate type (int, bool, null, or string)
        /// </summary>
        private object ParseValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null!;

            if (int.TryParse(value, out var intValue))
                return intValue;

            if (bool.TryParse(value, out var boolValue))
                return boolValue;

            if (double.TryParse(value, out var doubleValue))
                return doubleValue;

            return value;
        }

        #endregion
    }
}
