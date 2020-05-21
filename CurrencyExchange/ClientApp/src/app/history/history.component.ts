import {AfterViewInit, Component, Inject, OnInit, ViewChild} from '@angular/core';
import {MatPaginator, MatSort, MatTableDataSource} from '@angular/material';
import {catchError, finalize, tap} from 'rxjs/operators';
import {BehaviorSubject, Observable, of} from 'rxjs';
import {CollectionViewer, DataSource} from '@angular/cdk/collections';
import {HistoryLoadService} from './history-load.service';
import {HistoryRow} from './history-row';
import {HistoryRowsDataSource} from './history-rows-data-source';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit, AfterViewInit {
  HistoryRowsDataSource: HistoryRowsDataSource;
  TableTitles: string[];
  FromCurrencyFilter: string;
  ToCurrencyFilter: string;
  RecordsCount: number;

  constructor(private historyLoadService: HistoryLoadService) {
    this.TableTitles = ['FromCurrency', 'FromAmount', 'ToCurrency', 'ToAmount', 'Date'];
  }

  // @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  ngOnInit() {
    this.HistoryRowsDataSource = new HistoryRowsDataSource(this.historyLoadService);
  }

  ngAfterViewInit(): void {
    this.loadRows();
    this.paginator.page.pipe(tap(() => this.loadRows())).subscribe();
    this.historyLoadService.count.pipe(tap(next => this.RecordsCount = next)).subscribe();
  }

  loadRows(): void {
    const pageIndex = this.paginator.pageIndex;
    const pageSize = this.paginator.pageSize;
    this.HistoryRowsDataSource.loadRows(pageIndex * pageSize, (pageIndex + 1) * pageSize,
      this.FromCurrencyFilter, this.ToCurrencyFilter);
  }

  log(obj: any) {
    console.log(obj);
  }
}
