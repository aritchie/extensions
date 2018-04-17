using System;


namespace Acr
{
    public static partial class Platform
    {
        static Platform()
        {
            Current = new PlatformImpl();
        }
    }
}
