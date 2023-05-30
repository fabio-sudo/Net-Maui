using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;

namespace MauiAppCursoProgramacao.View;

public partial class LoginView : ContentPage
{
    public LoginModelView objLogin { get; set; }
    public LoginView()
    {
        InitializeComponent();

        objLogin = new LoginModelView();
        objLogin.loginModel = new LoginModel();

        BindingContext = this;
    }

    //Inicializa
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            //Barra de Estatus color
            statusBar.StatusBarColor = Colors.DarkCyan;
            statusBar.StatusBarStyle = CommunityToolkit.Maui.Core.StatusBarStyle.LightContent;
        }

    }


    //Valida Login
    async void btLogar_Clicked(object sender, EventArgs e)
    {

        try
        {
            //------------------Api + nGrok - Rota
            string urlBase = App.Current.Resources["urlBase"].ToString();
            string token = App.Current.Resources["token"].ToString();

            ClientHttp clientHttp = new ClientHttp();

            //-------------Barra de Progresso Carregamento
            barraProgresso.IsRunning = true;
            lbCarregando.IsVisible = true;

            bool loginValido = await clientHttp.ValidaLogin(urlBase, objLogin.loginModel.userLogin, objLogin.loginModel.senhaLogin, token);

            barraProgresso.IsRunning = false;
            lbCarregando.IsVisible = false;

            if (loginValido == true)
            {
                Application.Current.MainPage = new MenuPrincipalView();
                Preferences.Default.Set("logado", true);
                Preferences.Default.Set("usuarioLogado", objLogin.loginModel.userLogin);
            }
            else
            {
                Preferences.Default.Set("logado", false);
                await DisplayAlert("Validação", "Erro Usuário e Senha", "OK");
            }
        }
        catch (Exception ex)
        {

            await DisplayAlert("Erro Configuração", ex.Message, "OK");
        }
    }

    //Muda o focu para o botão logar
    private void txtSenha_Unfocused(object sender, FocusEventArgs e)
    {
        btLogar.Focus();
    }



}