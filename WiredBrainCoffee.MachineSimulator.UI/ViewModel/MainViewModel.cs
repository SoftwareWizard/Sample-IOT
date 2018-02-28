using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

// ReSharper disable MemberCanBePrivate.Global
namespace WiredBrainCoffee.MachineSimulator.UI.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private string _city;
        private int _counterCappucino = 0;
        private int _counterEspresso = 0;
        private string _serialNumber;

        public MainViewModel()
        {
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 8);
            MakeCappucinoCommand = new DelegateCommand(MakeCappucino);
            MakeEspressoCommand = new DelegateCommand(MakeEspresso);
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

        private void MakeEspresso()
        {
            CounterEspresso++;
            Console.WriteLine($@"Counter Espresso: {CounterEspresso}");
        }

        private void MakeCappucino()
        {
            CounterCappucino++;
            Console.WriteLine($@"Counter Cappucino: {CounterCappucino}");
        }
    }
}