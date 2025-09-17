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

    private async void OnSignOutClicked(object sender, EventArgs e)
    {
        await _auth.SignOutAsync();
        await Shell.Current.GoToAsync("//login");
    }
}
