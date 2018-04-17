using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace Acr.Caching
{
    public abstract class AbstractTimerCache : AbstractCache
    {
        readonly CancellationTokenSource cancelSource = new CancellationTokenSource();

        public TimeSpan CleanUpTime { get; set; }

        protected abstract void OnTimerElapsed();


        protected override void Init()
        {
            this.RunCleanUp();
        }


        async Task RunCleanUp()
        {
            while (!this.cancelSource.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(this.CleanUpTime);
                    if (this.cancelSource.IsCancellationRequested)
                        return;

                    this.OnTimerElapsed();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[cache cleanup error]: {0}", ex);
                }
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;

            this.cancelSource.Cancel(false);
        }
    }
}
