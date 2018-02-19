import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { StorageService } from '@app/core/services/storage-service';
import { environment } from '@env/environment';

import { CreateTokenResponse } from '../../responses/token/create-token-response';
import { CreateTokenRequest } from '../../requests/token/create-token-request';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { UserRole } from '../../responses/user/get-user-roles-response';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {

  createTokenResponse: CreateTokenResponse;
  createTokenRequest: CreateTokenRequest;

  constructor(private httpClient: HttpClient, private storageService: StorageService,
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

        this.storageService.SaveValueToLocal(this.storageService.tokenValueKey, tokenResponse.tokenValue, tokenResponse.expireTime);

        this.userService.GetUserRoles()
          .subscribe(getUserRolesResponse => {

            this.storageService.SaveArrayValuesToLocal(
              this.storageService.userRolesKey,
              getUserRolesResponse.roles.map(r => r.roleName),
              tokenResponse.expireTime);

            this.appRouter.navigateByUrl('/users-page/my-profile');
          });
      });
  }

}
