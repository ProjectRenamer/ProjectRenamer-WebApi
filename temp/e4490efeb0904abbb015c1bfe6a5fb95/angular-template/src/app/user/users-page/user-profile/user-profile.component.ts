import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { UserService } from '../../services/user.service';
import { GetUserResponse } from '../../responses/user/get-user-response';
import { UserRole } from '../../responses/user/get-user-roles-response';
import { StorageService } from '@app/core/services/storage.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  @Input() userId: number = 0;
  public user: GetUserResponse = new GetUserResponse();
  public userRoles: UserRole[];

  constructor(private userService: UserService, private storageService: StorageService, private route: ActivatedRoute) {

    if (this.userId != 0)
      return;

    this.route.params.subscribe(params => {
      this.userId = Number(params.userId);
      if (Number.isNaN(this.userId) || this.userId == 0) {
        this.userId = Number(this.storageService.GetValueFromLocal(storageService.userIdKey));
      }
    });
  }

  ngOnInit() {
    this.userService.GetUserById(this.userId)
      .subscribe(res => {
        this.user = res;
      });
    this.userService.GetUserRolesByUserId(this.userId)
      .subscribe(res => {
        this.userRoles = res.roles;
      });
  }
}
