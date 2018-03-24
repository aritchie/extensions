using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms.PlatformConfiguration;
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
                this.Control.KeyDown += (object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs eventArgs) =>
                {
                    if (eventArgs.Key == Windows.System.VirtualKey.Enter)
                    {
                        entry.InvokeCompleted();
                        // Make sure to set the Handled to true, otherwise the RoutedEvent might fire twice
                        eventArgs.Handled = true;
                    }
                };
            }
        }
    }
}
