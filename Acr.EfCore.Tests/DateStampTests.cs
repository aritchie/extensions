using System;
using FluentAssertions;
using Xunit;


namespace Acr.EfCore.Tests
{
    public class DateStampTests
    {
        [Fact]
        public void Test()
        {
            TestDbContext.ContextCreated.Subscribe(db => db.DateStamps());

            using (var db = new TestDbContext())
            {
                var e = db.Items.Add(new TestItem { Description = "1" });
                db.SaveChanges();

                var now = DateTimeOffset.UtcNow.ToString("f");
                e.Entity.DateCreated.ToString("f").Should().Be(now);
                e.Entity.DateUpdated.ToString("f").Should().Be(now);
            }
        }
    }
}
