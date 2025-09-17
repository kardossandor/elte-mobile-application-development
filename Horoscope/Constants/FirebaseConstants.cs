namespace Horoscope.Constants;

public static class FirebaseConstants
{
    public const string ApiKey = "AIzaSyBqokTqSZP52iYnYNK6DqLVY5k6UaUQgu0";

    public static readonly string SignUpUrl =
        $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}";
    public static readonly string SignInUrl =
        $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}";
}
