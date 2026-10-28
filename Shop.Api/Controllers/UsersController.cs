using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Api.DTOs.Users;
using Shop.Api.Entities;
using Shop.Api.Services;

namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController
    (ApplicationDbContext context,
    IPasswordHasherService passwordHashService
)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(CreateUserRequestDto request)
    {
        var normalizedUsername = request.Username.Trim().ToUpper();

        if (await context.Users.AnyAsync(r => r.Username == normalizedUsername))
        {
            return BadRequest("این نام کاربری قبلا به ثبت رسیده است");
        }

        var hashedPassword = passwordHashService.Hash(request.Password);

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Mobile = request.Mobile,
            Username = request.Username,
            PasswordHash = hashedPassword
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Created($"api/users/{user.Id}", new
        {
            Id = user.Id
        });
    }
}
