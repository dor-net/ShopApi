namespace Shop.Api.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Mobile { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public List<UserRole> UserRoles { get; set; } = null!;
}
