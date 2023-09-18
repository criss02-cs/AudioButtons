using AudioButtons.ViewModels;
using AudioButtons.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace AudioButtons
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                    fonts.AddFont("fa-solid-900.ttf", "FaSolid");
                    fonts.AddFont("TT-Commons-Bold.otf", "TTCommonsBold");
                });
            builder.Services.AddSingleton<Database>();
            builder.Services.AddSingleton<ButtonsViewModel>();
            builder.Services.AddTransient<ButtonViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<ButtonPage>();

            

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}