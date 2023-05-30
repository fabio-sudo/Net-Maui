namespace MauiAppCursoProgramacao.Model
{
    //Objeto Aluno
    public class AlunoModel
    {
        public int IdAluno { get; set; }
        public string NomeAluno { get; set; } = "";
        public string SobrenomeAluno { get; set; } = "";
        public string? CpfAluno { get; set; } = "";
        public string? CelularAluno { get; set; } = "";
        public string? EnderecoAluno { get; set; } = "";
        public string? DataNascimentoAlunoStr { get; set; } = null!;
        public DateTime? DataNascimentoAluno { get; set; } = null!;
        public string? Idade { get; set; } = "";
        public string? StatusAluno { get; set; } = "Espera";
        public string imgStr { get; set; } = "";
        public string? NomeImg { get; set; } = "";
        public byte[]? Img { get; set; } = null!;
    }

}
