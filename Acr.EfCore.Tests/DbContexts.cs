using System;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore.Tests
{
    public class SoftDeleteDbContext : BaseDbContext
    {
        public SoftDeleteDbContext() : base("deletes") { }
    }


    public class TenantDbContext : AcrTenancyDbContext
    {
        public DbSet<TestItem> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("tenants");
            base.OnConfiguring(optionsBuilder);
        }
    }


    public class DateStampDbContext : BaseDbContext
    {
        public DateStampDbContext() : base("dates") { }
    }


    public abstract class BaseDbContext : AcrDbContext
    {
        readonly string name;


        protected BaseDbContext(string name)
        {
            this.name = name;
        }


        public DbSet<TestItem> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(this.name);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
