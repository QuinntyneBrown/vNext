import { Injectable, ComponentRef, Injector } from "@angular/core";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { EditAddressOverlayComponent } from "./edit-address-overlay.component";
import { Observable } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { BaseOverlayService } from "../core/base-overlay.service";

@Injectable()
export class EditAddressOverlay extends BaseOverlayService<EditAddressOverlayComponent> {
  constructor(
    _injector: Injector,
    _overlayRefProvider: OverlayRefProvider
  ) {
    super(_injector, _overlayRefProvider, EditAddressOverlayComponent);
  }
}
