using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Yarny.Pages
{
    public partial class PostFeedPage : ContentPage
    {
        private const int PageSize = 10;
        private int _currentPage = 0;
        private bool _isLoading = false;

        public ObservableCollection<PostItem> Posts { get; } = new ObservableCollection<PostItem>();

        public PostFeedPage()
        {
            InitializeComponent();
            LoadFirstPage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadFirstPage(); // Перезагрузка при каждом входе
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
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        private async void PostFeedPageTapped(object sender, EventArgs e)
        {
            await PostFeedPageButton.ScaleTo(0.8, 100);
            await PostFeedPageButton.ScaleTo(1.0, 100);
            await MainScrollView.ScrollToAsync(0, 0, true);
        }

        private async void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            // Автоподгрузка при достижении конца
            if (e.ScrollY >= MainScrollView.ContentSize.Height - MainScrollView.Height - 100 &&
                !_isLoading && !RefreshView.IsRefreshing)
            {
                await LoadMorePosts();
            }
        }

        private async void OnRefreshing(object sender, EventArgs e)
        {
            await LoadFirstPage();
            RefreshView.IsRefreshing = false;
        }

        private async Task LoadFirstPage()
        {
            if (_isLoading) return;

            _isLoading = true;

            // Очистка текущих данных
            Posts.Clear();
            PostsContainer.Children.Clear();
            _currentPage = 0;

            // Имитация загрузки
            await Task.Delay(10);

            // Загрузка первых постов
            var newPosts = GetPosts(0, PageSize);
            foreach (var post in newPosts)
            {
                Posts.Add(post);
                PostsContainer.Children.Add(CreatePostView(post));
            }

            _currentPage = 1;
            _isLoading = false;
        }

        private async Task LoadMorePosts()
        {
            if (_isLoading) return;

            _isLoading = true;

            // Имитация загрузки
            await Task.Delay(10);

            var newPosts = GetPosts(_currentPage * PageSize, PageSize);
            foreach (var post in newPosts)
            {
                Posts.Add(post);
                PostsContainer.Children.Add(CreatePostView(post));
            }

            _currentPage++;
            _isLoading = false;
        }

        private List<PostItem> GetPosts(int skip, int take)
        {
            var allPosts = new List<PostItem>
            {
                new PostItem("Мария Крючкова", "1. Вязаный свитер", false, "sweater.jpg", "maria_avatar.jpg"),
                new PostItem("Иван Петров", "2. Шерстяные носки", true, "socks.jpg", "ivan_avatar.jpg"),
                new PostItem("Анна Сидорова", "3. Детский плед", false, "blanket.jpg", "anna_avatar.jpg"),
                new PostItem("Ольга Вязанкина", "4. Шапка с помпоном", true, "hat.jpg", "olga_avatar.jpg"),
                new PostItem("Сергей Спицын", "5. Ажурный шарф", false, "scarf.jpg", "sergey_avatar.jpg"),
                new PostItem("Елена Вязалкина", "6. Варежки с узором", true, "mittens.jpg", "elena_avatar.jpg"),
                new PostItem("Дмитрий Ниткин", "7. Теплый плед", false, "blanket2.jpg", "dmitry_avatar.jpg"),
                new PostItem("Светлана Петрова", "8. Детский комбинезон", true, "suit.jpg", "svetlana_avatar.jpg"),
                new PostItem("Алексей Крючков", "9. Мужской свитер", false, "sweater2.jpg", "alexey_avatar.jpg"),
                new PostItem("Татьяна Шерстяная", "10. Шарф-снуд", true, "snood.jpg", "tatyana_avatar.jpg"),
                new PostItem("Михаил Вязалов", "11. Носки с орнаментом", false, "socks2.jpg", "mikhail_avatar.jpg"),
                new PostItem("Наталья Пряжа", "12. Кофта с воротником", true, "cardigan.jpg", "natalya_avatar.jpg"),
                new PostItem("Артем Свитеркин", "13. Зимний свитер", false, "sweater3.jpg", "artem_avatar.jpg"),
                new PostItem("Виктория Пледова", "14. Плед из альпаки", true, "blanket3.jpg", "victoria_avatar.jpg"),
                new PostItem("Павел Шапочкин", "15. Шапка-бини", false, "beanie.jpg", "pavel_avatar.jpg"),
                new PostItem("Юлия Вязаная", "16. Кофта с пуговицами", true, "cardigan2.jpg", "yulia_avatar.jpg"),
                new PostItem("Андрей Нитко", "17. Шарф в полоску", false, "scarf2.jpg", "andrey_avatar.jpg"),
                new PostItem("Екатерина Моткова", "18. Варежки для детей", true, "mittens2.jpg", "ekaterina_avatar.jpg"),
                new PostItem("Игорь Пряжкин", "19. Толстовка с капюшоном", false, "hoodie.jpg", "igor_avatar.jpg"),
                new PostItem("Оксана Вязанка", "20. Шапка с ушками", true, "hat2.jpg", "oksana_avatar.jpg"),
                new PostItem("Роман Шерстянов", "21. Носки с пяткой", false, "socks3.jpg", "roman_avatar.jpg"),
                new PostItem("Людмила Клубок", "22. Плед в квадратах", true, "blanket4.jpg", "lyudmila_avatar.jpg"),
                new PostItem("Василий Свитеров", "23. Свитер с оленями", false, "sweater4.jpg", "vasily_avatar.jpg"),
                new PostItem("Галина Прядкина", "24. Шаль с бахромой", true, "shawl.jpg", "galina_avatar.jpg"),
                new PostItem("Станислав Вязальщик", "25. Жилет без рукавов", false, "vest.jpg", "stanislav_avatar.jpg"),
                new PostItem("Алина Ниточка", "26. Гетры для танцев", true, "legwarmers.jpg", "alina_avatar.jpg"),
                new PostItem("Борис Крючков", "27. Свитер", false, "sweater5.jpg", "boris_avatar.jpg"),
                new PostItem("Зоя Шерстова", "28. Шапка с косами", true, "hat3.jpg", "zoya_avatar.jpg"),
                new PostItem("Кирилл Пряжин", "29. Шарф-труба", false, "snood2.jpg", "kirill_avatar.jpg"),
                new PostItem("Лариса Вязаная", "30. Перчатки без пальцев", true, "gloves.jpg", "larisa_avatar.jpg"),
                new PostItem("Максим Свитерович", "31. Кофта на молнии", false, "zip_cardigan.jpg", "maxim_avatar.jpg"),
                new PostItem("Нина Клубочкина", "32. Плед из травки", true, "blanket5.jpg", "nina_avatar.jpg"),
                new PostItem("Олег Вязалов", "33. Шапка с помпоном", false, "hat4.jpg", "oleg_avatar.jpg"),
                new PostItem("Полина Ниткина", "34. Сумка-мешок", true, "bag.jpg", "polina_avatar.jpg"),
                new PostItem("Руслан Спицын", "35. Шарф клетка", false, "scarf3.jpg", "ruslan_avatar.jpg"),
                new PostItem("Снежана Прядова", "36. Варежки с оленями", true, "mittens3.jpg", "snezhana_avatar.jpg"),
                new PostItem("Тимофей Вязаный", "37. Свитер с узором", false, "sweater6.jpg", "timofey_avatar.jpg"),
                new PostItem("Ульяна Шерстяная", "38. Пончо с карманами", true, "poncho.jpg", "ulyana_avatar.jpg"),
                new PostItem("Федор Крючков", "39. Носки для похода", false, "socks4.jpg", "fedor_avatar.jpg"),
                new PostItem("Элина Вязанка", "40. Шапка-ушанка", true, "ushanka.jpg", "elina_avatar.jpg"),
                new PostItem("Ярослав Пряжа", "41. Жакет с пуговицами", false, "jacket.jpg", "yaroslav_avatar.jpg"),
                new PostItem("Ангелина Ниткова", "42. Головная повязка", true, "headband.jpg", "angelina_avatar.jpg"),
                new PostItem("Аркадий Вязалкин", "43. Шарф-пашмина", false, "scarf4.jpg", "arkadiy_avatar.jpg"),
                new PostItem("Богдана Клубок", "44. Плед для пикника", true, "picnic_blanket.jpg", "bogdana_avatar.jpg"),
                new PostItem("Владислав Свитеров", "45. Свитер с молнией", false, "sweater7.jpg", "vladislav_avatar.jpg"),
                new PostItem("Геннадий Шапочник", "46. Балаклава", true, "balaclava.jpg", "gennadiy_avatar.jpg"),
                new PostItem("Дарья Вязальщица", "47. Юбка-карандаш", false, "skirt.jpg", "darya_avatar.jpg"),
                new PostItem("Евгений Пряжкин", "48. Жилет с карманами", true, "vest2.jpg", "evgeniy_avatar.jpg"),
                new PostItem("Жанна Ниткина", "49. Чехол для телефона", false, "phonecase.jpg", "zhanna_avatar.jpg"),
                new PostItem("Захар Вязалов", "50. Шарф-сетка", true, "mesh_scarf.jpg", "zakhar_avatar.jpg"),
            };

            return allPosts.Skip(skip).Take(take).ToList();
        }

        private Border CreatePostView(PostItem post)
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
    }

    public class PostItem
    {
        public string UserName { get; }
        public string PostTitle { get; }
        public bool IsFavorite { get; set; }
        public string PostImage { get; }
        public string UserAvatar { get; }

        public PostItem(string userName, string postTitle, bool isFavorite, string postImage, string userAvatar)
        {
            UserName = userName;
            PostTitle = postTitle;
            IsFavorite = isFavorite;
            PostImage = postImage;
            UserAvatar = userAvatar;
        }
    }
}