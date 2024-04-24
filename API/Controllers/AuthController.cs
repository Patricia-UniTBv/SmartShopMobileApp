﻿using Microsoft.AspNetCore.Mvc;
using DTO;
using API.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO dto, CancellationToken cancellationToken = default)
        {
            var response = await _authService.LoginAsync(dto, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}
