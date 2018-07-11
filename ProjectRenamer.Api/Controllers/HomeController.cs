﻿using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectRenamer.Api.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet, Route("health-check")]
        [SwaggerResponse((int) HttpStatusCode.OK)]
        public IActionResult Get()
        {
            return StatusCode((int)HttpStatusCode.OK, Environment.MachineName);
        }

        [HttpGet, Route("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public RedirectResult Home()
        {
            return Redirect($"{Request.Scheme}://{Request.Host.ToUriComponent()}/swagger");
        }
    }
}