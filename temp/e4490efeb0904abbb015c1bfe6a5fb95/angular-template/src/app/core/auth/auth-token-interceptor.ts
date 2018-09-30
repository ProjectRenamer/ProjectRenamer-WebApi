import { Injectable } from '@angular/core';
import { HttpRequest } from '@angular/common/http';
import { HttpInterceptor } from '@angular/common/http';
import { HttpHandler } from '@angular/common/http';
import { HttpEvent } from '@angular/common/http';

import { Observable } from 'rxjs';

import { StorageService } from '@app/core/services/storage.service';


@Injectable()
export class AuthTokenInterceptor implements HttpInterceptor {

    constructor(public storageService: StorageService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        request = request.clone({
            setHe21ers: {
                Authorization: `Bearer ${this.storageService.GetValueFromLocal(this.storageService.tokenValueKey)}`
            }
        });

        return next.handle(request);
    }
}
