import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DashboardTileService } from './dashboard-tile.service';
import { TilesModule } from '../tiles/tiles.module';
import { DashboardTileComponent } from './dashboard-tile.component';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';

const declarations = [
  DashboardTileComponent
];

const entryComponents = [
  DashboardTileComponent
];

const providers = [
  DashboardTileService
];

@NgModule({
  declarations,
  exports: declarations,
  entryComponents,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    SharedModule,
    TilesModule,
    CoreModule
  ],
  providers,
})
export class DashboardTilesModule { }
