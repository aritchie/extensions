using System;
using System.Linq;
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
            TestDbContext.ContextCreated.Subscribe(db =>
                ((AcrTenancyDbContext)db).TenantId = 1
            );

            using (var db = new TestDbContext())
            {
                var entry = db.Items.Add(new TestItem { Description = "Test" });
                db.SaveChanges();

                entry.Entity.TenantId.Should().Be(1);
            }
        }


        [Fact]
        public void TenancyQueryTest()
        {
            var tenantId = 1;
            TestDbContext.ContextCreated.Subscribe(db =>
                ((AcrTenancyDbContext)db).TenantId = tenantId
            );

            tenantId = 1;
            using (var db = new TestDbContext())
            {
                db.Items.Add(new TestItem { Description = "1" });
                db.SaveChanges();
            }

            tenantId = 2;
            using (var db = new TestDbContext())
            {
                db.Items.Add(new TestItem { Description = "2" });
                db.SaveChanges();
            }

            tenantId = 1;
            using (var db = new TestDbContext())
            {
                var e = db.Items.ToList().First();
                e.Should().NotBeNull("Item 1 not found");
                e.Description.Should().Be("1");
                e.TenantId.Should().Be(1);
            }

            tenantId = 2;
            using (var db = new TestDbContext())
            {
                var e = db.Items.ToList().First();
                e.Should().NotBeNull("Item 2 not found");
                e.Description.Should().Be("2");
                e.TenantId.Should().Be(2);
            }

            using (var db = new TestDbContext())
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
