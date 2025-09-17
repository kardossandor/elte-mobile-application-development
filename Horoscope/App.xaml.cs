using Horoscope.Services;

namespace Horoscope;

public partial class App : Application
{
    private readonly IAuthService _auth;

    public App(IAuthService auth)
    {
        InitializeComponent();
        _auth = auth;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        window.Created += async (s, e) =>
        {
            if (await _auth.IsSignedInAsync())
                await Shell.Current.GoToAsync("///dashboard");
            else
                await Shell.Current.GoToAsync("///login");
        };

        return window;
    }
}
