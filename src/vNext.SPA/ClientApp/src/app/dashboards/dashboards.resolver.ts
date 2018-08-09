import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { flatMap, map, tap } from "rxjs/operators";
import { DashboardTileSettings } from "../dashboard-tiles/dashboard-tile-settings.model";
import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";
import { DashboardTileService } from "../dashboard-tiles/dashboard-tile.service";
import { DashboardStore } from "./dashboard-store";
import { Dashboard } from "./dashboard.model";
import { DashboardService } from "./dashboard.service";


@Injectable()
export class DashboardsResolver implements Resolve<boolean> {
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {

    return this._dashboardService.list()
      .pipe(
        tap((x: Dashboard[]) => {
          this._dashboardStore.dashboards$.next(x);
          this._dashboardStore.currentDashboard$.next(route.params["id"] ? x.find(x => x.dashboardId == route.params["id"]) : x[0])
        }),
        flatMap(() => this._dashboardTileService.getByDashboardId({ id: this._dashboardStore.currentDashboard$.value.dashboardId })),
        map((dashboardTiles: Array<DashboardTile<DashboardTileSettings>>) => {;
          this._dashboardStore.currentDashboard$.next(Object.assign(this._dashboardStore.currentDashboard$.value, { dashboardTiles: dashboardTiles }));
        }),
        map(() => true)
      );

  
  }
  constructor(
    private _dashboardService: DashboardService,
    private _dashboardStore: DashboardStore,
    private _dashboardTileService: DashboardTileService
  ) { }

  //async resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean  | Observable<Dashboard[] > | Promise<Dashboard[] > {
  //  const dashboards = (<Array<Dashboard>>await this._dashboardService.list().toPromise());
  //  const id = route.params["id"];

  //  this._dashboardStore.dashboards$.next(dashboards);

  //  this._dashboardStore.currentDashboard$.next(id ? dashboards.find(x => x.dashboardId == id) : dashboards[0]);

  //  return true;
  //}
}
