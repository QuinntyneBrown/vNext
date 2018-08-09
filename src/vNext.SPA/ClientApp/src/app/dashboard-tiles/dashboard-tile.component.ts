import { Component, Input, ElementRef, Output, EventEmitter } from "@angular/core";
import { Subject } from "rxjs";
import { DashboardTile } from "./dashboard-tile.model";
import { DashboardTileService } from "./dashboard-tile.service";
import { DashboardTileSettings } from "./dashboard-tile-settings.model";
import { TileStore } from "../tiles/tile-store";

@Component({
  templateUrl: "./dashboard-tile.component.html",
  styleUrls: ["./dashboard-tile.component.css"],
  selector: "cs-dashboard-tile"
})
export class DashboardTileComponent { 
  constructor(
    protected _elementRef: ElementRef,
    protected _tileStore: TileStore
  ) { }

  ngOnInit() {

  }

  public get tileName() {
    return this._tileStore.getTileNameByTileId(this.dashboardTile.tileId);
  }

  protected _setCustomProperty(key:string,value:any) {
    this._elementRef.nativeElement.style.setProperty(key,value)
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  @Output()
  onConfigure: EventEmitter<any> = new EventEmitter();

  @Output()
  onDelete: EventEmitter<any> = new EventEmitter();

  protected _dashboardTile: DashboardTile<DashboardTileSettings> = <DashboardTile<DashboardTileSettings>>{};

  @Input()
  public set dashboardTile(value) {
    this._dashboardTile = value;
    this._setCustomProperty('--grid-column-start', this.configurationMode ? 1 : this.dashboardTile.settings.left);
    this._setCustomProperty('--grid-row-start', this.configurationMode ? 1 : this.dashboardTile.settings.top);
    this._setCustomProperty('--grid-column-stop', (this.configurationMode ? 1 : this.dashboardTile.settings.left) + this.dashboardTile.settings.width);
    this._setCustomProperty('--grid-row-stop', (this.configurationMode ? 1 : this.dashboardTile.settings.top) + this.dashboardTile.settings.height);
    this.onDashboardChanged();
  } 

  public get dashboardTile(): DashboardTile<DashboardTileSettings> {
    return this._dashboardTile;
  }

  onDashboardChanged() {

  }
  
  public configurationMode: boolean = false;
}
