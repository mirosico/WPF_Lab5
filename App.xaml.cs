using System.Windows;
using Lab5_Mysko.Managers;
using Lab5_Mysko.Models;

namespace Lab5_Mysko
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow window = new MainWindow();
            NavigationManager.Instance.Initialise(new NavigationModel(window));
            NavigationManager.Instance.Navigate(Models.Views.ProcessesView);
            window.Show();
        }
    }
}