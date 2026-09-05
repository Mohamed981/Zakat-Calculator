import { Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ApiService } from '../core/services/api.service';
import { Revenue } from '../core/models/revenue';
import { Currency } from '../core/models/currency';
import { Response } from '../core/models/response';
import { CreateRevenuePayload } from '../core/models/createRevenue';
import { UpdateRevenuePayload } from '../core/models/updateRevenue';


@Injectable({
  providedIn: 'root',
})
export class RevenueService {

  constructor(private _api: ApiService) {}

  private readonly resource = 'Revenues';
  private readonly currenciesResource = 'currencies';

  /** Revenues created so far this session (no backend list endpoint yet). */
  readonly created = signal<Revenue[]>([]);

  /** GET /api/currencies */
  getCurrencies(): Observable<Currency[]> {
    return this._api.get<Currency[]>(this.currenciesResource);
  }

  /** POST /api/revenues — returns the backend result string, and records the revenue locally. */
  createRevenue(payload: CreateRevenuePayload, name: string): Observable<Response<string>> {
    return this._api.post<Response<string>>(this.resource, payload).pipe(
      tap((result) => {
        console.log(result.results);
        this.created.set([{ name, value: payload.value, id: result.results }, ...this.created()]);
      })
    );
  }

  loadRevenues(): Observable<Revenue[]> {
    return this._api.get<Revenue[]>(`${this.resource}`);
  }

  /** PUT /api/revenues — updates an existing revenue. */
  updateRevenue(payload: UpdateRevenuePayload, name: string): Observable<Response<string>> {
    return this._api.put<Response<string>>(this.resource, payload).pipe(
      tap(() => {
        const updated = this.created().map((revenue) =>
          revenue.id === payload.id ? { ...revenue, name, value: payload.value } : revenue
        );
        this.created.set(updated);
      })
    );
  }

  deleteRevenue(revenueId: string): Observable<void> {
    return this._api.delete<void>(`${this.resource}/${revenueId}`).pipe(
      tap(() => {
        const updatedRevenues = this.created().filter(revenue => revenue.id !== revenueId);
        this.created.set(updatedRevenues);
      })
    );
  }
}
