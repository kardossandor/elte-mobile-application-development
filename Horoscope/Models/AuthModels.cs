namespace Horoscope.Models;

public record SignUpRequest(string email, string password, bool returnSecureToken = true);
public record SignInRequest(string email, string password, bool returnSecureToken = true);

public record AuthResponse(string idToken, string refreshToken, string localId, string email);
