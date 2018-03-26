using System;
using Windows.System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Acr.XamForms.Controls;


[assembly: ExportRenderer(typeof(ExtEntry), typeof(ExtEntryRenderer))]
namespace Acr.XamForms.Controls
{
    public class ExtEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null && this.Element is ExtEntry entry)
            {
                this.Control.KeyDown += (sender, args) =>
                {
                    if (args.Key == VirtualKey.Enter)
                    {
                        //entry.InvokeCompleted();
                        args.Handled = true;
                    }
                };
            }
        }
    }
}
