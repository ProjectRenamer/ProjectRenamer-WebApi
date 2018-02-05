import { CanActivate } from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { StorageService } from '@app/core/services/storage-service';

@Injectable()
export class AuthGuard implements CanActivate {
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
        const userRoles = this.storageService.GetArrayValuesFromLocal(this.storageService.userRolesKey);

        const expectedRoles = route.data.expectedRoles;
        let result = false;
        if (expectedRoles === undefined || expectedRoles === null || expectedRoles.length === 0) {
            result = isAuthenticated;
        }
        else {
            for (const expectedRole in expectedRoles) {
                if (userRoles.includes(expectedRole)) {
                    result = true;
                    break;
                }
            }
        }

        const redirectUrl = route.data.redirectTo;
        if (result === false && redirectUrl !== undefined && redirectUrl !== null) {
            this.router.navigateByUrl(redirectUrl);
        }

        return result;
    }
}
