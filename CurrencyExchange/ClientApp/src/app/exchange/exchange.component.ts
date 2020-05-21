import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-exchange',
  templateUrl: './exchange.component.html',
  styleUrls: ['./exchange.component.css']
})
export class ExchangeComponent implements OnInit {
  Currencies: Currency[];
  Amount1: number;
  Currency1: Currency;
  Amount2: number;
  Currency2: Currency;
  constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {
    this.Currencies = [new Currency('us', 'USD'),
                       new Currency('eu', 'EUR'),
                       new Currency('gb', 'GBP'),
                       new Currency('cz', 'CZK'),
                       new Currency('ru', 'RUB')];
    this.Currency1 = this.Currencies[0];
    this.Currency2 = this.Currencies[1];
  }

  ngOnInit() {
  }

  swapCurrencies() {
    const currency1 = this.Currency1;
    this.Currency1 = this.Currency2;
    this.Currency2 = currency1;

    if (this.Amount2) {
      const amount1 = this.Amount1;
      this.Amount1 = this.Amount2;
      this.Amount2 = amount1;
    } else {
      this.Amount1 = null;
    }
  }

  exchange(): void {
    if (this.Amount1 === 0) {
      this.Amount2 = 0;
      return;
    }
    if (this.Currency1.CurrencyShortName === this.Currency2.CurrencyShortName) {
      this.Amount2 = this.Amount1;
      return;
    }
    this.Amount2 = null;
    this.getExchangedCurrency(this.Amount1, this.Currency1, this.Currency2)
      .subscribe(result => { this.Amount2 = result; }, console.error);
  }

  getExchangedCurrency(fromAmount: number, fromCurrency: Currency, toCurrency: Currency): Observable<number> {
    const params = new HttpParams()
      .set('fromCurrency', fromCurrency.CurrencyShortName)
      .set('toCurrency', toCurrency.CurrencyShortName)
      .set('fromAmount', fromAmount.toString());
    return this.http.get<number>(`${this.baseUrl}ExchangeRates`, { params: params });
  }

  clearAmount2(): void {
    this.Amount2 = null;
  }
}

export class Currency {
  Country: string;
  CurrencyShortName: string;
  get FlagImg() { return `https://www.countryflags.io/${this.Country}/flat/32.png`; }
  constructor(country: string, currencyShortName: string) {
    this.Country = country;
    this.CurrencyShortName = currencyShortName;
  }
}
