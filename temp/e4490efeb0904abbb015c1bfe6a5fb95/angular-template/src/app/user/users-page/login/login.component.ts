import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { CreateTokenResponse } from '../../responses/token/create-token-response';
import { CreateTokenRequest } from '../../requests/token/create-token-request';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { AuthService } from '@app/core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {

  createTokenResponse: CreateTokenResponse;
  createTokenRequest: CreateTokenRequest;

  constructor(private httpClient: HttpClient, private authService: AuthService,
    private tokenService: TokenService, private userService: UserService,
    private appRouter: Router) {
    this.createTokenResponse = null;
    this.createTokenRequest = new CreateTokenRequest();
  }

  ngOnInit() {
  }

  login(createTokenRequest: CreateTokenRequest): void {

    this.tokenService.CreateToken(createTokenRequest)
      .subscribe(tokenResponse => {

        this.authService.Login(tokenResponse.tokenValue, tokenResponse.expireTime, tokenResponse.userId);

        this.userService.GetUserRolesByUserId(tokenResponse.userId)
          .subscribe(getUserRolesResponse => {

            this.authService.SetUserRoles(getUserRolesResponse.roles.map(r => r.roleName), tokenResponse.expireTime);

            this.appRouter.navigateByUrl('/users-page/user-profile/' + tokenResponse.userId);
            this.createTokenRequest = new CreateTokenRequest();
          });
      });
  }

}
