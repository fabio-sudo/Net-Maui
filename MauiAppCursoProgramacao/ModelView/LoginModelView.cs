using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;

namespace MauiAppCursoProgramacao.ModelView
{
    public class LoginModelView: BaseBinding
    {
        private LoginModel _loginModel;

        public LoginModel loginModel
        {
            get { return _loginModel; }
            set => SetValue(ref _loginModel, value);
        }

    }
}
