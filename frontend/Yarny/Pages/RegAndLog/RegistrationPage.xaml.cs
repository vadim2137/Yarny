namespace Yarny.Pages;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void OnUserAgreementClicked(object sender, EventArgs e)
    {
        try
        {
            await Launcher.Default.OpenAsync("https://google.com");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось открыть ссылку: {ex.Message}", "OK");
        }
    }

    private async void OnLoginButton(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EmailConfirmationPage");
    }
}