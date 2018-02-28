using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using WiredBrainCoffee.EventHubSender.Model;
using WiredBrainCoffee.EventHubSender.Sender;

// ReSharper disable MemberCanBePrivate.Global
namespace WiredBrainCoffee.MachineSimulator.UI.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly ICoffeeMachineDataSender _sender;
        private int _beanLevel;
        private int _boilerTemp;
        private string _city;
        private int _counterCappucino;
        private int _counterEspresso;
        private bool _isSending;
        private string _serialNumber;

        public MainViewModel(ICoffeeMachineDataSender sender)
        {
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 8);
            MakeCappucinoCommand = new DelegateCommand(MakeCappucino);
            MakeEspressoCommand = new DelegateCommand(MakeEspresso);
            Logs = new ObservableCollection<string>();
            _sender = sender;
            _dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _dispatcherTimer.Tick += DispatcherTimerOnTick;
        }

        public ICommand MakeCappucinoCommand { get; }
        public ICommand MakeEspressoCommand { get; }

        public ObservableCollection<string> Logs { get; set; }

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

        public int BeanLevel
        {
            get => _beanLevel;
            set => SetProperty(ref _beanLevel, value);
        }

        public int BoilerTemp
        {
            get => _boilerTemp;
            set => SetProperty(ref _boilerTemp, value);
        }

        public bool IsSending
        {
            get => _isSending;
            set
            {
                SetTimer(value);
                SetProperty(ref _isSending, value);
            }
        }

        private async void MakeEspresso()
        {
            try
            {
                CounterEspresso++;
                Console.WriteLine($@"Counter Espresso: {CounterEspresso}");
                var coffeeMachineData = CreateCoffeeMachineData(nameof(CounterEspresso), CounterEspresso);
                await SendData(coffeeMachineData);
                WriteLog(coffeeMachineData.ToString());
            }
            catch (Exception exception)
            {
                WriteLog(exception.Message);
            }
        }

        private async void MakeCappucino()
        {
            try
            {
                CounterCappucino++;
                Console.WriteLine($@"Counter Cappucino: {CounterCappucino}");
                var coffeeMachineData = CreateCoffeeMachineData(nameof(CounterCappucino), CounterCappucino);
                await SendData(coffeeMachineData);
                WriteLog(coffeeMachineData.ToString());
            }
            catch (Exception exception)
            {
                WriteLog(exception.Message);
            }
        }

        private CoffeeMachineData CreateCoffeeMachineData(string sensorType, int sensorValue)
        {
            return new CoffeeMachineData
            {
                City = City,
                SerialNumber = SerialNumber,
                SensorType = sensorType,
                SensorValue = sensorValue,
                RecordingTime = DateTime.Now
            };
        }

        private async Task SendData(CoffeeMachineData coffeeMachineData)
        {
            await _sender.SendDataAsync(coffeeMachineData);
        }

        private async Task SendData(List<CoffeeMachineData> coffeeMachineDatas)
        {
            await _sender.SendDataAsync(coffeeMachineDatas);
        }

        private void WriteLog(string logData)
        {
            Logs.Insert(0, logData);
        }

        private void SetTimer(bool shouldSend)
        {
            if (shouldSend)
                _dispatcherTimer.Start();
            else
                _dispatcherTimer.Stop();
        }

        private async void DispatcherTimerOnTick(object sender, EventArgs eventArgs)
        {
            var boilerTempData = CreateCoffeeMachineData(nameof(BoilerTemp), BoilerTemp);
            var beanLevelData = CreateCoffeeMachineData(nameof(BeanLevel), BeanLevel);
            await SendData(new List<CoffeeMachineData> { boilerTempData, beanLevelData });

            WriteLog(boilerTempData.ToString());
            WriteLog(beanLevelData.ToString());
        }

    }
}