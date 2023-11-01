using Microsoft.AspNetCore.SignalR;
using Signal.Services;

namespace Signal.Hubs;

public class DadosHub : Hub
{
    private readonly IUsersService _usersService;

    public DadosHub(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("Send", $"{Context.ConnectionId} saiu.");
        var sala = await _usersService.RemoveUser(Context.ConnectionId);
        await Clients.Group(sala.Name).SendAsync("mostra", sala);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task GetSala(string groupName)
    {
        var sala = await _usersService.GetSala(groupName);
        await Clients.Group(groupName).SendAsync("Send", sala);
    }

    public async Task AddToGroup(string groupName)
    {
        var sala = await _usersService.CreateOrUpdateSala(groupName, Context.ConnectionId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} entrou no grupo {groupName}.");
        await Clients.Group(groupName).SendAsync("mostra", sala);
    }

    public async Task<string> WaitForMessage(string connectionId)
    {
        var message = await Clients.Client(connectionId).InvokeAsync<string>("mensagem", CancellationToken.None);
        return message;
    }
}
