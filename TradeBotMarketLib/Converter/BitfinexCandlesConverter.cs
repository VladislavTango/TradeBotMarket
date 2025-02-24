using System.Text.Json.Serialization;
using System.Text.Json;
using TradeBotMarket.Models;

namespace TradeBotMarket.Infrastructure.Converters
{
    public class BitfinexCandleConverter : JsonConverter<Candle>
    {
        private readonly string _pair;

        public BitfinexCandleConverter(string pair)
        {
            _pair = pair;
        }
        public override Candle Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elements = JsonSerializer.Deserialize<List<JsonElement>>(ref reader);

            return new Candle
            {
                OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(elements[0].GetInt64()),
                OpenPrice = elements[1].GetDecimal(),
                ClosePrice = elements[2].GetDecimal(),
                HighPrice = elements[3].GetDecimal(),
                LowPrice = elements[4].GetDecimal(),
                TotalVolume = elements[5].GetDecimal(),
                TotalPrice = (elements[1].GetDecimal() + elements[2].GetDecimal() +
                             elements[3].GetDecimal() + elements[4].GetDecimal()) / 4 *
                             elements[5].GetDecimal(),
                Pair = _pair
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            Candle value,
            JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
