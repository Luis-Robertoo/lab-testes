using Microsoft.Extensions.Caching.Memory;
using Signal.Domain;

namespace Signal.Services;

public class UsersService : IUsersService
{

    private MemoryCache _cache;

    public UsersService()
    {
        var op = new MemoryCacheOptions();

        _cache = new MemoryCache(op);
    }

    public async Task<Sala> GetSala(string nomeDaSala)
    {
        var sala = _cache.Get(nomeDaSala) as Sala;
        return sala;
    }

    public async Task<Sala> CreateOrUpdateSala(string nomeDaSala, string idUsuario)
    {
        var sala = new Sala { };
        var dados = _cache.Get(nomeDaSala);
        var nome = _cache.Get("nome") ?? nomeDaSala;

        if (dados is null)
        {
            sala = new Sala { Name = nomeDaSala, Users = new List<Usuario> { new Usuario { Id = idUsuario } } };
            _cache.Set(nomeDaSala, sala);
            _cache.Set("nome", nomeDaSala);
            return sala;
        }

        sala = dados as Sala;

        var user = sala.Users.FirstOrDefault(u => u.Id == idUsuario);
        if (user is null) sala.Users.Add(new Usuario { Id = idUsuario });

        _cache.Set("nome", nomeDaSala);
        _cache.Set(nomeDaSala, sala);
        return sala;
    }

    public async Task<Sala> RemoveUser(string idUsuario)
    {
        var nome = _cache.Get("nome");
        var sala = _cache.Get(nome) as Sala;
        sala.Users.RemoveAll(u => u.Id == idUsuario);
        _cache.Set(nome, sala);

        return sala;
    }

}
