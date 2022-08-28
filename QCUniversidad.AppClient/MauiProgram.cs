using IdentityModel.OidcClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Authentication;
using QCUniversidad.AppClient.PlataformServices;
using QCUniversidad.AppClient.Services.Authentication;
using QCUniversidad.AppClient.ViewModels;
using System.Net;

namespace QCUniversidad.AppClient;

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

		#region Page and ViewModels registrations

		builder.Services.AddSingleton<ShellViewModel>();

		builder.Services.AddTransient<LoadingPageViewModel>();
		builder.Services.AddTransient<LoadingPage>();

		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<MainPage>();

		#endregion
		builder.Services.AddSingleton<IUserManager, UserManager>();
		builder.Services.AddSingleton<ITokenManager, TokenManager>();
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

		builder.Services.AddTransient<WebAuthenticatorBrowser>();
		builder.Services.AddTransient<OidcClient>(sp => new OidcClient(
			new OidcClientOptions
			{
				Authority = "https://10.0.2.2:5001",
				ClientId = "QCUniversidad.AppClient",
				Scope = "openid profile qcuniversidad.api",
				RedirectUri = "qcuniversidadappclient://",
				PostLogoutRedirectUri = "qcuniversidadappclient://",
				ClientSecret = "qcforlife2022",
				BackchannelHandler = new HttpsClientHandlerService().GetPlatformMessageHandler(),
				Browser = sp.GetRequiredService<WebAuthenticatorBrowser>()
			}));
		return builder.Build();
	}
}