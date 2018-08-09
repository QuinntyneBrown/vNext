import { Injectable, Inject } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { DashboardTile } from "./dashboard-tile.model";
import { DashboardTileSettings } from "./dashboard-tile-settings.model";
import { baseUrl } from "../core/constants";


@Injectable()
export class DashboardTileService {
  constructor(
    @Inject(baseUrl) private _baseUrl: string,
    private _client: HttpClient
  ) { }

  public remove(options: { dashboardTile: DashboardTile<DashboardTileSettings> }): any  {    
    return this._client.delete(`${this._baseUrl}api/dashboardtiles/${options.dashboardTile.dashboardTileId}/${options.dashboardTile.concurrencyVersion}`);
  }

  public save(options: { dashboardTile: DashboardTile<DashboardTileSettings> }): Observable<{ dashboardTileId: number, concurrencyVersion:number }> {
    return this._client.post<{ dashboardTileId: number, concurrencyVersion: number }>(`${this._baseUrl}api/dashboardTiles`, { dashboardTile: options.dashboardTile });
  }

  public getByDashboardId(options: { id: number }): Observable<Array<DashboardTile<DashboardTileSettings>>> {
    return this._client.get<{ dashboardTiles: Array<DashboardTile<DashboardTileSettings>> }>(`${this._baseUrl}api/dashboardTiles/dashboard/${options.id}`)
      .pipe(
        map(x => x.dashboardTiles),
        
      );
  }  
}
