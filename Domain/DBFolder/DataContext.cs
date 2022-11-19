using IpaddressesWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IpaddressesWebAPI.DBFolder
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Countries> Countries { get; set; }
        public DbSet<IPAddresses> IPAddresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=NAG6CHANDRVAR01; initial catalog=OrderDb;integrated security=true;");
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ipaddressesdb;Trusted_Connection=True;Encrypt=False;");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
