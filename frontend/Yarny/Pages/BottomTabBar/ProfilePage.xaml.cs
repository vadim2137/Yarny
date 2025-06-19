using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace Yarny.Pages;

public partial class ProfilePage : ContentPage
{
    private ObservableCollection<ProfilePostItem> _mySchemes = new();
    private ObservableCollection<ProfilePostItem> _favorites = new();
    private ObservableCollection<ProgressItem> _progressItems = new();

    public ProfilePage()
    {
        InitializeComponent();
        LoadData();
        SetActiveTab(SchemesImage);
        LoadContent("schemes");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData();
        SetActiveTab(SchemesImage);
        LoadContent("schemes");
    }

    private void LoadData()
    {
        CreateProfileHeader();
        InitializeTestData();

    }

    private void InitializeTestData()
    {
        _mySchemes.Clear();
        _favorites.Clear();
        _progressItems.Clear();

        _mySchemes.Add(new ProfilePostItem("", "Зимний свитер", false, "sweater.jpg", ""));
        _mySchemes.Add(new ProfilePostItem("", "Шерстяные носки", false, "socks.jpg", ""));

        _favorites.Add(new ProfilePostItem("Мария Крючкова", "Вязаный свитер", true, "sweater.jpg", "maria_avatar.jpg"));
        _favorites.Add(new ProfilePostItem("Иван Петров", "Шерстяные носки", true, "socks.jpg", "ivan_avatar.jpg"));

        _progressItems.Add(new ProgressItem("Зимний свитер", "sweater.jpg", 65, true, "Анна Вязалова"));
        _progressItems.Add(new ProgressItem("Шерстяные носки", "socks.jpg", 30, false, "Петр Пряжкин"));
    }

    private async void OnTabTapped(object sender, TappedEventArgs e)
    {
        var image = sender as Image;
        SetActiveTab(image);
        LoadData();

        await image.ScaleTo(0.8, 100);
        await image.ScaleTo(1.0, 100);

        if (image == SchemesImage)
            LoadContent("schemes");
        else if (image == ProgressImage)
            LoadContent("progress");
        else if (image == FavoritesImage)
            LoadContent("favorites");
    }

    private void SetActiveTab(Image activeImage)
    {
        SchemesImage.Source = "my_schemes.svg";
        ProgressImage.Source = "my_progress.svg";
        FavoritesImage.Source = "my_favorites.svg";

        if (activeImage == SchemesImage)
            SchemesImage.Source = "my_schemes_active.svg";
        else if (activeImage == ProgressImage)
            ProgressImage.Source = "my_progress_active.svg";
        else if (activeImage == FavoritesImage)
            FavoritesImage.Source = "my_favorites_active.svg";
    }

    private void LoadContent(string tabName)
    {
        ContentArea.Content = tabName switch
        {
            "schemes" => CreateSchemesContent(),
            "progress" => CreateProgressContent(),
            "favorites" => CreateFavoritesContent(),
            _ => new Label { Text = "Контент не найден" }
        };
    }

    private View CreateSchemesContent()
    {
        var stackLayout = new VerticalStackLayout
        {
            Spacing = 20,
            Padding = new Thickness(20)
        };

        // 1. Сначала добавляем кнопку "Добавить схему +"
        var addButton = new Border
        {
            HeightRequest = 45,
            WidthRequest = 260,
            StrokeShape = new RoundRectangle { CornerRadius = 30 },
            StrokeThickness = 3,
            BackgroundColor = Color.FromArgb("#FDD5BD"),
            Stroke = Color.FromArgb("#FFBD96"),
            HorizontalOptions = LayoutOptions.Fill,
            Content = new Label
            {
                Text = "Добавить схему +",
                FontSize = 20,
                FontFamily = "MontserratAlternatesSemiBold",
                TextColor = Color.FromArgb("#FFF"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            }
        };

        // Обработчик нажатия на кнопку
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            // Анимация при нажатии
            await addButton.ScaleTo(0.95, 100);
            await addButton.ScaleTo(1.0, 100);

            // Логика добавления новой схемы (можно заменить на Navigation.PushModalAsync и т.д.)
            Console.WriteLine("Кнопка 'Добавить схему' нажата");
        };
        addButton.GestureRecognizers.Add(tapGesture);

        stackLayout.Children.Add(addButton); // Кнопка теперь вверху!

        // 2. Затем добавляем все существующие схемы
        foreach (var post in _mySchemes)
        {
            stackLayout.Children.Add(CreateMySchemeView(post));
        }

        return new ScrollView { Content = stackLayout };
    }

