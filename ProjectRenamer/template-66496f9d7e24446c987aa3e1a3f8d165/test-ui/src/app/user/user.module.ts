import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared/shared.module';

import { userRouter } from './user.routing';

import { UsersPageComponent } from './users-page/users-page.component';
import { LoginComponent } from './users-page/login/login.component';
import { CreateUserComponent } from './users-page/create-user/create-user.component';
import { LogoutComponent } from './users-page/logout/logout.component';
import { MyProfileComponent } from './users-page/my-profile/my-profile.component';

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
    CreateUserComponent,
    LogoutComponent,
    MyProfileComponent],
  providers: [
    UserService,
    TokenService
  ]
})

export class UserModule { }
