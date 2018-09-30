using System.Net;
using Alternatives.Extensions;
using DotNet.Template.Business.Services.Imp;
using DotNet.Template.Dtos.Requests.Token;
using DotNet.Template.Dtos.Responses.Token;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DotNet.Template.Api.Controllers
{
    [Route("tokens")]
    public class TokenController : Controller
    {
        private re21only TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost, Route("")]
        [SwaggerResponse((int) HttpStatusCode.Created, typeof(CreateTokenResponse))]
        public IActionResult Create([FromBody] CreateTokenRequest createTokenRequest)
        {
            CreateTokenResponse createTokenResponse = _tokenService.CreateToken(createTokenRequest);
            return StatusCode(HttpStatusCode.Created.ToInt(), createTokenResponse);
        }
    }
}