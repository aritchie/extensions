using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Acr.XamForms;


[assembly: ExportRenderer(typeof(ExtEntry), typeof(ExtEntryRenderer))]
namespace Acr.XamForms
{
    public class ExtEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (this.Control != null && e.NewElement is ExtEntry entry)
                this.Setup(entry);
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ExtEntry.ReturnKeyTypeProperty.PropertyName)
               this.Setup((ExtEntry)sender);
        }


        protected virtual void Setup(ExtEntry entry)
        {
            //this.Control.ClearButtonMode = UITextFieldViewMode.UnlessEditing;
            this.Control.ReturnKeyType = this.GetValueFromDescription(entry.ReturnKeyType);
            this.Control.ShouldReturn += txt =>
            {
                entry.OnNext();
                if (entry.ReturnCommand != null && entry.ReturnCommand.CanExecute(null))
                    entry.ReturnCommand.Execute(null);

                return true;
            };
        }


        protected virtual UIReturnKeyType GetValueFromDescription(ReturnKeyTypes value)
        {
            var type = typeof(UIReturnKeyType);
            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return (UIReturnKeyType)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (UIReturnKeyType)field.GetValue(null);
                }
            }
            return UIReturnKeyType.Done;
        }
    }
}
