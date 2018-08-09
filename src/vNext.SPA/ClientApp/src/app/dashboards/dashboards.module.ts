import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DashboardService } from './dashboard.service';
import { DashboardPageComponent } from './dashboard-page.component';
import { TilesModule } from '../tiles/tiles.module';
import { ManageDashboardsOverlayComponent } from './manage-dashboards-overlay.component';
import { ManageDashboardsOverlayService } from './manage-dashboards-overlay.service';
import { DashboardStore } from './dashboard-store';
import { DashboardSettings } from './dashboard-settings.model';
import { DashboardEditComponent } from './dashboard-edit.component';
import { DashboardListComponent } from './dashboard-list.component';
import { DashboardListItemComponent } from './dashboard-list-item.component';
import { DashboardGridComponent } from './dashboard-grid.component';
import { DashboardTilesModule } from '../dashboard-tiles/dashboard-tiles.module';
import { DashboardsResolver } from './dashboards.resolver';
import { DashboardTilesManagementModule } from '../dashboard-tiles-management/dashboard-tiles-management.module';
import { SharedModule } from '../shared/shared.module';

const declarations = [
  DashboardEditComponent,
  DashboardGridComponent,
  DashboardListComponent,
  DashboardListItemComponent,
  DashboardPageComponent,
  ManageDashboardsOverlayComponent
];

const providers = [
  DashboardsResolver,
  DashboardService,
  DashboardStore,
  ManageDashboardsOverlayService
];

const entryComponents = [
  ManageDashboardsOverlayComponent
];

@NgModule({
  declarations,
  entryComponents,
  imports: [
    CommonModule,
    DashboardTilesModule,
    DashboardTilesManagementModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    RouterModule,
    TilesModule
  ],
  providers,
})
export class DashboardsModule { }
