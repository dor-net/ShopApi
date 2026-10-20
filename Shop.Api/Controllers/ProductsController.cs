using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Api.DTOs.Products;
using Shop.Api.Entities;

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

}
