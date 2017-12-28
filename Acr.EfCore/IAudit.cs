using System;


namespace Acr.EfCore
{
    public interface IAudit
    {
        int Version { get; set; }
    }
}
