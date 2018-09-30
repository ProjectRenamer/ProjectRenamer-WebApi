import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared/shared.module';

import { userRouter } from './user.routing';

import { UsersPageComponent } from './users-page/users-page.component';
import { LoginComponent } from './users-page/login/login.component';
import { RegisterComponent } from './users-page/register/register.component';
import { LogoutComponent } from './users-page/logout/logout.component';
import { UserProfileComponent } from './users-page/user-profile/user-profile.component';

import { UserService } from './services/user.service';
import { TokenService } from './services/token.service';


@NgModule({
  imports: [
    userRouter,
    SharedModule
  ],
  declarations: [
    UsersPageComponent,
    LoginComponent,
    RegisterComponent,
    LogoutComponent,
    UserProfileComponent],
  providers: [
    UserService,
    TokenService
  ]
})

export class UserModule { }
