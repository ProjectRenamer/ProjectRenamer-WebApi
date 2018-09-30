using System.Collections.Generic;

namespace DotNet.Template.Dtos.Responses.User
{
    public class GetUserRolesResponse
    {
        public List<GetUserRolesResponseInternal> Roles { get; set; }
    }

    public class GetUserRolesResponseInternal
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
    }
}