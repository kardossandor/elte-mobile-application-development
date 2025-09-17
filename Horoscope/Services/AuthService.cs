namespace Horoscope.Services;

public class AuthService : IAuthService
{
    private const string TokenKey = "auth_id_token";
    private const string RefreshKey = "auth_refresh_token";
    private const string EmailKey = "auth_email";
    private readonly HttpClient _http;

    public AuthService(IHttpClientFactory httpFactory)
    {
        _http = httpFactory.CreateClient();
    }

    public async Task SignUpAsync(string email, string password)
    {
        var req = new SignUpRequest(email, password, true);
        var resp = await _http.PostAsJsonAsync(FirebaseConstants.SignUpUrl, req);
        if (!resp.IsSuccessStatusCode)
            throw new Exception(await ReadFirebaseErrorAsync(resp));

        var data = await resp.Content.ReadFromJsonAsync<AuthResponse>()
                   ?? throw new Exception("Empty response from Firebase.");
        await SaveTokensAsync(data);
    }

    public async Task SignInAsync(string email, string password)
    {
        var req = new SignInRequest(email, password, true);
        var resp = await _http.PostAsJsonAsync(FirebaseConstants.SignInUrl, req);
        if (!resp.IsSuccessStatusCode)
            throw new Exception(await ReadFirebaseErrorAsync(resp));

        var data = await resp.Content.ReadFromJsonAsync<AuthResponse>()
                   ?? throw new Exception("Empty response from Firebase.");
        await SaveTokensAsync(data);
    }

    public async Task SignOutAsync()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(RefreshKey);
        SecureStorage.Remove(EmailKey);
        await Task.CompletedTask;
    }

    public async Task<bool> IsSignedInAsync()
        => !string.IsNullOrEmpty(await GetIdTokenAsync());

    public async Task<string?> GetIdTokenAsync()
        => await SecureStorage.GetAsync(TokenKey);

    private static async Task<string> ReadFirebaseErrorAsync(HttpResponseMessage resp)
    {
        var text = await resp.Content.ReadAsStringAsync();

        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(text);
            if (doc.RootElement.TryGetProperty("error", out var err) &&
                err.TryGetProperty("message", out var msgElem))
            {
                var code = msgElem.GetString() ?? "";
                return Humanize(code);
            }
        }
        catch
        {
            // ignore parse error, fallback below
        }

        // fallback: just return raw text if we couldn’t parse
        return $"Auth failed: {text}";
    }

    private static string Humanize(string code) => code switch
    {
        "EMAIL_EXISTS" => "This email is already registered.",
        "OPERATION_NOT_ALLOWED" => "Password sign-in is disabled for this project.",
        "TOO_MANY_ATTEMPTS_TRY_LATER" => "Too many attempts. Please try again later.",
        "EMAIL_NOT_FOUND" => "No account found with this email.",
        "INVALID_PASSWORD" or "INVALID_LOGIN_CREDENTIALS" => "Incorrect email or password.",
        "USER_DISABLED" => "This account has been disabled.",
        "INVALID_EMAIL" => "The email address is not valid.",
        _ => "Authentication failed. Please try again."
    };

    private static async Task SaveTokensAsync(AuthResponse data)
    {
        await SecureStorage.SetAsync(TokenKey, data.idToken);
        await SecureStorage.SetAsync(RefreshKey, data.refreshToken);
        await SecureStorage.SetAsync(EmailKey, data.email ?? "");
    }
}
