using System;
using System.Reactive.Linq;
using FluentAssertions;
using Xunit;


namespace Acr.EfCore.Tests
{
    public class DateStampTests
    {
        [Fact]
        public void Test()
        {
            using (DateStampDbContext.ContextCreated.OfType<DateStampDbContext>().Subscribe(db => db.DateStamps()))
            {
                using (var db = new DateStampDbContext())
                {
                    var e = db.Items.Add(new TestItem {Description = "1"});
                    db.SaveChanges();

                    var now = DateTimeOffset.UtcNow.ToString("f");
                    e.Entity.DateCreated.ToString("f").Should().Be(now);
                    e.Entity.DateUpdated.ToString("f").Should().Be(now);
                }
            }
        }
    }
}
