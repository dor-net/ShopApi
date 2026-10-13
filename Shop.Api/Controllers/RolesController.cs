using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Api.DTOs.Roles;
using Shop.Api.Entities;

namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(ApplicationDbContext context) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult> Create(CreateRoleRequestDto request)
    {

        var normalizedName = request.Name.Trim().ToUpper();

        if (await context.Roles.AnyAsync(r => r.Name == normalizedName))
        {
            return BadRequest("این نام قبلا به ثبت رسیده است");
        }

        var role = new Role
        {
            Name = normalizedName,
            DisplayName = request.DisplayName,
        };

        context.Roles.Add(role);
        await context.SaveChangesAsync();

        return Created($"api/roles/{role.Id}", new
        {
            Id = role.Id
        });
    }

    [HttpPut]
    public async Task<ActionResult> Update(UpdateRoleRequestDto request)
    {
        var normalizedName = request.Name.Trim().ToUpper();

        if (await context.Roles.AnyAsync(r => r.Id != request.Id && r.Name == normalizedName))
        {
            return BadRequest("این نام قبلا به ثبت رسیده است");
        }

        var role = await context.Roles
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (role is null)
        {
            return NotFound();
        }

        role.Name = normalizedName;
        role.DisplayName = request.DisplayName;

        await context.SaveChangesAsync();
        return NoContent();
    }



    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var roles = await context.Roles.Select(r => new GetRolesResponseDto
        {
            Id = r.Id,
            Name = r.Name,
            DisplayName = r.DisplayName,
            DateCreated = r.DateCreated
        }).ToListAsync();

        return Ok(roles);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var role = await context.Roles
            .Where(r => r.Id == id)
            .Select(r => new GetRoleByIdResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                DisplayName = r.DisplayName,
                DateCreated = r.DateCreated
            }).FirstOrDefaultAsync();


        if (role is null)
        {
            return NotFound();
        }

        return Ok(role);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {

        var role = await context.Roles
            .FirstOrDefaultAsync(x => x.Id == id);

        if (role is null)
        {
            return NotFound();
        }

        context.Roles.Remove(role);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
