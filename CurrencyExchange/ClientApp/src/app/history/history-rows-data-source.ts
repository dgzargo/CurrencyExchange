import {CollectionViewer, DataSource} from '@angular/cdk/collections';
import {BehaviorSubject, Observable, of} from 'rxjs';
import {HistoryLoadService} from './history-load.service';
import {catchError, finalize} from 'rxjs/operators';
import {HistoryRow} from './history-row';

export class HistoryRowsDataSource implements DataSource<HistoryRow> {
  private historyRowsSubject = new BehaviorSubject<HistoryRow[]>([]);
  private historyRowsCountSubject = new BehaviorSubject<number>(0);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();
  public count$ = this.historyRowsCountSubject.asObservable();

  constructor(private historyLoadService: HistoryLoadService) {}

  connect(collectionViewer: CollectionViewer): Observable<HistoryRow[]> {
    return this.historyRowsSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.historyRowsSubject.complete();
    this.loadingSubject.complete();
  }

  loadRows(fromRecord: number, toRecord: number, fromCurrency: string, toCurrency: string) {

    this.loadingSubject.next(true);

    this.historyLoadService.findRows(fromRecord, toRecord, fromCurrency, toCurrency).pipe(
      catchError(() => of([])),
      finalize(() => this.loadingSubject.next(false))
    ).subscribe(rows => this.historyRowsSubject.next(rows));
  }
}
