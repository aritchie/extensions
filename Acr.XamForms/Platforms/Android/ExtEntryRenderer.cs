using System;
using System.ComponentModel;
using Android.Views.InputMethods;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Acr.XamForms.Controls;


[assembly: ExportRenderer(typeof(ExtEntry), typeof(ExtEntryRenderer))]
namespace Acr.XamForms.Controls
{
    public class ExtEntryRenderer : EntryRenderer
    {
        public ExtEntryRenderer(Android.Content.Context context) : base(context) { }


        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (this.Control != null && e.NewElement != null)
                this.Setup((ExtEntry)e.NewElement);
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ExtEntry.ReturnKeyTypeProperty.PropertyName)
                this.Setup((ExtEntry)sender);
        }


        protected virtual void Setup(ExtEntry entry)
        {
            this.Control.ImeOptions = this.GetValueFromDescription(entry.ReturnKeyType);
            this.Control.SetImeActionLabel(entry.ReturnKeyType.ToString(), this.Control.ImeOptions);
            this.Control.EditorAction += (sender, args) =>
            {
                entry.OnNext();

                if (entry.ReturnCommand != null && entry.ReturnCommand.CanExecute(null))
                    entry.ReturnCommand.Execute(null);
            };
        }


        protected virtual ImeAction GetValueFromDescription(ReturnKeyTypes value)
        {
            var type = typeof(ImeAction);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return (Android.Views.InputMethods.ImeAction)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (ImeAction)field.GetValue(null);
                }
            }
            throw new NotSupportedException($"Not supported on Android: {value}");
        }
    }
}