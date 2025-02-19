using System.Windows;
using TradeBotMarket.Services;
using TradeBotMarket.ViewModels;
using TradeBotMarket.Views.Pages;

namespace TradeBotMarket
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var navigationService = new NavigationService();

            navigationService.RegisterPage("Page1", () => new Page1());
            navigationService.RegisterPage("Page2", () => new Page2());

            var mainViewModel = new MainWindowViewModel(navigationService);

            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }
    }

}
