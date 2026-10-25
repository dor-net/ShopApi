using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Constants;
using Shop.Api.Data;
using Shop.Api.DTOs.Products;
using Shop.Api.DTOs.Roles;
using Shop.Api.Entities;
using System.Data;

namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ApplicationDbContext context,
    IWebHostEnvironment environment)
    : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult> Create(CreateProductRequestDto request)
    {





        if (!await context.ProductCategories
            .AnyAsync(x => x.Id == request.CategoryId))
        {
            return BadRequest("شناسه دسته بندی صحیح نمی باشد");
        }

        if (request.Images != null && request.Images.Any())
        {

            #region FileUpload


            //TODO: move to Helper File
            var uploadFolder = Path.Combine(environment.WebRootPath, "Products");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }


            var product = new Product()
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Price = request.Price,
                Images = []
            };

            bool isFirst = true;

            foreach (var file in request.Images)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadFolder, fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);


                product.Images.Add(new ProductImage
                {
                    Url = $"Products/{fileName}",
                    IsMain = isFirst ? true : false
                });

                isFirst = false;
            }

            context.Products.Add(product);
            await context.SaveChangesAsync();

            #endregion FileUpload

        }





        return Ok();

    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromForm] UpdateProductRequestDto request)
    {
        if (id != request.Id)
        {
            return BadRequest("شناسه بدنه درخواست با شناسه آدرس مطابقت ندارد");
        }

        var product = await context
            .Products
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.CategoryId = request.CategoryId;


        var forRemoveImageNames = new List<string>();

        if (request.RemovedImageIds != null &&
            request.RemovedImageIds.Any())
        {
            var forRemoveImages = product
                .Images
                .Where(x => request.RemovedImageIds.Contains(x.Id))
                .ToList();

            if (forRemoveImages.Count
                != request.RemovedImageIds.Count)
            {

                return BadRequest("شناسه ارسالی تصاویر جهت حذف معتبر نمی باشد");
            }

            context.ProductImages.RemoveRange(forRemoveImages);

            forRemoveImageNames.AddRange(forRemoveImages.Select(x => x.Url));
        }

        if (request.Images != null && request.Images.Any())
        {
            #region FileUpload

            //TODO: move to Helper File
            var uploadFolder = Path.Combine(environment.WebRootPath, "Products");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            foreach (var file in request.Images)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadFolder, fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                product.Images.Add(new ProductImage
                {
                    Url = $"Products/{fileName}",
                });
            }

            await context.SaveChangesAsync();

            if (forRemoveImageNames.Any())
            {

                foreach (var url in forRemoveImageNames)
                {
                    var normalizedName = url.Replace('/', Path.DirectorySeparatorChar);
                    var imagePath = Path.Combine(environment.WebRootPath, normalizedName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }

            #endregion FileUpload

        }

        return Ok();
    }


    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var products = await context.Products.Select(p => new GetProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name,
            Price = p.Price,

        }).ToListAsync();

        return Ok(products);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var product = await context.Products
            .Where(x => x.Id == id)
            .Select(p => new GetProductByIdResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Price = p.Price,
                Images = p.Images.Select(i => new ProductImageResponseDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    IsMain = i.IsMain
                }).ToList()

            }).FirstOrDefaultAsync();


        if (product is null)
        {
            return NotFound(AppMessages.NotFoundMessage);
        }

        return Ok(product);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await context.Products
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
        {
            return NotFound();
        }

        context.ProductImages.RemoveRange(product.Images);
        context.Products.Remove(product);
        await context.SaveChangesAsync();

        if (product.Images.Any())
        {
            foreach (var image in product.Images)
            {
                var normalizedName = image.Url.Replace('/', Path.DirectorySeparatorChar);
                var imagePath = Path.Combine(environment.WebRootPath, normalizedName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
        }

        return NoContent();
    }
}