    private View CreateFavoritesContent()
    {
        var stackLayout = new VerticalStackLayout
        {
            Spacing = 20,
            Padding = new Thickness(20)
        };

        foreach (var post in _favorites)
        {
            stackLayout.Children.Add(CreateFavoriteView(post));
        }

        return new ScrollView { Content = stackLayout };
    }

    private View CreateProgressContent()
    {


        var stackLayout = new VerticalStackLayout
        {
            Spacing = 20,
            Padding = new Thickness(20)
        };

        foreach (var item in _progressItems)
        {
            stackLayout.Children.Add(CreateProgressView(item));
        }

        return new ScrollView { Content = stackLayout };
    }

    private Border CreateMySchemeView(ProfilePostItem post)
    {
        return new Border
        {
            HeightRequest = 200,
            StrokeShape = new RoundRectangle { CornerRadius = 40 },
            StrokeThickness = 3,
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#BCE4FF"),
            Content = new VerticalStackLayout
            {
                Padding = 15,
                Children =
                {
                    new Border
                    {
                        HeightRequest = 140,
                        StrokeShape = new RoundRectangle { CornerRadius = 40 },
                        BackgroundColor = Color.FromArgb("#BCE4FF"),
                        StrokeThickness = 0,
                        Content = new Image
                        {
                            Source = post.PostImage,
                            Aspect = Aspect.AspectFill
                        }
                    },
                    new Label
                    {
                        Text = post.PostTitle,
                        FontFamily = "MontserratAlternatesRegular",
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 20,
                        Margin = new Thickness(0, 5, 0, 0)
                    }
                }
            }
        };
    }

    private Border CreateFavoriteView(ProfilePostItem post)
    {
        var favoriteIcon = new Image
        {
            Source = post.IsFavorite ? "favorite_active.svg" : "favorite.svg",
            Aspect = Aspect.AspectFit,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            WidthRequest = 30,
            HeightRequest = 30,
            Margin = new Thickness(0, 10, 15, 0)
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) =>
        {
            post.IsFavorite = !post.IsFavorite;
            favoriteIcon.Source = post.IsFavorite ? "favorite_active.svg" : "favorite.svg";
            favoriteIcon.ScaleTo(0.8, 100).ContinueWith(t => favoriteIcon.ScaleTo(1.0, 100));
        };
        favoriteIcon.GestureRecognizers.Add(tapGesture);

        return new Border
        {
            HeightRequest = 255,
            StrokeShape = new RoundRectangle { CornerRadius = 40 },
            StrokeThickness = 3,
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#BCE4FF"),
            Content = new VerticalStackLayout
            {
                Padding = 15,
                Children =
                {
                    new Border
                    {
                        HeightRequest = 140,
                        StrokeShape = new RoundRectangle { CornerRadius = 40 },
                        BackgroundColor = Color.FromArgb("#BCE4FF"),
                        StrokeThickness = 0,
                        Content = new Grid
                        {
                            Children =
                            {
                                new Image
                                {
                                    Source = post.PostImage,
                                    Aspect = Aspect.AspectFill
                                },
                                favoriteIcon
                            }
                        }
                    },
                    new Label
                    {
                        Text = post.PostTitle,
                        FontFamily = "MontserratAlternatesRegular",
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 20,
                        Margin = new Thickness(0, 5, 0, 0)
                    },
                    new HorizontalStackLayout
                    {
                        Spacing = 10,
                        Children =
                        {
                            new Border
                            {
                                BackgroundColor = Color.FromArgb("#BCE4FF"),
                                StrokeShape = new RoundRectangle { CornerRadius = 25 },
                                HeightRequest = 50,
                                WidthRequest = 50,
                                Content = new Image
                                {
                                    Source = post.UserAvatar,
                                    Aspect = Aspect.AspectFill
                                }
                            },
                            new Label
                            {
                                Text = post.UserName,
                                FontFamily = "MontserratAlternatesRegular",
                                VerticalOptions = LayoutOptions.Center,
                                FontSize = 20,
                                TextColor = Color.FromArgb("#A3A3A3")
                            }
                        }
                    }
                }
            }
        };
    }

