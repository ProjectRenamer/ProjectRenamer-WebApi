import { Routes, RouterModule } from '@angular/router';

import { NotAuthGuard } from '@app/core/auth/not-auth-guard';
import { AuthGuard } from '@app/core/auth/auth-guard';

import { UsersPageComponent } from './users-page/users-page.component';
import { UserProfileComponent } from '@app/user/users-page/user-profile/user-profile.component';

const UserRoutes: Routes = [
    {
        path: 'entrance',
        component: UsersPageComponent,
        canActivate: [NotAuthGuard],
        data: {
            redirectTo: '/users-page/user-profile'
        }
    },
    {
        path: 'user-profile',
        redirectTo: '/users-page/user-profile/0'
    },
    {
        path: 'user-profile/:userId',
        component: UserProfileComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: []
        }
    },
    {
        path: '**',
        redirectTo: '/users-page/entrance'
    }
];

export const userRouter = RouterModule.forChild(UserRoutes);
