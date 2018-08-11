import { Component, Input, Injector, ComponentRef, EventEmitter, Output, ComponentFactoryResolver, ViewChild, ViewContainerRef, ChangeDetectionStrategy } from "@angular/core";
import { Subject } from "rxjs";
import { Dashboard } from "../dashboards/dashboard.model";
import { Overlay } from "@angular/cdk/overlay";
import { ConfigureDashboardTileOverlayRef } from "../dashboard-tiles-management/configure-dashboard-tile-overlay-ref";
import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";
import { DashboardTileSettings } from "../dashboard-tiles/dashboard-tile-settings.model";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { ConfigureDashboardTileOverlayComponent } from "../dashboard-tiles-management/configure-dashboard-tile-overlay.component";
import { TileStore } from "../tiles/tile-store";
import { DashboardTileComponent } from "../dashboard-tiles/dashboard-tile.component";
import { DashboardStore } from "./dashboard-store";
import { BehaviorSubject } from "rxjs";
import { tap, takeUntil, filter } from "rxjs/operators";

@Component({
  templateUrl: "./dashboard-grid.component.html",
  styleUrls: ["./dashboard-grid.component.css"],
  selector: "cs-dashboard-grid"
})
export class DashboardGridComponent { 
  constructor(
    private readonly _componentFactoryResolver: ComponentFactoryResolver,
    private readonly _dashboardStore: DashboardStore,
    private readonly _injector: Injector,
    private readonly _overlay: Overlay,
    private readonly _tileStore: TileStore
  ) { }

  ngOnInit() {
    this.dashboard$
      .pipe(
        filter(x => x != null),
        tap((dashboard) => {
          if (this._currentDashboardId == dashboard.dashboardId && this._dashboardTileComponentRefs.length > 0) {
            this._dashboardTileComponentRefs.forEach((dtcr) => {
              var existingDasboardTile = dashboard.dashboardTiles.find(x => x.dashboardTileId == dtcr.instance.dashboardTile.dashboardTileId);

              if (!existingDasboardTile)
                dtcr.destroy();
            });

            dashboard.dashboardTiles.forEach((dt) => {
              var existingDasboardTileComponent = this._dashboardTileComponentRefs.find(x => x.instance.dashboardTile.dashboardTileId == dt.dashboardTileId);

              if (!existingDasboardTileComponent) {
                this.addDashboardComponentRef(dt);
              } else {
                existingDasboardTileComponent.instance.dashboardTile = dt;
              }
            });

          } else {

            this._dashboardTileComponentRefs.forEach((dtc) => {
              dtc.destroy()
            });

            this._dashboardTileComponentRefs = [];
          
            dashboard.dashboardTiles.forEach((dashboardTile) => this.addDashboardComponentRef(dashboardTile));

            this._currentDashboardId = dashboard.dashboardId;
          }
        }),
        takeUntil(this.onDestroy)
      ).subscribe();
  }


  protected addDashboardComponentRef(dashboardTile) {
    let componentFactory: ComponentRef<DashboardTileComponent>;

    switch (dashboardTile.tileId) {
      default:
        componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(DashboardTileComponent));
        break;
    }

    const dashboardTileComponentRef: ComponentRef<DashboardTileComponent> = this.target.createComponent(<any>componentFactory, null, this._injector);

    dashboardTileComponentRef.instance.dashboardTile = dashboardTile;

    dashboardTileComponentRef.instance.onConfigure
      .pipe(
        takeUntil(dashboardTileComponentRef.instance.onDestroy),
        tap((dashboardTile) => this.handleConfigure(dashboardTile))
      ).subscribe();

    dashboardTileComponentRef.instance.onDelete
      .pipe(
        takeUntil(dashboardTileComponentRef.instance.onDestroy),
        tap((dashboardTile) => this.onDelete.emit(dashboardTile))
      ).subscribe();

    this._dashboardTileComponentRefs.push(dashboardTileComponentRef);
  }

  protected _currentDashboardId: number;

  handleConfigure(dashboardTile) {
    const positionStrategy = this._overlay.position()
      .global()
      .centerHorizontally()
      .centerVertically();

    const overlayRef = this._overlay.create({
      hasBackdrop: true,
      positionStrategy
    });

    const configureDashboardTileOverlayRef = new ConfigureDashboardTileOverlayRef(overlayRef,dashboardTile);

    const overlayComponent = this.attachDialogContainer(overlayRef, configureDashboardTileOverlayRef);
    
  }

  public attachDialogContainer(overlayRef, configureDashboardTileOverlayRef) {
    const injectionTokens = new WeakMap();
    injectionTokens.set(ConfigureDashboardTileOverlayRef, configureDashboardTileOverlayRef);
    const injector = new PortalInjector(this._injector, injectionTokens);
    const overlayPortal = new ComponentPortal(ConfigureDashboardTileOverlayComponent, null, injector);
    const overlayPortalRef: ComponentRef<ConfigureDashboardTileOverlayComponent> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }

  @Output()
  onDelete: EventEmitter<any> = new EventEmitter();
  
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  @Input()
  public dashboard$: BehaviorSubject<Dashboard>;


  public _domainDashboardTiles: Array<any> = [];

  public _dashboardTileComponentRefs:Array<ComponentRef<any>> = [];

  @ViewChild("target", { read: ViewContainerRef })
  target: ViewContainerRef;
}
