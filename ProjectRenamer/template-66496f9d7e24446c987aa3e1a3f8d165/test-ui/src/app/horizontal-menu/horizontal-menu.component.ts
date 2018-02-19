import { Component } from '@angular/core';

import { Link } from '@app/shared/helper/link';
import { StorageService } from '@app/core/services/storage-service';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
  selector: 'app-horizontal-menu',
  templateUrl: './horizontal-menu.component.html',
  styleUrls: ['./horizontal-menu.component.css']
})


export class HorizontalMenuComponent implements OnInit {

  linkArray: Link[];

  constructor(private storageService: StorageService) {
  }

  ngOnInit(): void {
    this.Refresh();
  }

  public Refresh() {
    this.linkArray = [new Link('Home', '/')];
    this.linkArray.push(new Link('Users', '/users-page/entrance'));

    // const tokenValue = this.storageService.GetValueFromLocal(this.storageService.tokenValueKey);
    // if (tokenValue === undefined || tokenValue === null || tokenValue === '') {
    //   this.linkArray.push(new Link('Login / Register', '/users-page/entrance'));
    // }
    // else {
    //   this.linkArray.push(new Link('Users', '/users-page/my-profile'));
    // }
  }
}
