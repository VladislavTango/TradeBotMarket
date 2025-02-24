using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using TradeBotMarket.Infrastructure.Commands;
using TradeBotMarket.Models;
using TradeBotMarket.ViewModels.Base;
using TradeBotMarketLib.Services;
using TradeBotMarketLib.Services.Clients;

namespace TradeBotMarket.ViewModels
{
    public class TradePageViewModel : ViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly BitfinexRestService _restClient;
        private readonly BitFinexWebSocketService _webSocketClient;
        private readonly BitfinexConnector _bitfinexConnector;

        private string _pair = "BTCUSD";
        public string Pair
        {
            get => _pair;
            set
            {
                Set(ref _pair, value);
                _ = FillTable();
                if (_bitfinexConnector.IsSubscribed())
                {
                    ExecuteUnsubscribeFromTrades(null);
                }
            }
        }

        private ObservableCollection<Trade> _trades = new();
        public ObservableCollection<Trade> Trades
        {
            get => _trades;
            private set => Set(ref _trades, value);
        }

        public TradePageViewModel()
        {
            _httpClient = new HttpClient();
            _restClient = new BitfinexRestService(_httpClient);
            _webSocketClient = new BitFinexWebSocketService(_pair);
            _bitfinexConnector = new BitfinexConnector(_restClient, _webSocketClient);
            SubscribeToTrades = new LambdaCommand(ExecuteSubscribeToTrades, CanExecute);
            UnSubscribeToTrades = new LambdaCommand(ExecuteUnsubscribeFromTrades, CanExecute);
            _ = FillTable();
        }

        public ICommand SubscribeToTrades { get; }
        public ICommand UnSubscribeToTrades { get; }

        private void ExecuteSubscribeToTrades(object parameter)
        {
            if (_bitfinexConnector.IsSubscribed())
            {
                MessageBox.Show("Сначала отпишитесь от других сокетов");
            }
            else
            {
                _bitfinexConnector.NewTrade += OnNewTrade;
                _ = _bitfinexConnector.SubscribeTrades(_pair);
            }
        }

        private async void ExecuteUnsubscribeFromTrades(object parameter)
        {
            await _bitfinexConnector.Unsubscribe();
            MessageBox.Show("Вы отсоединились от сокета");
        }

        private void OnNewTrade(Trade trade)
        {
            Trades.Insert(0, trade);
        }

        private async Task FillTable()
        {
            Trades = new ObservableCollection<Trade>(await _bitfinexConnector.GetNewTradesAsync(_pair, 125));
        }

        private bool CanExecute(object parameter) => true;
    }
}