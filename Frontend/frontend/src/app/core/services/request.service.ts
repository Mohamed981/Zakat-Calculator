import {Inject, Injectable, PLATFORM_ID} from '@angular/core';
import {Router} from '@angular/router';
import {Observable, throwError as observableThrowError, throwError} from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';


@Injectable({
	providedIn: 'root'
})
export class RequestService {
	private isBrowser: boolean;
	token: string | null = null;
	lang: string | null = null;
    constructor(
        private http: HttpClient, private _router: Router, @Inject(PLATFORM_ID) private platformId: object) {
			this.isBrowser = isPlatformBrowser(this.platformId);
			if(this.isBrowser){
			this.token = localStorage.getItem('accessToken');
			this.lang = localStorage.getItem('language');
		}
    }

    SendRequest(method: string, url: string, data: any, responseType: string): Observable<any> {
		console.log(url);
		return this.http.request(method, url,
		{
            headers: this.jwt(),
            body: data
            }).pipe(catchError((err: HttpErrorResponse) => this.handleError(err)));
    }

    private jwt() {
		// create authorization header with jwt token
		
        if (this.token || this.lang) {
            const headers = new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + this.token,
				'Accept-Language':this.lang,
				// 'cashe': 'false',
				// 'foobar': '' + new Date().getTime() + '',
            });
            return headers;
        }
		return null;
    }


	private handleError(res: HttpErrorResponse) {
		if (res.status === 500) {
			return throwError(res.error);
		} else if (res.status === 400) {
			return throwError(res.error);
		} else if (res.status === 401) {
			return throwError(res.error);
		} else if (res.status === 404) {
			return throwError(res.error);
		}else if (res.status === 409) {
			return throwError(res.error);
		} else{
			return throwError(res.error);
		}
	}
}
