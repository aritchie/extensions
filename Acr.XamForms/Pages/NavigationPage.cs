using System;


namespace Acr.XamForms.Pages
{
    public class NavigationPage : Xamarin.Forms.NavigationPage
    {
        public NavigationPage()
        {
            this.Popped += (sender, args) => (args.Page.BindingContext as IViewModelLifecycle)?.OnDestroy();
        }
    }
}
