import { Injectable, Injector } from "@angular/core";
import { BaseOverlayService } from "../core/base-overlay.service";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { CreateDivisionOverlayComponent } from "./create-division-overlay.component";

@Injectable()
export class CreateDivisionOverlay extends BaseOverlayService<CreateDivisionOverlayComponent> {
  constructor(
    public injector: Injector,
    public overlayRefProvider: OverlayRefProvider
  ) {
    super(injector, overlayRefProvider, CreateDivisionOverlayComponent);
  }
}
