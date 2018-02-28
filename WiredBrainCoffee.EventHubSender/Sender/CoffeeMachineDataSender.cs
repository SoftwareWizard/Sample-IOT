using System;
using System.Collections.Generic;
using System.Linq;
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
            var eventData = CreateEventData(data);
            await _eventHubClient.SendAsync(eventData);
        }

        public async Task SendDataAsync(IEnumerable<CoffeeMachineData> datas)
        {
            var eventDatas = datas.Select(CreateEventData);
            await _eventHubClient.SendAsync(eventDatas);
        }

        private static EventData CreateEventData(CoffeeMachineData data)
        {
            var dataAsJson = JsonConvert.SerializeObject(data);
            var eventData = new EventData(Encoding.UTF8.GetBytes(dataAsJson));
            return eventData;
        }
    }
}
