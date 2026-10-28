using Shop.Api.Entities;

namespace Shop.Api.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}