import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

import { StorageService } from '@app/core/services/storage.service';

@Injectable()
export class AuthService {
    private isAuthenticatedSubject = new Subject<boolean>();
    private userRolesSubject = new Subject<string[]>();
    private userIdSubject = new Subject<number>();

    constructor(private storageService: StorageService) {
    }

    IsAuthenticated() {
        const token = this.storageService.GetValueFromLocal(this.storageService.tokenValueKey);
        return !(token === undefined || token === null || token === '');
    }

    Logout() {
        this.storageService.DeleteValueFromLocal(this.storageService.tokenValueKey);
        this.storageService.DeleteValueFromLocal(this.storageService.userRolesKey);
        this.storageService.DeleteValueFromLocal(this.storageService.userIdKey);
        this.isAuthenticatedSubject.next(false);
        this.userRolesSubject.next([]);
        this.userIdSubject.next(0);
    }

    Login(tokenValue: string, expireTime: Date, userId: number) {
        this.storageService.SaveValueToLocal(this.storageService.tokenValueKey, tokenValue, expireTime);
        this.storageService.SaveValueToLocal(this.storageService.userIdKey, String(userId), expireTime);
        this.isAuthenticatedSubject.next(true);
        this.userIdSubject.next(userId);
    }

    SetUserRoles(userRoles: string[], expireTime: Date) {
        this.storageService.SaveArrayValuesToLocal(this.storageService.userRolesKey, userRoles, expireTime);
        this.userRolesSubject.next(userRoles);
    }

    GetUserRoles(): string[] {
        return this.storageService.GetArrayValuesFromLocal(this.storageService.userRolesKey);
    }

    ObserveAuthentication(): Observable<boolean> {
        return this.isAuthenticatedSubject.asObservable();
    }

    ObserveUserRoles(): Observable<string[]> {
        return this.userRolesSubject.asObservable();
    }

    ObserveUserId(): Observable<number> {
        return this.userIdSubject.asObservable();
    }
}
