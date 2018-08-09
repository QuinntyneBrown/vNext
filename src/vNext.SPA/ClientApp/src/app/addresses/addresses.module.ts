import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AddressService } from './address.service';
import { AddressPageComponent } from './address-page.component';
import { SharedModule } from '../shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { AddressEmailTypeService } from './address-email-type.service';
import { AddressPhoneTypeService } from './address-phone-type.service';
import { CountryService } from './country.service';
import { AddressEmailService } from './address-email.service';
import { AddressPhoneService } from './address-phone.service';
import { AddressesResolver } from './addresses-resolver';
import { AddressStore } from './address-store';
import { AddressEditorComponent } from './address-editor.component';
import { EditAddressOverlayComponent } from './edit-address-overlay.component';
import { AddressComponent } from './address.component';
import { AddressEmailComponent } from './address-email.component';
import { AddressPhoneComponent } from './address-phone.component';
import { EditAddressOverlay } from './edit-address-overlay';
import { AddressConverter } from './address-converter';
import { EditAddressSideBarComponent } from './edit-address-side-bar.component';
import { CoreModule } from '../core/core.module';
import { CountrySubdivisionService } from './country-subdivision.service';

const declarations = [
  AddressComponent,
  AddressEmailComponent,
  AddressPageComponent,
  AddressPhoneComponent,
  AddressEditorComponent,
  EditAddressOverlayComponent,
  EditAddressSideBarComponent
];

const providers = [
  AddressService,
  AddressEmailService,
  AddressEmailTypeService,
  AddressesResolver,
  AddressPhoneService,
  AddressPhoneTypeService,
  AddressStore,
  AddressConverter,
  CountryService,
  CountrySubdivisionService,
  EditAddressOverlay
];

const entryComponents = [
  EditAddressOverlayComponent,
  EditAddressSideBarComponent
];

@NgModule({
  declarations,
  entryComponents,
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    SharedModule,
  ],
  exports:declarations,
  providers,
})
export class AddressesModule { }
