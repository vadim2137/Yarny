namespace Yarny.Pages;

public partial class SearchPage : ContentPage
{
    private readonly Dictionary<string, bool> _tagStates = new();

    public SearchPage()
    {
        InitializeComponent();
    }

    private async void OnTagTapped(object sender, EventArgs e)
    {
        if (sender is Border border)
        {
            // Анимация уменьшения и увеличения
            await border.ScaleTo(0.8, 100);
            await border.ScaleTo(1.0, 100);

            // Получаем имя тега через x:Name
            var tagName = border.StyleId ?? border.AutomationId ?? border.ClassId ?? border.BindingContext?.ToString() ?? Guid.NewGuid().ToString();

            var isActive = _tagStates.TryGetValue(tagName, out var state) ? !state : true;
            _tagStates[tagName] = isActive;

            // Применяем стиль в зависимости от состояния
            if (isActive)
            {
                border.Background = border.Stroke; // Используем Stroke как Background
                if (border.Content is Label label)
                {
                    label.TextColor = Colors.White;
                }
            }
            else
            {
                border.Background = new SolidColorBrush(Colors.White);
                if (border.Content is Label label)
                {
                    label.TextColor = Colors.Black;
                }
            }
        }
    }

    private async void ResetFiltersTapped(object sender, EventArgs e)
    {
        await ResetFilters.ScaleTo(0.95, 100);
        await ResetFilters.ScaleTo(1.0, 100);
    }

    private async void PostFeedPageTapped(object sender, EventArgs e)
    {
        await PostFeedPageButton.ScaleTo(0.8, 100);
        await PostFeedPageButton.ScaleTo(1.0, 100);
        await Shell.Current.GoToAsync("//PostFeedPage");
    }

    private async void SearchPageTapped(object sender, EventArgs e)
    {
        await SearchPageeButton.ScaleTo(0.8, 100);
        await SearchPageeButton.ScaleTo(1.0, 100);
    }

    private async void CounterPageTapped(object sender, EventArgs e)
    {
        await CounterPageeButton.ScaleTo(0.8, 100);
        await CounterPageeButton.ScaleTo(1.0, 100);
        await Shell.Current.GoToAsync("//CounterPage");
    }

    private async void ProfilePageTapped(object sender, EventArgs e)
    {
        await ProfilePageButton.ScaleTo(0.8, 100);
        await ProfilePageButton.ScaleTo(1.0, 100);
        await Shell.Current.GoToAsync("//ProfilePage");
    }
}