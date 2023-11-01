// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");





var dados = File.OpenRead("C:\\Projetos\\Luis\\CSHARP\\TestarAsParadas\\fileToBinary\\dummy.pdf");

var binario = new BinaryReader(dados);

var tamanho = dados.Length;

var bytes = binario.ReadBytes((int)tamanho);

var a = bytes;
