using System;
using FluentAssertions;
using Xunit;


namespace Acr.EfCore.Tests
{
    public class AfterStateTests
    {
        [Fact]
        public void StateTests()
        {
            using (var db = new StateDbContext())
            {
                var e = db.Items.Add(new TestItem { Description = "1" });
                db.SaveChanges();

                PreEntityEntry afterEntry = null;
                using (db.AfterEach.Subscribe(x => afterEntry = x))
                {
                    var e2 = db.Items.Find(e.Entity.Id);
                    e2.Description = "1";
                    db.SaveChanges();
                    afterEntry.OriginalValues["Description"].Should().Be(afterEntry.Entry.CurrentValues["Description"], "No change should have been detected");

                    var e3 = db.Items.Find(e.Entity.Id);
                    e3.Description = "2";
                    db.SaveChanges();

                    afterEntry.OriginalValues["Description"].Should().Be("1", "Original values in pre-entry did not clone");
                    afterEntry.Entry.OriginalValues["Description"].Should().Be("2", "Original values didn't change in standard entry");
                    afterEntry.Entry.CurrentValues["Description"].Should().Be("2", "Current values didn'tchange in standard entry");
                }
            }
        }
    }
}
