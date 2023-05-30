using CommunityToolkit.Maui.Alerts;
using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;
using CommunityToolkit.Maui.Core;


namespace MauiAppCursoProgramacao.View;

public partial class AlunoView : ContentPage
{
    //Instanciado constantes para utilização API
    string urlBase = App.Current.Resources["urlBase"].ToString();
    string token = App.Current.Resources["token"].ToString();
    string rotaApi = App.Current.Resources["urlAluno"].ToString();

    //Lista de alunos cadastrados
    public List<AlunoModel> lista = new List<AlunoModel>();

    //Classe de validação
    Metodos.ValidaCPF metodoValidaCPF = new Metodos.ValidaCPF();

    public AlunoModelView objAluno { get; set; }//Obejto MVVM
    AlunoModel alunoSelecionado = new AlunoModel();//Objeto vem do formulário anterior

    public event Action<string> OnAlunoAddCompleted;//Evento para Dialog result
    string acaoPage;//Tipo de Pagina para construtor Cadastro/Exclusão/Alteração


    public AlunoView(AlunoModel _alunoSelecionado, string _acaoPage)
    {
        InitializeComponent();

        objAluno = new AlunoModelView();
        objAluno.Aluno = new AlunoModel();

        alunoSelecionado = _alunoSelecionado;
        acaoPage = _acaoPage;

        recuperaImagem();
        metodoConstrutor();

        BindingContext = this;    
    }

        //Metodo Construtor
        private void metodoConstrutor() {

        if (alunoSelecionado != null) {

            if (acaoPage == "Alterar")
            {

                this.Title = "Alterar Aluno";
                btnCadastrar.Text = "Alterar";
                btnCadastrar.BackgroundColor = Color.FromArgb("#2196F3");
                objAluno.Aluno = alunoSelecionado;
                objAluno.imagen = alunoSelecionado.imgStr;
                btnCamera.BackgroundColor = Color.FromArgb("#2196F3");
                btSelecionarFoto.BackgroundColor = Color.FromArgb("#2196F3");
                frameFoto.BackgroundColor = Color.FromArgb("#2196F3");
                App.Navegacao.BarBackgroundColor = Color.FromArgb("#2196F3");

            }

            else if (acaoPage == "Excluir")
            {
                this.Title = "Excluir Aluno";
                btnCadastrar.Text = "Excluir";
                btnCadastrar.BackgroundColor = Colors.Red;
                objAluno.Aluno = alunoSelecionado;
                objAluno.imagen = alunoSelecionado.imgStr;
                btnCamera.BackgroundColor = Colors.Red;
                btSelecionarFoto.BackgroundColor = Colors.Red;
                frameFoto.BackgroundColor = Colors.Red;
                App.Navegacao.BarBackgroundColor = Colors.Red;
            }
        }
        
        
        else {  App.Navegacao.BarBackgroundColor = Colors.DarkCyan; }
    }

        //Metodo limpar cadastro
        private void limpaCampos() {
        //Limpa o objeto aluno
        objAluno.Aluno = new AlunoModel();
        objAluno.imagen = "photo_icone.png";
        txtNome.Focus();

    }

        //Exibe imagem na memoria do computador
        public void recuperaImagem()
        {

            objAluno.imagen = objAluno.Aluno.Img == null ? "photo_icone.jpg" :
            ImageSource.FromStream(() => new MemoryStream(objAluno.Aluno.Img));

            objAluno.Aluno.DataNascimentoAluno = DateTime.Today;
        }

