namespace DotNet.Template.Dtos.Responses.User
{
    public class QueryUserResponse : PagedResponse<QueryUserResponseInternal>
    { }

    public class QueryUserResponseInternal
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}