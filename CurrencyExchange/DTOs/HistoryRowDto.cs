using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyExchange.DTOs
{
    public class HistoryRowDto
    {
        public HistoryRowDto(decimal fromAmount, string fromCurrency, string toCurrency,
            decimal? toAmount = null, DateTime? date = null)
        {
            FromAmount = fromAmount;
            ToAmount = toAmount;
            Date = date;
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
        }
        public HistoryRowDto() { }
        public decimal FromAmount { get; set; }
        public decimal? ToAmount { get; set; }
        public DateTime? Date { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
    }
}