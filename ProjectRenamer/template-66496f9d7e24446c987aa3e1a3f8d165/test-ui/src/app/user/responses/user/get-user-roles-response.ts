export class GetUserRolesResponse {
    roles: UserRole[];
}

export class UserRole {
    roleId: number;
    roleName: string;
}
