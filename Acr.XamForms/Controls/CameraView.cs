using System;
using System.Windows.Input;
using Xamarin.Forms;


namespace Acr.XamForms.Controls
{
    public class CameraView : View
    {
        public ICommand TakePictureCommand { get; set; }

        // binds one way to viewmodel - we use temp paths internally
        public string PhotoFilePath { get; set; }
    }
}
