using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using TradeBotMarket.Models;
using TradeBotMarket.Infrastructure.Converters;

public class BitFinexWebSocketService
{
    private readonly ClientWebSocket _ws = new ClientWebSocket();
    private readonly Uri _uri = new Uri("wss://api.bitfinex.com/ws/2");
    private readonly string _pair;

    public event Action<Trade> NewTrade;

    public event Action<Candle> NewCandle;

    private bool _isSubscribed = false;
    private string _currentChannel;
    private string _currentPair;

    public BitFinexWebSocketService(string pair)
    {
        _pair = pair;
    }

    public async Task ConnectAsync()
    {
        await _ws.ConnectAsync(_uri, CancellationToken.None);
        _ = ReceiveMessagesAsync();
    }

    public bool IsSubscribed()
    {
        return _isSubscribed;
    }

    public async Task SubscribeToTradesAsync(string pair)
    {
        _currentChannel = "trades";
        _currentPair = pair;
        await SubscribeAsync(pair, _currentChannel);
        _isSubscribed = true;
    }

    public async Task SubscribeToCandlesAsync(string pair, string timeframe)
    {
        _currentChannel = "candles";
        _currentPair = pair;
        await SubscribeAsync(pair, _currentChannel, timeframe);
        _isSubscribed = true;
    }

    public async Task UnsubscribeAsync()
    {
        _isSubscribed = false;
    }

    private async Task SubscribeAsync(string pair, string channel, string timeframe = null)
    {
        var subscribeMessage = new
        {
            @event = "subscribe",
            channel = channel,
            symbol = pair,
            key = $"trade:{timeframe}:t{pair}"
        };

        string jsonMessage = JsonSerializer.Serialize(subscribeMessage);
        byte[] buffer = Encoding.UTF8.GetBytes(jsonMessage);
        await _ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[2048];

        while (_ws.State == WebSocketState.Open)
        {
            var message = new StringBuilder();
            WebSocketReceiveResult result;
            do
            {
                result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var messageChunk = Encoding.UTF8.GetString(buffer, 0, result.Count);
                message.Append(messageChunk);
            } while (!result.EndOfMessage);

            string jsonResponse = message.ToString();
            ProcessData(jsonResponse);
        }
    }

    private void ProcessData(string json)
    {
        try
        {
            if (!_isSubscribed) return;

            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Object) return;

            if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 1)
            {
                if (_currentChannel == "candles")
                {
                    if (root[1].ValueKind == JsonValueKind.String && root[1].GetString() == "hb") return;

                    if (root[1].ValueKind == JsonValueKind.Array && root[1].GetArrayLength() >= 6)
                    {
                        ProcessCandleData(root[1]);
                    }
                }
                else if (_currentChannel == "trades" && root[1].GetString() == "te")
                {
                    ProcessTradeData(root[2]);
                }
            }
        }
        catch (Exception ex){ }
    }

    private void ProcessTradeData(JsonElement tradeData)
    {
        if (tradeData.ValueKind == JsonValueKind.Array && tradeData.GetArrayLength() == 4)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new BitfinexTradeConverter(_currentPair) }
            };

            var trade = JsonSerializer.Deserialize<Trade>(tradeData.GetRawText(), options);

            NewTrade.Invoke(trade);
        }
    }

    private void ProcessCandleData(JsonElement candleData)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new BitfinexCandleConverter(_currentPair) }
            };

            var candle = JsonSerializer.Deserialize<Candle>(candleData.GetRawText(), options);

            NewCandle?.Invoke(candle);
        }
        catch (Exception ex) { }
    }
}