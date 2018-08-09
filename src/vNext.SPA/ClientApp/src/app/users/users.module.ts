import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { UsersPageComponent } from './users-page.component';
import { UserService } from './user.service';
import { UserPageComponent } from './user-page.component';
import { UserEditorComponent } from './user-editor.component';

const declarations = [
  LoginComponent,
  UserPageComponent,
  UsersPageComponent,
  UserEditorComponent
];

const providers = [
  UserService
];

@NgModule({
  declarations,
  providers,
  imports: [CommonModule, CoreModule, SharedModule],
  exports: declarations
})
export class UsersModule {}
