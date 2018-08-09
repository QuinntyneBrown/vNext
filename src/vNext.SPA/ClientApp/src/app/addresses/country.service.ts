import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { Country } from "./country.model";

@Injectable()
export class CountryService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<Country>> {
    return this._client.get<{ countries: Array<Country> }>(`${this._baseUrl}api/countries`)
      .pipe(
        map(x => x.countries)
      );
  }

  public getById(options: { countryId: string }): Observable<Country> {
    return this._client.get<{ country: Country }>(`${this._baseUrl}api/countries/${options.countryId}`)
      .pipe(
        map(x => x.country)
      );
  }

  public remove(options: { country: Country }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/countries/${options.country.countryId}`);
  }

  public create(options: { country: Country }): Observable<{ countryId: string }> {
    return this._client.post<{ countryId: string }>(`${this._baseUrl}api/countries`, { country: options.country });
  }

  public update(options: { country: Country }): Observable<{ countryId: string }> {
    return this._client.put<{ countryId: string }>(`${this._baseUrl}api/countries`, { country: options.country });
  }
}
