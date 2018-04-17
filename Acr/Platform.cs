using System;


namespace Acr
{
    public static partial class Platform
    {
        static IPlatform current;
        public static IPlatform Current
        {
            get
            {
                if (current == null)
                    throw new Exception("[Acr.Core] Platform implementation not found.  Have you added a nuget reference to your platform project?");

                return current;
            }
            set => current = value;
        }
    }
}
