using System;


namespace Acr.XamForms.Pages
{
    public class TabbedPage : Xamarin.Forms.TabbedPage
    {
        protected override void OnAppearing()
        {
            (this.BindingContext as IViewModelLifecycle)?.OnActivated();
            base.OnAppearing();
        }


        protected override void OnDisappearing()
        {
            (this.BindingContext as IViewModelLifecycle)?.OnDeactivated();
            base.OnDisappearing();
        }


        protected override bool OnBackButtonPressed()
            => (this.BindingContext as IViewModelLifecycle)?.OnBackRequested() ?? true;


        protected override void OnSizeAllocated(double width, double height)
        {
            (this.BindingContext as IViewModelLifecycle)?.OnOrientationChanged(width < height);
            base.OnSizeAllocated(width, height);
        }
    }
}
