namespace Shop.Api.Entities;

public class ProductImage : BaseEntity
{
    public int ProductId { get; set; }
    public bool IsMain { get; set; }
    public string Url { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
