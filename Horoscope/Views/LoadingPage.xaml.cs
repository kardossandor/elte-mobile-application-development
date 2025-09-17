using Horoscope.Services;

namespace Horoscope.Views;

public partial class LoadingPage : ContentPage
{
    private readonly IAuthService _auth;

    public LoadingPage(IAuthService auth)
    {
        InitializeComponent();
        _auth = auth;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (await _auth.IsSignedInAsync())
            await Shell.Current.GoToAsync("///dashboard");
        else
            await Shell.Current.GoToAsync("///login");
    }
}
