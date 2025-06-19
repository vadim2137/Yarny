
using Yarny.Api;

namespace Yarny.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegistrationPage");
    }

    private async void OnLoginButton(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//PostFeedPage");

        //var login = LoginEntry.Text;
        //var pass = PassEntry.Text;

        //var error = await ApiClient.SendRequestAsync(
        //    "/login",
        //    new LoginModel { Email = login, Password = pass },
        //    HttpMethod.Post);

        //if (error is null)
        //{
        //    // Успех - нет ошибок
        //    Console.WriteLine("Успешно!");
        //    await Launcher.Default.OpenAsync("https://google.com");
        //}
        //else
        //{
        //    // Обработка ошибки
        //    //Console.WriteLine($"Ошибка: {error}");
        //    await Shell.Current.DisplayAlert("Возникло исключение", $"{error}", "OK");
        //}
    }

    private async void OnPRClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//PasswordRecoveryPage1");
    }
}
