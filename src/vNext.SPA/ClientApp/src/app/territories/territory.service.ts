import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { Territory } from "./territory.model";

@Injectable()
export class TerritoryService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<Territory>> {
    return this._client.get<{ territories: Array<Territory> }>(`${this._baseUrl}api/territories`)
      .pipe(
        map(x => x.territories)
      );
  }

  public getById(options: { territoryId: string }): Observable<Territory> {
    return this._client.get<{ territory: Territory }>(`${this._baseUrl}api/territories/${options.territoryId}`)
      .pipe(
        map(x => x.territory)
      );
  }

  public remove(options: { territory: Territory }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/territories/${options.territory.territoryId}`);
  }

  public create(options: { territory: Territory }): Observable<{ territoryId: string }> {
    return this._client.post<{ territoryId: string }>(`${this._baseUrl}api/territories`, { territory: options.territory });
  }

  public update(options: { territory: Territory }): Observable<{ territoryId: string }> {
    return this._client.put<{ territoryId: string }>(`${this._baseUrl}api/territories`, { territory: options.territory });
  }
}
