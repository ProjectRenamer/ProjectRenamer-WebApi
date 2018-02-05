import { NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { StorageService } from '@app/core/services/storage-service';

import { CustomErrorHandler } from '@app/core/error-handling/custom-error-handler';

import { AuthTokenInterceptor } from '@app/core/auth/auth-token-interceptor';
import { AuthGuard } from '@app/core/auth/auth-guard';
import { NotAuthGuard } from '@app/core/auth/not-auth-guard';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  declarations: [],
  providers: [
    StorageService,
    CustomErrorHandler,
    AuthTokenInterceptor,
    AuthGuard,
    NotAuthGuard
  ]
})
export class CoreModule {
  constructor(
    @Optional() @SkipSelf() parentModule: CoreModule
  ) {
    if (parentModule) {
      throw new Error('CoreModule is already loaded. Import only in AppModule');
    }
  }
}
