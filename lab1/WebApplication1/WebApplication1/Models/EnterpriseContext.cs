using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class EnterpriseContext : DbContext
    {
        public EnterpriseContext(DbContextOptions<EnterpriseContext> options) : base(options)
        {

        }
        public DbSet<Enterprise> Enterprises { get; set; }
    }
}
