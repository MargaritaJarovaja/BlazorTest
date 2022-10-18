using BlazorTestApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
namespace BlazorTestApp.Server.Models
{
    public class DatabaseContext:DbContext

    {
        public DatabaseContext()
        {
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; } = null!;
        
    }

}
