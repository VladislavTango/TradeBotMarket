using TradeBotMarket.ViewModels.Base;

namespace TradeBotMarket.ViewModels
{
    public class Page1ViewModel : ViewModel
    {
        private string _Title = "TradeBotMarket";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
    }
}
