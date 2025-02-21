using System.Collections.ObjectModel;
using System.Net.Http;
using TradeBotMarket.Models;
using TradeBotMarket.Services;
using TradeBotMarket.ViewModels.Base;

namespace TradeBotMarket.ViewModels
{
    public class Page2ViewModel : ViewModel
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private string _Title = "TradeBotMarket";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        private string _period = "1m";
        public string Period
        {
            get => _period;
            set => Set(ref _period, value);
        }
        private string _pair = "BTCUSD";
        public string Pair
        {
            get => _pair;
            set { Set(ref _pair, value);}
        }
        private DateTimeOffset _from = DateTimeOffset.UtcNow.AddDays(-1);
        public DateTimeOffset From
        {
            get => _from;
            set { Set(ref _from, value); }
        }
        private DateTimeOffset _to = DateTimeOffset.UtcNow;
        public DateTimeOffset To
        {
            get => _to;
            set { Set(ref _to, value); }
        }

        private ObservableCollection<Candle> _Candles = new();
        public ObservableCollection<Candle> Candles
        {
            get => _Candles;
            private set => Set(ref _Candles, value);
        }


        public Page2ViewModel()
        {

            _ = FillTable();
        }

        private async Task FillTable()
        {
            BitfinexService bitfinexService = new(_httpClient);
            Candles = new ObservableCollection<Candle>(await bitfinexService.GetCandleSeriesAsync(Pair, Period, From,To,125));
        }
    }
}
