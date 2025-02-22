using System.Windows.Controls;
using System.Windows.Input;
using TradeBotMarket.Infrastructure.Commands;
using TradeBotMarket.Services;
using TradeBotMarket.ViewModels.Base;

namespace TradeBotMarket.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private string _Title = "TradeBotMarket";
        public string Title { get => _Title; set => Set(ref _Title, value); }

        private readonly NavigationService _navigationService;
        private Page _currentPage;
        public Page CurrentPage { get => _currentPage; set => Set(ref _currentPage, value); }

        private Dictionary<string, bool> _buttonStates;
        public Dictionary<string, bool> ButtonStates
        {
            get => _buttonStates;
            set => Set(ref _buttonStates, value);
        }

        public MainWindowViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            ButtonStates = new Dictionary<string, bool>
            {
                { "Trades", true },
                { "Candles", false },
                { "Balance", true }
            };

            NavigateToTradePageCommand = new LambdaCommand(ExecuteNavigateToTradePage, CanExecuteNavigate);
            NavigateToCandlePageCommand = new LambdaCommand(ExecuteNavigateToCandlePage, CanExecuteNavigate);
            NavigateToBalancePageCommand = new LambdaCommand(ExecuteNavigateToBalancePageCommand, CanExecuteNavigate);

            _navigationService.NavigateTo("CandlePage");
            CurrentPage = _navigationService.CurrentPage;
        }

        public ICommand NavigateToTradePageCommand { get; }
        public ICommand NavigateToCandlePageCommand { get; }
        public ICommand NavigateToBalancePageCommand { get; }

        private void ExecuteNavigateToTradePage(object parameter)
        {
            _navigationService.NavigateTo("TradePage");
            CurrentPage = _navigationService.CurrentPage;

            UpdateButtonStates("Trades");
        }
        private void ExecuteNavigateToBalancePageCommand(object parameter)
        {
            _navigationService.NavigateTo("BalancePage");
            CurrentPage = _navigationService.CurrentPage;

            UpdateButtonStates("Balance");
        }
        private void ExecuteNavigateToCandlePage(object parameter)
        {
            _navigationService.NavigateTo("CandlePage");
            CurrentPage = _navigationService.CurrentPage;

            UpdateButtonStates("Candles");
        }

        private void UpdateButtonStates(string buttonName)
        {
            foreach (var key in ButtonStates.Keys.ToList())
            {
                ButtonStates[key] = key != buttonName;
            }
            OnPropertyChanged(nameof(ButtonStates));
        }

        private bool CanExecuteNavigate(object parameter) => true;
    }
}