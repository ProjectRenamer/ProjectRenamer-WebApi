import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

export const rootRouter: Routes = [
    { path: 'home-page', loadChildren: 'app/home/home.module#HomeModule' },
    { path: 'users-page', loadChildren: 'app/user/user.module#UserModule' },
    { path: '**', redirectTo: 'home-page' }
];

export const appRouter: ModuleWithProviders = RouterModule.forRoot(rootRouter);
