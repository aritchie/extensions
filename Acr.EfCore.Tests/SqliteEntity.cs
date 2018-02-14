using System;


namespace Acr.EfCore.Tests
{
    public class SqliteEntity
    {
        public int Id { get; set; }
        public long SetId { get; set; }
        public long? NullId { get; set; }
    }


    public class SqliteDto
    {
        public int SetId { get; set; }
        public int? NullId { get; set; }
    }
}
