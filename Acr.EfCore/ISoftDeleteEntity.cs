using System;


namespace Acr.EfCore
{
    public interface ISoftDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}
