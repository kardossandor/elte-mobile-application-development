using Horoscope.Services;

namespace Horoscope.Views;

public partial class DashboardPage : ContentPage
{
    private readonly IAuthService _auth;

    public DashboardPage(IAuthService auth)
    {
        InitializeComponent();
        _auth = auth;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var signedIn = await _auth.IsSignedInAsync();
            if (!signedIn)
            {
                await Shell.Current.GoToAsync("///login");
                return;
            }

            var email = await _auth.GetEmailAsync();
            SignedInAsLabel.Text = string.IsNullOrWhiteSpace(email)
                ? "Signed in"
                : $"Signed in as {email}";

            Busy.IsRunning = false;
            Busy.IsVisible = false;
            Root.Opacity = 1;
        }
        catch
        {
            await Shell.Current.GoToAsync("///login");
        }
    }

    private async void OnSignOutClicked(object sender, EventArgs e)
    {
        await _auth.SignOutAsync();
        await Shell.Current.GoToAsync("///login");
    }
}
