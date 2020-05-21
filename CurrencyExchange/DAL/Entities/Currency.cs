using System.Collections;
using System.Collections.Generic;

namespace CurrencyExchange.DAL.Entities
{
    public class Currency
    {
        public Currency(string country, string currency): this()
        {
            CurrencyShortName = currency;
            CountryShortName = country;
        }
        public Currency()
        {
            HistoryFromCurrencyNavigation = new HashSet<HistoryRow>();
            HistoryToCurrencyNavigation = new HashSet<HistoryRow>();
        }

        public string CurrencyShortName { get; set; }
        public string CountryShortName { get; set; }

        public virtual ICollection<HistoryRow> HistoryFromCurrencyNavigation { get; set; }
        public virtual ICollection<HistoryRow> HistoryToCurrencyNavigation { get; set; }
    }
}