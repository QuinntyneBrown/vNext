import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { DivisionPageComponent } from './division-page.component';
import { DivisionsPageComponent } from './divisions-page.component';
import { CreateDivisionOverlayComponent } from './create-division-overlay.component';
import { CreateDivisionOverlay } from './create-division-overlay';
import { DivisionService } from './division.service';

const declarations = [
  DivisionPageComponent,
  DivisionsPageComponent,
  CreateDivisionOverlayComponent
];

const entryComponents = [
  CreateDivisionOverlayComponent
];

const providers = [
  CreateDivisionOverlay,
  DivisionService
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
export class DivisionsModule { }
