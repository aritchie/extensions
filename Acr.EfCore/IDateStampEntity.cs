using System;


namespace Acr.EfCore
{
    public interface IDateStampEntity
    {
        DateTimeOffset DateUpdated { get; set; }
        DateTimeOffset DateCreated { get; set; }
    }
}
