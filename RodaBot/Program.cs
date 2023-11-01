using ClientSoundCloud;
using System.Diagnostics;

var httpClient = new HttpClient { };
var obtemArquivos = new ObtemArquivos();

var entrada = string.Empty;
Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
Console.WriteLine("AS MUSICAS BAIXADAS SERÃO SALVAS NUMA PASTA DENTRO DAS SUAS MUSICAS");
while (entrada != "exit")
{
    Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
    Console.WriteLine("COLE O LINK DA MUSICA DO SOUNDCLOUD: \n OU ESCREVA \"EXIT\" PARA SAIR");
    entrada = Console.ReadLine();
    if (entrada.ToLower() == "exit" || entrada.Length < 10) break;
    await BaixarMusica(httpClient, obtemArquivos, entrada);
}

static async Task BaixarMusica(HttpClient httpClient, ObtemArquivos obtemArquivos, string? linkMusica)
{
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    var paginaPrincipal = await obtemArquivos.ObtemPaginaPrincipal(httpClient, linkMusica);

    if (paginaPrincipal.IndexOf("\"title\":\"") == -1)
    {
        Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
        Console.WriteLine($"                     MÚSICA NÃO ENCONTRADA                                  ");
        Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
        return;
    }

    if (paginaPrincipal.IndexOf("stream/progressive") == -1)
    {
        Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
        Console.WriteLine($"               TIPO DE ARQUIVO NÃO SUPORTADO                                ");
        Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
        return;
    }

    var titulo = obtemArquivos.ObtemTitulo(paginaPrincipal);
    Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
    Console.WriteLine($"BAIXANDO A MUSICA: {titulo}");
    Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
    var trackAuthorization = obtemArquivos.ObtemTrackAuthorization(paginaPrincipal);
    var clientId = await obtemArquivos.ObtemClientId(httpClient, paginaPrincipal);
    var urlDaMusica = obtemArquivos.ObtemUrlDaMusica(paginaPrincipal, trackAuthorization, clientId);
    var dadosDaMusica = await obtemArquivos.ObtemDadosDaMusica(httpClient, urlDaMusica);
    obtemArquivos.SalvarMusica(titulo, dadosDaMusica);

    stopWatch.Stop();
    TimeSpan ts = stopWatch.Elapsed;
    var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);

    Console.WriteLine("BAIXOU EM: " + elapsedTime);
}
