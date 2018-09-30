import { Component, OnInit } from '@angular/core';

import { Link } from '@app/shared/helper/link';

import { AuthService } from '@app/core/services/auth.service';

@Component({
  selector: 'app-horizontal-menu',
  templateUrl: './horizontal-menu.component.html',
  styleUrls: ['./horizontal-menu.component.css']
})


export class HorizontalMenuComponent implements OnInit {

  linkArray: Link[] = [new Link('', '/')];

  constructor(private authService: AuthService) {
    this.authService.ObserveAuthentication()
      .subscribe(() => { this.Refresh(); });

    this.authService.ObserveUserRoles()
      .subscribe(() => { this.Refresh(); });

    this.Refresh();
  }

  ngOnInit(): void {

  }

  Refresh() {
    const isAuthenticated = this.authService.IsAuthenticated();
    const roles = this.authService.GetUserRoles();

    this.linkArray = [new Link('Home', '/')];

    if (isAuthenticated === false) {
      this.linkArray.push(new Link('Login / Register', '/users-page/entrance'));
    }
    else {
      this.linkArray.push(new Link('Users', '/users-page/user-profile'));
    }
  }
}
