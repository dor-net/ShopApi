namespace Shop.Api.DTOs.Roles;

public class GetRolesResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public DateTime DateCreated { get; set; }
}
