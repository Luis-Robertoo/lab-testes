// See https://aka.ms/new-console-template for more information
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Teste;
using File = System.IO.File;

Console.WriteLine("Hello, World!");
Console.WriteLine($"-=-=-=-=-=-=-=-=-=-=-=--=-=-=-=--=-=-=-=-=-=-=-=-=-=");

DotEnv.Load();


 var variaveis = Environment.GetEnvironmentVariables();

var tipo = variaveis.GetType();

foreach(DictionaryEntry variavel in variaveis)
    Console.WriteLine($"{variavel.Key} = {variavel.Value}");



Console.WriteLine("digite algo:");
Console.ReadLine();















async Task criaArquivo(string texto, Guid id)
{
    var caminho = Path.Combine(Path.GetTempPath() + $"0000000000000000000{id}.hjk");
    var bytes = Encoding.UTF8.GetBytes(texto);
    await File.WriteAllBytesAsync(caminho, bytes);
}

async Task<string> pegaArquivo(Guid id)
{
    var caminho = Path.Combine(Path.GetTempPath() + $"0000000000000000000{id}.hjk");
    var bytes = await File.ReadAllBytesAsync(caminho);
    return Encoding.UTF8.GetString(bytes);
}

static void criptografa(out string segredo, out byte[] stringCriptografada)
{
    var data = DateTime.Now.ToString("MM-dd-yyyy");
    segredo = "kYp3s5v8y/B?E(H+";
    var _tripleDES = new TripleDESCryptoServiceProvider();
    var resultado = string.Empty;
    var stringAserCriptografada = "String de teste aqui";


    //byte[] arrayDeBytesDaStringDescriptografada = Convert.FromBase64String("a");//Convert.FromBase64String(stringAserCriptografada);

    byte[] arrayDeBytesDaStringDescriptografada = Encoding.Unicode.GetBytes(stringAserCriptografada);

    _tripleDES.Key = Encoding.UTF8.GetBytes(segredo);
    _tripleDES.Mode = CipherMode.ECB;
    _tripleDES.Padding = PaddingMode.PKCS7;

    ICryptoTransform cTransform = _tripleDES.CreateEncryptor();
    stringCriptografada = cTransform.TransformFinalBlock(arrayDeBytesDaStringDescriptografada, 0, arrayDeBytesDaStringDescriptografada.Length);

    //byte[] stringDescriptografada = cTransform.TransformFinalBlock(arrayDeBytesDaStringCriptografada, 0, arrayDeBytesDaStringCriptografada.Length);
    _tripleDES.Clear();
    resultado = Encoding.UTF8.GetString(stringCriptografada);
    _tripleDES.Dispose();
}

static void descriptografa(string segredo, string a)
{
    var _tripleDES42 = new TripleDESCryptoServiceProvider();


    byte[] arrayDeBytesDaStringCriptografada42 = Convert.FromBase64String(a);

    _tripleDES42.Key = Encoding.UTF8.GetBytes(segredo);
    _tripleDES42.Mode = CipherMode.ECB;
    _tripleDES42.Padding = PaddingMode.PKCS7;

    ICryptoTransform cTransform42 = _tripleDES42.CreateDecryptor();
    byte[] stringDescriptografada = cTransform42.TransformFinalBlock(arrayDeBytesDaStringCriptografada42, 0, arrayDeBytesDaStringCriptografada42.Length);
    _tripleDES42.Clear();

    var aa = Convert.ToBase64String(stringDescriptografada);

    var resultado42 = Encoding.UTF8.GetString(stringDescriptografada);


    _tripleDES42.Dispose();
}

static void JurosCompostos(string dados)
{

    var taxa = ConverteJsonToDouble(dados) / 12;
    double porc = taxa / 100;
    double aporteMensal = 600;
    double aporteAcumulado = 0;
    double aportePrimario = 20000;
    double total = 0;
    int meses = 12;

    for (int i = 1; i <= meses; i++)
    {
        //taxa / 100 = porcetagem // 2 / 100 = 0,02
        //porcetagem x numero = rendimento // 0,02 * 1000 = 20
        
        if(i == 1)
            aporteAcumulado = (porc * aportePrimario) + aportePrimario + aporteMensal;
        else
        {
            var rendiMensal = porc * aporteAcumulado;
            aporteAcumulado = aporteAcumulado + aporteMensal + rendiMensal;
        }
        Console.WriteLine($"Mês - {i:D3} || Valor Acumulado: {aporteAcumulado:00000.00}");

    }

    var totalSemJuros = (meses * aporteMensal) + aportePrimario;
    Console.WriteLine($"-=-=-=-=-=-=-=-=-=-=-=--=-=-=-=--=-=-=-=-=-=-=-=-=-=");
    Console.WriteLine($"Total que saiu do seu bolso: {totalSemJuros}");
    Console.WriteLine($"-=-=-=-=-=-=-=-=-=-=-=--=-=-=-=--=-=-=-=-=-=-=-=-=-=");
    Console.WriteLine($"Sendo {meses} de {aporteMensal} mais 1 de {aportePrimario}");
    Console.WriteLine($"-=-=-=-=-=-=-=-=-=-=-=--=-=-=-=--=-=-=-=-=-=-=-=-=-=");
    Console.WriteLine($"Total que você ganhou sem fazer nada: {aporteAcumulado - totalSemJuros:.00}");

    var aa = Console.ReadLine();

    total = aporteMensal;

}

 async Task<string> PegaSelic()
{
    var urlBase = "https://api.bcb.gov.br";
    var url = urlBase + "/dados/serie/bcdata.sgs.1178/dados/ultimos/1?formato=json";
    var url2 = urlBase + "/dados/serie/bcdata.sgs.1178/dados/ultimos/1";

    HttpClient client = new HttpClient();
    client.DefaultRequestHeaders.Add("Host", "api.bcb.gov.br");
    client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.31.1");

    var response = await client.GetAsync(url);

    if(response.IsSuccessStatusCode)
        return await response.Content.ReadAsStringAsync();

    return string.Empty;

}

static double ConverteJsonToDouble(string json)
{
    var dados = JsonSerializer.Deserialize<List<Retorno>>(json);
    return Convert.ToDouble(dados.FirstOrDefault().valor.Replace(".", ","));
}
public class Retorno
{
    public string data { get; set; }
    public string valor { get; set; }
}