namespace Ecommerce.BackendAPI.Interfaces
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string GenerateToken(IUserInterface user);
        string GenerateRefreshToken(IUserInterface user);
    }
}