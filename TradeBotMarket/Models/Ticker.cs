namespace TradeBotMarket.Models
{
    public class Ticker
    {
        /// <summary>
        /// Flash Return Rate - средняя фиксированная ставка за последний час (опционально)
        /// </summary>
        public decimal? FRR { get; set; }

        /// <summary>
        /// Цена последней самой высокой заявки (BID)
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// Количество дней, покрываемых BID (опционально)
        /// </summary>
        public int? BidPeriod { get; set; }

        /// <summary>
        /// Сумма 25 самых высоких заявок
        /// </summary>
        public decimal BidSize { get; set; }

        /// <summary>
        /// Цена последнего самого низкого предложения (ASK)
        /// </summary>
        public decimal Ask { get; set; }

        /// <summary>
        /// Количество дней, покрываемых ASK (опционально)
        /// </summary>
        public int? AskPeriod { get; set; }

        /// <summary>
        /// Сумма 25 самых низких предложений
        /// </summary>
        public decimal AskSize { get; set; }

        /// <summary>
        /// Изменение цены за день
        /// </summary>
        public decimal DailyChange { get; set; }

        /// <summary>
        /// Изменение цены за день в процентах
        /// </summary>
        public decimal DailyChangePerc { get; set; }

        /// <summary>
        /// Цена последней сделки
        /// </summary>
        public decimal LastPrice { get; set; }

        /// <summary>
        /// Суточный объем
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Максимальная цена за сутки
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Минимальная цена за сутки
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// Количество доступного финансирования по Flash Return Rate (опционально)
        /// </summary>
        public decimal? FRRAmountAvailable { get; set; }
    }

}
