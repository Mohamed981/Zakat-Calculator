import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/services/api.service';
import { Currency } from '../core/models/currency';


@Injectable({
  providedIn: 'root',
})
export class BackendCurrencyService {

  constructor(private _api: ApiService) {}

  private readonly resource = 'currencies';

  /** GET /api/currencies */
  getCurrencies(): Observable<Currency[]> {
    return this._api.get<Currency[]>(this.resource);
  }
}
