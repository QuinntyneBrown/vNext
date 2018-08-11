import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { Region } from "./region.model";

@Injectable()
export class RegionService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<Region>> {
    return this._client.get<{ regions: Array<Region> }>(`${this._baseUrl}api/regions`)
      .pipe(
        map(x => x.regions)
      );
  }

  public getById(options: { regionId: string }): Observable<Region> {
    return this._client.get<{ region: Region }>(`${this._baseUrl}api/regions/${options.regionId}`)
      .pipe(
        map(x => x.region)
      );
  }

  public remove(options: { region: Region }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/regions/${options.region.regionId}`);
  }

  public create(options: { region: Region }): Observable<{ regionId: string }> {
    return this._client.post<{ regionId: string }>(`${this._baseUrl}api/regions`, { region: options.region });
  }

  public update(options: { region: Region }): Observable<{ regionId: string }> {
    return this._client.put<{ regionId: string }>(`${this._baseUrl}api/regions`, { region: options.region });
  }
}
