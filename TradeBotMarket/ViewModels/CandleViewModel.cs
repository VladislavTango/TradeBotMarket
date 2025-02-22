using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using TradeBotMarket.Models;
using TradeBotMarket.Services;
using TradeBotMarket.ViewModels.Base;

namespace TradeBotMarket.ViewModels
{
    public class CandleViewModel : ViewModel
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly BitfinexService _bitfinexService = new(_httpClient);

        private string _period = "1m";
        public string Period
        {
            get => _period;
            set { Set(ref _period, value);_ = FillTable(); }
        }
        private string _pair = "BTCUSD";
        public string Pair
        {
            get => _pair;
            set { Set(ref _pair, value); _ = FillTable(); }

        }
        private DateTimeOffset _from = DateTimeOffset.UtcNow.AddDays(-1);
        public DateTimeOffset From
        {
            get => _from;
            set {
                if (value > _to)
                    MessageBox.Show($"Дата конца должна быть больше даты начала {_from} конец : {_to}");
                else
                {
                    Set(ref _from, value);
                    _= FillTable();
                }

            }

        }
        private DateTimeOffset _to = DateTimeOffset.UtcNow;
        public DateTimeOffset To
        {
            get => _to;
            set
            {
                if (_from > value)
                    MessageBox.Show("Дата конца должна быть больше даты начала");
                else
                {
                    Set(ref _to, value);
                    _ = FillTable();
                }

            }
        }

        private ObservableCollection<Candle> _Candles = new();
        public ObservableCollection<Candle> Candles
        {
            get => _Candles;
            private set => Set(ref _Candles, value);
        }


        public CandleViewModel()
        {

            _ = FillTable();
        }

        private async Task FillTable()
        {
            Candles = new ObservableCollection<Candle>(await _bitfinexService.GetCandleSeriesAsync(Pair, Period, From,To,125));
        }
    }
}
