using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;


namespace Acr.Tests
{
    public class RxExtensionTests
    {
        [Fact]
        public async Task WhenAnyValue_InitialValue()
        {
            var obj = new TestObject { Value = "hi" };
            var value = await obj.RxWhenAnyValue(x => x.Value).Take(1).ToTask();
            value.Should().Be(obj.Value);
        }


        [Fact]
        public void WhenAnyValue_Change()
        {
            var update = "";
            var obj = new TestObject();
            obj.RxWhenAnyValue(x => x.Value).Skip(1).Subscribe(value => update = value);
            obj.Value = "changed";
            update.Should().Be(obj.Value);
        }


        [Fact]
        public void MaxLength()
        {
            var obj = new TestObject();
            obj.ApplyMaxLengthConstraint(x => x.Value, 5);

            obj.Value = "12345";
            obj.Value.Should().Be("12345");

            obj.Value = "123456";
            obj.Value.Should().Be("12345");
        }
    }
}
