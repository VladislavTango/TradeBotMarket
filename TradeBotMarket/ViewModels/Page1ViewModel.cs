using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using TradeBotMarket.Infrastructure;
using TradeBotMarket.Models;
using TradeBotMarket.Services;
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


        public Page1ViewModel()
        {
           
            _ = FillTable(); 
        }

        private async Task FillTable()
        {
            HttpClient httpClient = new();
            BitfinexService _bitfinexService = new(httpClient);
            Trades = new ObservableCollection<Trade>(await _bitfinexService.GetNewTradesAsync(_pair, 125));
        }
    }
}
