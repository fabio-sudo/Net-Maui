using MauiAppCursoProgramacao.View;

namespace MauiAppCursoProgramacao;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

         MainPage = new MenuPrincipalView();
        //MainPage = new AppShell();


        /* if (Preferences.Get("logado", false) == true)
          {
              MainPage = new MenuPrincipalView();
          }

            MainPage = new LoginView();
      }*/
    }
    
    public static MenuPrincipalView MenuApp { get; internal set; }
    public static NavigationPage Navegacao { get; internal set; }

}
