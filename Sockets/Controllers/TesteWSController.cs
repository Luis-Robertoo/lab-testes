using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Sockets.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TesteWSController : ControllerBase
{
    private readonly ILogger<TesteWSController> _logger;
    private Dados _data;

    public TesteWSController(ILogger<TesteWSController> logger)
    {
        _logger = logger;
        _data = new Dados { Campo = "Construtor" };
    }

    [HttpGet]
    public async Task Get()
    {
        try
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest || _data.Clientes.Count > 10)
            {
                BadRequest();
                return;
            }

            using var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();

            var id = Guid.NewGuid().ToString();

            _data.Clientes.Add(id);
            _data.Id = id;

            await ws.SendAsync(
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize<Dados>(_data)),
                WebSocketMessageType.Text,
                true, CancellationToken.None
            );

            while (ws.State == WebSocketState.Open)
            {

                var recebido = new byte[1024];

                var rec = await ws.ReceiveAsync(recebido, CancellationToken.None);

                //var recebidoString = Encoding.ASCII.GetString(recebido).TrimEnd('\0');

                //Console.WriteLine(recebidoString);

                //var recebidoJson = JsonSerializer.Deserialize<Dados>(recebidoString);

                //Console.WriteLine(recebidoJson);

                //recebidoJson.Id = null;
                //_data = recebidoJson;

                await ws.SendAsync(
                    Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"/*JsonSerializer.Serialize<Dados>(_data)*/),
                    WebSocketMessageType.Text,
                    true, CancellationToken.None
                );

                await Task.Delay(1000);

                await ws.SendAsync(
                    Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"/*JsonSerializer.Serialize<Dados>(_data)*/),
                    WebSocketMessageType.Text,
                    true, CancellationToken.None
                );

                await Task.Delay(2000);

                await ws.SendAsync(
                    Encoding.UTF8.GetBytes($"{Guid.NewGuid()}"/*JsonSerializer.Serialize<Dados>(_data)*/),
                    WebSocketMessageType.Text,
                    true, CancellationToken.None
                );

                await Task.Delay(3000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private class Dados
    {
        public List<string> Clientes { get; set; } = new List<string>();
        public string Campo { get; set; }
        public string? Id { get; set; }
    }
}
