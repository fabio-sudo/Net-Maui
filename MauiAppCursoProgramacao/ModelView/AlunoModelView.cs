using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using System.ComponentModel;

namespace MauiAppCursoProgramacao.ModelView
{
    public class AlunoModelView : BaseBinding
    {
        private AlunoModel _alunoModel;
        private List<AlunoModel> _alunoLista;
        private ImageSource _imagen;
        public ImageSource imagen
        {
            get
            {
                return _imagen;
            }
            set
            {
                SetValue(ref _imagen, value);
            }
        }

        public List<AlunoModel> ListaAluno
        {
            get { return _alunoLista; }
            set => SetValue(ref _alunoLista, value);
        }

        public AlunoModel Aluno
        {
            get { return _alunoModel; }
            set => SetValue(ref _alunoModel, value);
        }


        //----------------------------Atualizando Grid
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set => SetValue(ref isRefreshing, value);
        }
        

        //--------------------------Command
        private Command _RefreshCommand;
        public Command RefreshCommand
        {

            get { return _RefreshCommand; }
            set => SetValue(ref _RefreshCommand, value);
        }


    }

    }
