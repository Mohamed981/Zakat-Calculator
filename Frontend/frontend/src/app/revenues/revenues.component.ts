import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RevenueService } from './revenue.service';
import { Currency } from '../core/models/currency';
import { Revenue } from '../core/models/revenue';
import { log } from 'console';

@Component({
  selector: 'app-revenues',
  imports: [CommonModule, FormsModule],
  templateUrl: './revenues.component.html',
  styleUrl: './revenues.component.scss',
})
export class RevenuesComponent implements OnInit {

  constructor(private _revenue: RevenueService) {}

  currencies = signal<Currency[]>([]);
  revenues = signal<Revenue[]>([]);
  userId = signal<number | null>(null);
  currencyId = signal<number | null>(null);
  value = signal<number | null>(null);

  loading = signal<boolean>(false);
  saving = signal<boolean>(false);
  error = signal<string>('');
  success = signal<string>('');

  ngOnInit(): void {
    this.loadCurrencies();
    // this.userId.set(1);
    this.loadRevenues();
    console.log(this.revenues());
  }

  /** Revenues created this session. */
  list(): Revenue[] {
    return this._revenue.created();
  }

  loadCurrencies(): void {
    this.loading.set(true);
    this.error.set('');

    this._revenue.getCurrencies().subscribe({
      next: (res) => {
        this.currencies.set(res ?? []);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load currencies. Please try again.');
        this.loading.set(false);
      },
    });
  }

   loadRevenues(): void {
    this.loading.set(true);
    this.error.set('');

    this._revenue.loadRevenues().subscribe({
      next: (res) => {
        this.revenues.set(res ?? []);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load revenues. Please try again.');
        this.loading.set(false);
      },
    });
  }

  currencyCode(currencyId: number): string {
    return this.currencies().find((c) => c.currencyId === currencyId)?.code ?? '—';
  }

  add(): void {
    const userId = this.userId();
    const currencyId = this.currencyId();
    const value = this.value();

    this.error.set('');
    this.success.set('');

    if (userId === null || isNaN(userId) || userId <= 0) {
      this.error.set('Enter a valid user id.');
      return;
    }
    if (currencyId === null) {
      this.error.set('Select a currency.');
      return;
    }
    if (value === null || isNaN(value) || value <= 0) {
      this.error.set('Enter a revenue value greater than zero.');
      return;
    }

    const code = this.currencyCode(currencyId);

    this.saving.set(true);
    this._revenue.createRevenue({ currencyId: currencyId, value: value }, code).subscribe({
      next: (e) => {
        this.revenues.set([...this.revenues(), { id: e.results, name: code, value: value }]);
        this.success.set('Revenue saved.');
        this.value.set(null);
        this.saving.set(false);
      },
      error: (e) => {
        this.error.set('Failed to save revenue. Please try again.');
        this.saving.set(false);
      },
    });
  }

  // --- Edit modal state ---
  editing = signal<Revenue | null>(null);
  editCurrencyId = signal<number | null>(null);
  editValue = signal<number | null>(null);
  editError = signal<string>('');
  updating = signal<boolean>(false);

  openEdit(revenue: Revenue): void {
    this.editError.set('');
    this.editing.set(revenue);
    // Resolve the currency id from the stored currency code (name).
    const currency = this.currencies().find((c) => c.code === revenue.name);
    this.editCurrencyId.set(currency?.currencyId ?? null);
    this.editValue.set(revenue.value);
  }

  closeEdit(): void {
    this.editing.set(null);
    this.editCurrencyId.set(null);
    this.editValue.set(null);
    this.editError.set('');
    this.updating.set(false);
  }

  saveEdit(): void {
    const revenue = this.editing();
    const currencyId = this.editCurrencyId();
    const value = this.editValue();

    this.editError.set('');

    if (!revenue) {
      return;
    }
    if (currencyId === null) {
      this.editError.set('Select a currency.');
      return;
    }
    if (value === null || isNaN(value) || value <= 0) {
      this.editError.set('Enter a revenue value greater than zero.');
      return;
    }

    const code = this.currencyCode(currencyId);

    this.updating.set(true);
    this._revenue
      .updateRevenue({ id: revenue.id, userId: this.userId(), currencyId: currencyId, value: value }, code)
      .subscribe({
        next: () => {
          this.revenues.set(
            this.revenues().map((r) =>
              r.id === revenue.id ? { ...r, name: code, value: value } : r
            )
          );
          this.success.set('Revenue updated.');
          this.closeEdit();
        },
        error: () => {
          this.editError.set('Failed to update revenue. Please try again.');
          this.updating.set(false);
        },
      });
  }

  deleteRevenue(revenue: Revenue): void {
    console.log('Deleting revenue:', revenue);
    this._revenue.deleteRevenue(revenue.id).subscribe({
      next: () => {
        this.revenues.set(this.revenues().filter(r => r !== revenue));
      },
      error: () => {
        this.error.set('Failed to delete revenue. Please try again.');
      }
    });
  }
}
