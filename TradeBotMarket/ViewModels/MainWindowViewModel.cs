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
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private readonly NavigationService _navigationService;

        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set => Set(ref _currentPage, value);
        }

        public MainWindowViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateToPage1Command = new LambdaCommand(ExecuteNavigateToPage1, CanExecuteNavigate);
            NavigateToPage2Command = new LambdaCommand(ExecuteNavigateToPage2, CanExecuteNavigate);

            _navigationService.NavigateTo("Page2");
            CurrentPage = _navigationService.CurrentPage;
        }

        public ICommand NavigateToPage1Command { get; }
        public ICommand NavigateToPage2Command { get; }

        private void ExecuteNavigateToPage1(object parameter)
        {
            _navigationService.NavigateTo("Page1");
            CurrentPage = _navigationService.CurrentPage;
        }

        private void ExecuteNavigateToPage2(object parameter)
        {
            _navigationService.NavigateTo("Page2");
            CurrentPage = _navigationService.CurrentPage;
        }

        private bool CanExecuteNavigate(object parameter) => true;
    }
}
