using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ConsoleAppServiceBusReceive.Blockchain
{
    class ApiTasks
    {
        private static readonly string AUTHORITY_TENANT = "https://login.microsoftonline.com/xxxxxxxxx";
        /**
         * Azure Blockchain Workbench REST API
         * https://docs.microsoft.com/es-es/rest/api/azure-blockchain-workbench/
         * The REST API subdomain is similar to the Blockchain Workbench subdomain.
         * The third level domain name includes a -api suffix.
         * WORKBENCH_URL: https://xxxxxxxxx.azurewebsites.net
         * WORKBENCH_API_URL: https://xxxxxxxxx-api.azurewebsites.net
        **/
        private static readonly string WORKBENCH_API_URL = "https://xxxxxxxxx-api.azurewebsites.net";
        private static readonly string WORKBENCH_APP_ID = "xxx-xxx-xxx-xxx-xxx";
        private static readonly string CLIENT_APP_ID = "xxx-xxx-xxx-xxx-xxx";
        private static readonly string CLIENT_APP_KEY = "xxxxxxxxx";

        private static readonly HttpClient _httpClient;
        private static readonly AuthenticationContext _authenticationContext;
        private static readonly ClientCredential _clientCredential;

        static ApiTasks()
        {
            _httpClient = new HttpClient();
            _authenticationContext = new AuthenticationContext(AUTHORITY_TENANT);
            _clientCredential = new ClientCredential(CLIENT_APP_ID, CLIENT_APP_KEY);
        }

        private async Task<string> GetTokenAsync()
        {
            // Getting a token, it is recommended to call AcquireTokenAsync before every Workbench API call
            // The library takes care of refreshing the token when it expires
            var result = await _authenticationContext.AcquireTokenAsync(WORKBENCH_APP_ID, _clientCredential).ConfigureAwait(false);
            var token = result.AccessToken;

            //Console.WriteLine("TOKEN:");
            //Console.WriteLine(token);
            //Console.WriteLine("------");

            return token;
        }

        public async Task GetApplicationsAsync()
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/applications");

            Console.WriteLine("GET ALL APPLICATIONS:");
            if (response.IsSuccessStatusCode)
            {
                var applications = await response.Content.ReadAsStringAsync();
                Console.WriteLine(applications);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetApplicationAsync(int applicationId)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/applications/{applicationId}");

            Console.WriteLine($"GET APPLICATION #{applicationId}:");
            if (response.IsSuccessStatusCode)
            {
                var application = await response.Content.ReadAsStringAsync();
                Console.WriteLine(application);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetUsersAsync()
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/users");

            Console.WriteLine("GET ALL USERS:");
            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadAsStringAsync();
                Console.WriteLine(users);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetMeAsync()
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/users/me");

            Console.WriteLine("GET USER ME:");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsStringAsync();
                Console.WriteLine(user);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task PostUserAsync(string userJson)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(userJson, System.Text.Encoding.UTF8, "application/userJson");

            // Get Users
            var response = await _httpClient.PostAsync($"{WORKBENCH_API_URL}/api/v2/users", content);

            Console.WriteLine($"POST USER:");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsStringAsync();
                Console.WriteLine(user);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task PostRoleAssignments(int applicationId, string roleJson)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(roleJson, System.Text.Encoding.UTF8, "application/userJson");

            // Get Users
            var response = await _httpClient.PostAsync($"{WORKBENCH_API_URL}/api/v2/applications/{applicationId}/roleAssignments", content);

            Console.WriteLine($"POST APP#{applicationId} ROLE ASSIGNMENTS:");
            if (response.IsSuccessStatusCode)
            {
                var assignments = await response.Content.ReadAsStringAsync();
                Console.WriteLine(assignments);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetRoleAssignmentsAsync(int applicationId)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/applications/{applicationId}/roleAssignments");

            Console.WriteLine($"GET ALL ROLE ASSIGNMENTS APP#{applicationId}:");
            if (response.IsSuccessStatusCode)
            {
                var application = await response.Content.ReadAsStringAsync();
                Console.WriteLine(application);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetWorkflowsAsync(int applicationId)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/applications/{applicationId}/workflows");

            Console.WriteLine($"GET WORKFLOWS APP#{applicationId}:");
            if (response.IsSuccessStatusCode)
            {
                var workflows = await response.Content.ReadAsStringAsync();
                Console.WriteLine(workflows);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetWorkflowAsync(int workflowId)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/applications/workflows/{workflowId}");

            Console.WriteLine($"GET WORKFLOW #{workflowId}:");
            if (response.IsSuccessStatusCode)
            {
                var workflows = await response.Content.ReadAsStringAsync();
                Console.WriteLine(workflows);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetContractsAsync()
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/contracts");

            Console.WriteLine("GET ALL CONTRACTS:");
            if (response.IsSuccessStatusCode)
            {
                var contracts = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contracts);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetContractAsync(int contractId)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/contracts/{contractId}");

            Console.WriteLine($"GET CONTRACT #{contractId}:");
            if (response.IsSuccessStatusCode)
            {
                var contract = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contract);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task GetContractActionsAsync(int contractId)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v2/contracts/{contractId}/actions");

            Console.WriteLine($"GET CON#{contractId} ACTIONS:");
            if (response.IsSuccessStatusCode)
            {
                var contract = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contract);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }

        public async Task PostContractActionAsync(int contractId, string actionJson)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(actionJson, System.Text.Encoding.UTF8, "application/json");

            // Get Users
            var response = await _httpClient.PostAsync($"{WORKBENCH_API_URL}/api/v2/contracts/{contractId}/actions", content);

            Console.WriteLine($"POST CON#{contractId} ACTION:");
            if (response.IsSuccessStatusCode)
            {
                var action = await response.Content.ReadAsStringAsync();
                Console.WriteLine(action);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            Console.WriteLine("------");
        }



        public async Task ShowWorkflows()
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/userJson"));

            // Get Users
            var response = await _httpClient.GetAsync($"{WORKBENCH_API_URL}/api/v1/users");

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadAsStringAsync();
                Console.WriteLine("USERS:");
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(users);
                Console.WriteLine("------");
            }
            else
            {
                Console.WriteLine("ERROR:");
                Console.WriteLine(response.StatusCode);
                Console.WriteLine("------");
            }
        }

        private const string HostDomain = "azbcwbexperiment-7ena5p.azurewebsites.net";
        private const string ContractId = "3";

        public async Task<string> GetApplicationListAsync()
        {
            var uri = $"https://{HostDomain}/api/v1/applications";
            var method = "GET";
            var token = await GetTokenAsync();

            // HTTP Request
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            // Request Headers
            httpWebRequest.Method = method;
            httpWebRequest.Headers.Add("Host", HostDomain);
            httpWebRequest.Headers.Add("Authorization", $"Bearer {token}");
            // HTTP Response
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            // Response Stream Reader
            string result;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            Console.WriteLine();
            Console.WriteLine(result);
            Console.WriteLine();

            return result;
        }

        public async Task<string> GetActionsAsync()
        {
            var uri = $"https://{HostDomain}/api/v2/actions";
            var method = "GET";
            var token = await GetTokenAsync();

            // HTTP Request
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            // Request Headers
            httpWebRequest.Method = method;
            httpWebRequest.Headers.Add("Host", HostDomain);
            httpWebRequest.Headers.Add("Authorization", $"Bearer {token}");
            // HTTP Response
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            // Response Stream Reader
            string result;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            Console.WriteLine();
            Console.WriteLine(result);
            Console.WriteLine();

            return result;
        }

        public async Task<string> SendTelemetryAsync(string json)
        {
            var uri = $"https://{HostDomain}/api/v2/contracts/{ContractId}/actions";
            var method = "POST";
            var token = await GetTokenAsync();

            // HTTP Request
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            // Request Headers
            httpWebRequest.ContentType = "application/userJson";
            httpWebRequest.Method = method;
            httpWebRequest.Headers.Add("Host", HostDomain);
            httpWebRequest.Headers.Add("Authorization", $"Bearer {token}");
            json = "actionInformation: {[" +
                "\"workflowFunctionId\":3," +
                "\"workflowActionParameters\":" +
                    "[{\"temperature\": \"15\"}" +
                    "]}";

            // Request Stream Writer
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            // HTTP Response
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            // Response Stream Reader
            string result;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            Console.WriteLine();
            Console.WriteLine(result);
            Console.WriteLine();

            return result;
        }

    }
}
