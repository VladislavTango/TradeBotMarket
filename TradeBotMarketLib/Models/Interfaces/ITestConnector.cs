﻿namespace TradeBotMarket.Models.Interfaces
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
        Task<decimal> GetTicker(string pair);
        #endregion

        #region Socket
        /// <summary>
        /// поменял все void на таски из-за того что всю работу с сокетами вынес в отдельный класс
        /// (сделал так потому что не хотел 500 строчек в 1 файле)
        /// еще поменял как и выше periodInSec(int) просто на period(string) потому-что api
        /// всё-равно ест только строки да и писать что-то вроде switch(sec){case 60:"1m"} не хочется
        /// </summary>

        event Action<Trade> NewTrade;
        /// <summary>
        /// когда я подключаюсь к сокету трейда у него нет разделения на buy и sell 
        /// там просто приходит te , tu ,а дальше я разбираюсь что это 
        /// поэтому 2 евента тут не нужно , хотя если у других апи
        /// есть разделение то второй евент бы не помешал
        /// </summary>
        Task SubscribeTrades(string pair);
        Task Unsubscribe();

        event Action<Candle> NewCandle;
        Task SubscribeCandles(string pair, string period);
        #endregion

    }
}
