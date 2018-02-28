using System.Windows;
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
            DataContext = new MainViewModel();
        }
    }
}
