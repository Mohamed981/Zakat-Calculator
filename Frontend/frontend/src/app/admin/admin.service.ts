import { Injectable, signal } from '@angular/core';

export interface ManualRate {
  code: string;
  rate: number;
}

@Injectable({
  providedIn: 'root',
})
export class AdminService {

  /** Map of currency code -> manually set rate (relative to base). */
  readonly rates = signal<Record<string, number>>({});

  setRate(code: string, rate: number): void {
    const next = { ...this.rates() };
    next[code.toUpperCase()] = rate;
    this.rates.set(next);
  }

  removeRate(code: string): void {
    const next = { ...this.rates() };
    delete next[code.toUpperCase()];
    this.rates.set(next);
  }

  clear(): void {
    this.rates.set({});
  }

  asList(): ManualRate[] {
    return Object.entries(this.rates())
      .map(([code, rate]) => ({ code, rate }))
      .sort((a, b) => a.code.localeCompare(b.code));
  }
}
