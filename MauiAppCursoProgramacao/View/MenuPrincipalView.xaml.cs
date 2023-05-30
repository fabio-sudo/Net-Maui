
namespace MauiAppCursoProgramacao.View;

public partial class MenuPrincipalView : FlyoutPage
{
	public MenuPrincipalView()
	{
		InitializeComponent();

		App.MenuApp = this;
		App.Navegacao = Navegacao;
    }





}