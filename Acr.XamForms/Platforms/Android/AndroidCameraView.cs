// copied from https://github.com/xamarin/xamarin-forms-samples/blob/master/CustomRenderers/View/Droid/CameraPreview.cs
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Hardware;
using Android.Runtime;
using Android.Views;


namespace Acr.XamForms.Controls
{
    public sealed class AndroidCameraView : ViewGroup, ISurfaceHolderCallback
    {
        readonly IWindowManager windowManager;
        readonly SurfaceView surfaceView;
        Camera.Size previewSize;
        //IList<CameraLocation.Size> supportedPreviewSizes;
        //CameraLocation camera;



        public AndroidCameraView(Context context) : base(context)
        {
            this.surfaceView = new SurfaceView(context);
            this.AddView(this.surfaceView);
            this.windowManager = context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            this.surfaceView.Holder.AddCallback(this);
        }


        public bool IsPreviewing { get; set; }

        public Camera Preview
        {
            get => this.camera;
            set
            {
                this.camera = value;
                if (this.camera != null)
                {
                    this.supportedPreviewSizes = Preview.GetParameters().SupportedPreviewSizes;
                    this.RequestLayout();
                }
            }
        }



        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = ResolveSize(SuggestedMinimumWidth, widthMeasureSpec);
            int height = ResolveSize(SuggestedMinimumHeight, heightMeasureSpec);
            SetMeasuredDimension(width, height);

            if (supportedPreviewSizes != null)
            {
                previewSize = GetOptimalPreviewSize(supportedPreviewSizes, width, height);
            }
        }


        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            this.surfaceView.Measure(msw, msh);
            this.surfaceView.Layout(0, 0, r - l, b - t);
        }


        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                if (Preview != null)
                {
                    Preview.SetPreviewDisplay(holder);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            if (Preview != null)
            {
                Preview.StopPreview();
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            var parameters = Preview.GetParameters();
            parameters.SetPreviewSize(previewSize.Width, previewSize.Height);
            RequestLayout();

            switch (windowManager.DefaultDisplay.Rotation)
            {
                case SurfaceOrientation.Rotation0:
                    camera.SetDisplayOrientation(90);
                    break;
                case SurfaceOrientation.Rotation90:
                    camera.SetDisplayOrientation(0);
                    break;
                case SurfaceOrientation.Rotation270:
                    camera.SetDisplayOrientation(180);
                    break;
            }

            Preview.SetParameters(parameters);
            Preview.StartPreview();
            IsPreviewing = true;
        }

        CameraLocation.Size GetOptimalPreviewSize(IList<CameraLocation.Size> sizes, int w, int h)
        {
            const double AspectTolerance = 0.1;
            double targetRatio = (double)w / h;

            if (sizes == null)
            {
                return null;
            }

            CameraLocation.Size optimalSize = null;
            double minDiff = double.MaxValue;

            int targetHeight = h;
            foreach (CameraLocation.Size size in sizes)
            {
                double ratio = (double)size.Width / size.Height;

                if (Math.Abs(ratio - targetRatio) > AspectTolerance)
                    continue;
                if (Math.Abs(size.Height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null)
            {
                minDiff = double.MaxValue;
                foreach (CameraLocation.Size size in sizes)
                {
                    if (Math.Abs(size.Height - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }

            return optimalSize;
        }
    }
}