        //Mensagem de erro TOAST
        private async void toastMessage(string MessageToast)
        {

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(MessageToast, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        //Valida Campos
        private bool ValidaCampos()
        {
            // Verificar se o campo "Nome" está preenchido
            if (string.IsNullOrEmpty(txtNome.Text))
            {

                toastMessage("Informe o nome do aluno!");
                return false;
            }

            // Verificar se o campo "Sobrenome" está preenchido
            if (string.IsNullOrEmpty(txtSobrenome.Text))
            {
                toastMessage("Informe o sobrenome do aluno!");
                return false;
            }

            // Verificar se o campo "CPF" está preenchido
            if (string.IsNullOrEmpty(txtCpf.Text) || !metodoValidaCPF.ValidarCPF(txtCpf.Text))
            {
                toastMessage("CPF inválido!");
                return false;
            }

            // Verificar se o campo "Celular" está preenchido
            if (string.IsNullOrEmpty(txtCelular.Text))
            {
                toastMessage("Informe o celular do aluno!");
                return false;
            }

            // Verificar se o campo "Endereço" está preenchido
            if (string.IsNullOrEmpty(txtEndereco.Text))
            {
                toastMessage("Informe o endereço do aluno!");
                return false;
            }

            // Verificar se o campo "Data de Nascimento" foi selecionado corretamente
            if (dtpDataNascimento.Date == DateTime.Now)
            {
                toastMessage("Informe a data de nascimento do aluno!");
                return false;
            }

            // Verificar se o campo "Status" foi selecionado corretamente
            if (string.IsNullOrEmpty(picEstatus.SelectedItem?.ToString()))
            {
                toastMessage("Informe o estatus do aluno!");
                return false;
            }

            return true;
        }

        //Storage
        private async void btSelecionarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Armazenamento", "Erro de permissão storage imagens!", "OK");
                    return;
                }
                if (MediaPicker.Default.IsCaptureSupported)
                {

                    FileResult foto = await MediaPicker.Default.PickPhotoAsync();
                    if (foto != null)
                    {
                        byte[] buffer = File.ReadAllBytes(foto.FullPath);
                        Stream stream = new MemoryStream(buffer);
                        ImageSource oImageSource = ImageSource.FromStream(() => stream);

                        objAluno.imagen = oImageSource;
                        objAluno.Aluno.Img = buffer;
                        objAluno.Aluno.NomeImg = foto.FileName;
                    }

                }

            }
            catch (Exception ex) { await DisplayAlert("Erro", ex.Message, "OK"); }
        }

        //Câmera
        private async void btnCamera_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Armazenamento", "Erro de permissão storage imagens!", "OK");
                    return;
                }

                if (MediaPicker.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.CapturePhotoAsync();

                    if (photo != null)
                    {
                        // Obtenha o caminho completo do arquivo da foto capturada
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        // Salve a foto no armazenamento local
                        using Stream sourceStream = await photo.OpenReadAsync();
                        using FileStream localFileStream = File.OpenWrite(localFilePath);
                        await sourceStream.CopyToAsync(localFileStream);

                        // Defina a propriedade de imagem com a nova imagem capturada
                        ImageSource oImageSource = ImageSource.FromFile(localFilePath);
                        byte[] buffer = File.ReadAllBytes(photo.FullPath);

                        objAluno.Aluno.Img = buffer;
                        objAluno.imagen = oImageSource;
                        objAluno.Aluno.NomeImg = photo.FileName;

                    }
                }
            }
            catch (Exception ex) { await DisplayAlert("Erro", ex.Message, "OK"); }
        }

        //Preenchimetno Máscara de CPF
        private void txtCpf_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;
            string text = e.NewTextValue;

            if (text.Length > 13) { txtCelular.Focus(); }

            // Remover todos os caracteres não numéricos
            string digitsOnly = new string(text.Where(char.IsDigit).ToArray());

            // Aplicar a máscara do CPF (###.###.###-##)

            entry.CursorPosition = text.Length;

            if (digitsOnly.Length > 3 && digitsOnly.Length < 7)
            {
                digitsOnly = digitsOnly.Insert(3, ".");
                entry.CursorPosition = text.Length + 1;
            }
            else if (digitsOnly.Length >= 7 && digitsOnly.Length < 11)
            {
                digitsOnly = digitsOnly.Insert(3, ".");
                digitsOnly = digitsOnly.Insert(7, ".");
                entry.CursorPosition = text.Length + 1;
            }
            else if (digitsOnly.Length >= 11)
            {
                digitsOnly = digitsOnly.Insert(3, ".");
                digitsOnly = digitsOnly.Insert(7, ".");
                digitsOnly = digitsOnly.Insert(11, "-");
                entry.CursorPosition = text.Length + 1;
            }

            // Atualizar o texto no Entry
            entry.Text = digitsOnly;

        }
        
        //Preenchimento mascara de Celular
        private void txtCelular_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = (Entry)sender;
            string text = e.NewTextValue;


            if (text.Length > 14) { txtEndereco.Focus(); }

            // Remover todos os caracteres não numéricos
            string digitsOnly = new string(text.Where(char.IsDigit).ToArray());

            // Aplicar a formatação de telefone (###) ###-####

            entry.CursorPosition = text.Length;

            if (digitsOnly.Length >= 4)
            {
                digitsOnly = digitsOnly.Insert(0, "(");
                entry.CursorPosition = text.Length + 1;
            }
            if (digitsOnly.Length >= 7)
            {
                digitsOnly = digitsOnly.Insert(3, ") ");
                entry.CursorPosition = text.Length + 1;
            }
            if (digitsOnly.Length >= 11)
            {
                digitsOnly = digitsOnly.Insert(9, "-");
                entry.CursorPosition = text.Length + 1;
            }

            // Atualizar o texto no Entry
            entry.Text = digitsOnly;
        }

       //Realiza Cadastro ou  Alteração ou Exclusão
       private async void btnCadastrar_Clicked(object sender, EventArgs e)
        {
        try
        {
            if (btnCadastrar.Text == "Cadastrar")
            {
                if (ValidaCampos() == true)
                {
                    //Consumindo API
                    int result = await ClientHttp.Adicionar(urlBase, rotaApi + "/AdicionarAluno", objAluno.Aluno, token);

                    if (result == 1)
                    {
                        //Busca ultimo id Cadastrado -> no caso id que acabou de ser cadastrado
                        objAluno.Aluno.IdAluno = await ClientHttp.BuscarId(urlBase, rotaApi + "/BuscarIDAluno", token);

                        //cria a imagem
                        objAluno.Aluno.imgStr = objAluno.Aluno.Img == null ? ""
                                       : "data:image/" + System.IO.Path.GetExtension(objAluno.Aluno.NomeImg).Substring(1) +
                                       ";base64," + Convert.ToBase64String(objAluno.Aluno.Img);

                        //caso não haja imagem define a imagem padrão
                        if (objAluno.Aluno.imgStr == "") {

                            objAluno.Aluno.imgStr = "/img/noimagen.jpg";
                        }
                        

                        //Arruama a data
                        int idade = DateTime.Now.Year - objAluno.Aluno.DataNascimentoAluno.Value.Year;
                        if (objAluno.Aluno.DataNascimentoAluno.Value > DateTime.Now.AddYears(-idade))
                        { idade--; }
                        objAluno.Aluno.Idade = idade.ToString();
                   
                        //Retorno para formulario anterior Para o mesmo atualizar
                        lista.Add(objAluno.Aluno);//Aluno cadastrado
                        ConcluirAcao("ok");


                        // Success
                        bool resposta = await DisplayAlert("Sucesso", "Aluno cadastrado com sucesso!", "Continuar", "Sair");

                        if (resposta == true)
                        {
                            //Deseja Continuar cadastrando
                            limpaCampos();
                        }
                        else
                        {

                            await Navigation.PopAsync();

                        }
                    }
                    else
                    {
                        // Failure
                        await DisplayAlert("Erro", "Falha ao cadastrar aluno.", "OK");
                    }

                }
            }
            
            
            else if (btnCadastrar.Text == "Alterar")
            {
                if (ValidaCampos() == true)
                {
                    // Success
                    bool resposta = await DisplayAlert("Aterar?", "Deseja realmenter realizar a alteração?", "Sim", "Não");

                    if (resposta == true)
                    {
                        //Consumindo API
                        bool result = await ClientHttp.Alterar(urlBase, rotaApi + "/AlterarAluno", objAluno.Aluno, token);

                        if (result == true)
                        {
                            // Success
                            await DisplayAlert("Sucesso", "Aluno alterado com sucesso!", "Continuar");

                            //Retorno para formulario anterior Para o mesmo atualizar
                            ConcluirAcao("AlterarOK");
                            await Navigation.PopAsync();

                        }
                        else
                        {
                            await DisplayAlert("Erro", "Erro ao alterar Aluno!", "OK");

                        }
                    }
                }
            }


            else if (btnCadastrar.Text == "Excluir")
            {

                // Success
                bool resposta = await DisplayAlert("Excluir?", "Deseja realmenter excluir o  Aluno?", "Sim", "Não");

                if (resposta == true)
                {
                    //Consumindo API
                    int result = await ClientHttp.Excluir(urlBase, rotaApi + "/ExcluirAluno"+objAluno.Aluno.IdAluno, token);

                    if (result == 1)
                    {
                        // Success
                        await DisplayAlert("Sucesso", "Aluno excluido com sucesso!", "Continuar");

                        //Retorno para formulario anterior Para o mesmo atualizar
                        ConcluirAcao("ExcluirOK");
                        await Navigation.PopAsync();

                    }
                    else
                    {
                        await DisplayAlert("Erro", "Erro ao excluir Aluno!", "OK");

                    }
                }

            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }

    }

      // Método ou evento que aciona a conclusão da ação na página AlunoView
      private void ConcluirAcao(string retorno)
      {
        OnAlunoAddCompleted?.Invoke(retorno);
      }

}