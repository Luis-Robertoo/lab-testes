using EnviarDocumentos.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace EnviarDocumentos.Controllers;

[ApiController]
[Route("api/documentos")]
public class DocumentoController : ControllerBase
{
        

    private readonly ILogger<DocumentoController> _logger;

    public DocumentoController(ILogger<DocumentoController> logger)
    {
        _logger = logger;
    }

  
    [HttpPost("/upload")]
    public async Task <ActionResult> Upload(IFormFile arquivo)
    {
        var servicoMinio = new MinioService();

        var dadosStream = new MemoryStream();

        arquivo.CopyTo(dadosStream);

        var dados = servicoMinio.Armazenar(arquivo.FileName, arquivo.ContentType, dadosStream);

        return Ok(dados);

    }
}
