namespace TradeBotMarket.Models.Interfaces
{
    public interface ITestConnector
    {
        #region Rest

        Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);
        /// <summary>
        ///  поменял тип на string т.к. api ест только 
        ///  "1m" "5m" "15m" "30m" "1h" "3h" "6h" "12h" "1D" "7D" "14D" "1M"
        ///  секунды он не переваривает
        /// </summary>
        /// <param name="periodInSec"></param>
        Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, string periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0);
        /// <summary>
        /// Rest метод для получение информации о тикере (3 пункт первого задания)
        /// </summary>
        Task<Ticker> GetTicker(string pair);
        #endregion

        #region Socket


        event Action<Trade> NewBuyTrade;
        event Action<Trade> NewSellTrade;
        void SubscribeTrades(string pair, int maxCount = 100);
        void UnsubscribeTrades(string pair);

        event Action<Candle> CandleSeriesProcessing;
        void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0);
        void UnsubscribeCandles(string pair);

        #endregion

    }
}
