import { Component } from "@angular/core";
import { Subject, forkJoin } from "rxjs";
import { TileService } from "../tiles/tile.service";
import { DashboardTileService } from "../dashboard-tiles/dashboard-tile.service";
import { Tile } from "../tiles/tile.model";
import { Observable } from "rxjs";
import { AddDashboardTilesOverlayRef } from "./add-dashboard-tile-ref";
import { LocalStorageService as Storage } from "../core/local-storage.service";
import { DashboardStore } from "../dashboards/dashboard-store";
import { DashboardTileSettings } from "../dashboard-tiles/dashboard-tile-settings.model";
import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";
import { tap, flatMap } from "rxjs/operators";
import { Dashboard } from "../dashboards/dashboard.model";
import { DashboardService } from "../dashboards/dashboard.service";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";

@Component({
  templateUrl: "./add-dashboard-tile-overlay.component.html",
  styleUrls: ["./add-dashboard-tile-overlay.component.css"],
  selector: "cs-add-dashboard-tile-overlay"
})
export class AddDashboardTileOverlayComponent { 
  constructor(
    private _addDashboardTilesOverlayRef: OverlayRefWrapper,
    private _dashboardService: DashboardService,
    private _dashboardStore: DashboardStore,
    private _dashboardTileService: DashboardTileService,
    private _tileService: TileService,
    private _storage: Storage
  ) {

  }

  public tryToAddDashboardTiles() {
    
    let observables: Array<Observable<any>> = [];
    let maxLeft = 1;

    for (let i = 0; i < this.selectedTiles.length; i++) {

      for (let i = 0; i < this.currentDashboard.dashboardTiles.length; i++) {
        const item = this.currentDashboard.dashboardTiles[i];
        const settings = item.settings as DashboardTileSettings;

        if ((+settings.left + +settings.width) > maxLeft && maxLeft < 10)
          maxLeft = +settings.left + +settings.width;
      }

      let dashboardTile;

      switch (this.selectedTiles[i].code) {
        default:
          dashboardTile = new DashboardTile<DashboardTileSettings>();
          dashboardTile.settings = new DashboardTileSettings();
          break;
      }

      (dashboardTile.settings as DashboardTileSettings).left = maxLeft;
      dashboardTile.tile = this.selectedTiles[i];
      dashboardTile.tileId = this.selectedTiles[i].tileId;
      dashboardTile.dashboardId = this.currentDashboard.dashboardId;

      this.currentDashboard.dashboardTiles.push(dashboardTile);

      observables.push(this._dashboardTileService.save({ dashboardTile: dashboardTile }));
    }

    forkJoin(observables)
      .pipe(flatMap(() => this._dashboardService.getById({ dashboardId: this.currentDashboard.dashboardId })))
      .subscribe((x:any) => {
        this._dashboardStore.currentDashboard$.next(x);
        this._addDashboardTilesOverlayRef.close(x);
      });
  }

  public tiles$: Observable<Array<Tile>>;

  ngOnInit() {
    this.currentDashboard = Object.assign({}, this._dashboardStore.currentDashboard$.value);
    this.tiles$ = this._tileService.list();
  }

  public cancel() {
    this._addDashboardTilesOverlayRef.close();
  }

  public add() {
    this._addDashboardTilesOverlayRef.close();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleCardClick(tile: Tile) {
    this.tileIsSelected(tile)
      ? this.selectedTiles.splice(this.selectedTiles.indexOf(tile), 1)
      : this.selectedTiles.push(tile);
  }

  public currentDashboard: Dashboard = <Dashboard>{};

  public selectedTiles: Array<Tile> = [];

  public tileIsSelected(tile: Tile) {
    return this.selectedTiles.indexOf(tile) > -1;
  }
}
