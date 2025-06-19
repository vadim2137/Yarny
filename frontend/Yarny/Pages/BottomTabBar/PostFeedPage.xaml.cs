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
            LoadFirstPage(); // ������������ ��� ������ �����
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
            // ������������� ��� ���������� �����
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

            // ������� ������� ������
            Posts.Clear();
            PostsContainer.Children.Clear();
            _currentPage = 0;

            // �������� ��������
            await Task.Delay(10);

            // �������� ������ ������
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

            // �������� ��������
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
                new PostItem("����� ��������", "1. ������� ������", false, "sweater.jpg", "maria_avatar.jpg"),
                new PostItem("���� ������", "2. ��������� �����", true, "socks.jpg", "ivan_avatar.jpg"),
                new PostItem("���� ��������", "3. ������� ����", false, "blanket.jpg", "anna_avatar.jpg"),
                new PostItem("����� ���������", "4. ����� � ��������", true, "hat.jpg", "olga_avatar.jpg"),
                new PostItem("������ ������", "5. ������� ����", false, "scarf.jpg", "sergey_avatar.jpg"),
                new PostItem("����� ���������", "6. ������� � ������", true, "mittens.jpg", "elena_avatar.jpg"),
                new PostItem("������� ������", "7. ������ ����", false, "blanket2.jpg", "dmitry_avatar.jpg"),
                new PostItem("�������� �������", "8. ������� ����������", true, "suit.jpg", "svetlana_avatar.jpg"),
                new PostItem("������� �������", "9. ������� ������", false, "sweater2.jpg", "alexey_avatar.jpg"),
                new PostItem("������� ���������", "10. ����-����", true, "snood.jpg", "tatyana_avatar.jpg"),
                new PostItem("������ �������", "11. ����� � ����������", false, "socks2.jpg", "mikhail_avatar.jpg"),
                new PostItem("������� �����", "12. ����� � ����������", true, "cardigan.jpg", "natalya_avatar.jpg"),
                new PostItem("����� ���������", "13. ������ ������", false, "sweater3.jpg", "artem_avatar.jpg"),
                new PostItem("�������� �������", "14. ���� �� �������", true, "blanket3.jpg", "victoria_avatar.jpg"),
                new PostItem("����� ��������", "15. �����-����", false, "beanie.jpg", "pavel_avatar.jpg"),
                new PostItem("���� �������", "16. ����� � ����������", true, "cardigan2.jpg", "yulia_avatar.jpg"),
                new PostItem("������ �����", "17. ���� � �������", false, "scarf2.jpg", "andrey_avatar.jpg"),
                new PostItem("��������� �������", "18. ������� ��� �����", true, "mittens2.jpg", "ekaterina_avatar.jpg"),
                new PostItem("����� �������", "19. ��������� � ���������", false, "hoodie.jpg", "igor_avatar.jpg"),
                new PostItem("������ �������", "20. ����� � ������", true, "hat2.jpg", "oksana_avatar.jpg"),
                new PostItem("����� ���������", "21. ����� � ������", false, "socks3.jpg", "roman_avatar.jpg"),
                new PostItem("������� ������", "22. ���� � ���������", true, "blanket4.jpg", "lyudmila_avatar.jpg"),
                new PostItem("������� ��������", "23. ������ � �������", false, "sweater4.jpg", "vasily_avatar.jpg"),
                new PostItem("������ ��������", "24. ���� � ��������", true, "shawl.jpg", "galina_avatar.jpg"),
                new PostItem("��������� ���������", "25. ����� ��� �������", false, "vest.jpg", "stanislav_avatar.jpg"),
                new PostItem("����� �������", "26. ����� ��� ������", true, "legwarmers.jpg", "alina_avatar.jpg"),
                new PostItem("����� �������", "27. ������", false, "sweater5.jpg", "boris_avatar.jpg"),
                new PostItem("��� ��������", "28. ����� � ������", true, "hat3.jpg", "zoya_avatar.jpg"),
                new PostItem("������ ������", "29. ����-�����", false, "snood2.jpg", "kirill_avatar.jpg"),
                new PostItem("������ �������", "30. �������� ��� �������", true, "gloves.jpg", "larisa_avatar.jpg"),
                new PostItem("������ ����������", "31. ����� �� ������", false, "zip_cardigan.jpg", "maxim_avatar.jpg"),
                new PostItem("���� ����������", "32. ���� �� ������", true, "blanket5.jpg", "nina_avatar.jpg"),
                new PostItem("���� �������", "33. ����� � ��������", false, "hat4.jpg", "oleg_avatar.jpg"),
                new PostItem("������ �������", "34. �����-�����", true, "bag.jpg", "polina_avatar.jpg"),
                new PostItem("������ ������", "35. ���� ������", false, "scarf3.jpg", "ruslan_avatar.jpg"),
                new PostItem("������� �������", "36. ������� � �������", true, "mittens3.jpg", "snezhana_avatar.jpg"),
                new PostItem("������� �������", "37. ������ � ������", false, "sweater6.jpg", "timofey_avatar.jpg"),
                new PostItem("������ ���������", "38. ����� � ���������", true, "poncho.jpg", "ulyana_avatar.jpg"),
                new PostItem("����� �������", "39. ����� ��� ������", false, "socks4.jpg", "fedor_avatar.jpg"),
                new PostItem("����� �������", "40. �����-������", true, "ushanka.jpg", "elina_avatar.jpg"),
                new PostItem("������� �����", "41. ����� � ����������", false, "jacket.jpg", "yaroslav_avatar.jpg"),
                new PostItem("�������� �������", "42. �������� �������", true, "headband.jpg", "angelina_avatar.jpg"),
                new PostItem("������� ��������", "43. ����-�������", false, "scarf4.jpg", "arkadiy_avatar.jpg"),
                new PostItem("������� ������", "44. ���� ��� �������", true, "picnic_blanket.jpg", "bogdana_avatar.jpg"),
                new PostItem("��������� ��������", "45. ������ � �������", false, "sweater7.jpg", "vladislav_avatar.jpg"),
                new PostItem("�������� ��������", "46. ���������", true, "balaclava.jpg", "gennadiy_avatar.jpg"),
                new PostItem("����� ����������", "47. ����-��������", false, "skirt.jpg", "darya_avatar.jpg"),
                new PostItem("������� �������", "48. ����� � ���������", true, "vest2.jpg", "evgeniy_avatar.jpg"),
                new PostItem("����� �������", "49. ����� ��� ��������", false, "phonecase.jpg", "zhanna_avatar.jpg"),
                new PostItem("����� �������", "50. ����-�����", true, "mesh_scarf.jpg", "zakhar_avatar.jpg"),
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