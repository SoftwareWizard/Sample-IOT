using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using WiredBrainCoffee.EventHubSender.Model;
using WiredBrainCoffee.EventHubSender.Sender;

// ReSharper disable MemberCanBePrivate.Global
namespace WiredBrainCoffee.MachineSimulator.UI.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private readonly ICoffeeMachineDataSender _sender;
        private string _city;
        private int _counterCappucino;
        private int _counterEspresso;
        private string _serialNumber;

        public MainViewModel(ICoffeeMachineDataSender sender)
        {
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 8);
            MakeCappucinoCommand = new DelegateCommand(MakeCappucino);
            MakeEspressoCommand = new DelegateCommand(MakeEspresso);
            _sender = sender;
        }

        public ICommand MakeCappucinoCommand { get; }
        public ICommand MakeEspressoCommand { get; }

        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        public string SerialNumber
        {
            get => _serialNumber;
            set => SetProperty(ref _serialNumber, value);
        }

        public int CounterEspresso
        {
            get => _counterEspresso;
            set => SetProperty(ref _counterEspresso, value);
        }

        public int CounterCappucino
        {
            get => _counterCappucino;
            set => SetProperty(ref _counterCappucino, value);
        }

        private async void MakeEspresso()
        {
            CounterEspresso++;
            Console.WriteLine($@"Counter Espresso: {CounterEspresso}");
            var coffeeMachineData = CreateCoffeeMachineData(nameof(CounterEspresso), CounterEspresso);
            await SendData(coffeeMachineData);
        }

        private async void MakeCappucino()
        {
            CounterCappucino++;
            Console.WriteLine($@"Counter Cappucino: {CounterCappucino}");
            var coffeeMachineData = CreateCoffeeMachineData(nameof(CounterCappucino), CounterCappucino);
            await SendData(coffeeMachineData);
        }

        private CoffeeMachineData CreateCoffeeMachineData(string sensorType, int sensorValue)
        {
            return new CoffeeMachineData
            {
                City = City,
                SerialNumber = SerialNumber,
                Sensortype = sensorType,
                SensorValue = sensorValue,
                RecordingTime = DateTime.Now
            };
        }

        private async Task SendData(CoffeeMachineData coffeeMachineData)
        {
            await _sender.SendDataAsync(coffeeMachineData);
        }
    }
}