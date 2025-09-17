namespace Horoscope.Services;

public interface IAuthService
{
    Task SignUpAsync(string email, string password);
    Task SignInAsync(string email, string password);
    Task SignOutAsync();
    Task<bool> IsSignedInAsync();
    Task<string?> GetIdTokenAsync();
    Task<string?> GetEmailAsync();
}
