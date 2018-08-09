import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TileService } from './tile.service';
import { TileComponent } from './tile.component';
import { TileStore } from './tile-store';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';

const declarations = [
  TileComponent,
];

const entryComponents = [

];

const providers = [
  TileService,
  TileStore
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
  exports:declarations
})
export class TilesModule { }
