using System.Threading.Tasks;
using WiredBrainCoffee.EventHubSender.Model;

namespace WiredBrainCoffee.EventHubSender.Sender
{
    public interface ICoffeeMachineDataSender
    {
        Task SendDataAsync(CoffeeMachineData data);
    }
}