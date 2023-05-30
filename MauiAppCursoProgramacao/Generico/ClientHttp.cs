using MauiAppCursoProgramacao.Model;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace MauiAppCursoProgramacao.Generico
{
    public class ClientHttp
    {

        public HttpClient client;

        public ClientHttp()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            client = new HttpClient(handler);
        }

        //Busca Lista Generica
        public static async Task<List<T>> BuscarLista<T>(
            string urlbase, string rotaApi, string token = "")
        {
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlbase);

                if (token != "") cliente.DefaultRequestHeaders.Add("token", token);

                string retornoJson = await cliente.GetStringAsync(rotaApi);
                List<T> lista = JsonConvert.DeserializeObject<List<T>>(retornoJson);

                return lista;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }

        }

        //Busca objeto
        public static async Task<T> Buscar<T>(
        string urlbase, string rotaApi, string token = "")
        {
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlbase);

                if (token != "") cliente.DefaultRequestHeaders.Add("token", token);

                string retornoJson = await cliente.GetStringAsync(rotaApi);

                T obj = JsonConvert.DeserializeObject<T>(retornoJson);

                return obj;

            }
            catch (Exception ex)
            {

                return (T)Activator.CreateInstance(typeof(T));

            }

        }

        public static async Task<int> BuscarId(
      string urlbase, string rotaApi, string token = "")
        {
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlbase);

                if (token != "") cliente.DefaultRequestHeaders.Add("token", token);

                string respostaJason = await cliente.GetStringAsync(rotaApi);
                return int.Parse(respostaJason);
            }
            catch (Exception ex)
            {
                return 0;
            }

        }


        //Método genérico excluir objeto via HTTP
        public static async Task<int> Excluir(string urlbase, string rotaApi, string token = "")
        {
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlbase);


                if (token != "") cliente.DefaultRequestHeaders.Add("token", token);

                var response = await cliente.DeleteAsync(rotaApi);

                if (response.IsSuccessStatusCode)
                {

                    string escreva = await response.Content.ReadAsStringAsync();
                    return int.Parse(escreva);

                }
                else
                {
                    return 0;

                }

            }
            catch (Exception ex) { return 0; };

        }


        //Método adiciona ou altera objeto via Http
        public static async Task<int> Adicionar<T>(string urlbase, string rotaApi, T obj, string token = "")
        {
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlbase);

                if (token != "") cliente.DefaultRequestHeaders.Add("token", token);

                var retorno = await cliente.PostAsJsonAsync<T>(rotaApi, obj);

                if (retorno.IsSuccessStatusCode)
                {
                    string escreva = await retorno.Content.ReadAsStringAsync();
                    return int.Parse(escreva);

                }
                else
                {

                    return 0;

                }

            }
            catch (Exception ex) { return 0; }
        }

        //Método adiciona ou altera objeto via Http
        public static async Task<int> AdicionarLista<T>(string urlbase, string rotaApi, List<T> obj, string token = "")
        {
            try
            {
                string escreva = "0";

                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlbase);

                if (token != "") cliente.DefaultRequestHeaders.Add("token", token);

                foreach (T item in obj)
                {
                    var retorno = await cliente.PostAsJsonAsync<T>(rotaApi, item);

                    if (retorno.IsSuccessStatusCode)
                    {
                        escreva = await retorno.Content.ReadAsStringAsync();

                    }
                    else
                    {
                        return 0;
                    }

                }

                return 1;


            }
            catch (Exception ex) { return 0; }


        }


        // Método Retorna Login
        public async Task<bool> ValidaLogin(string baseUrl, string user, string senha, string token)
        {
            try
            {
                if (token != "") client.DefaultRequestHeaders.Add("token", token);

                var response = await client.GetAsync(baseUrl + "/api/Login?nomeUsuario=" + user + "&senhaUsuario=" + senha);

                if (response.IsSuccessStatusCode)
                {
                    string usuarioLogado = user;

                    string resposta = await response.Content.ReadAsStringAsync();
                    bool loginValido = bool.Parse(resposta);

                    return loginValido;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                return false;
            }
        }


        //Método altera objeto via Http
        public static async Task<Boolean> Alterar<T>(string urlbase, string rotaApi, T obj, string token = "")
        {
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlbase);

                if (token != "")
                {
                    cliente.DefaultRequestHeaders.Add("token", token);
                }

                var retorno = await cliente.PutAsJsonAsync<T>(rotaApi, obj);

                if (retorno.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }











    }
}
