using System;


namespace Acr.Platforms.iOS
{
    public static partial class Platform
    {
        static Platform()
        {
            Current = new PlatformImpl();
        }
    }
}
