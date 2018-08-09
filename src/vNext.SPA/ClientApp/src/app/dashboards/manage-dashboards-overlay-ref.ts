import { OverlayRef } from "@angular/cdk/overlay";

export class ManageDashboardsOverlayRef {
  constructor(private overlayRef: OverlayRef) { }

  close(): void {
    this.overlayRef.dispose();
  }
}
