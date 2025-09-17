namespace Horoscope;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        window.Created += async (s, e) =>
        {
            await Shell.Current.GoToAsync("//login");
        };

        return window;
    }
}
