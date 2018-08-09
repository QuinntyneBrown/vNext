import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ContactService } from './contact.service';
import { ContactPageComponent } from './contact-page.component';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AddressesModule } from '../addresses/addresses.module';
import { ContactStore } from './contact-store';
import { ContactEditorComponent } from './contact-editor.component';
import { ContactsPageComponent } from './contacts-page.component';

const declarations = [
  ContactPageComponent,
  ContactsPageComponent,
  ContactEditorComponent

];

const entryComponents = [

];

const providers = [
  ContactService,
  ContactStore
];

@NgModule({
  declarations,
  entryComponents,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,

    AddressesModule,
    CoreModule,
    SharedModule
  ],
  providers,
  exports: declarations
})
export class ContactsModule { }
