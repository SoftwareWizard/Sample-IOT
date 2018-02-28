using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using WiredBrainCoffee.EventHubSender.Model;

namespace WiredBrainCoffee.EventHubSender.Sender
{

    public class CoffeeMachineDataSender : ICoffeeMachineDataSender
    {
        private readonly EventHubClient _eventHubClient;

        public CoffeeMachineDataSender(string connectionString)
        {
            _eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);
        }

        public async Task SendDataAsync(CoffeeMachineData data)
        {
            var dataAsJson = JsonConvert.SerializeObject(data);
            var eventData = new EventData(Encoding.UTF8.GetBytes(dataAsJson));
            await _eventHubClient.SendAsync(eventData);
        }
    }
}
