import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Currency} from '../exchange.component';

@Component({
  selector: 'app-curency-input',
  templateUrl: './currency-input.component.html',
  styleUrls: ['./currency-input.component.css']
})
export class CurrencyInputComponent implements OnInit {
  @Input() Label: string;
  @Input() Currencies: Currency[];
  @Input() InputDisabled: boolean;

  @Input()  CurrencyShortName: Currency|null;
  @Output() CurrencyShortNameChange:  EventEmitter<Currency|null> = new EventEmitter();

  @Input() Amount: number|null;
  @Output() AmountChange:  EventEmitter<number|null> = new EventEmitter();

  constructor() { }

  ngOnInit() { }

  declineNonDigitChars($event: KeyboardEvent) {
    const key = $event.keyCode;
    return (key >= 48 && key <= 57) || key === 46;
  }

  updateAmount(newAmount) {
    let n = Number.parseFloat(newAmount);
    n = Number.isNaN(n) ? null : n;
    this.Amount = n;
    this.AmountChange.emit(n);
  }
}
