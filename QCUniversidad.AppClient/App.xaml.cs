using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient;

public partial class App : Application
{
	public App(ShellViewModel viewModel)
	{
		InitializeComponent();

		var shell = new AppShell(viewModel);
		MainPage = shell;
	}
}