import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CurrencyRate, CurrencyService } from './currency.service';

@Component({
  selector: 'app-currencies',
  imports: [CommonModule, FormsModule],
  templateUrl: './currencies.component.html',
  styleUrl: './currencies.component.scss',
})
export class CurrenciesComponent implements OnInit {

  constructor(private _currency: CurrencyService) {}

  rates = signal<CurrencyRate[]>([]);
  base = signal<string>('USD');
  date = signal<string>('');
  loading = signal<boolean>(false);
  error = signal<string>('');
  search = signal<string>('');

  bases: string[] = ['USD', 'EUR', 'GBP', 'EGP', 'SAR', 'AED'];

  ngOnInit(): void {
    this.loadRates();
  }

  loadRates(): void {
    this.loading.set(true);
    this.error.set('');

    this._currency.getRates(this.base()).subscribe({
      next: (res) => {
        const list: CurrencyRate[] = Object.entries(res.rates)
          .map(([code, rate]) => ({ code, rate }))
          .sort((a, b) => a.code.localeCompare(b.code));

        this.rates.set(list);
        this.date.set(res.date);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load currency rates. Please try again.');
        this.loading.set(false);
      },
    });
  }

  onBaseChange(value: string): void {
    this.base.set(value);
    this.loadRates();
  }

  filteredRates(): CurrencyRate[] {
    const term = this.search().trim().toLowerCase();
    if (!term) {
      return this.rates();
    }
    return this.rates().filter((r) => r.code.toLowerCase().includes(term));
  }
}
