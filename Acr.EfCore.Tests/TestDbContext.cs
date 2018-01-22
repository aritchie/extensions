using System;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore.Tests
{
    public class TestDbContext : AcrTenancyDbContext
    {
        public DbSet<TestItem> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("tests");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
