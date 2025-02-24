using System.Collections.ObjectModel;
using System.Net.Http;
using TradeBotMarket.Models;
using TradeBotMarket.Services;
using TradeBotMarket.ViewModels.Base;
using TradeBotMarketLib.Services.Clients;
using TradeBotMarketLib.Services;

namespace TradeBotMarket.ViewModels
{
    public class BalanceViewModel : ViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly BitfinexRestService _restClient;
        private readonly BitFinexWebSocketService _webSocketClient;
        private readonly BitfinexConnector _bitfinexConnector;

        public BalanceViewModel()
        {
            _httpClient = new HttpClient();
            _restClient = new BitfinexRestService(_httpClient);
            _webSocketClient = new BitFinexWebSocketService("");
            _bitfinexConnector = new BitfinexConnector(_restClient, _webSocketClient);
            _ = CalculatePortfolioBalances();
        }
        public IEnumerable<KeyValuePair<string, decimal>> CryptoBalances => new Dictionary<string, decimal>
        {
            { "BTC", 1 },
            { "XRP", 15000 },
            { "XMR", 50 },
            { "DASH", 30 }
        }.ToList(); 

        private readonly Dictionary<string, decimal> _cryptoBalances = new Dictionary<string, decimal>
        {
            { "BTC", 1 },
            { "XRP", 15000 },
            { "XMR", 50 },
            { "DASH", 30 }
        };

        private ObservableCollection<BalanceItem> _portfolioBalances = new ObservableCollection<BalanceItem>
         {
             new (){Asset = "USDT" , Balance = 0},
             new (){Asset = "BTC" , Balance = 0},
             new (){Asset = "XRP" , Balance = 0},
             new (){Asset = "XMR" , Balance = 0},
             new (){Asset = "DASH" , Balance = 0},
         };

        public ObservableCollection<BalanceItem> PortfolioBalances
        {
            get => _portfolioBalances;
            set => Set(ref _portfolioBalances, value);
        }

        public async Task CalculatePortfolioBalances()
        {
            foreach (var balance in _cryptoBalances) 
            {
                _portfolioBalances[0].Balance +=await _bitfinexConnector.GetTicker($"{balance.Key}USDT");
            }
            _portfolioBalances[0].Balance = Math.Round(_portfolioBalances[0].Balance, 3);
            foreach (var balance in _cryptoBalances)
            {
                await FromUSDTtoAny(balance.Key);
            }

        }
        public async Task FromUSDTtoAny(string symbol) 
        {
            _portfolioBalances.FirstOrDefault(x => x.Asset == symbol).Balance =
                Math.Round(_portfolioBalances[0].Balance / await _bitfinexConnector.GetTicker($"{symbol}USDT"),3);
        }
    }
}