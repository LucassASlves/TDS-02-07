using Microsoft.EntityFrameworkCore;

namespace ApiAutenticacao.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ApiAutenticacao.Models.Usuario> Usuarios
        {
            get; set;



        }


    }
}
       