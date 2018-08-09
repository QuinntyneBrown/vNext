import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { accessTokenKey, baseUrl, userIdKey } from '../core/constants';
import { LocalStorageService } from '../core/local-storage.service';
import { Logger } from './logger.service';
import { RedirectService } from './redirect.service';

@Injectable()
export class AuthService {
  constructor(
    @Inject(baseUrl) private _baseUrl: string,
    private _httpClient: HttpClient,
    private _localStorageService: LocalStorageService,
    private _loggerService: Logger,
    private _redirectService: RedirectService
  ) {}

  public logout() {    
    this._localStorageService.put({ name: accessTokenKey, value: null });
    this._redirectService.redirectToLogin();
  }

  public tryToLogin(options: { code: string; password: string }) {
    this._loggerService.trace('AuthService', 'tryToLogin');

    return this._httpClient.post<any>(`${this._baseUrl}api/users/signin`, options).pipe(
      map(response => {
        this._localStorageService.put({ name: accessTokenKey, value: response.accessToken });
        this._localStorageService.put({ name: userIdKey, value: response.userId });
        return response.accessToken;
      })
    );
  }
}
