using System;
using Android.App;
using Android.Content;
using Android.OS;


namespace Acr
{
    public class StickyService : Service
    {
        public override IBinder OnBind(Intent intent) => null;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
            => StartCommandResult.Sticky;
    }
}
