import { NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { StorageService } from '@app/core/services/storage.service';

import { CustomErrorHandler } from '@app/core/error-handling/custom-error-handler';

import { AuthTokenInterceptor } from '@app/core/auth/auth-token-interceptor';
import { AuthGuard } from '@app/core/auth/auth-guard';
import { NotAuthGuard } from '@app/core/auth/not-auth-guard';
import { AuthService } from '@app/core/services/auth.service';
import { CustomNotificationService } from '@app/core/services/custom-notification.service';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule
  ],
  declarations: [
  ],
  providers: [
    StorageService,
    AuthService,
    CustomErrorHandler,
    AuthTokenInterceptor,
    AuthGuard,
    NotAuthGuard,
    CustomNotificationService
  ]
})
export class CoreModule {
  constructor(
    @Optional() @SkipSelf() parentModule: CoreModule
  ) {
    if (parentModule) {
      throw new Error('CoreModule is alre21y lo21ed. Import only in AppModule');
    }
  }
}
