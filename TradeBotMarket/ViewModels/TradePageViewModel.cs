using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using TradeBotMarket.Infrastructure.Commands;
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
            set
            {
                Set(ref _pair, value);
                _ = FillTable();
                if (_bitfinexService.IsSubscribed())
                {
                    ExecuteUnsubscribeFromTrades(1);
                }
            }
        }

        private ObservableCollection<Trade> _Trades = new();
        public ObservableCollection<Trade> Trades
        {
            get => _Trades;
            private set => Set(ref _Trades, value);
        }

        public TradePageViewModel()
        {
            SubscribeToTrades = new LambdaCommand(ExecuteSubscribeToTrades, CanExecute);
            UnSubscribeToTrades = new LambdaCommand(ExecuteUnsubscribeFromTrades, CanExecute);
            _ = FillTable();
        }

        public ICommand SubscribeToTrades { get; }
        public ICommand UnSubscribeToTrades { get; }

        private void ExecuteSubscribeToTrades(object parameter)
        {
            if (_bitfinexService.IsSubscribed())
            {
                MessageBox.Show("Сначала отпишитесь от других сокетов");
            }
            else
            {
                _bitfinexService.NewBuyTrade += OnNewTrade;
                _bitfinexService.NewSellTrade += OnNewTrade;

                _ = _bitfinexService.SubscribeTrades($"t{_pair}");
            }
        }
        private void ExecuteUnsubscribeFromTrades(object parameter)
        {
            _ = _bitfinexService.UnsubscribeTrades($"t{_pair}");
            MessageBox.Show("Вы отсоеденились от сокета");
        }
        private void OnNewTrade(Trade trade)
        {
            trade.Pair = trade.Pair.Remove(0, 1);
            Trades.Insert(0, trade);
        }

        private async Task FillTable()
        {
            Trades = new ObservableCollection<Trade>(await _bitfinexService.GetNewTradesAsync(_pair, 125));
        }

        private bool CanExecute(object parameter) => true;
    }
}
