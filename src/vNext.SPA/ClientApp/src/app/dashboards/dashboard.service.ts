import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Dashboard } from "./dashboard.model";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";

@Injectable()
export class DashboardService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public list(): Observable<Array<Dashboard>> {
    return this._client.get<{ dashboards: Array<Dashboard> }>(`${this._baseUrl}api/dashboards`)
      .pipe(
        map(x => x.dashboards)
      );
  }

  public getById(options: { dashboardId: number }): Observable<Dashboard> {
    return this._client.get<{ dashboard: Dashboard }>(`${this._baseUrl}api/dashboards/${options.dashboardId}`)
      .pipe(
        map(x => x.dashboard)
      );
  }

  public getAllByUserId(options: { userId: number }): Observable<Dashboard[]> {
    return this._client.get<{ dashboards: Dashboard[] }>(`${this._baseUrl}api/dashboards/user/${options.userId}`)
      .pipe(
        map(x => x.dashboards)
      );
  }

  public remove(options: { dashboard: Dashboard }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/dashboards/${options.dashboard.dashboardId}/${options.dashboard.concurrencyVersion}`);
  }

  public save(options: { dashboard: Dashboard }): Observable<{ dashboardId: number, concurrencyVersion: number }> {
    return this._client.post<{ dashboardId: number, concurrencyVersion: number }>(`${this._baseUrl}api/dashboards`, { dashboard: options.dashboard });
  }
}
