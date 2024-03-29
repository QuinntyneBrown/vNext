import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { AuthGuard } from './core/auth.guard';
import { MasterPageComponent } from './master-page.component';
import { LoginComponent } from './users/login.component';
import { DashboardPageComponent } from './dashboards/dashboard-page.component';
import { LoginPageComponent } from './login-page.component';
import { ContactsPageComponent } from './contacts/contacts-page.component';
import { UsersPageComponent } from './users/users-page.component';
import { WarehousesPageComponent } from './warehouses/warehouses-page.component';
import { ContactPageComponent } from './contacts/contact-page.component';
import { AddressesResolver } from './addresses/addresses-resolver';

export const routes: Routes = [
  {
    path: 'login',
    component: AnonymousMasterPageComponent,
    children: [
      {
        path: '',
        component: LoginPageComponent
      }
    ]
  },
  {
    path: '',
    component: MasterPageComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: DashboardPageComponent,
      },
      {
        path: 'contacts',
        component: ContactsPageComponent,
      },
      {
        path: 'contact/create',
        component: ContactPageComponent,
        resolve: {
          resolveAddresses: AddressesResolver
        }
      },
      {
        path: 'contact/edit/:id',
        component: ContactPageComponent,
        resolve: {
          resolveAddresses: AddressesResolver
        }
      },
      {
        path: 'users',
        component: UsersPageComponent,
      },
      {
        path: 'warehouses',
        component: WarehousesPageComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
