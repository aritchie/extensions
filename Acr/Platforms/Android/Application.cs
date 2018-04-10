using System;
using System.Threading;
using DroidApp = Android.App.Application;


namespace Acr
{
    public static class Application
    {
        public static void InvokeOnMainThread(Action action)
        {
            if (DroidApp.SynchronizationContext == SynchronizationContext.Current)
                action();
            else
                DroidApp.SynchronizationContext.Post(_ => action(), null);
        }
    }
}
