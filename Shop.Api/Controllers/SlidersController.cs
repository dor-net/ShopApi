using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Constants;
using Shop.Api.Data;
using Shop.Api.DTOs.Sliders;
using Shop.Api.Entities;
using System.Data;


namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SlidersController(ApplicationDbContext context,
    IWebHostEnvironment environment)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromForm] CreateSliderRequestDto request)
    {
        #region FileUpload
        //TODO: move to Helper Filer
        var uploadFolder = Path.Combine(environment.WebRootPath, "Sliders");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
        var filePath = Path.Combine(uploadFolder, fileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await request.Image.CopyToAsync(stream);
        #endregion FileUpload

        #region SaveToDatabase

        var slider = new Slider
        {
            ImageUrl = $"Sliders/{fileName}",
            IsActive = request.IsActive
        };

        context.Sliders.Add(slider);
        await context.SaveChangesAsync();

        #endregion SaveToDatabase

        return Created($"api/sliders/{slider.Id}", new
        {
            Id = slider.Id
        });
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var sliders = await context.Sliders.Select(s => new GetSliderResponseDto
        {
            Id = s.Id,
            ImageUrl = s.ImageUrl,
            IsActive = s.IsActive,
            DateCreated = s.DateCreated
        }).ToListAsync();

        return Ok(sliders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var slider = await context.Sliders
            .Where(s => s.Id == id)
            .Select(s => new GetSliderResponseDto
            {
                Id = s.Id,
                ImageUrl = s.ImageUrl,
                IsActive = s.IsActive,
                DateCreated = s.DateCreated
            }).FirstOrDefaultAsync();


        if (slider == null)
        {
            return NotFound(AppMessages.NotFoundMessage);
        }

        return Ok(slider);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromForm]UpdateSliderRequestDto request)
    {
        var slider = await context.Sliders
            .FirstOrDefaultAsync(s => s.Id == request.Id);

        if (slider == null)
        {
            return NotFound(AppMessages.NotFoundMessage);
        }
        
        #region FileUpload
        if (request.Image != null)
        {
            //Upload New File
            //TODO: move to Helper Filer
            var uploadFolder = Path.Combine(environment.WebRootPath, "Sliders");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
            var filePath = Path.Combine(uploadFolder, fileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await request.Image.CopyToAsync(stream);

            var oldFileName = slider.ImageUrl;

            slider.ImageUrl = $"Sliders/{fileName}";

            //Delete Old File

            var nornalizedName = oldFileName.Replace('/', Path.DirectorySeparatorChar);
            var oldFilePath = Path.Combine(environment.WebRootPath, nornalizedName);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

        }

        #endregion FileUpload


        #region UpdateDatabase

        slider.IsActive = request.IsActive;
        await context.SaveChangesAsync();

        #endregion UpdateDatabase


        return NoContent();
    }
}




