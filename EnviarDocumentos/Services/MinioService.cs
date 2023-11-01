using System.Net;
using EnviarDocumentos.Models;
using Minio;
namespace EnviarDocumentos.Services
{
    public class MinioService
    {
        private readonly MinioClient _minio;
        private readonly string _minIoBucket;
        private readonly string _nomeDiretorio;

  //          "NomeDiretorio": "cloud",
  //  "TempoLimiteConexaoEmSegundos": 120
  //},
  //"Minio": {
  //  "BucketName": "digix-horus-novo",
  //  "Endpoint": "minio-zyna.onrender.com",
  //  "AccessKey": "GMSXRWvidI7tiGxh76jT",
  //  "SecretKey": "gQDk7lGLyAdHx1N44WiiCxKEp7Nx6NbMovOAKxLp"


        public MinioService()
        {
            this._nomeDiretorio = "cloud";
            this._minIoBucket = "digix-horus-novo";
            try
            {
                this._minio = new MinioClient()
                    .WithEndpoint("minio-zyna.onrender.com")
                    .WithSSL(false)
                    .WithCredentials(
                        "GMSXRWvidI7tiGxh76jT",
                        "gQDk7lGLyAdHx1N44WiiCxKEp7Nx6NbMovOAKxLp"
                    )
                    .Build();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao criar o cliente do MinIO: {EMessage}", e.Message);
                throw;
            }
        }

        public async Task<ArquivoUpload?> Armazenar(string nomeArquivo, string tipoArquivo, Stream arquivoStream)
        {
            var dataAtual = DateTime.Now;
            var diretorio =
                $"{dataAtual.Year}/{dataAtual.Month}/{dataAtual.Day}/{dataAtual.Hour}/{dataAtual.Minute}";

            ArquivoUpload arquivoUpload = null;
            var anexoId = Guid.NewGuid();
            var caminhoArquivo = $"{diretorio}/{anexoId}-{nomeArquivo}";

            var args = new PutObjectArgs()
                .WithBucket(this._minIoBucket)
                .WithObject($"{this._nomeDiretorio}/{caminhoArquivo}")
                .WithStreamData(arquivoStream)
                .WithObjectSize(arquivoStream.Length)
                .WithMatchETag(anexoId.ToString())
                .WithContentType(tipoArquivo);
            

            arquivoUpload = new ArquivoUpload
            { Id = anexoId, Nome = nomeArquivo, Tipo = tipoArquivo, CaminhoArquivo = caminhoArquivo };
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(this._minIoBucket)
                .WithObject(caminhoArquivo)
                .WithExpiry(7);
            var urlPreSigned = await this._minio.PresignedGetObjectAsync(presignedGetObjectArgs);
            arquivoUpload.UrlArquivo = urlPreSigned ?? "";

            return arquivoUpload;
        }
    }
}
