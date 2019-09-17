using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleAppServiceBusReceive.Blockchain;
using Microsoft.Azure.ServiceBus;
using Action = ConsoleAppServiceBusReceive.Blockchain.Action;

namespace ConsoleAppServiceBusReceive
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://xxxxxxxxx.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xxxxxxxxx";
        const string QueueName = "experiment_iot_queue";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var api = new ApiTasks();
            await api.GetApplicationsAsync();
            await api.GetApplicationAsync(4);
            //await api.GetUsersAsync();
            //await api.GetMeAsync();
            //await api.GetRoleAssignmentsAsync(4);
            await api.GetWorkflowsAsync(4);
            await api.GetWorkflowAsync(4);
            //await api.GetContractsAsync();
            //await api.GetContractAsync(10);
            //await api.GetContractActionsAsync(10);

            /**
             * User 'Me' without FirstName or LastName or EmailAddress
             * Post User API request don't work for an existing user
             * Add trusted IP client to Blockchain Workbench SQL Server
             * Modify Blockchain Workbench SQL Database (SQL Editor)
            **/
            //User user = new User();
            //user.externalID = "373e51d8-53f6-4774-aa58-394092b5195d";
            //user.emailAddress = "api_admin@test.com";
            //user.firstName = "Api";
            //user.lastName = "Admin";
            //string userJson = Helpers.Serializer.SerializeToJson(user);
            //await api.PostUserAsync(userJson);

            /**
             * Add new Role to the Current API User
            **/
            //Role role = new Role();
            //role.applicationRoleId = 5;
            //role.userId = 11;
            //string roleJson = Helpers.Serializer.SerializeToJson(role);
            //await api.PostRoleAssignments(3, roleJson);

            /**
             * Take Action: Organ In Transit
            **/
            //Action transit = new Action(13);
            //string transitJson = Helpers.Serializer.SerializeToJson(transit);
            //await api.PostContractActionAsync(11, transitJson);

            /**
             * Take Action Temperature Check with telemetry values
            **/
            //Action telemetry = new Action(15, 35);
            //string telemetryJson = Helpers.Serializer.SerializeToJson(telemetry);
            //await api.PostContractActionAsync(11, telemetryJson);

            Console.ReadKey();

            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            Console.WriteLine("=======================");
            Console.WriteLine("Press ENTER key to exit");
            Console.WriteLine("=======================");

            // Register the queue message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await queueClient.CloseAsync();
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            // Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            var body = Encoding.UTF8.GetString(message.Body);

            var api = new ApiTasks();

            if (body.Contains("temp"))
            {
                var telemetry = Helpers.Serializer.DeserializeFromJson<Messages.Telemetry>(body);
                Console.WriteLine("\nREAD: OK");
                Console.WriteLine("TEMP: " + telemetry.temp + "\n");

                Action send = new Action(15, (int)telemetry.temp);
                string sendJson = Helpers.Serializer.SerializeToJson(send);
                await api.PostContractActionAsync(11, sendJson);
            }

            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        // Use this handler to examine the exceptions received on the message pump.
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
