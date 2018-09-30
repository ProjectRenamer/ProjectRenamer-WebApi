import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';
import { environment } from '@env/environment';

import { CreateTokenRequest } from '../requests/token/create-token-request';
import { CreateTokenResponse } from '../responses/token/create-token-response';

@Injectable()
export class TokenService {

    private tokenExpireTimeKey = 'auth-token-expire';
    private tokenValueKey = 'auth-token-value';

    constructor(private httpClient: HttpClient) {
    }

    public CreateToken(createTokenRequest: CreateTokenRequest): Observable<CreateTokenResponse> {
        return this.httpClient.post<CreateTokenResponse>(environment.DotNetTemplateUrl + '/tokens', createTokenRequest);
    }
}
