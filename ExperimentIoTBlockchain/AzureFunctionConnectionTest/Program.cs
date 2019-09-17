using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureFunctionConnectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            ApiTasks api = new ApiTasks();
            Action telemetry = new Action(15, 35);
            string telemetryJson = Serializer.SerializeToJson(telemetry);
            await api.PostContractActionAsync(10, telemetryJson);
        }
    }

    class ApiTasks
    {
        private static readonly string AUTHORITY_TENANT = "https://login.microsoftonline.com/bravent.net";
        private static readonly string WORKBENCH_API_URL = "https://azbcwbexperiment-7ena5p-api.azurewebsites.net";
        private static readonly string WORKBENCH_APP_ID = "4aca04f2-5a1a-477d-8089-17b066bd3e8f";
        private static readonly string CLIENT_APP_ID = "f64f8e23-f437-4ead-8172-85f2fe78d819";
        private static readonly string CLIENT_APP_KEY = "+2X5sR+9DvJ0GfqDBFZhBbjXddYnW4biNpPMTjguTiY=";

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

        public async Task PostContractActionAsync(int contractId, string actionJson)
        {
            // Getting the token
            var token = await GetTokenAsync();

            // Using token to call Workbench's API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(actionJson, System.Text.Encoding.UTF8, "application/json");

            // Get Users
            await _httpClient.PostAsync($"{WORKBENCH_API_URL}/api/v2/contracts/{contractId}/actions", content);
        }
    }

    public class Action
    {
        public int workflowFunctionId { get; set; }
        public List<Workflowactionparameter> workflowActionParameters { get; set; }

        public Action() { }

        public Action(int functionId)
        {
            workflowFunctionId = functionId;
            workflowActionParameters = new List<Workflowactionparameter>();
        }

        public Action(int functionId, float temperature)
        {
            workflowFunctionId = functionId;
            workflowActionParameters = new List<Workflowactionparameter>();
            workflowActionParameters.Add(new Workflowactionparameter(temperature));
        }
    }

    public class Workflowactionparameter
    {
        public string name { get; set; }
        public float value { get; set; }

        public Workflowactionparameter() { }

        public Workflowactionparameter(float temperature)
        {
            name = "temperature";
            value = temperature;
        }
    }

    class Serializer
    {
        public static string SerializeToJson<T>(T obj)
        {
            MemoryStream jsonMemoryStream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
            jsonSerializer.WriteObject(jsonMemoryStream, obj);
            jsonMemoryStream.Position = 0;
            StreamReader jsonStreamReader = new StreamReader(jsonMemoryStream);
            string json = jsonStreamReader.ReadToEnd();
            jsonMemoryStream.Close();
            return json;
        }

        public static T DeserializeFromJson<T>(string json)
        {
            MemoryStream jsonMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            T obj = (T)jsonSerializer.ReadObject(jsonMemoryStream);
            jsonMemoryStream.Close();
            return obj;
        }
    }
}
