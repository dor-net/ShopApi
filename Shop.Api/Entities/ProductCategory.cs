namespace Shop.Api.Entities;

public class ProductCategory : BaseEntity
{
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
    public ProductCategory? Parent { get; set; }
    public List<ProductCategory> Children { get; set; } = null!;
    public List<Product> Products { get; set; } = null!;
}
