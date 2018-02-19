import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { TokenService } from '../../services/token.service';
import { GetUserResponse } from '../../responses/user/get-user-response';
import { UserRole } from '../../responses/user/get-user-roles-response';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {

  public user: GetUserResponse = new GetUserResponse();
  public userRoles: UserRole[];

  constructor(private userService: UserService, private tokenService: TokenService) { }

  ngOnInit() {
    this.userService.GetUser()
      .subscribe(res => {
        this.user = res;
      });
    this.userService.GetUserRoles()
      .subscribe(res => {
        this.userRoles = res.roles;
      });
  }

}
