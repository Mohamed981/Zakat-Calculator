import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { RequestService } from './request.service';

/**
 * Thin wrapper around HttpClient that targets the backend REST API.
 * All endpoints are resolved relative to environment.apiBaseUrl.
 */
@Injectable({
  providedIn: 'root',
})
export class ApiService {

  constructor(private _requestService: RequestService) {}

  private readonly baseUrl = environment.apiBaseUrl;

  private url(path: string): string {
    const clean = path.startsWith('/') ? path.slice(1) : path;
    return `${this.baseUrl}/${clean}`;
  }

  get<T>(path: string, params?: Record<string, string | number | boolean>): Observable<T> {
    return this._requestService.SendRequest('GET', this.url(path), null, null);
  }

  post<T>(path: string, body: unknown): Observable<T> {
    return this._requestService.SendRequest('POST', this.url(path), body, null);
  }

  put<T>(path: string, body: unknown): Observable<T> {
    return this._requestService.SendRequest('PUT', this.url(path), body, null);
  }

  delete<T>(path: string): Observable<T> {
    return this._requestService.SendRequest('DELETE', this.url(path), null, null);
  }

  private toParams(params?: Record<string, string | number | boolean>): HttpParams {
    let httpParams = new HttpParams();
    if (params) {
      for (const [key, value] of Object.entries(params)) {
        httpParams = httpParams.set(key, String(value));
      }
    }
    return httpParams;
  }
}
