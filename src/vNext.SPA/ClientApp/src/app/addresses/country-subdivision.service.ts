import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { CountrySubdivision } from "./country-subdivision.model";

@Injectable()
export class CountrySubdivisionService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<CountrySubdivision>> {
    return this._client.get<{ countrySubdivisions: Array<CountrySubdivision> }>(`${this._baseUrl}api/countrySubdivisions`)
      .pipe(
        map(x => x.countrySubdivisions)
      );
  }

  public getById(options: { countrySubdivisionId: string }): Observable<CountrySubdivision> {
    return this._client.get<{ countrySubdivision: CountrySubdivision }>(`${this._baseUrl}api/countrySubdivisions/${options.countrySubdivisionId}`)
      .pipe(
        map(x => x.countrySubdivision)
      );
  }

  public remove(options: { countrySubdivision: CountrySubdivision }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/countrySubdivisions/${options.countrySubdivision.countrySubdivisionId}`);
  }

  public create(options: { countrySubdivision: CountrySubdivision }): Observable<{ countrySubdivisionId: string }> {
    return this._client.post<{ countrySubdivisionId: string }>(`${this._baseUrl}api/countrySubdivisions`, { countrySubdivision: options.countrySubdivision });
  }

  public update(options: { countrySubdivision: CountrySubdivision }): Observable<{ countrySubdivisionId: string }> {
    return this._client.put<{ countrySubdivisionId: string }>(`${this._baseUrl}api/countrySubdivisions`, { countrySubdivision: options.countrySubdivision });
  }
}
