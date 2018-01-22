using System;


namespace Acr.EfCore
{
    public interface ITenantEntity
    {
        int TenantId { get; set; }
    }
}
