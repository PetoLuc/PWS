using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyPWS.Models.pwsstore
{
    public class PwsStoreContextFactory : IDesignTimeDbContextFactory<PwsStoreContext>
    {
        public PwsStoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PwsStoreContext>();
            optionsBuilder.UseSqlServer("SERVER =.\\SqlExpress; TRUSTED_CONNECTION = yes; DATABASE = PwsStore_design", x=>x.UseNetTopologySuite());
            return new PwsStoreContext(optionsBuilder.Options);
        }
    }
}
