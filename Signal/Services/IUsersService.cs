using Signal.Domain;

namespace Signal.Services;

public interface IUsersService
{
    Task<Sala> CreateOrUpdateSala(string nomeDaSala, string idUsuario);
    Task<Sala> RemoveUser(string idUsuario);
    Task<Sala> GetSala(string nomeDaSala);
}
