namespace Yarny.Pages;

public partial class CounterPage : ContentPage
{
	public CounterPage()
	{
		InitializeComponent();
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
        await Shell.Current.GoToAsync("//SearchPage");
    }

    private async void CounterPageTapped(object sender, EventArgs e)
    {
        await CounterPageeButton.ScaleTo(0.8, 100);
        await CounterPageeButton.ScaleTo(1.0, 100);
    }

    private async void ProfilePageTapped(object sender, EventArgs e)
    {
        await ProfilePageButton.ScaleTo(0.8, 100);
        await ProfilePageButton.ScaleTo(1.0, 100);
        await Shell.Current.GoToAsync("//ProfilePage");
    }

    private async void NewCounterTapped(object sender, EventArgs e)
    {
        await NewCounter.ScaleTo(0.95, 100);
        await NewCounter.ScaleTo(1.0, 100);

        Overlay.IsVisible = true;

        await Task.WhenAll(
            Overlay.FadeTo(1, 200),
            BottomSheet.TranslateTo(0, 0, 200, Easing.SinOut)
        );
    }

    private async Task HideBottomSheet()
    {
        await Task.WhenAll(
            Overlay.FadeTo(0, 200),
            BottomSheet.TranslateTo(0, BottomSheet.Height, 200, Easing.SinIn)
        );

        Overlay.IsVisible = false;
    }

    private async void OnOverlayTapped(object sender, EventArgs e)
    {
        await HideBottomSheet();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        

        await SaveButton.ScaleTo(0.95, 100);
        await SaveButton.ScaleTo(1.0, 100);

        await HideBottomSheet();
        // Логика сохранения
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await CancelButton.ScaleTo(0.95, 100);
        await CancelButton.ScaleTo(1.0, 100);

        await HideBottomSheet();
        // Логика отмены
    }

    private int _counter = 3;

    private void OnIncrementClicked(object sender, EventArgs e)
    {
        _counter++;
        CounterLabel.Text = _counter.ToString();
        AnimateButton(PlusBorder);
    }

    private void OnDecrementClicked(object sender, EventArgs e)
    {
        if (_counter > 0)
        {
            _counter--;
            CounterLabel.Text = _counter.ToString();
            AnimateButton(MinusBorder);
        }
    }

    private async void AnimateButton(Border border)
    {
        await border.ScaleTo(0.9, 100);
        await border.ScaleTo(1.0, 100);
    }
}