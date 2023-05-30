namespace MauiAppCursoProgramacao.Metodos
{
    public class ValidaCPF
    {
        public bool ValidarCPF(string cpf)
        {
            // Remover caracteres não numéricos do CPF
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verificar se o CPF possui 11 dígitos
            if (cpf.Length != 11)
            {
                return false;
            }

            // Verificar se todos os dígitos do CPF são iguais
            bool todosDigitosIguais = cpf.Distinct().Count() == 1;
            if (todosDigitosIguais)
            {
                return false;
            }

            // Verificar os dígitos verificadores do CPF
            int[] multiplicadoresPrimeiroDigito = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadoresSegundoDigito = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Verificar o primeiro dígito verificador do CPF
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * multiplicadoresPrimeiroDigito[i];
            }

            int resto = soma % 11;
            int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto;
            if (int.Parse(cpf[9].ToString()) != primeiroDigitoVerificador)
            {
                return false;
            }

            // Verificar o segundo dígito verificador do CPF
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * multiplicadoresSegundoDigito[i];
            }

            resto = soma % 11;
            int segundoDigitoVerificador = resto < 2 ? 0 : 11 - resto;
            if (int.Parse(cpf[10].ToString()) != segundoDigitoVerificador)
            {
                return false;
            }

            return true;
        }

    }
}
