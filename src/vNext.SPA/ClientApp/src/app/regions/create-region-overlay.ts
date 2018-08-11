import { Injectable, Injector } from "@angular/core";
import { BaseOverlayService } from "../core/base-overlay.service";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { CreateRegionOverlayComponent } from "./create-region-overlay.component";

@Injectable()
export class CreateRegionOverlay extends BaseOverlayService<CreateRegionOverlayComponent> {
  constructor(
    public injector: Injector,
    public overlayRefProvider: OverlayRefProvider
  ) {
    super(injector, overlayRefProvider, CreateRegionOverlayComponent);
  }
}
