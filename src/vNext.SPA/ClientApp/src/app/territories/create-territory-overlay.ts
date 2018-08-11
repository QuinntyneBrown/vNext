import { Injectable, Injector } from "@angular/core";
import { BaseOverlayService } from "../core/base-overlay.service";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { CreateTerritoryOverlayComponent } from "./create-territory-overlay.component";

@Injectable()
export class CreateTerritoryOverlay extends BaseOverlayService<CreateTerritoryOverlayComponent> {
  constructor(
    public injector: Injector,
    public overlayRefProvider: OverlayRefProvider
  ) {
    super(injector, overlayRefProvider, CreateTerritoryOverlayComponent);
  }
}
