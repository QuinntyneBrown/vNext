import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { MasterPageComponent } from './master-page.component';
import { AppRoutingModule } from './app-routing.module';
import { UsersModule } from './users/users.module';
import { baseUrl } from './core/constants';
import { TilesModule } from './tiles/tiles.module';
import { DashboardsModule } from './dashboards/dashboards.module';
import { DashboardTilesModule } from './dashboard-tiles/dashboard-tiles.module';
import { DashboardTilesManagementModule } from './dashboard-tiles-management/dashboard-tiles-management.module';
import { LoginPageComponent } from './login-page.component';
import { AddressesModule } from './addresses/addresses.module';
import { ContactsModule } from './contacts/contacts.module';
import { WarehousesModule } from './warehouses/warehouses.module';
import { RegionsModule } from './regions/regions.module';
import { TerritoriesModule } from './territories/territories.module';


@NgModule({
  declarations: [
    AppComponent,
    AnonymousMasterPageComponent,
    MasterPageComponent,
    LoginPageComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    UsersModule,
    WarehousesModule,

    AddressesModule,
    ContactsModule,
    TilesModule,
    DashboardsModule,
    DashboardTilesModule,
    DashboardTilesManagementModule,
    RegionsModule,
    TerritoriesModule
  ],
  providers: [
    { provide: baseUrl, useValue: 'http://localhost:8853/' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
