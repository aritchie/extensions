using System;
using System.Windows.Input;
using Xamarin.Forms;


namespace Acr.XamForms.Controls
{
    public enum ReturnKeyTypes
    {
        Default,
        Go,
        Next,
        Done,
        Send,
        Search,
        Continue
    }


    public class ExtEntry : Entry
    {
        public static readonly BindableProperty ReturnKeyTypeProperty = BindableProperty.Create(
            nameof(ReturnKeyType),
            typeof(ReturnKeyTypes),
            typeof(ExtEntry),
            ReturnKeyTypes.Default,
            BindingMode.OneWay
        );
        public ReturnKeyTypes ReturnKeyType
        {
            get => (ReturnKeyTypes)this.GetValue(ReturnKeyTypeProperty);
            set => this.SetValue(ReturnKeyTypeProperty, value);
        }


        public static readonly BindableProperty NextViewProperty = BindableProperty.Create(
            nameof(NextView),
            typeof(View),
            typeof(ExtEntry)
        );
        public View NextView
        {
            get => (View)this.GetValue(NextViewProperty);
            set => this.SetValue(NextViewProperty, value);
        }


        //public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
        //    nameof(ReturnCommand),
        //    typeof(ICommand),
        //    typeof(ExtEntry)
        //);
        //public ICommand ReturnCommand
        //{
        //    get => (ICommand)this.GetValue(ReturnCommandProperty);
        //    set => this.SetValue(ReturnCommandProperty, value);
        //}


        public virtual void OnNext() => this.NextView?.Focus();
    }
}
