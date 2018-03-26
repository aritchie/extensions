using System;
using Acr.XamForms.Controls;
using Android.Content;
using Android.Hardware;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace Acr.XamForms.Controls
{
    public class CameraViewRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<CameraView, AndroidCameraView>
    {
        AndroidCameraView nativeView;


        public CameraViewRenderer(Context context) : base(context) { }


        protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                this.nativeView = new AndroidCameraView(this.Context);
                this.SetNativeControl(this.nativeView);
            }

            //if (e.OldElement != null)
            //{
            //    // Unsubscribe
            //    cameraPreview.Click -= OnCameraPreviewClicked;
            //}
            //if (e.NewElement != null)
            //{
            //    Control.Preview = Camera.Open((int)e.NewElement.Camera);

            //    // Subscribe
            //    cameraPreview.Click += OnCameraPreviewClicked;
            //}
        }

        //void OnCameraPreviewClicked(object sender, EventArgs e)
        //{
        //    if (cameraPreview.IsPreviewing)
        //    {
        //        cameraPreview.Preview.StopPreview();
        //        cameraPreview.IsPreviewing = false;
        //    }
        //    else
        //    {
        //        cameraPreview.Preview.StartPreview();
        //        cameraPreview.IsPreviewing = true;
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //    this.Control.Preview.Release();

            base.Dispose(disposing);
        }
    }
}
