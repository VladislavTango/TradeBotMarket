using AutoMapper;
using System.Net.Http;
using System.Text.Json;
using TradeBotMarket.Models;
using TradeBotMarket.Models.Interfaces;

namespace TradeBotMarket.Services
{
    public class BitfinexService : ITestConnector
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public BitfinexService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }


        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;
        public event Action<Candle> CandleSeriesProcessing;

        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            string url = $"https://api-pub.bitfinex.com/v2/candles/trade:{periodInSec}s:t{pair}/hist?limit={count}&start={from?.ToUnixTimeMilliseconds()}&end={to?.ToUnixTimeMilliseconds()}";
            var response = await _httpClient.GetStringAsync(url);
            var rawCandles = JsonSerializer.Deserialize<List<List<object>>>(response);
            return _mapper.Map<IEnumerable<Candle>>(rawCandles);
        }

        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            string url = $"https://api-pub.bitfinex.com/v2/trades/t{pair}/hist?limit={maxCount}";
            var response = await _httpClient.GetStringAsync(url);
            var rawTrades = JsonSerializer.Deserialize<List<List<object>>>(response);
            return _mapper.Map<IEnumerable<Trade>>(rawTrades);
        }

        public async Task<Ticker> GetTicker(string pair)
        {
            string url = pair.Length<6 ? $"https://api-pub.bitfinex.com/v2/ticker/t{pair}" : $"https://api-pub.bitfinex.com/v2/ticker/f{pair}";
            var response = await _httpClient.GetStringAsync(url);
            var rawTicker = JsonSerializer.Deserialize<List<object>>(response);
            return _mapper.Map<List<object>, Ticker>(rawTicker);
        }

        public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0)
        {
            throw new NotImplementedException();
        }

        public void SubscribeTrades(string pair, int maxCount = 100)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeCandles(string pair)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeTrades(string pair)
        {
            throw new NotImplementedException();
        }
    }
}
