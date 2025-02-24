using TradeBotMarket.Models;
using TradeBotMarket.Models.Interfaces;
using TradeBotMarketLib.Services.Clients;

namespace TradeBotMarketLib.Services
{
    public class BitfinexConnector : ITestConnector
    {
        private readonly BitfinexRestService _restClient;
        private readonly BitFinexWebSocketService _webSocketClient;

        public event Action<Trade> NewTrade;
        public event Action<Candle> NewCandle;

        public BitfinexConnector(BitfinexRestService restClient, BitFinexWebSocketService webSocketClient)
        {
            _restClient = restClient;
            _webSocketClient = webSocketClient;

        }

        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, string period, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            return await _restClient.GetCandleSeriesAsync(pair, period, from, to, count);
        }

        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            return await _restClient.GetNewTradesAsync(pair, maxCount);
        }

        public async Task<decimal> GetTicker(string pair)
        {
            return await _restClient.GetTicker(pair);
        }
        #region socket
        public async Task SubscribeTrades(string pair)
        {
            _webSocketClient.NewTrade += trade => NewTrade.Invoke(trade);

            await _webSocketClient.ConnectAsync();
            await _webSocketClient.SubscribeToTradesAsync(pair);
        }
        public async Task SubscribeCandles(string pair, string period)
        {
            await _webSocketClient.ConnectAsync();
            await _webSocketClient.SubscribeToCandlesAsync(pair, period);

            _webSocketClient.NewCandle += candle => NewCandle.Invoke(candle);
        }

        public async Task Unsubscribe()
        {
            await _webSocketClient.UnsubscribeAsync();
        }

        public bool IsSubscribed()
        {
            return _webSocketClient.IsSubscribed();
        }
        #endregion
    }
}
