import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { Division } from "./division.model";

@Injectable()
export class DivisionService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<Division>> {
    return this._client.get<{ divisions: Array<Division> }>(`${this._baseUrl}api/divisions`)
      .pipe(
        map(x => x.divisions)
      );
  }

  public getById(options: { divisionId: string }): Observable<Division> {
    return this._client.get<{ division: Division }>(`${this._baseUrl}api/divisions/${options.divisionId}`)
      .pipe(
        map(x => x.division)
      );
  }

  public remove(options: { division: Division }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/divisions/${options.division.divisionId}`);
  }

  public create(options: { division: Division }): Observable<{ divisionId: string }> {
    return this._client.post<{ divisionId: string }>(`${this._baseUrl}api/divisions`, { division: options.division });
  }

  public update(options: { division: Division }): Observable<{ divisionId: string }> {
    return this._client.put<{ divisionId: string }>(`${this._baseUrl}api/divisions`, { division: options.division });
  }
}
