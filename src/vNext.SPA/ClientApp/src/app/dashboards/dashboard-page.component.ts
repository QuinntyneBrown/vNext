import { Component, Injector, ComponentRef } from "@angular/core";
import { Subject } from "rxjs";
import { DashboardService } from "./dashboard.service";
import { Overlay } from "@angular/cdk/overlay";
import { ComponentPortal, PortalInjector } from "@angular/cdk/portal";
import { AddDashboardTileOverlayComponent } from "../dashboard-tiles-management/add-dashboard-tile-overlay.component";
import { AddDashboardTilesOverlayRef } from "../dashboard-tiles-management/add-dashboard-tile-ref";
import { DashboardStore } from "./dashboard-store";
import { DashboardTileService } from "../dashboard-tiles/dashboard-tile.service";
import { tap } from "rxjs/operators";
import { deepCopy } from "../core/deep-copy";
import { Router, RouterState, ActivatedRoute } from "@angular/router";
import { LocalStorageService } from "../core/local-storage.service";
import { dashboardsKey } from "../core/constants";
import { Dashboard } from "./dashboard.model";
import { AddDashboardTileOverlay } from "../dashboard-tiles-management/add-dashboard-tile-overlay";

@Component({
    templateUrl: "./dashboard-page.component.html",
    styleUrls: ["./dashboard-page.component.css"],
    selector: "cs-dashboard-page"
})
export class DashboardPageComponent { 
  constructor(
    private _addDashboardTileOverlay: AddDashboardTileOverlay,
    private _dashboardService: DashboardService,
    private _dashboardTileService: DashboardTileService,
    public _dashboardStore: DashboardStore,
    private _injector: Injector,
    private _overlay: Overlay,
    private _activateRoute: ActivatedRoute,
    private _localStorage: LocalStorageService
  ) { }

  public ngOnInit() {    
    if (!this.dashboardId) {
      this._dashboardStore.currentDashboard$.next(this.dashboards[0]);
    } else {
      this._dashboardStore.currentDashboard$.next(this.dashboards.find(x => x.dashboardId == +this.dashboardId));
    }
  }

  public get dashboards(): Dashboard[] {
    return this._localStorage.get({ name: dashboardsKey });
  }

  public get dashboardId():number {
    return +this._activateRoute.snapshot.params["id"];
  }
  public onDestroy: Subject<void> = new Subject<void>();

  public handleDelete(dashboardTile) {
    this._dashboardTileService.remove({ dashboardTile })
      .pipe(
        tap(() => {
          const currentDashboard = deepCopy(this._dashboardStore.currentDashboard$.value);
          const index = currentDashboard.dashboardTiles.findIndex(x => x.dashboardTileId == dashboardTile.dashboardTileId);
          currentDashboard.dashboardTiles.splice(index, 1);
          this._dashboardStore.currentDashboard$.next(currentDashboard);
        })
      )
      .subscribe();
  }

  public handleFabButtonClick() {
    this._addDashboardTileOverlay.create()
      .pipe(
        
      tap(x => { this._dashboardStore.currentDashboard$.next(x); })
      )
      .subscribe();
  }
  
  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
