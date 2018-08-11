import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { WarehousesPageComponent } from './warehouses-page.component';
import { WarehouseService } from './warehouse.service';

const declarations = [
  WarehousesPageComponent
];

const entryComponents = [

];

const providers = [
  WarehouseService
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
export class WarehousesModule { }
