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
            var rawTicker = JsonSerializer.Deserialize<List<List<object>>>(response);
            var ticker = new Ticker();
            if (rawTicker.Count > 10)
            {
                ticker.FRR = Convert.ToDecimal(rawTicker[0]);
                ticker.Bid = Convert.ToDecimal(rawTicker[1]);
                ticker.BidPeriod = Convert.ToInt32(rawTicker[2]);
                ticker.BidSize = Convert.ToDecimal(rawTicker[3]);
                ticker.Ask = Convert.ToDecimal(rawTicker[4]);
                ticker.AskPeriod = Convert.ToInt32(rawTicker[5]);
                ticker.AskSize = Convert.ToDecimal(rawTicker[6]);
                ticker.DailyChange = Convert.ToDecimal(rawTicker[7]);
                ticker.DailyChangePerc = Convert.ToDecimal(rawTicker[8]);
                ticker.LastPrice = Convert.ToDecimal(rawTicker[9]);
                ticker.Volume = Convert.ToDecimal(rawTicker[10]);
                ticker.High = Convert.ToDecimal(rawTicker[11]);
                ticker.Low = Convert.ToDecimal(rawTicker[12]);

                if (rawTicker.Count > 15 && rawTicker[15] != null)
                {
                    ticker.FRRAmountAvailable = Convert.ToDecimal(rawTicker[15]);
                }
            }
            else if (rawTicker.Count == 10)
            {
                ticker.Bid = Convert.ToDecimal(rawTicker[0]);
                ticker.BidSize = Convert.ToDecimal(rawTicker[1]);
                ticker.Ask = Convert.ToDecimal(rawTicker[2]);
                ticker.AskSize = Convert.ToDecimal(rawTicker[3]);
                ticker.DailyChange = Convert.ToDecimal(rawTicker[4]);
                ticker.DailyChangePerc = Convert.ToDecimal(rawTicker[5]);
                ticker.LastPrice = Convert.ToDecimal(rawTicker[6]);
                ticker.Volume = Convert.ToDecimal(rawTicker[7]);
                ticker.High = Convert.ToDecimal(rawTicker[8]);
                ticker.Low = Convert.ToDecimal(rawTicker[9]);
            }
            return ticker;
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
