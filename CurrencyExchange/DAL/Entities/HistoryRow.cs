using System;

namespace CurrencyExchange.DAL.Entities
{
    public class HistoryRow
    {
        public int Id { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public DateTime? Date { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }

        public virtual Currency FromCurrencyNavigation { get; set; }
        public virtual Currency ToCurrencyNavigation { get; set; }
    }
}