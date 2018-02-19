import { Routes, RouterModule } from '@angular/router';

import { UsersPageComponent } from './users-page/users-page.component';
import { MyProfileComponent } from './users-page/my-profile/my-profile.component';

import { NotAuthGuard } from '@app/core/auth/not-auth-guard';
import { AuthGuard } from '@app/core/auth/auth-guard';

const UserRoutes: Routes = [
    {
        path: 'entrance',
        component: UsersPageComponent,
        canActivate: [NotAuthGuard],
        data: {
            redirectTo: '/users-page/my-profile'
        }
    },
    {
        path: 'my-profile',
        component: MyProfileComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: []
        }
    },
    { path: '**', redirectTo: '/users-page/entrance' }
];

export const userRouter = RouterModule.forChild(UserRoutes);
