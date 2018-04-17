//using System;
//using System.Threading.Tasks;


//namespace Acr.Caching.Tests
//{

//    public abstract class ProviderTestFixture<T> where T : ICache, new()
//    {
//        protected ICache Provider { get; set; }


//        [SetUp]
//        public virtual void OnBeforeTest()
//        {
//            this.Provider = new T();
//        }


//        [TearDown]
//        public virtual void OnAfterTest()
//        {
//            var disp = this.Provider as IDisposable;
//            if (disp != null)
//                disp.Dispose();
//        }


//        [Test]
//        public void BasicTest()
//        {
//            var dt = DateTime.Now;
//            this.Provider.Set("BasicTest", dt);
//            var get = this.Provider.Get<DateTime>("BasicTest");
//            Assert.AreEqual(dt, get);
//        }


//        [Test]
//        public void RemoveTest()
//        {
//            this.Provider.Set("RemoveTest", new object());
//            this.Provider.Remove("RemoveTest");
//            var obj = this.Provider.Get<object>("RemoveTest");
//            Assert.IsNull(obj);
//        }


//        [Test]
//        public void UpdateTest()
//        {
//            this.Provider.Set("UpdateTest", "version1");
//            this.Provider.Set("UpdateTest", "version2");
//            var store = this.Provider.Get<string>("UpdateTest");
//            Assert.AreEqual("version2", store);
//        }


//        [Timeout(10000)]
//        [Test]
//        public async Task CleanUpTest()
//        {
//            this.Provider.CleanUpTime = TimeSpan.FromSeconds(1);
//            this.Provider.Set("CleanUpTest", new object(), TimeSpan.FromSeconds(1));
//            await Task.Delay(3000);
//            var obj = this.Provider.Get<object>("CleanUpTest");
//            Assert.IsNull(obj);
//        }
//    }
//}
