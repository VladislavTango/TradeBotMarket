using System.Collections.ObjectModel;
using System.Net.Http;
using TradeBotMarket.Models;
using TradeBotMarket.Services;
using TradeBotMarket.ViewModels.Base;

namespace TradeBotMarket.ViewModels
{
    public class TradePageViewModel : ViewModel
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        BitfinexService _bitfinexService = new(_httpClient);


        private string _pair = "BTCUSD";
        public string Pair
        {
            get => _pair;
            set  { Set(ref _pair, value); _ = FillTable(); }
        }

        private ObservableCollection<Trade> _Trades = new();
        public ObservableCollection<Trade> Trades
        {
            get => _Trades;
            private set => Set(ref _Trades, value); 
        }

        public TradePageViewModel()
        {
           
            _ = FillTable(); 
        }

        private async Task FillTable()
        {
            Trades = new ObservableCollection<Trade>(await _bitfinexService.GetNewTradesAsync(_pair, 125));
        }
    }
}
