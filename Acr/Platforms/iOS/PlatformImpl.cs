using System;
using Foundation;
using UIKit;


namespace Acr
{
    public class PlatformImpl : IPlatform
    {
        public void InvokeOnMainThread(Action action)
        {
            if (NSThread.Current.IsMainThread)
                action();
            else
                UIApplication.SharedApplication.BeginInvokeOnMainThread(action);
        }
    }
}
