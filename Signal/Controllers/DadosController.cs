using Microsoft.AspNetCore.Mvc;
using Signal.Services;

namespace Signal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DadosController : ControllerBase
{
    private readonly IUsersService _usersService;
    public DadosController(IUsersService usersService)
    {
        _usersService = usersService;
    }


    [HttpGet]
    public async Task<ActionResult> Jogo([FromQuery] string nomeDaSala)
    {

        return Ok("olá");
    }

}
