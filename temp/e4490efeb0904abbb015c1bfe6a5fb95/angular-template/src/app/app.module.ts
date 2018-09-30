import { NgModule, ErrorHandler } from '@angular/core';
import { CoreModule } from '@app/core/core.module';
import { SharedModule } from '@app/shared/shared.module';
import { ToastrModule, ToastContainerModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { appRouter } from './app.routing';

import { CustomErrorHandler } from '@app/core/error-handling/custom-error-handler';
import { AuthTokenInterceptor } from '@app/core/auth/auth-token-interceptor';

import { AppComponent } from './app.component';
import { HorizontalMenuComponent } from './horizontal-menu/horizontal-menu.component';


@NgModule({
  declarations: [
    AppComponent,
    HorizontalMenuComponent,
  ],
  imports: [
    appRouter,
    CoreModule,
    SharedModule,
    ToastrModule.forRoot(
      {
        onActivateTick: true,
        closeButton: true
      }
    ),
  ],
  providers: [
    {
      provide: ErrorHandler,
      useClass: CustomErrorHandler
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthTokenInterceptor,
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
