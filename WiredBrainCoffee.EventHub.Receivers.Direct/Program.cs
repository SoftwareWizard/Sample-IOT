using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace WiredBrainCoffee.EventHub.Receivers.Direct
{
    public static class Program
    {
        private static EventHubClient _eventHubClient;

        public static async Task Main(string[] args)
        {
            try
            {
                _eventHubClient = CreateEventHubClient();
                await ReceiveData();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine("-- Press Key --");
            Console.ReadKey();
            _eventHubClient.Close();
        }

        private static async Task ReceiveData()
        {
            var runTimeInformation = await _eventHubClient.GetRuntimeInformationAsync();

            var partitionReceivers = runTimeInformation
                .PartitionIds
                .Select(id => _eventHubClient.CreateReceiver("wired_brain_coffee_console_direct", id, EventPosition.FromEnqueuedTime(DateTime.Now) ));

            Console.WriteLine("Waiting for incoming Data");
            foreach (var partitionReceiver in partitionReceivers)
            {
                partitionReceiver.SetReceiveHandler(new WiredBrainCoffeePartitionReceiveHandler(partitionReceiver.PartitionId));
            }
        }

        private static EventHubClient CreateEventHubClient()
        {
            Console.WriteLine("Connecting to EventHub");
            var eventHubConnectionString = ConfigurationManager.ConnectionStrings["eventHub"].ConnectionString;
            var eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString);
            return eventHubClient;
        }
    }
}