    private Border CreateProgressView(ProgressItem item)
    {
        var favoriteIcon = new Image
        {
            Source = item.IsFavorite ? "favorite_active.svg" : "favorite.svg",
            Aspect = Aspect.AspectFit,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            WidthRequest = 30,
            HeightRequest = 30,
            Margin = new Thickness(0, 10, 15, 0)
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) =>
        {
            item.IsFavorite = !item.IsFavorite;
            favoriteIcon.Source = item.IsFavorite ? "favorite_active.svg" : "favorite.svg";
            favoriteIcon.ScaleTo(0.8, 100).ContinueWith(t => favoriteIcon.ScaleTo(1.0, 100));
        };
        favoriteIcon.GestureRecognizers.Add(tapGesture);

        return new Border
        {
            HeightRequest = 280,
            StrokeShape = new RoundRectangle { CornerRadius = 40 },
            StrokeThickness = 3,
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#BCE4FF"),
            Content = new VerticalStackLayout
            {
                Padding = 15,
                Children =
                {
                    new Border
                    {
                        HeightRequest = 140,
                        StrokeShape = new RoundRectangle { CornerRadius = 40 },
                        BackgroundColor = Color.FromArgb("#BCE4FF"),
                        StrokeThickness = 0,
                        Content = new Grid
                        {
                            Children =
                            {
                                new Image
                                {
                                    Source = item.Image,
                                    Aspect = Aspect.AspectFill
                                },
                                favoriteIcon
                            }
                        }
                    },
                    new Label
                    {
                        Text = item.Title,
                        FontFamily = "MontserratAlternatesRegular",
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 20,
                        Margin = new Thickness(0, 5, 0, 0)
                    },
                    new Border
                    {
                        HeightRequest = 18,
                        StrokeShape = new RoundRectangle { CornerRadius = 9 },
                        StrokeThickness = 0,
                        HorizontalOptions = LayoutOptions.Fill,
                        BackgroundColor = Color.FromArgb("#E0E0E0"),
                        Margin = new Thickness(0, 5, 0, 5),
                        Content = new Grid
                        {
                            Children =
                            {
                                new Border
                                {
                                    WidthRequest = (255 * item.Progress / 100),
                                    HeightRequest = 18,
                                    StrokeShape = new RoundRectangle { CornerRadius = 9 },
                                    BackgroundColor = Color.FromArgb("#D1FF8F"),
                                    HorizontalOptions = LayoutOptions.Start
                                },
                                new Label
                                {
                                    Text = $"{item.Progress}%",
                                    FontSize = 12,
                                    TextColor = Color.FromArgb("#FFF"),
                                    HorizontalOptions = LayoutOptions.Center,
                                    VerticalOptions = LayoutOptions.Center
                                }
                            }
                        }
                    },
                    new HorizontalStackLayout
                    {
                        Spacing = 10,
                        Children =
                        {
                            new Border
                            {
                                BackgroundColor = Color.FromArgb("#BCE4FF"),
                                StrokeShape = new RoundRectangle { CornerRadius = 25 },
                                HeightRequest = 50,
                                WidthRequest = 50,
                                Content = new Image
                                {
                                    Source = "default_avatar.jpg",
                                    Aspect = Aspect.AspectFill
                                }
                            },
                            new Label
                            {
                                Text = item.AuthorName,
                                FontFamily = "MontserratAlternatesRegular",
                                VerticalOptions = LayoutOptions.Center,
                                FontSize = 20,
                                TextColor = Color.FromArgb("#A3A3A3")
                            }
                        }
                    }
                }
            }
        };
    }

