using System;
using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace Acr.EfCore.Tests
{
    public class SoftDeleteTests
    {
        [Fact]
        public void Tests()
        {
            using (SoftDeleteDbContext.ContextCreated.OfType<SoftDeleteDbContext>().Subscribe(db => db.SoftDeletes()))
            {
                using (var db = new SoftDeleteDbContext())
                {
                    db.Items.Add(new TestItem {Description = "1"});
                    db.SaveChanges();
                }

                using (var db = new SoftDeleteDbContext())
                {
                    var item = db.Items.Single(x => x.Description == "1");
                    db.Items.Remove(item);
                    db.SaveChanges();
                }

                using (var db = new SoftDeleteDbContext())
                {
                    var item = db.Items.IgnoreQueryFilters().Single(x => x.Description == "1");
                    item.Should().NotBeNull();
                    item.IsDeleted.Should().Be(true, "IsDeleted should be marked true");
                }

                using (var db = new SoftDeleteDbContext())
                {
                    var item = db.Items.SingleOrDefault(x => x.Description == "1");
                    item.Should().BeNull("IsDeleted auto filter did not work");
                }
            }
        }
    }
}
