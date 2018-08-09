import { Component, Input, Output, EventEmitter } from "@angular/core";
import { Subject } from "rxjs";
import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";
import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";
import { DashboardTileSettings } from "../dashboard-tiles/dashboard-tile-settings.model";
import { DashboardTileService } from "../dashboard-tiles/dashboard-tile.service";
import { takeUntil, tap } from "rxjs/operators";
import { TileService } from "../tiles/tile.service";
import { TileStore } from "../tiles/tile-store";
import { DashboardStore } from "../dashboards/dashboard-store";
import { Dashboard } from "../dashboards/dashboard.model";
import { deepCopy } from "../core/deep-copy";


@Component({
  templateUrl: "./configure-dashboard-tile-side-nav.component.html",
  styleUrls: ["./configure-dashboard-tile-side-nav.component.css"],
  selector: "cs-configure-dashboard-tile-side-nav"
})
export class ConfigureDashboardTileSideNavComponent { 
  constructor(
    private _dashboardStore: DashboardStore,
    private _dashboardTileService: DashboardTileService,
    private _tileStore: TileStore
  ) {
    
  }

  public tryToSave() {
    const originalDashboardTile:any = deepCopy(this.dashboardTile);
    this.dashboardTile.settings.height = Number(this.form.value.height);
    this.dashboardTile.settings.width = Number(this.form.value.width);
    this.dashboardTile.settings.top = Number(this.form.value.top);
    this.dashboardTile.settings.left = Number(this.form.value.left);
    
    this._dashboardTileService.save({ dashboardTile: this.dashboardTile })
      .pipe(
        tap((result:any) => {
          this.dashboardTile.concurrencyVersion = result.concurrencyVersion;

          var currentDashboard: Dashboard = deepCopy(this._dashboardStore.currentDashboard$.value);

          var index = currentDashboard.dashboardTiles.findIndex(x => x.dashboardTileId == originalDashboardTile.dashboardTileId);
        
          currentDashboard.dashboardTiles[index] = this.dashboardTile;

          this._dashboardStore.currentDashboard$.next(currentDashboard);          

          this.onExit.emit();
        })
      )
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public getTileNameByTileId(tileId: number) {
    return this._tileStore.getTileNameByTileId(tileId);
  }

  ngOnInit() {
    this.form.patchValue({
      top: this.dashboardTile.settings.top,
      left: this.dashboardTile.settings.left,
      height: this.dashboardTile.settings.height,
      width: this.dashboardTile.settings.width
    });
  }

  @Output()
  public onExit: EventEmitter<any> = new EventEmitter();

  @Input()
  public dashboardTile: DashboardTile<DashboardTileSettings> = <DashboardTile<DashboardTileSettings>>{
      settings: {}
  };

  public form = new FormGroup({
    top: new FormControl(this.dashboardTile.settings.top),
    left: new FormControl(this.dashboardTile.settings.left),
    height: new FormControl(this.dashboardTile.settings.height),
    width: new FormControl(this.dashboardTile.settings.width)
  });  
}
