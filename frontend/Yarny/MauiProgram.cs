using Microsoft.Extensions.Logging;

namespace Yarny
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("MontserratAlternates-Regular.ttf", "MontserratAlternatesRegular");
                    fonts.AddFont("MontserratAlternates-Bold.ttf", "MontserratAlternatesBold");
                    fonts.AddFont("MontserratAlternates-Medium.ttf", "MontserratAlternatesMedium");
                    fonts.AddFont("MontserratAlternates-SemiBold.ttf", "MontserratAlternatesSemiBold");
                    fonts.AddFont("MontserratAlternates-Light.ttf", "MontserratAlternatesLight");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
