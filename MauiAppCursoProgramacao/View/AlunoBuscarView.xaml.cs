using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;
using System.Text;

namespace MauiAppCursoProgramacao.View;

public partial class AlunoBuscarView : ContentPage
{
    //Atualizações em segundo plano
    private System.Timers.Timer timer;

    public AlunoModelView objAluno { get; set; }
    int quantidadeLimitador = 10; // Define a quantidade máxima de itens desejada
    int quantidadeMaximaItem = 0;

    List<AlunoModel> lista = new List<AlunoModel>();

    string urlBase = App.Current.Resources["urlBase"].ToString();
    string token = App.Current.Resources["token"].ToString();
    string rotaApi = App.Current.Resources["urlAluno"].ToString();

    public AlunoBuscarView()
	{
        objAluno = new AlunoModelView();
        objAluno.ListaAluno = new List<AlunoModel>();
        objAluno.IsRefreshing = new bool();

        objAluno.RefreshCommand = new Command(ExecuteRefreshCommand);

        InitializeComponent();

        BindingContext = this;

        //busca alunos cadastrados no banco via API
        MetodoBuscarAlunos("");

        // Registre o evento Appearing para receber retonro do formulário
        this.Appearing += MinhaContentPage_Appearing;

        //Atualiza a collection view com o banco de dados periodicamente
        IniciarAtualizacaoPeriodica();
    }
    
