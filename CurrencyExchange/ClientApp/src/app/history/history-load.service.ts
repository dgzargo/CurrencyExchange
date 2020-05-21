import { Injectable } from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {HttpClient, HttpParams} from '@angular/common/http';
import {HistoryRow} from './history-row';
import {map, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class HistoryLoadService {
  public count: Subject<number> = new Subject();

  constructor(private http: HttpClient) { }

  findRows(fromRecord: number, toRecord: number, fromCurrency: string|null, toCurrency: string|null): Observable<HistoryRow[]> {
    let params = new HttpParams()
      .set('fromRecord', fromRecord.toString())
      .set('toRecord', toRecord.toString());
    if (fromCurrency && fromCurrency.length > 0) {
      params = params.set('fromCurrency', fromCurrency);
    }
    if (toCurrency && toCurrency.length > 0) {
      params = params.set('toCurrency', toCurrency);
    }
    return this.http.get<any>('ExchangeRates/history', { params: params })
      .pipe(
        tap(h => this.count.next(h.rowsCount)),
        map(h => h.historyRows)
      );
  }
}
