// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

string json = @"
        [
            {
                ""id"": 1,
                ""associaFilhos"": true,
                ""filhos"":[]
            },
            {
                ""id"": 2,
                ""associaFilhos"": false,
                ""filhos"": [
                    {
                        ""id"": 12,
                        ""associaFilhos"": false,
                        ""filhos"":[
                            {
                                ""id"": 150,
                                ""associaFilhos"": true,
                                ""filhos"":[]      
                            },
                            {
                                ""id"": 151,
                                ""associaFilhos"": false,
                                ""filhos"":[]      
                            }
                        ]         
                    },
                    {
                        ""id"": 13,
                        ""associaFilhos"": true,
                        ""filhos"":[]     
                    },
                    {
                        ""id"": 14,
                        ""associaFilhos"": true,
                        ""filhos"":[]        
                    },
                    {
                        ""id"": 15,
                        ""associaFilhos"": true,
                        ""filhos"":[]        
                    },
                    {
                        ""id"": 16,
                        ""associaFilhos"": false,
                        ""filhos"":[]        
                    }
                ]
            }
        ]";

List<SalvaPastasDTO> pastasa = JsonConvert.DeserializeObject<List<SalvaPastasDTO>>(json);




var idsGeral = new List<int>();
var a = PegaIdsDasPastas(pastasa, ref idsGeral);

var bb = a;


static List<int> PegaIdsDasPastas(List<SalvaPastasDTO> pastas, ref List<int> ids)
{
    
    if (pastas.Count == 0)
        return ids;

    foreach (var pasta in pastas)
    {
        if (pasta.AssociaFilhos)
        {
            ids.Add(pasta.Id);
            //var pastasFilhas = await _pastaRepositorio.FiltrarPastasFilhasPorId(pasta.Id);
            //ids.AddRange(pastasFilhas.Select(x => x.Id).ToList());
            continue;
        }

        if (pasta.Filhos.Count != 0)
        {   
            //ids.Add((int)pasta.Id);
            PegaIdsDasPastas(pasta.Filhos, ref ids);
        }

        ids.Add(pasta.Id);

    }

    return ids;

}

public class SalvaPastasDTO
{
    public int Id { get; set; }
    public bool AssociaFilhos { get; set; }
    public List<SalvaPastasDTO> Filhos { get; set; }
}