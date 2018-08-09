import { BaseOverlayService } from "../core/base-overlay.service";
import { AddDashboardTileOverlayComponent } from "./add-dashboard-tile-overlay.component";
import { Injector, Injectable } from "@angular/core";
import { OverlayRefProvider } from "../core/overlay-ref-provider";

@Injectable()
export class AddDashboardTileOverlay extends BaseOverlayService<AddDashboardTileOverlayComponent> {
  constructor(
    injector: Injector,
    overlayRefProvider: OverlayRefProvider
  ) {
    super(injector, overlayRefProvider, AddDashboardTileOverlayComponent);
  }
}
