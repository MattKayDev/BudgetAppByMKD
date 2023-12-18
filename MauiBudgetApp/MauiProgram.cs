#if ANDROID
using MauiBudgetApp.Platforms.Android;
#endif
//#if IOS
//using MauiBudgetApp.Platforms.iOS;
//#endif

using MauiBudgetApp.Services;
using Microsoft.Extensions.Logging;

namespace MauiBudgetApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureMauiHandlers(handlers => {
#if ANDROID
                handlers.AddHandler<CustomViewCell, CustomViewCellHandler>();
#endif

//#if IOS
//				handlers.AddHandler<CustomViewCell, CustomViewCellHandler>();
//#endif
			})
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var expensePayItemService = new ExpensePayItemService();
        var incomePayItemService = new IncomePayItemService();
        builder.Services.AddSingleton<ExpensePayItemService>(expensePayItemService);
        builder.Services.AddSingleton<IncomePayItemService>(incomePayItemService);

        return builder.Build();
	}
}
