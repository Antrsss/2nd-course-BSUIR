using LAB_1_MAUI.Lab_3;
using LAB_1_MAUI.Lab_3.Services;
using Microsoft.Extensions.Logging;

namespace LAB_1_MAUI
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
                });

            builder.Services.AddTransient<IDbService, SQLiteService>();
            builder.Services.AddTransient<TeamList>();

            builder.Services.AddHttpClient<IRateService, RateService>("RB Currencies",
                opt => opt.BaseAddress = new Uri("https://www.nbrb.by/api/exrates/rates")
             );
            //builder.Services.AddTransient<IRateService, RateService>();
            builder.Services.AddSingleton<CurrencyConverter>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
