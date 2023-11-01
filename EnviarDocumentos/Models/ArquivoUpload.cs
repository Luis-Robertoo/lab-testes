namespace EnviarDocumentos.Models
{
    public class ArquivoUpload
    {
        public Guid Id { set; get; }
        public string Nome { set; get; }
        public string Tipo { get; set; }
        public string CaminhoArquivo { get; set; }
        public string UrlArquivo { get; set; }
    }
}
