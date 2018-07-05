using System;
using System.Threading;
using Android.App;


namespace Acr
{
    public class PlatformImpl : IPlatform
    {
        public void InvokeOnMainThread(Action action)
        {
            if (Application.SynchronizationContext == SynchronizationContext.Current)
                action();
            else
                Application.SynchronizationContext.Post(_ => action(), null);
        }
    }
}
