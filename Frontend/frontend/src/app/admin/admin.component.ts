import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ManualRate, AdminService } from './admin.service';

@Component({
  selector: 'app-admin',
  imports: [CommonModule, FormsModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
})
export class AdminComponent {

  constructor(private _admin: AdminService) {}

  code = signal<string>('');
  rate = signal<number | null>(null);
  error = signal<string>('');

  // rows = this._manual.rates;

  list(): ManualRate[] {
    return this._admin.asList();
  }

  add(): void {
    const code = this.code().trim().toUpperCase();
    const rate = this.rate();

    if (!code) {
      this.error.set('Currency code is required.');
      return;
    }
    if (!/^[A-Z]{3}$/.test(code)) {
      this.error.set('Use a 3-letter currency code (e.g. USD).');
      return;
    }
    if (rate === null || isNaN(rate) || rate <= 0) {
      this.error.set('Enter a rate greater than zero.');
      return;
    }

    this._admin.setRate(code, rate);
    this.code.set('');
    this.rate.set(null);
    this.error.set('');
  }

  remove(code: string): void {
    this._admin.removeRate(code);
  }

  clearAll(): void {
    this._admin.clear();
  }
}
