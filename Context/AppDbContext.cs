using Microsoft.EntityFrameworkCore;
using apiUsuarios.Models;

namespace apiUsuarios.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
            
        public DbSet<Persona> personas { get; set; }

        internal Task FindAsync(string numero_identificacion)
        {
            throw new NotImplementedException();
        }

       
    }
}
