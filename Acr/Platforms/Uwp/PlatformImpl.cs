using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;


namespace Acr
{
    public class PlatformImpl : IPlatform
    {
        public void InvokeOnMainThread(Action action)
        {
            //this.dispatcher = dispatcher ?? new Func<Action, Task>(x => CoreApplication
            //                                           +                .MainView
            //    .CoreWindow
            //    .Dispatcher
            //    .RunAsync(CoreDispatcherPriority.Normal, () => x())
            //    .AsTask()
            //);
            CoreApplication.MainView.CoreWindow.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
