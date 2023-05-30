using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;

namespace MauiAppCursoProgramacao.View;

public partial class MenuView : ContentPage
{

    public MenuViewModel objMenuLista { get; set; }//Encapsulameto
    public MenuView()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        objMenuLista = new MenuViewModel();
        objMenuLista.Lista = new List<MenuModel>//Adiciona menus a lista
        {
            new MenuModel() { nomeDescricao = "Usuario", nomeIcone = "usericone" },
            new MenuModel() { nomeDescricao = "Alunos", nomeIcone = "estudante_icone" },
            new MenuModel() { nomeDescricao = "Professor", nomeIcone = "professor_icone" },
            new MenuModel() { nomeDescricao = "Período", nomeIcone = "periodo_icone" },
            new MenuModel() { nomeDescricao = "Matrícula", nomeIcone = "matricula_icone" },
            new MenuModel() { nomeDescricao = "Sair", nomeIcone = "sair_icone" }
        }; // Inicializa a lista

        BindingContext = this;
    }

    private async void lstMenu_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        try
        {
            MenuModel selectedItem = (MenuModel)e.Item;


            if (selectedItem.nomeDescricao == "Usuario")
            {
                _ = App.Navegacao.PushAsync(new UsuarioView());

            }

            else if (selectedItem.nomeDescricao == "Alunos")
            {
                _ = App.Navegacao.PushAsync(new AlunoBuscarView());

            }
            else if (selectedItem.nomeDescricao == "Sair")
            {
                string usuarioLogado = Preferences.Get("usuarioLogado", "");

                await Application.Current.MainPage.DisplayAlert("Logout", "Usuário Desconectado: " + usuarioLogado, "OK");
                Preferences.Remove("usuarioLogado");
                Application.Current.MainPage = new LoginView();
            }
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                App.MenuApp.IsPresented = false;
            }
        }
        catch (Exception ex) { await DisplayAlert("Alterar Senha", ex.Message, "OK"); }

    }

}