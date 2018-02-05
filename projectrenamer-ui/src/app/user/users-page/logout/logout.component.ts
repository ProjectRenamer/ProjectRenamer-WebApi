import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { StorageService } from '@app/core/services/storage-service';

import { TokenService } from '../../services/token.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(private storageService: StorageService, private router: Router) { }

  ngOnInit() {
  }

  logout() {
    this.storageService.DeleteValueFromLocal(this.storageService.tokenValueKey);
    this.storageService.DeleteValueFromLocal(this.storageService.userRolesKey);
    this.router.navigateByUrl('');
  }

}
