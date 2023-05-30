using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;

namespace MauiAppCursoProgramacao.View;

public partial class UsuarioView : ContentPage
{
    private string urlBase;
    private string url;
    private string token;

    public LoginModelView objLogin { get; set; }


    public UsuarioView()
    {
        InitializeComponent();


        objLogin = new LoginModelView();
        objLogin.loginModel = new LoginModel();

        BindingContext = this;

        urlBase = App.Current.Resources["urlBase"].ToString();
        token = App.Current.Resources["token"].ToString();
        url = App.Current.Resources["urlLogin"].ToString();

        BuscarDadosUsuario();
    }

    public async void BuscarDadosUsuario()
    {

        try
        {
            string nomeUsuario = Preferences.Get("usuarioLogado", "");

            objLogin.loginModel =
            await ClientHttp.Buscar<LoginModel>(urlBase, url + "/BuscarLoginPorNome?nomeUsuario=" + nomeUsuario, token);


        }
        catch (Exception ex)
        {

            await DisplayAlert("Erro Buscar Login", ex.Message, "OK");

        }
    }

    private async void btAlterarUsuario_Clicked(object sender, EventArgs e)
    {

        UsuarioSenhaView usuarioSenhaView = new UsuarioSenhaView(objLogin.loginModel.idLogin, objLogin.loginModel.senhaLogin); // Instancie a página do formulário
        await Navigation.PushAsync(usuarioSenhaView);


    }
}