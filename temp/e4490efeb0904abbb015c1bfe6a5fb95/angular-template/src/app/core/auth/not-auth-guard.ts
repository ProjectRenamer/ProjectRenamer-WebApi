import { CanActivate } from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { StorageService } from '@app/core/services/storage.service';


@Injectable()
export class NotAuthGuard implements CanActivate {
    constructor(private storageService: StorageService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        let isAuthenticated;

        const tokenValue = this.storageService.GetValueFromLocal(this.storageService.tokenValueKey);
        if (tokenValue === undefined || tokenValue === null || tokenValue === '') {
            isAuthenticated = false;
        }
        else {
            isAuthenticated = true;
        }

        const redirectUrl = route.data.redirectTo;
        if (isAuthenticated === true && redirectUrl !== undefined && redirectUrl !== null) {
            this.router.navigateByUrl(redirectUrl);
        }


        return !isAuthenticated;
    }
}
