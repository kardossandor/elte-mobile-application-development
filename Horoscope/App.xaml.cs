using Horoscope.Services;

namespace Horoscope;

public partial class App : Application
{
    public App(IAuthService auth)
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
        => new Window(new AppShell());
}
