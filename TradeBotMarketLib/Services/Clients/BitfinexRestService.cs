using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TradeBotMarket.Infrastructure.Converters;
using TradeBotMarket.Models;

namespace TradeBotMarketLib.Services.Clients
{
    public class BitfinexRestService
    {
        private readonly HttpClient _httpClient;
        public BitfinexRestService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, string period, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            var start = from != null ? $"start={from.Value.ToUnixTimeMilliseconds()}" : "";
            var end = to != null ? $"end={to.Value.ToUnixTimeMilliseconds()}" : "";
            var limit = count > 0 ? $"limit={count}" : "";
            var query = string.Join("&", new[] { start, end, limit }.Where(s => !string.IsNullOrEmpty(s)));
            var encodedPeriod = Uri.EscapeDataString($"trade:{period}:t{pair}");

            var url = $"https://api-pub.bitfinex.com/v2/candles/{encodedPeriod}/hist{(query.Length > 0 ? "?" + query : "")}";

            var options = new JsonSerializerOptions
            {
                Converters = { new BitfinexCandleConverter(pair) }
            };

            var candles = await _httpClient.GetFromJsonAsync<List<Candle>>(url, options);


            return candles;
        }

        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            string url = $"https://api-pub.bitfinex.com/v2/trades/t{pair}/hist?limit={maxCount}";

            var options = new JsonSerializerOptions
            {
                Converters = { new BitfinexTradeConverter(pair) }
            };

            var trades = await _httpClient.GetFromJsonAsync<List<Trade>>(url, options);

            return trades;
        }


        /// <summary>
        /// у bitfinex не работало с DASH поэтому исспользовал binance
        /// </summary>
        public async Task<decimal> GetTicker(string pair)
        {
            string url = $"https://api.binance.com/api/v3/ticker/price?symbol={pair}";
            string response = await _httpClient.GetStringAsync(url);

            JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;

            var priceElement = root.GetProperty("price");
            string priceStr = priceElement.GetString();

            decimal.TryParse(priceStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal price);

            return price;
        }
    }
}
