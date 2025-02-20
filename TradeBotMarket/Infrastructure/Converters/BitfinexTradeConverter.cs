using System.Text.Json;
using System.Text.Json.Serialization;
using TradeBotMarket.Models;

namespace TradeBotMarket.Infrastructure.Converters
{
    public class BitfinexTradeConverter : JsonConverter<Trade>
    {
        public override Trade Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elements = JsonSerializer.Deserialize<List<JsonElement>>(ref reader);

            return new Trade
            {
                Id = elements[0].GetInt64().ToString(),
                Time = DateTimeOffset.FromUnixTimeMilliseconds(elements[1].GetInt64()),
                Amount = elements[2].GetDecimal(),
                Price = elements[3].GetDecimal(),
                Side = elements[2].GetDecimal() > 0 ? "buy" : "sell"
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            Trade value,
            JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
