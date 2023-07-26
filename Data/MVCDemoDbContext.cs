using UserMvcProject.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace UserMvcProject.Data
{
    public class MVCDemoDbContext:DbContext
    {
        public MVCDemoDbContext(DbContextOptions options): base(options)
        { 

        }
        public DbSet<User> Users { get; set; }
        
    }
}
