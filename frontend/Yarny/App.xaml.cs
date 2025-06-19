using Microsoft.Maui.Handlers;
using Yarny.Pages;

namespace Yarny
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ConfigureEntryHandler();


        }

        private void ConfigureEntryHandler()
        {
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell())
            {
                Title = "Yarny"
            };
        }

    }
}