import { Component, OnInit } from '@angular/core';

import { CustomNotificationService } from '@app/core/services/custom-notification.service';
import { StringResources } from '@constants/string-resources';

import { CreateUserRequest } from '../../requests/user/create-user-request';
import { UserService } from '../../services/user.service';
import { CreateUserResponse } from '../../responses/user/create-user-response';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {

  createUserRequest: CreateUserRequest;
  createUserResponse: CreateUserResponse;

  constructor(private userService: UserService, private notificationService: CustomNotificationService) {
    this.createUserRequest = new CreateUserRequest();
    this.createUserResponse = null;
  }

  ngOnInit() { }

  register(createUserRequest: CreateUserRequest): void {
    this.userService.CreateUser(createUserRequest)
      .subscribe(res => {
        this.notificationService.Success({ MessageContent: StringResources.UserCreatedSuccessfully });
        this.createUserRequest = new CreateUserRequest();
      });
  }
}
