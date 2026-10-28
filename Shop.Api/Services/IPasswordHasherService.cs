namespace Shop.Api.Services;

public interface IPasswordHasherService
{
    string Hash(string password);
    bool Verifiy(string passwordHash, string password);
}