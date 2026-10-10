namespace Shop.Api.Entities;

public class Product : BaseEntity
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public long Price { get; set; }
    public ProductCategory Category { get; set; } = null!;
    public List<ProductImage> Images { get; set; } = null!;
}
