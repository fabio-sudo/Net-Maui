namespace MauiAppCursoProgramacao.Metodos
{
    public class ValidaCEP
    {
        private async Task<string> ConsultarCEP(string cep)
        {
            string url = $"https://viacep.com.br/ws/{cep}/json/";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return json;
                }
                else
                {
                    throw new Exception("Não foi possível consultar o CEP.");
                }
            }
        }

    }
}
