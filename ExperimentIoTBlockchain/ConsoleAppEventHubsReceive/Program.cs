using System;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;

namespace ConsoleAppEventHubsReceive
{
    class Program
    {
        private const string EventHubConnectionString = "Endpoint=sb://xxxxxxxxx.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xxxxxxxxx";
        private const string EventHubName = "experiment_iot"; // Event Hub Entity Name
        private const string StorageContainerName = "experiment-event-hubs";
        private const string StorageAccountName = "experimenteventhubs";
        private const string StorageAccountKey = "xxxxxxxxx";

        private static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                EventHubName,
                PartitionReceiver.DefaultConsumerGroupName,
                EventHubConnectionString,
                StorageConnectionString,
                StorageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}

/*
Message received. Partition: '0', Data: 
'{
    "id":"vjzvzy",
    "name":"Experiment MXChip",
    "simulated":false,
    "deviceId":"605c643c-d939-4b28-ad42-6d33e3a093ba",
    "deviceTemplate":
    {
        "id":"130772c7-97dd-4a76-bbdb-9209888293f6",
        "version":"1.0.0"
    },
    "properties":
    {
        "device":
        {
            "dieNumber":6,
            "location":
            {
                "lon":-122.33077,
                "lat":47.60759
            }
        }
    },
    "settings":
    {
        "device":
        {
            "setCurrent":0,
            "setVoltage":0,
            "fanSpeed":0,
            "activateIR":false
        }
    }
}'

Message received. Partition: '1', Data: 
'{
    "humidity":35.1,
    "temp":32.1,
    "pressure":935.1,
    "magnetometerX":143.1,
    "magnetometerY":-153.1,
    "magnetometerZ":-319.1,
    "accelerometerX":3.1,
    "accelerometerY":-3.1,
    "accelerometerZ":1000.1,
    "gyroscopeX":-2660.1,
    "gyroscopeY":910.1,
    "gyroscopeZ":980.1,
    "deviceState":"DANGER"
}'
*/
