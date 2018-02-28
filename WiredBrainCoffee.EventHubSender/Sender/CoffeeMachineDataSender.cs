using System;
using System.Threading.Tasks;
using WiredBrainCoffee.EventHubSender.Model;

namespace WiredBrainCoffee.EventHubSender.Sender
{

    public class CoffeeMachineDataSender : ICoffeeMachineDataSender
    {
        public Task SendDataAsync(CoffeeMachineData data)
        {
            throw new NotImplementedException();
        }
    }
}
