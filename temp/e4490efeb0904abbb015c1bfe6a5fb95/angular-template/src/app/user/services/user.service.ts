import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { environment } from '@env/environment';

import { TokenService } from './token.service';
import { CreateTokenRequest } from '../requests/token/create-token-request';
import { CreateUserRequest } from '../requests/user/create-user-request';
import { CreateUserResponse } from '../responses/user/create-user-response';
import { GetUserResponse } from '../responses/user/get-user-response';
import { GetUserRolesResponse, UserRole } from '../responses/user/get-user-roles-response';


@Injectable()
export class UserService {

    constructor(private httpClient: HttpClient) {
    }

    public CreateUser(createUserRequest: CreateUserRequest): Observable<CreateUserResponse> {
        return this.httpClient.post<CreateUserResponse>(environment.DotNetTemplateUrl + '/users', createUserRequest);
    }

    public GetUserById(id: number): Observable<GetUserResponse> {
        return this.httpClient.get<GetUserResponse>(environment.DotNetTemplateUrl + '/users/' + id);
    }

    public GetUserRolesByUserId(userId: number): Observable<GetUserRolesResponse> {
        return this.httpClient.get<GetUserRolesResponse>(environment.DotNetTemplateUrl + '/users/' + userId + '/roles');
    }
}
