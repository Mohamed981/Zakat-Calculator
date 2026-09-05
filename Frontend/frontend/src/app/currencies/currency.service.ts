import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface CurrencyRate {
  code: string;
  rate: number;
}

export interface RatesResponse {
  base: string;
  date: string;
  rates: Record<string, number>;
}

@Injectable({
  providedIn: 'root',
})
export class CurrencyService {

  constructor(private _http: HttpClient) {}

  base_url: string = 'https://api.frankfurter.app/latest';

  getRates(base: string = 'USD'): Observable<RatesResponse> {
    return this._http.get<RatesResponse>(`${this.base_url}?base=${base}`);
  }
}
