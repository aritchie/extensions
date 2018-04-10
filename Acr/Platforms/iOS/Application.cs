using System;
using Foundation;
using UIKit;


namespace Acr
{
    public static class Application
    {
        public static void InvokeOnMainThread(Action action)
        {
            if (NSThread.Current.IsMainThread)
                action();
            else
                UIApplication.SharedApplication.InvokeOnMainThread(action);
        }
    }
}
