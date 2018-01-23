using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace Acr.EfCore.Tests
{

    public class DataExtensionTests
    {

        [Fact]
        public async Task ParameterizedSql()
        {
            await this.Execute("parameterized_sql.db", async db =>
            {
                using (var reader = await db.ExecuteReader("SELECT * FROM Items WHERE Id = @Id", CancellationToken.None, ("Id", 1)))
                {
                    reader.Read();
                }
            });
        }


        [Fact]
        public async Task ReflectionExecuteToList()
        {
            await this.Execute("executetolist.db", async db =>
            {
                // TODO: different property types
                // TODO: nullable property types
                // TODO: missed properties
                // TODO: partial data sets
                await db.ReflectionExecuteToList<TestItem>("SELECT * FROM Items");
            });
        }


        async Task Execute(string dbName, Func<SqliteDbContext, Task> func)
        {
            using (var db = new SqliteDbContext(dbName))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                await func(db);
            }
        }
    }
}
