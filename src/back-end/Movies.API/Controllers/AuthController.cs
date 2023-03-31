using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.API.Common;
using Movies.API.Common.Jwt;
using Movies.API.Filters;
using Movies.Application.Users.Commands;

namespace Movies.API.Controllers;

[ApiController]
[ApiExceptionFilter]
[Authorize]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtConfiguration _jwtConfig;
    // private readonly string tokenKey = "B?E(H+MbQeThWmZq4t7w9z$C&F)J@NcR";


    public AuthController(IMediator mediator, IOptions<JwtConfiguration> jwtConfig)
    {
        _mediator = mediator;
        _jwtConfig = jwtConfig.Value;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginCmd request)
    {
        var user = await _mediator.Send(request);
        return Ok(new
        {
            Name = user.Name,
            Token = "Bearer " + TokenGenerator.Create(user, _jwtConfig.SecretKey)
        });
    }
    
    [HttpGet("test")]
    public ActionResult Test()
    {
        var userName = User?.FindFirstValue(ClaimType.Name);
        var id = User?.FindFirstValue(ClaimType.Id);
        return Ok(new { userName, id });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> CreateAccount([FromBody] CreateUserCmd request)
    {
        await _mediator.Send(request);
        return Ok();
    }
}