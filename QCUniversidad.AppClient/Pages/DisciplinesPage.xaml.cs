using QCUniversidad.AppClient.ViewModels;

namespace QCUniversidad.AppClient.Pages;

public partial class DisciplinesPage : ContentPage
{
	public DisciplinesPage(DisciplinesPageViewModel disciplinesPage)
	{
		InitializeComponent();
		BindingContext = disciplinesPage;
	}
}