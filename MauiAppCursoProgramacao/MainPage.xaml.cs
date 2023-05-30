using CommunityToolkit.Maui.Storage;
using System.Text;

namespace MauiAppCursoProgramacao;

public partial class MainPage : ContentPage
{

	IFileSaver _fileSaver;
    CancellationToken cancellationToken = new CancellationToken();

    public MainPage(IFileSaver fileSaver)
	{
		InitializeComponent();
		_fileSaver = fileSaver;
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
        try
        {

            using var stream = new MemoryStream(Encoding.Default.GetBytes("Texte Salvar arquivo localmente ?"));
            var path = await _fileSaver.SaveAsync("arquivo.txt", stream, cancellationToken); 
            fileServerResult.Text = path.ToString();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao salvar o arquivo na pasta da rede: " + ex.Message);
        }
    }
}

