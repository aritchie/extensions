using System;


namespace Acr.EfCore.Tests
{
    public class TestItem : ISoftDeleteEntity, IDateStampEntity, ITenantEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public int? TenantId { get; set; }
    }
}
