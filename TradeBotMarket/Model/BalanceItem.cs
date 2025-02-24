using TradeBotMarket.ViewModels.Base;

namespace TradeBotMarket.Models
{
    public class BalanceItem : ViewModel
    {
        private string _asset;
        private decimal _balance;

        public string Asset
        {
            get => _asset;
            set => Set(ref _asset, value);
        }

        public decimal Balance
        {
            get => _balance;
            set => Set(ref _balance, value);
        }
    }

}
