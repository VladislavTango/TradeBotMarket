using AutoMapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TradeBotMarket.Infrastructure.Converters;
using TradeBotMarket.Models;
using TradeBotMarket.Models.Interfaces;

namespace TradeBotMarket.Services
{
    public class BitfinexService : ITestConnector
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public BitfinexService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;
        public event Action<Candle> CandleSeriesProcessing;

        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            var start = from != null ? $"start={from.Value.ToUnixTimeMilliseconds()}" : "";
            var end = to != null ? $"end={to.Value.ToUnixTimeMilliseconds()}" : "";
            var limit = count > 0 ? $"limit={count}" : "";
            var query = string.Join("&", new[] { start, end, limit }.Where(s => !string.IsNullOrEmpty(s)));
            var url = $"https://api-pub.bitfinex.com/v2/candles/trade:{periodInSec}s:{pair}/hist{(query.Length > 0 ? "?" + query : "")}";
            
            var options = new JsonSerializerOptions
            {
                Converters = { new BitfinexCandleConverter() }
            };

            var candles = await _httpClient.GetFromJsonAsync<List<Candle>>(url, options);

            candles?.ForEach(x => x.Pair = pair);

            return candles;
        }

        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            string url = $"https://api-pub.bitfinex.com/v2/trades/t{pair}/hist?limit={maxCount}";

            var options = new JsonSerializerOptions
            {
                Converters = { new BitfinexTradeConverter() }
            };

            var trades = await _httpClient.GetFromJsonAsync<List<Trade>>(url, options);

            trades?.ForEach(x =>
            {
                x.Pair = pair;
                x.Amount = Math.Abs(x.Amount);
            });

            return trades;
        }



        public async Task<Ticker> GetTicker(string pair)
        {
            string url = pair.Length<6 ? $"https://api-pub.bitfinex.com/v2/ticker/t{pair}" : $"https://api-pub.bitfinex.com/v2/ticker/f{pair}";
            var response = await _httpClient.GetStringAsync(url);
            var rawTicker = JsonSerializer.Deserialize<List<object>>(response);
            throw new NotImplementedException();
            //return _mapper.Map<List<object>, Ticker>(rawTicker);
        }

        public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0)
        {
            throw new NotImplementedException();
        }

        public void SubscribeTrades(string pair, int maxCount = 100)
        {

            //wss://api-pub.bitfinex.com/ws/2
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
