using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyExchange.BLL;
using CurrencyExchange.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController: ControllerBase
    {
        private readonly HistoryService _historyService;

        public ExchangeRatesController(HistoryService historyService)
        {
            _historyService = historyService;
        }
        
        [HttpGet]
        public async Task<ActionResult<decimal>> Get([FromQuery]HistoryRowDto rowDto)
        {
            decimal toAmount;
            try
            {
                var rate = await GetRatesFromExternalApi(rowDto.FromCurrency, rowDto.ToCurrency);
                toAmount = rowDto.FromAmount * rate.rates[rowDto.ToCurrency];
            }
            catch
            {
                return BadRequest();
            }
            rowDto.ToAmount = toAmount;
            await _historyService.RightDownExchange(rowDto);
            return toAmount;
        }

        [HttpGet("history")]
        public HistoryDto History([FromQuery]HistoryRowSearchCriteriaDto searchCriteriaDto, [FromQuery]PaginationInfoDto paginationInfoDto)
        {
            return _historyService.GetHistory(searchCriteriaDto, paginationInfoDto);
        }

        private async Task<ExchangeRate> GetRatesFromExternalApi(string fromCurrency, string toCurrency)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"https://api.exchangeratesapi.io/latest?base={fromCurrency}&symbols={toCurrency}");
            if (!responseMessage.IsSuccessStatusCode) throw new ExternalException();
            var rateString = await responseMessage.Content.ReadAsStringAsync();
            var rate = JsonSerializer.Deserialize<ExchangeRate>(rateString);
            return rate;
        }
    }

    public class ExchangeRate
    {
        public string @base { get; set; }
        public Dictionary<string, decimal> rates { get; set; }
    }
}