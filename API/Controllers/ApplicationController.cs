﻿using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetApplicationDetails()
        {
            // Get it from some config, AppSettings or Database
            var appDetails = new ApplicationDetails("Login Flow with JWT", "1.0", DateTime.Now);
            return Ok(ApiResponse<ApplicationDetails>.Success(appDetails));
        }
    }
}
