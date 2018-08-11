import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { TerritoryService } from './territory.service';
import { CreateTerritoryOverlay } from './create-territory-overlay';
import { CreateTerritoryOverlayComponent } from './create-territory-overlay.component';
import { TerritoriesPageComponent } from './territories-page.component';

const declarations = [
  TerritoriesPageComponent,
  CreateTerritoryOverlayComponent
];

const entryComponents = [
  CreateTerritoryOverlayComponent
];

const providers = [
  TerritoryService,
  CreateTerritoryOverlay
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
export class TerritoriesModule { }