    private void CreateProfileHeader()
    {
        ProfileHeaderContainer.Children.Clear();

        var avatarBorder = new Border
        {
            WidthRequest = 170,
            HeightRequest = 170,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
            BackgroundColor = Color.FromArgb("#BCE4FF"),
            HorizontalOptions = LayoutOptions.Center,
            StrokeThickness = 0,
            Margin = new Thickness(0, 20, 0, 0)
        };

        var avatarImage = new Image
        {
            Source = "dotnet_bot.png",
            Aspect = Aspect.AspectFill,
            WidthRequest = 170,
            HeightRequest = 170
        };
        avatarBorder.Content = avatarImage;
        ProfileHeaderContainer.Children.Add(avatarBorder);

        var usernameLabel = new Label
        {
            Text = "Имя пользователя",
            FontSize = 30,
            FontFamily = "MontserratAlternatesRegular",
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
        };
        ProfileHeaderContainer.Children.Add(usernameLabel);

        var statsGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 10,
            HorizontalOptions = LayoutOptions.Fill,
            Margin = new Thickness(0, 0, 0, 5)
        };

        var statsData = new[]
        {
            new { Value = "125", Label = "подписок" },
            new { Value = "342", Label = "подписчиков" },
            new { Value = "56", Label = "схем" }
        };

        for (int i = 0; i < statsData.Length; i++)
        {
            var statStack = new VerticalStackLayout { Spacing = 2 };

            statStack.Children.Add(new Label
            {
                Text = statsData[i].Value,
                FontSize = 18,
                FontFamily = "MontserratAlternatesMedium",
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Center
            });

            statStack.Children.Add(new Label
            {
                Text = statsData[i].Label,
                FontSize = 14,
                TextColor = Color.FromArgb("#A3A3A3"),
                FontFamily = "MontserratAlternatesMedium",
                HorizontalTextAlignment = TextAlignment.Center
            });

            statsGrid.Add(statStack, i, 0);
        }
        ProfileHeaderContainer.Children.Add(statsGrid);

        var editButton = new Border
        {
            WidthRequest = 250,
            HeightRequest = 45,
            StrokeShape = new RoundRectangle { CornerRadius = 25 },
            StrokeThickness = 3,
            BackgroundColor = Color.FromArgb("#FDBCE5"),
            Stroke = Color.FromArgb("#FF95D8"),
            HorizontalOptions = LayoutOptions.Center,
            Content = new Label
            {
                Text = "Изменить профиль",
                FontSize = 20,
                TextColor = Color.FromArgb("#FFF"),
                FontFamily = "MontserratAlternatesSemiBold",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            }
        };

        // Обработчик нажатия с анимацией
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            await editButton.ScaleTo(0.95, 100);
            await editButton.ScaleTo(1.0, 100);
            OnEditProfileClicked(); // Вызов существующего метода
        };
        editButton.GestureRecognizers.Add(tapGesture);

        ProfileHeaderContainer.Children.Add(editButton);
    }

    private void OnEditProfileClicked()
    {
        Console.WriteLine("Кнопка 'Изменить профиль' нажата");
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
        await Shell.Current.GoToAsync("//CounterPage");
    }

    private async void ProfilePageTapped(object sender, EventArgs e)
    {
        await ProfilePageButton.ScaleTo(0.8, 100);
        await ProfilePageButton.ScaleTo(1.0, 100);
    }


}

public class ProfilePostItem
{
    public string UserName { get; }
    public string PostTitle { get; }
    public bool IsFavorite { get; set; }
    public string PostImage { get; }
    public string UserAvatar { get; }

    public ProfilePostItem(string userName, string postTitle, bool isFavorite, string postImage, string userAvatar)
    {
        UserName = userName;
        PostTitle = postTitle;
        IsFavorite = isFavorite;
        PostImage = postImage;
        UserAvatar = userAvatar;
    }
}

public class ProgressItem
{
    public string Title { get; }
    public string Image { get; }
    public int Progress { get; }
    public bool IsFavorite { get; set; }
    public string AuthorName { get; }

    public ProgressItem(string title, string image, int progress, bool isFavorite, string authorName)
    {
        Title = title;
        Image = image;
        Progress = progress;
        IsFavorite = isFavorite;
        AuthorName = authorName;
    }
}