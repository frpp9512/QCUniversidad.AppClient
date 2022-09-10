using IdentityModel.OidcClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Authentication;
using QCUniversidad.AppClient.Pages;
using QCUniversidad.AppClient.PlataformServices;
using QCUniversidad.AppClient.Services.Authentication;
using QCUniversidad.AppClient.Services.Data;
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

		builder.Services.AddTransient<TeachersViewModel>();
		builder.Services.AddTransient<TeachersPage>();

		builder.Services.AddTransient<FacultiesPageViewModel>();
		builder.Services.AddTransient<FacultiesPage>();

		builder.Services.AddTransient<AddEditFacultyPageViewModel>();
		builder.Services.AddTransient<AddEditFacultyPage>();

		builder.Services.AddTransient<FacultyDetailsPageViewModel>();
		builder.Services.AddTransient<FacultyDetailsPage>();

		builder.Services.AddTransient<AddEditDeparmentPageViewModel>();
		builder.Services.AddTransient<AddEditDeparmentPage>();

		builder.Services.AddTransient<AddEditCareerPageViewModel>();
		builder.Services.AddTransient<AddEditCareerPage>();

		builder.Services.AddTransient<DisciplinesPageViewModel>();
		builder.Services.AddTransient<DisciplinesPage>();

		builder.Services.AddTransient<AddEditDisciplinePageViewModel>();
		builder.Services.AddTransient<AddEditDisciplinePage>();

        #endregion

        builder.Services.AddTransient<IHttpClientFactory, MauiHttpClientFactory>();
		builder.Services.AddTransient<IApiCallerHttpClientFactory, ApiCallerHttpClientFactory>();
		builder.Services.AddSingleton<IUserManager, UserManager>();
		builder.Services.AddTransient<IAuthenticationHandler, AuthenticationHandler>();
		builder.Services.AddSingleton<ITokenManager, TokenManager>();
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		builder.Services.AddTransient<IDataProvider, DataProvider>();
		builder.Services.AddSingleton<ITimersHandler, TimersHandler>();

		builder.Services.AddTransient<WebAuthenticatorBrowser>();
		builder.Services.AddTransient<OidcClient>(sp => new OidcClient(
			new OidcClientOptions
			{
				Authority = "https://10.0.2.2:5001",
				ClientId = "QCUniversidad.AppClient",
				Scope = "openid profile roles qcuniversidad.api",
				RedirectUri = "qcuniversidadappclient://",
				PostLogoutRedirectUri = "qcuniversidadappclient://",
				ClientSecret = "qcforlife2022",
				BackchannelHandler = new HttpsClientHandlerService().GetPlatformMessageHandler(),
				Browser = sp.GetRequiredService<WebAuthenticatorBrowser>()
			}));

		builder.Services.AddAutoMapper(typeof(MauiProgram));

        return builder.Build();
	}
}