using System;


namespace Acr.XamForms
{
    public interface IViewModelLifecycle
    {
        /*
        public bool FireInpcOnMainThread { get; set; } = true;
        public static Action<Action> MainThreadCall { get; set; }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.FireInpcOnMainThread && MainThreadCall != null)
            {
                MainThreadCall.Invoke(() =>
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))
                );
            }
            else
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
         */
        void OnActivated();
        void OnDeactivated();
        void OnOrientationChanged(bool isPortrait);
        bool OnBackRequested();
    }
}
