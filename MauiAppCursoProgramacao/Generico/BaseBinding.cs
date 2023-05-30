using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MauiAppCursoProgramacao.Generico
{
    public class BaseBinding : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string nome = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));

        }
        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);

        }

    }
}

