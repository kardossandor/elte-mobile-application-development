namespace Horoscope.Views;

public partial class LoginPage : ContentPage
{
    private readonly IAuthService _auth;
    private bool _isBusy;

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

    private void OnBackgroundTapped(object sender, TappedEventArgs e)
    {
        EmailEntry?.Unfocus();
        PasswordEntry?.Unfocus();
    }
}
