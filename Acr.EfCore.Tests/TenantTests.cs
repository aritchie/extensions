using System;
using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace Acr.EfCore.Tests
{
    public class TenantTests
    {
        [Fact]
        public void SaveTest()
        {
            using (TenantDbContext.ContextCreated.OfType<TenantDbContext>().Subscribe(db => db.TenantId = 1))
            {
                using (var db = new TenantDbContext())
                {
                    var entry = db.Items.Add(new TestItem { Description = "Test" });
                    db.SaveChanges();

                    entry.Entity.TenantId.Should().Be(1);
                }
            }
        }


        [Fact]
        public void TenancyQueryTest()
        {
            var tenantId = 1;
            using (TenantDbContext.ContextCreated.OfType<TenantDbContext>().Subscribe(db => db.TenantId = tenantId))
            {

                tenantId = 1;
                using (var db = new TenantDbContext())
                {
                    db.Items.Add(new TestItem {Description = "1"});
                    db.SaveChanges();
                }

                tenantId = 2;
                using (var db = new TenantDbContext())
                {
                    db.Items.Add(new TestItem {Description = "2"});
                    db.SaveChanges();
                }

                tenantId = 1;
                using (var db = new TenantDbContext())
                {
                    var e = db.Items.ToList().First();
                    e.Should().NotBeNull("Item 1 not found");
                    e.Description.Should().Be("1");
                    e.TenantId.Should().Be(1);
                }

                tenantId = 2;
                using (var db = new TenantDbContext())
                {
                    var e = db.Items.ToList().First();
                    e.Should().NotBeNull("Item 2 not found");
                    e.Description.Should().Be("2");
                    e.TenantId.Should().Be(2);
                }

                using (var db = new TenantDbContext())
                {
                    db
                        .Items
                        .IgnoreQueryFilters()
                        .ToList()
                        .Count
                        .Should()
                        .Be(2);
                }
            }
        }
    }
}
