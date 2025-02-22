using AutoMapper;
using TradeBotMarket.Models;

namespace TradeBotMarket.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            #region маппинг GetTrades(bitfinex) в Trade
            CreateMap<List<object>, Trade>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src[0].ToString()))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(src[1]))))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Convert.ToDecimal(src[3])))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => Math.Abs(Convert.ToDecimal(src[2]))))
                .ForMember(dest => dest.Side, opt => opt.MapFrom(src => Convert.ToDecimal(src[2]) > 0 ? "buy" : "sell"));
            #endregion

            #region маппинг GetCandles(bitfinex) в Candle
            CreateMap<List<object>, Candle>()
                .ForMember(dest => dest.OpenTime, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(src[0]))))
                .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[1])))
                .ForMember(dest => dest.ClosePrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[2])))
                .ForMember(dest => dest.HighPrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[3])))
                .ForMember(dest => dest.LowPrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[4])))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => 
                (Convert.ToDecimal(src[1]) + Convert.ToDecimal(src[2]) + Convert.ToDecimal(src[3]) + Convert.ToDecimal(src[4])) / 4 * Convert.ToDecimal(src[5])))
                .ForMember(dest => dest.TotalVolume, opt => opt.MapFrom(src => Convert.ToDecimal(src[5])));
            #endregion

        }
    }
}

