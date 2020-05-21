using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExchange.DAL;
using CurrencyExchange.DAL.Entities;
using CurrencyExchange.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.BLL
{
    public class HistoryService
    {
        private readonly CurrencyExchangeDbContext _dbContext;

        public HistoryService(CurrencyExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RightDownExchange(HistoryRowDto rowDto)
        {
            if (!rowDto.ToAmount.HasValue) throw new ArgumentException();
            //var from = await _dbContext.Currencies.FindAsync(rowDto.FromCurrency);
            //var to = await _dbContext.Currencies.FindAsync(rowDto.ToCurrency);
            var historyRow = new HistoryRow {FromAmount = rowDto.FromAmount, ToAmount = rowDto.ToAmount.Value, FromCurrency = rowDto.FromCurrency, ToCurrency = rowDto.ToCurrency};
            await _dbContext.History.AddAsync(historyRow);
            await _dbContext.SaveChangesAsync();
        }

        public HistoryDto GetHistory(HistoryRowSearchCriteriaDto searchCriteria, PaginationInfoDto paginationInfo)
        {
            IQueryable<HistoryRow> query = _dbContext.History;
            if (searchCriteria?.FromCurrency != null)
            {
                query = query.Where(e => EF.Functions.Like(e.FromCurrency, searchCriteria.FromCurrency));
            }
            if (searchCriteria?.ToCurrency != null)
            {
                query = query.Where(e => EF.Functions.Like(e.FromCurrency, searchCriteria.ToCurrency));
            }

            var totalCount = query.Count();
            
            if (paginationInfo != null)
            {
                var offset = paginationInfo.FromRecord;
                var count = paginationInfo.ToRecord - offset;
                if (offset >= 0 && count > 0)
                {
                    query = query.Skip(offset).Take(count);
                }
            }

            var historyRows = query.AsEnumerable()
                .Select(row => new HistoryRowDto(row.FromAmount, row.FromCurrency, row.ToCurrency, row.ToAmount, row.Date));
            return new HistoryDto {HistoryRows = historyRows, RowsCount = totalCount};
        }
    }
}