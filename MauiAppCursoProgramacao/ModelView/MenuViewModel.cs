using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;


namespace MauiAppCursoProgramacao.ModelView
{
    public class MenuViewModel:BaseBinding
    {

        private List<MenuModel> _menuLista;
        public List<MenuModel> Lista
        {
            get
            {
                return _menuLista;
            }
            set
            {
                SetValue(ref _menuLista, value);
            }
        }

    }
}
