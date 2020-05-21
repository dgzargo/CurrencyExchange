using System.Collections.Generic;

namespace CurrencyExchange.DTOs
{
    public class HistoryDto
    {
        public IEnumerable<HistoryRowDto> HistoryRows { get; set; }
        public int RowsCount { get; set; }
    }
}