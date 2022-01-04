using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyPWS.Models.pwsstore
{
    public class PwsStoreContextFactory : IDesignTimeDbContextFactory<PwsStoreContext>
    {
        public PwsStoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PwsStoreContext>();
            optionsBuilder.UseSqlServer("SERVER =.; TRUSTED_CONNECTION = yes; DATABASE = PwsStore_design");
            return new PwsStoreContext(optionsBuilder.Options);
        }
    }
}
