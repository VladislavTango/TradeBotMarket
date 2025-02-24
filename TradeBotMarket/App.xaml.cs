using System.Windows;
using System.Windows.Threading;
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

            navigationService.RegisterPage("TradePage", () => new TradePage());
            navigationService.RegisterPage("CandlePage", () => new CandlePage());
            navigationService.RegisterPage("BalancePage", () => new BalancePage());

            var mainViewModel = new MainWindowViewModel(navigationService);

            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }
    }
}
