using System;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs.Processor;

namespace WiredBrainCoffee.EventHub.Processor
{
    public static class Program
    {
        private const string eventHubPath = "wiredbraincoffeeeh";
        private const string consumerGroupName = "wired_brain_coffee_console_processor";
        private const string eventHubConnectionString = "Endpoint=sb://wiredbraincoffeeeh-nspace.servicebus.windows.net/;" +
                                                        "SharedAccessKeyName=SendAndListenPolicy;" +
                                                        "SharedAccessKey=rH6DE3pOI3WOsnOqxWhhcT0TAkt8dHvcTa6A1vhCCjk=;" +
                                                        "EntityPath=wiredbraincoffeeeh";
        private const string storageConnectionString = "DefaultEndpointsProtocol=https;" +
                                                       "AccountName=wiredbraincoffeestorage1;" +
                                                       "AccountKey=GzjK+I3FcD01EpF+qi+zBOW54QEaPpl4q7Fo/nuyofp+mOT8237SfjbEMk5LQUCTWSd10SboqjAH+WQdpSv47g==;" +
                                                       "EndpointSuffix=core.windows.net";
        private const string leaseContainerName = "process-checkpoint";

        public static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine($"Register the {nameof(WiredBrainCoffeeEventProcessor)}");

            var eventProcessorHost = new EventProcessorHost(
                eventHubPath, 
                consumerGroupName,
                eventHubConnectionString,
                storageConnectionString,
                leaseContainerName);

            await eventProcessorHost.RegisterEventProcessorAsync<WiredBrainCoffeeEventProcessor>();

            Console.WriteLine($"Waiting fo incoming events...");
            Console.WriteLine("Press any key to shutdown");
            Console.ReadLine();

            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}
