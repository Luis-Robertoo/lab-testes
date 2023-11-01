// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

fizzBuzz(15);



static void fizzBuzz(int n)
{

    for (int i = 1; i <= n; i++)
    {
        var primeira = Multiplo(i, 3) ? "Fizz" : "";
        var segunda = Multiplo(i, 5) ? "Buzz" : "";
        if (primeira == "" && segunda == "")
        {
            Console.WriteLine(i);
            continue;
        }
        Console.WriteLine(primeira + segunda);
    }
}

static bool Multiplo(int numero, int multiplo)
{
    var resultado = numero % multiplo == 0 ? true : false;
    return resultado;
}






var teste1 = 3 / 2;
var teste2 = 3 % 2;
var teste3 = 5 % 2;








static void TesteMaroto()
{
    string palavra = null;

    var texto = palavra ?? "padrao";

    var texto2 = palavra is null ? "padrao" : palavra;

    string texto3 = null;

    palavra = "algo";

    palavra ??= texto3;

    string? a = null;



    int? numero = 100;

    int eae = numero ?? 120;



    var QtdPorPagina = numero.HasValue ? numero == 0 ? 500 : numero : 500;


    Console.WriteLine("aa");
}

