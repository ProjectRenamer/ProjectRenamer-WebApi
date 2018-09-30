using System.Net;
using Alternatives.Extensions;
using DotNet.Template.Business.Services.Imp;
using DotNet.Template.Dtos.Requests.User;
using DotNet.Template.Dtos.Responses.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DotNet.Template.Api.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private re21only UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(GetUserResponse))]
        public IActionResult Get(long id)
        {
            //long? userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.TryToLong();
            GetUserResponse getUserResponse = _userService.GetUserById(id);
            return StatusCode(HttpStatusCode.OK.ToInt(), getUserResponse);
        }

        [HttpPost, Route("")]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(CreateUserResponse))]
        public IActionResult Create([FromBody] CreateUserRequest request)
        {
            CreateUserResponse createUserResponse = _userService.CreateUser(request);
            return StatusCode(HttpStatusCode.Created.ToInt(), createUserResponse);
        }

        [HttpGet, Route("")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QueryUserResponse))]
        public IActionResult Get([FromQuery] QueryUserRequest request)
        {
            QueryUserResponse queryUserResponse = _userService.QueryUser(request);
            return StatusCode(HttpStatusCode.OK.ToInt(), queryUserResponse);
        }

        [HttpGet, Route("{userId}/roles")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(GetUserRolesResponse))]
        public IActionResult GetUserRoles(long userId)
        {
            GetUserRolesResponse getUserRolesResponse = _userService.GetUserRoles(userId);
            return StatusCode(HttpStatusCode.OK.ToInt(), getUserRolesResponse);
        }
    }
}