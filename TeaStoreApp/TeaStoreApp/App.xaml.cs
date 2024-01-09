using TeaStoreApp.Pages;

namespace TeaStoreApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        var accesstoken = Preferences.Get("accesstoken", string.Empty);
        if (string.IsNullOrEmpty(accesstoken))
        {
            MainPage = new NavigationPage(new SignupPage());
        }
        else
        {
            MainPage = new AppShell();
        }
    }
}
