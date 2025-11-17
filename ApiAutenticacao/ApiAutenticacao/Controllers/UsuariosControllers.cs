using ApiAutenticacao.Data;
using ApiAutenticacao.Models;
using ApiAutenticacao.Models.DTO;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
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


        [HttpPost("cadastrar")]
        public async Task<ActionResult> CadastrarUsuario([FromBody] CadastroUsuarioDTO dadosUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            Usuario? usuarioExistente = await _context.Usuarios.
                FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioExistente != null)
            {
                return BadRequest(new { mensagem = "Email já cadastrado" });
            }
            Usuario usuario = new Usuario
            {
                Nome = dadosUsuario.Nome,
                Email = dadosUsuario.Email,
                Senha = HashPassword(dadosUsuario.Senha),
                ConfirmarSenha = dadosUsuario.ConfirmarSenha
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
    }
}

    


     