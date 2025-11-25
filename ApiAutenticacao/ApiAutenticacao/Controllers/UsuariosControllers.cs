using ApiAutenticacao.Data;
using ApiAutenticacao.Models;
using ApiAutenticacao.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace ApiAutenticacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosControllers : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosControllers(AppDbContext context)
        {
            _context = context;
        }

        // CADASTRAR USUÁRIO
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuarioAsync([FromBody] CadastroUsuarioDTO dadosUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar se o email já existe
            Usuario? usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioExistente != null)
            {
                return BadRequest(new { mensagem = "Email já cadastrado" });
            }

            // Criar usuário
            Usuario usuario = new Usuario
            {
                Nome = dadosUsuario.Nome,
                Email = dadosUsuario.Email,
                Senha = HashPassword(dadosUsuario.Senha)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = usuario.Id,
                nome = usuario.Nome,
                email = usuario.Email
            });
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO dadosUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Usuario? usuarioEncontrado = await _context.Usuarios
                .FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioEncontrado == null)
            {
                return NotFound("Usuário não encontrado");
            }

            bool senhaValida = Verify(dadosUsuario.Senha, usuarioEncontrado.Senha);

            if (!senhaValida)
            {
                return Unauthorized("Login não realizado. Email ou senha inválidos.");
            }

            return Ok("Login realizado com sucesso");
        }
    }
}





