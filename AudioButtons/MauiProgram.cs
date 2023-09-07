using AudioButtons.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using The49.Maui.BottomSheet;

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
                .UseBottomSheet()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<Database>();
            builder.Services.AddSingleton<ButtonsViewModel>();
            builder.Services.AddTransient<ButtonViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}