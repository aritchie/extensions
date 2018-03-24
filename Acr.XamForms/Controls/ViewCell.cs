using System;


namespace Acr.XamForms.Controls
{

    public class ViewCell : Xamarin.Forms.ViewCell
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            (this.BindingContext as IViewModelLifecycle)?.OnActivated();
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (this.BindingContext as IViewModelLifecycle)?.OnDeactivated();
        }
    }
}
