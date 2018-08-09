import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CoreModule } from "../core/core.module";
import { DashboardTilesModule } from "../dashboard-tiles/dashboard-tiles.module";
import { SharedModule } from "../shared/shared.module";
import { AddDashboardTileOverlayComponent } from "./add-dashboard-tile-overlay.component";
import { ConfigureDashboardTileOverlayComponent } from "./configure-dashboard-tile-overlay.component";
import { ConfigureDashboardTileSideNavComponent } from "./configure-dashboard-tile-side-nav.component";
import { EstimateDashboardTileFilterPipe } from "./estimate-dashboard-tile-filter.pipe";
import { GenericDashboardTileFilterPipe } from "./generic-dashboard-tile-filter.pipe";
import { AddDashboardTileOverlay } from "./add-dashboard-tile-overlay";


const declarations: Array<any> = [
  AddDashboardTileOverlayComponent,
  ConfigureDashboardTileOverlayComponent,
  ConfigureDashboardTileSideNavComponent,

  EstimateDashboardTileFilterPipe,
  GenericDashboardTileFilterPipe,
];

const providers: Array<any> = [
  AddDashboardTileOverlay
];

const entryComponents: Array<any> = [
  AddDashboardTileOverlayComponent,
  ConfigureDashboardTileOverlayComponent,
  ConfigureDashboardTileSideNavComponent
];

@NgModule({
  imports: [
    CommonModule,
    DashboardTilesModule,
    FormsModule,
    HttpClientModule,
    CoreModule,
    ReactiveFormsModule,    
    SharedModule
  ],
  declarations,
  entryComponents,
  exports: declarations,
  providers  
})
export class DashboardTilesManagementModule { }
