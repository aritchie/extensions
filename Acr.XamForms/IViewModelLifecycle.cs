using System;


namespace Acr.XamForms
{
    public interface IViewModelLifecycle
    {
        void OnActivated();
        void OnDeactivated();
        void OnOrientationChanged(bool isPortrait);
        bool OnBackRequested();
    }
}
