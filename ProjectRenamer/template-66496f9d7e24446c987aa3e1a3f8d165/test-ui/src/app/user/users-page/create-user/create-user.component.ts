import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { CreateUserRequest } from '../../requests/user/create-user-request';
import { UserService } from '../../services/user.service';
import { CreateUserResponse } from '../../responses/user/create-user-response';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css']
})

export class CreateUserComponent implements OnInit {

  createUserRequest: CreateUserRequest;
  createUserResponse: CreateUserResponse;

  constructor(private httpClient: HttpClient, private userService: UserService) {
    this.createUserRequest = new CreateUserRequest();
    this.createUserResponse = null;
  }

  ngOnInit() { }

  createUser(createUserRequest: CreateUserRequest): void {
    this.userService.CreateUser(createUserRequest)
      .subscribe(res => {
        alert('Kullanıcı bilgileriniz sisteme başarılı bir şekilde kayıt edilmiştir');
        window.location.reload();
      });
  }
}
