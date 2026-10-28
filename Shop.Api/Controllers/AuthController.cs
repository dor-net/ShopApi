using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Api.DTOs.Auth;
using Shop.Api.Services;

namespace Shop.Api.Controllers;

[Route("api/[controller]/[Action]")]
[ApiController]
public class AuthController(ApplicationDbContext context,
    IPasswordHasherService passwordHasherService,
    IJwtService jwtService

    )
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Login(LoginRequestDto request)
    {
        var normalizedUsername = request.Username.Trim().ToUpper();

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Username == normalizedUsername);

        var errorMessage = "نام کاربری  یا کلمه عبور صحیح نمی باشد";

        if (user == null)
        {
            return BadRequest(errorMessage);
        }

        var isPasswordCorrect = passwordHasherService
            .Verifiy(user.PasswordHash, request.Password);

        if (!isPasswordCorrect)
        {
            return BadRequest(errorMessage);
        }

        var token = jwtService.GenerateToken(user);

        return Ok(new
        {
            Token = token,
        });

    }
}
