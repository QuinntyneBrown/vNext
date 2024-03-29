import { OverlayRef } from '@angular/cdk/overlay';

export class AddDashboardTilesOverlayRef {
  constructor(private overlayRef: OverlayRef) { }

  close(): void {
    this.overlayRef.dispose();
  }
}
