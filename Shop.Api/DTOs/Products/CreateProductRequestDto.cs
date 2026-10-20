using Shop.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shop.Api.DTOs.Products;

public class CreateProductRequestDto
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "نام الزامی می باشد")]
    [MaxLength(200, ErrorMessage = "نام نمی تواند بیشتر از 200 کاراکتر باشد")]

    public string Name { get; set; } = null!;

    [Range(0, long.MaxValue)]
    public long Price { get; set; }


    public List<IFormFile>? Images { get; set; } = null!;
}
