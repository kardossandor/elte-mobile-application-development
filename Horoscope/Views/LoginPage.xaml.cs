using Horoscope.Services;

namespace Horoscope.Views;

public partial class LoginPage : ContentPage
{
    private readonly IAuthService _auth;
    private bool _isBusy;
    private bool _passwordVisible;

    public LoginPage(IAuthService auth)
    {
        InitializeComponent();
        _auth = auth;
    }

    void SetBusy(bool busy)
    {
        _isBusy = busy;
        BusyIndicator.IsVisible = busy;
        BusyIndicator.IsRunning = busy;
        SignInButton.IsEnabled = !busy;
        SignUpButton.IsEnabled = !busy;
    }

    void ClearError()
    {
        ErrorLabel.IsVisible = false;
        ErrorLabel.Text = string.Empty;
    }

    private async void OnSignInClicked(object sender, EventArgs e)
        => await DoAuth(async () => await _auth.SignInAsync(GetEmail(), GetPwd()), goDashboard: true);

    private async void OnSignUpClicked(object sender, EventArgs e)
        => await DoAuth(async () => await _auth.SignUpAsync(GetEmail(), GetPwd()), goDashboard: true);

    private async Task DoAuth(Func<Task> taskAuth, bool goDashboard)
    {
        if (_isBusy) return;

        ClearError();

        if (!ValidateForm()) return;

        var email = GetEmail();
        var pwd = GetPwd();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pwd))
        {
            ShowError("Please enter email and password.");
            return;
        }

        try
        {
            SetBusy(true);
            await taskAuth();
            if (goDashboard)
                await Shell.Current.GoToAsync("///dashboard");
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private bool ValidateForm()
    {
        bool valid = true;

        var email = GetEmail();
        var pwd = GetPwd();

        if (string.IsNullOrWhiteSpace(email) || !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            EmailErrorLabel.Text = "Please enter a valid email address.";
            EmailErrorLabel.IsVisible = true;
            valid = false;
        }
        else
        {
            EmailErrorLabel.IsVisible = false;
        }

        if (string.IsNullOrWhiteSpace(pwd) || pwd.Length < 6)
        {
            PasswordErrorLabel.Text = "Password must be at least 6 characters.";
            PasswordErrorLabel.IsVisible = true;
            valid = false;
        }
        else
        {
            PasswordErrorLabel.IsVisible = false;
        }

        return valid;
    }

    private string GetEmail() => EmailEntry.Text?.Trim() ?? "";
    private string GetPwd() => PasswordEntry.Text ?? "";

    private async void OnPasswordCompleted(object sender, EventArgs e)
        => OnSignInClicked(sender, e);

    private void ShowError(string message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        });
    }

    private void OnPasswordToggleClicked(object sender, EventArgs e)
    {
        _passwordVisible = !_passwordVisible;
        PasswordEntry.IsPassword = !_passwordVisible;
        PasswordToggle.Source = _passwordVisible ? "eye_closed.png" : "eye.png";
    }

    private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        PasswordToggle.IsVisible = !string.IsNullOrEmpty(e.NewTextValue);
    }

    private void OnBackgroundTapped(object sender, TappedEventArgs e)
    {
        EmailEntry?.Unfocus();
        PasswordEntry?.Unfocus();
    }
}