    //Command Preeche mais itens a lista
    private async void ExecuteRefreshCommand()
    {
        try
        {

            if (quantidadeMaximaItem > quantidadeLimitador)
            {
                objAluno.IsRefreshing = true;

                quantidadeLimitador = quantidadeLimitador + 10;
                objAluno.ListaAluno = lista;

                objAluno.ListaAluno = objAluno.ListaAluno.Take(quantidadeLimitador).ToList();

                objAluno.IsRefreshing = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    //Define cor padrão para barra de navegação
    private void MinhaContentPage_Appearing(object sender, EventArgs e)
    {
        App.Navegacao.BarBackgroundColor = Color.FromRgba("#2196F3");
    }

    //Busca Alunos
    public async void MetodoBuscarAlunos(string nomeAluno)
    {
        try
        {
            //Barra de Progresso 
            objAluno.IsRefreshing = true;

            if (nomeAluno == "")
            {
                //Consumindo API   
                objAluno.ListaAluno =
                await ClientHttp.BuscarLista<AlunoModel>(urlBase, rotaApi, token);
            }
            else
            {
                //Consumindo API   
                objAluno.ListaAluno =
                await ClientHttp.BuscarLista<AlunoModel>(urlBase, rotaApi + "/BuscarPorNome" + nomeAluno, token);
            }
            objAluno.IsRefreshing = false;

            //Coloca Img Sem imagem aonde nao foi cadastrado imagem 
            objAluno.ListaAluno.Where(p => p.imgStr == "").ToList().ForEach(p => p.imgStr = "/img/noimagen.jpg");

            //Convert  Data de Nascimento para id
            objAluno.ListaAluno = objAluno.ListaAluno.Select(aluno =>
            {
                if (aluno.DataNascimentoAluno != null)
                {
                    int idade = DateTime.Now.Year - aluno.DataNascimentoAluno.Value.Year;
                    // Verificar se já completou aniversário neste ano
                    if (aluno.DataNascimentoAluno.Value > DateTime.Now.AddYears(-idade))
                    { idade--; }
                    aluno.Idade = idade.ToString();
                }
                return aluno;
            }).ToList();

            lista = objAluno.ListaAluno;


            quantidadeMaximaItem = objAluno.ListaAluno.Count();
            objAluno.ListaAluno = objAluno.ListaAluno.Take(quantidadeLimitador).ToList();
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }

    //Carregar itens Delimitados
   private void scvScroll_Scrolled(object sender, ScrolledEventArgs e)
     {
        double scrollY = scvScroll.ContentSize.Height - scvScroll.Height;

         if (e.ScrollY >= scrollY - 40) // Define um valor de margem inferior para acionar a páginação (40 no exemplo)
         {

            objAluno.RefreshCommand.Execute(null);

        }
    }

    //----------------Item selecionado Aluno
    //Frame
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

        var frame = (Frame)sender;
        var item = frame.BindingContext;

        lstListaAlunos.SelectedItem = item; // Simula a seleção do item no CollectionView

    }
    //Grid
    private async void lstListaAlunos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Verifique se algum item foi selecionado
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            // Obtenha o aluno selecionado
            AlunoModel alunoSelecionado = e.CurrentSelection[0] as AlunoModel;

            string acaoSelecionada = await Application.Current.MainPage.DisplayActionSheet("Opções", "Cancelar", null, "Alterar", "Excluir");

            // Verifique a ação selecionada
            if (acaoSelecionada == "Excluir")
            {
                AlunoView alunoAdd = new AlunoView(alunoSelecionado, "Excluir"); // Instancie a página do formulário
                await Navigation.PushAsync(alunoAdd);

                metodoAtulizaFormulario(alunoAdd);
            }
            else if (acaoSelecionada == "Alterar")
            {
                AlunoView alunoAdd = new AlunoView(alunoSelecionado, "Alterar"); // Instancie a página do formulário
                await Navigation.PushAsync(alunoAdd);

                metodoAtulizaFormulario(alunoAdd);
            }
        }

    // Limpe a seleção para evitar que o evento seja acionado novamente acidentalmente
    ((CollectionView)sender).SelectedItem = null;

    }
   
    //Filtrar Alunos por nome carregados
    private async void txtBuscarAluno_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string searchText = e.NewTextValue;

            if (!string.IsNullOrEmpty(searchText))
            {
                string normalizedSearchText = searchText.Normalize(NormalizationForm.FormD);
                var filteredAlunos = lista.Where(p =>
                    p.NomeAluno != null &&
                    p.NomeAluno.Normalize(NormalizationForm.FormD)
                        .IndexOf(normalizedSearchText, StringComparison.OrdinalIgnoreCase) >= 0
                );

                objAluno.ListaAluno = filteredAlunos.ToList();           
            }
            else {

                objAluno.ListaAluno = lista;
            }

        }catch (Exception ex) { await DisplayAlert("Erro",ex.Message,"OK"); }

    }

    //Buscar alunos por nome
    private void txtBuscarAluno_Completed(object sender, EventArgs e)
    {
        string nomeABuscar = "";

        if (!string.IsNullOrEmpty(txtBuscarAluno.Text)) {
            
            nomeABuscar = txtBuscarAluno.Text; 
        
        }

        MetodoBuscarAlunos(nomeABuscar);
        quantidadeLimitador = 10;//Volta a quantidade do limitador para 9 registros carregados
        
    }

    //Chama formulário de cadastro
    private async void btnAdicionar_Clicked(object sender, EventArgs e)
    {
        AlunoView alunoAdd = new AlunoView(null,"Cadastro"); // Instancie a página do formulário
        await Navigation.PushAsync(alunoAdd);

        metodoAtulizaFormulario(alunoAdd);
    }

    //Metodo Atualiza Formulario resposta do formulárioa anterior
    private void metodoAtulizaFormulario(AlunoView alunoAdd)
    {
        alunoAdd.OnAlunoAddCompleted += (retorno) =>
        {
            //Espera o retorno para poder realizar atualização
            if (retorno == "ok")
            {
                foreach (AlunoModel alunoAddLista in alunoAdd.lista) {

                    lista.Add(alunoAddLista);
                }
            }
            else if (retorno == "AlterarOK")
            {
                var alunoAlterarLista = this.lista.FirstOrDefault(aluno => aluno.IdAluno == alunoAdd.objAluno.Aluno.IdAluno);
                if (alunoAlterarLista != null)
                {
                    alunoAlterarLista = alunoAdd.objAluno.Aluno;
                }
            }
            else if (retorno == "ExcluirOK")
            {
                // Encontra o aluno com o idAluno correspondente e remove-o da lista
                var alunoRemover = this.objAluno.ListaAluno.FirstOrDefault(aluno => aluno.IdAluno == alunoAdd.objAluno.Aluno.IdAluno);
                if (alunoRemover != null)
                {
                    this.lista.Remove(alunoRemover);
                }
            }
        };

        //Atualiza valores da Lista
        objAluno.ListaAluno = new List<AlunoModel>();
        objAluno.ListaAluno = lista;

    }

    //------------------------Atualiza listas de tempo em tempo
    private async Task MetodoAtualizarRegistrosLista()
    {
        // Consumindo API   
        objAluno.ListaAluno = await ClientHttp.BuscarLista<AlunoModel>(urlBase, rotaApi, token);

        // Coloca Img Sem imagem onde não foi cadastrada uma imagem
        objAluno.ListaAluno.Where(p => p.imgStr == "").ToList().ForEach(p => p.imgStr = "/img/noimagen.jpg");

        // Converter Data de Nascimento para idade
        objAluno.ListaAluno = objAluno.ListaAluno.Select(aluno =>
        {
            if (aluno.DataNascimentoAluno != null)
            {
                int idade = DateTime.Now.Year - aluno.DataNascimentoAluno.Value.Year;
                // Verificar se já completou aniversário neste ano
                if (aluno.DataNascimentoAluno.Value > DateTime.Now.AddYears(-idade))
                {
                    idade--;
                }
                aluno.Idade = idade.ToString();
            }
            return aluno;
        }).ToList();

        lista = objAluno.ListaAluno;
        objAluno.ListaAluno = objAluno.ListaAluno.Take(quantidadeLimitador).ToList();
    }

    public void IniciarAtualizacaoPeriodica()
    {
        timer = new System.Timers.Timer();
        timer.Interval = 180000; // Intervalo de 60 segundos (1 minuto)
        timer.Elapsed += async (sender, e) =>
        {
        await MetodoAtualizarRegistrosLista();
        };
        timer.Start();
    }

    public void PararAtualizacaoPeriodica()
    {
        timer?.Stop();
        timer?.Dispose();
        timer = null;
    }


}