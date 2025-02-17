using TradeBotMarket.ViewModels.Base;

namespace TradeBotMarket.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private string _Title = "TradeBotMarket";
        public string Title {
            get => _Title;
            set => Set(ref _Title, value);
        }
    }
}
