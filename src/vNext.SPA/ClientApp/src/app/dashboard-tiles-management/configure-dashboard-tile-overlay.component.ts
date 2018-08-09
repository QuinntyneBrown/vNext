import { Component, ComponentFactoryResolver, ComponentRef, ElementRef, ViewChild, ViewContainerRef } from "@angular/core";
import { Subject } from "rxjs";
import { takeUntil, tap } from "rxjs/operators";
import { DashboardTileSettings } from "../dashboard-tiles/dashboard-tile-settings.model";
import { DashboardTileComponent } from "../dashboard-tiles/dashboard-tile.component";
import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";
import { ConfigureDashboardTileOverlayRef } from "./configure-dashboard-tile-overlay-ref";
import { ConfigureDashboardTileSideNavComponent } from "./configure-dashboard-tile-side-nav.component";

@Component({
  templateUrl: "./configure-dashboard-tile-overlay.component.html",
  styleUrls: ["./configure-dashboard-tile-overlay.component.css"],
  selector: "cs-configure-dashboard-tile-overlay"
})
export class ConfigureDashboardTileOverlayComponent { 
  constructor(
    private _configureDashboardTileOverlayRef: ConfigureDashboardTileOverlayRef,
    private _componentFactoryResolver: ComponentFactoryResolver,
    private _elementRef: ElementRef
  ) {    
    this.dashboardTile = Object.assign({}, _configureDashboardTileOverlayRef.dashboardTile);
  }


  protected _setCustomProperty(key: string, value: any) {
    this._elementRef.nativeElement.style.setProperty(key, value)
  }

  ngOnInit() {

    this._setCustomProperty('--grid-template-columns', this.dashboardTile.settings.width);
    this._setCustomProperty('--grid-template-rows', this.dashboardTile.settings.height);

    this.renderDashboardTile();
    this.renderConfigurationSideNav();
  }

  public renderDashboardTile() {
    let componentFactory: ComponentRef<DashboardTileComponent>;

    switch (this.dashboardTile.tileId) {
      default:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(DashboardTileComponent));
        break;
    }

    const dashboardTileComponentRef: ComponentRef<DashboardTileComponent> = this.dashboardTileViewContainerRef.createComponent(<any>componentFactory);
    dashboardTileComponentRef.instance.configurationMode = true;
    dashboardTileComponentRef.instance.dashboardTile = this.dashboardTile;  
  }

  public renderConfigurationSideNav() {
    let componentFactory: ComponentRef<ConfigureDashboardTileSideNavComponent>;

    switch (this.dashboardTile.tileId) {
      default:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(ConfigureDashboardTileSideNavComponent));
        break;
    }

    const componentRef: ComponentRef<ConfigureDashboardTileSideNavComponent> = this.configureDashboardTileSideNavViewContainerRef.createComponent(<any>componentFactory);
    
    componentRef.instance.dashboardTile = this.dashboardTile;

    componentRef.instance.onExit.pipe(
      tap(() => this.cancel()),
      takeUntil(this.onDestroy)
    ).subscribe();
  }

  @ViewChild("dashboardTileTarget", { read: ViewContainerRef })
  dashboardTileViewContainerRef: ViewContainerRef;

  @ViewChild("configureDashboardTileSideNavTarget", { read: ViewContainerRef })
  public configureDashboardTileSideNavViewContainerRef: ViewContainerRef;

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  cancel() {
    this._configureDashboardTileOverlayRef.close();
  }

  public dashboardTile: DashboardTile<DashboardTileSettings>;
}
