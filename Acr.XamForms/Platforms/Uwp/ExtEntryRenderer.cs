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
                this.Setup(entry);
        }



        protected virtual void Setup(ExtEntry entry)
        {
            this.Control.KeyDown += (sender, args) =>
            {
                switch (args.Key)
                {
                    case VirtualKey.Tab:
                        entry.OnNext();
                        args.Handled = true;
                        break;

                    case VirtualKey.Enter:
                        entry.OnNext();
                        if (entry.ReturnCommand != null && entry.ReturnCommand.CanExecute(null))
                            entry.ReturnCommand.Execute(null);

                        args.Handled = true;
                        break;
                }

            };
        }
    }
}
