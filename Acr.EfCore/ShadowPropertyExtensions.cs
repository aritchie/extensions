using System;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public static class ShadowPropertyExtensions
    {
        public static void ExplicitSetShadowPropertiesFromDefaults(this AcrDbContext data)
        {
            data.BeforeAll.Subscribe(entries =>
            {
                foreach (var entry in entries)
                {
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.CurrentValue == null && prop.Metadata.IsShadowProperty && !prop.Metadata.IsNullable)
                        {
                            prop.CurrentValue = prop.Metadata.Relational().DefaultValue;
                        }
                    }
                }
            });
        }
    }
}
