import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { RegionService } from './region.service';
import { RegionsPageComponent } from './regions-page.component';
import { CreateRegionOverlay } from './create-region-overlay';
import { CreateRegionOverlayComponent } from './create-region-overlay.component';

const declarations = [
  RegionsPageComponent,
  CreateRegionOverlayComponent
];

const entryComponents = [
  CreateRegionOverlayComponent
];

const providers = [
  RegionService,
  CreateRegionOverlay
];

@NgModule({
  declarations,
  entryComponents,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    SharedModule	
  ],
  providers,
})
export class RegionsModule { }
