import { Injectable } from "@angular/core";
import { OverlayRef } from "@angular/cdk/overlay";
import { DashboardTileSettings } from "../dashboard-tiles/dashboard-tile-settings.model";
import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";

@Injectable()
export class ConfigureDashboardTileOverlayRef {
  constructor(
    private overlayRef: OverlayRef,
    public dashboardTile: DashboardTile<DashboardTileSettings>) { }

  close(): void {
    this.overlayRef.dispose();
  }
}
