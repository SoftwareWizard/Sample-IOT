using System.Configuration;
using System.Windows;
using WiredBrainCoffee.EventHubSender.Sender;
using WiredBrainCoffee.MachineSimulator.UI.ViewModel;

namespace WiredBrainCoffee.MachineSimulator.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["eventHub"].ConnectionString;
            DataContext = new MainViewModel(new CoffeeMachineDataSender(connectionString));
        }
    }
}
