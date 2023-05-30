using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;


namespace MauiAppCursoProgramacao.View;

public partial class UsuarioSenhaView : ContentPage
{
    private string urlBase;
    private string url;
    private string token;

    public LoginModelView objLogin { get; set; }

    readonly Metodos.AvisoInterface IntefaceAviso = new();

    string senhaCorrente;
    int idLogado;

    public UsuarioSenhaView(int idUsuarioLogado, string senhaUsuarioLogado)
    {
        InitializeComponent();

        idLogado = idUsuarioLogado;
        senhaCorrente = senhaUsuarioLogado;

        objLogin = new LoginModelView();
        objLogin.loginModel = new LoginModel();

        urlBase = App.Current.Resources["urlBase"].ToString();
        token = App.Current.Resources["token"].ToString();
        url = App.Current.Resources["urlLogin"].ToString();

        BindingContext = this;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            string user = Preferences.Get("usuarioLogado", "");

            if (senhaCorrente != objLogin.loginModel.senhaLogin)
            {
                await DisplayAlert("Senha Usuário", "Senha do usuário: " + user + " está incorreta!", "OK");
            }
            else if (objLogin.loginModel.senhaNova != objLogin.loginModel.senhaRepetidaNova || string.IsNullOrEmpty(objLogin.loginModel.senhaRepetidaNova))
            {

                await IntefaceAviso.ShakeInterface(formulario);
                await DisplayAlert("Nova Senha", "Novas senhas não coicidem!", "OK");

            }
            else
            {
                //Completando dados do objeto que não estão na vinculação MVVM
                objLogin.loginModel.userLogin = user;
                objLogin.loginModel.idLogin = idLogado;

                bool confirmacaoSenha = await ClientHttp.Alterar<LoginModel>(urlBase, url + "/AlterarSenhaLogin",
                objLogin.loginModel, token);

                if (confirmacaoSenha == true)
                {
                    await DisplayAlert("Senha Usuário", "Senha do usuário alterada com sucesso!", "OK");
                    Preferences.Remove("usuarioLogado");
                    Application.Current.MainPage = new LoginView();

                }
                else
                {
                    await DisplayAlert("Erro Senha Usuário", "Erro comunicação com a base de dados!", "OK");

                }
            }


        }
        catch (Exception ex) { await DisplayAlert("Alterar Senha", ex.Message, "OK"); }

    }

}