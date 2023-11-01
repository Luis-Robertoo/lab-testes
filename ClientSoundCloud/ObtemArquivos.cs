using System.Text.RegularExpressions;

namespace ClientSoundCloud;

public class ObtemArquivos
{
    public async Task<string>? ObtemPaginaPrincipal(HttpClient httpClient, string url)
    {
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var paginaPrincipal = response.Content.ReadAsStringAsync().Result;

        return paginaPrincipal;
    }

    public async Task<string> ObtemClientId(HttpClient httpClient, string paginaPrincipal)
    {
        var comecoLinks = paginaPrincipal.IndexOf("<script src=\"https://a-v2.sndcdn.com");
        var links = paginaPrincipal.Substring(comecoLinks, paginaPrincipal.Length - comecoLinks);
        var listaDeLinks = paginaPrincipal.Substring(comecoLinks, paginaPrincipal.Length - comecoLinks).Split("</script>\n<script").ToList().Select(src =>
        {
            if (src.Contains("<script src=\"https")) return string.Empty;
            string nova = src.Replace(" crossorigin src=\"", "").Replace("\">", "").Replace("</script>\n</body>\n</html>", "").Replace("\n", "");
            return nova;
        });

        var clientId = string.Empty;

        foreach (var link in listaDeLinks)
        {
            if (link.Equals("")) continue;

            using HttpResponseMessage js = await httpClient.GetAsync(link);
            var texto = js.Content.ReadAsStringAsync().Result;
            var comecoLink = texto.IndexOf("{client_id:\"");
            var fimLink = texto.IndexOf("\",nonce:e.nonce}))");

            if (comecoLink == -1) continue;

            clientId = texto.Substring(comecoLink + 12, fimLink - (comecoLink + 12));
            if (clientId.Length == 32) break;
        }

        return clientId;
    }

    public async Task<byte[]> ObtemDadosDaMusica(HttpClient httpClient, string url)
    {
        var response = await httpClient.GetAsync(url);
        var urlDosDados = response.Content.ReadAsStringAsync().Result.Replace("{\"url\":\"", "").Replace("\"}", "");

        var dados = await httpClient.GetByteArrayAsync(urlDosDados);
        return dados;
    }

    public string ObtemTitulo(string paginaPrincipal)
    {
        var tituloInicio = paginaPrincipal.IndexOf("\"title\":\"");
        var tituloFim = paginaPrincipal.IndexOf("\",\"track_format\"");
        var titulo = paginaPrincipal.Substring(tituloInicio, tituloFim - tituloInicio).Replace("\"title\":\"", "");
        return Regex.Replace(titulo, "[^0-9A-Za-z _ -]", "");

    }

    public string ObtemTrackAuthorization(string paginaPrincipal)
    {
        var trackAuthorizationInicio = paginaPrincipal.IndexOf("\"track_authorization\":\"");
        var trackAuthorizationFim = paginaPrincipal.IndexOf("\",\"monetization_model\"");
        var trackAuthorization = paginaPrincipal.Substring(trackAuthorizationInicio, trackAuthorizationFim - trackAuthorizationInicio).Replace("\"track_authorization\":\"", "");
        return trackAuthorization;
    }

    public string ObtemUrlDaMusica(string paginaPrincipal, string trackAuthorization, string clientId)
    {
        var urlBytesInicio = paginaPrincipal.IndexOf(",{\"url\":\"");
        var urlBytesFim = paginaPrincipal.IndexOf("e\",\"preset\"");
        var urlBytes = paginaPrincipal.Substring(urlBytesInicio, urlBytesFim - urlBytesInicio).Replace(",{\"url\":\"", "");
        return $"{urlBytes}e?client_id={clientId}&track_authorization={trackAuthorization}";
    }

    public async Task<List<string>?> ObtemListaDeArquivosDeAudio(HttpClient httpClient, string url, string caminhoBase)
    {
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var musicaEmTrechos = await response.Content.ReadAsStringAsync();
        var trechos = EditaArquivos.ExtrairLinksDosTrechos(musicaEmTrechos);

        var listaDeArquivosDeAudio = new List<string>();

        var c = 0;
        var trechoAtual = string.Empty;
        try
        {
            foreach (var trecho in trechos)
            {
                trechoAtual = trecho;

                if (trecho == string.Empty) continue;

                var dados = await httpClient.GetByteArrayAsync(trechoAtual);
                listaDeArquivosDeAudio.Add(SalvarTrechoMusica(caminhoBase, c, dados));
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.WriteLine($"Trecho = {trechoAtual} \n Contador = {c}");
        }

        return listaDeArquivosDeAudio;
    }

    private static string SalvarTrechoMusica(string caminho, int contador, byte[] dados)
    {
        var caminhoCompleto = $"{caminho}/{contador}.mp3";

        using (FileStream fs = File.Create(caminho))
        {
            fs.Write(dados, 0, dados.Length);
        }
        return caminhoCompleto;
    }

    public void SalvarMusica(string nome, byte[] dados)
    {
        try
        {
            var caminhopasta = Path.Combine(@"C:\Users\Luis\Music", "soundcloud");
            if (!Directory.Exists(caminhopasta)) Directory.CreateDirectory(caminhopasta);

            var caminhoCompleto = Path.Combine(caminhopasta, $"{nome}.mp3");

            using (FileStream fs = File.Create(caminhoCompleto))
            {
                fs.Write(dados, 0, dados.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
