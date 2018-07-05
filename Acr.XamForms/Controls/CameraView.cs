//using System;
//using System.Windows.Input;
//using Xamarin.Forms;


//namespace Acr.XamForms.Controls
//{
//    public enum CameraLocation
//    {
//        Rear,
//        Front
//    }


//    public enum PhotoImageType
//    {
//        Png,
//        Jpg
//    }


//    public class CameraView : View
//    {
//        // ideas
//        // VideoStream binding so stream can be analyzed
//        // Authorization check
//        // light on/off binding

//        public static readonly BindableProperty IsFeedEnabledProperty = BindableProperty.Create(
//            nameof(IsFeedEnabled),
//            typeof(bool),
//            typeof(CameraView),
//            true,
//            BindingMode.OneWay
//        );
//        /// <summary>
//        /// Setting this to false will freeze the camera stream, true sets the stream to live
//        /// </summary>
//        public bool IsFeedEnabled
//        {
//            get => (bool)this.GetValue(IsFeedEnabledProperty);
//            set => this.SetValue(IsFeedEnabledProperty, value);
//        }


//        public static readonly BindableProperty PictureTakenCommandProperty = BindableProperty.Create(
//            nameof(PictureTakenCommand),
//            typeof(ICommand),
//            typeof(CameraView),
//            null,
//            BindingMode.OneWayToSource
//        );
//        public ICommand PictureTakenCommand
//        {
//            get => (ICommand)this.GetValue(PictureTakenCommandProperty);
//            set => this.SetValue(PictureTakenCommandProperty, value);
//        }


//        public static readonly BindableProperty PhotoTypeProperty = BindableProperty.Create(
//            nameof(PhotoType),
//            typeof(PhotoImageType),
//            typeof(CameraView),
//            PhotoImageType.Png
//        );
//        public PhotoImageType PhotoType
//        {
//            get => (PhotoImageType)this.GetValue(PhotoTypeProperty);
//            set => this.SetValue(PhotoTypeProperty, value);
//        }


//        public static readonly BindableProperty TakePictureCommandProperty = BindableProperty.Create(
//            nameof(TakePictureCommand),
//            typeof(ICommand),
//            typeof(CameraView)
//        );
//        public ICommand TakePictureCommand
//        {
//            get => (ICommand)this.GetValue(TakePictureCommandProperty);
//            set => this.SetValue(TakePictureCommandProperty, value);
//        }


//        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
//            nameof(Camera),
//            typeof(CameraLocation),
//            typeof(CameraView)
//        );
//        public CameraLocation Camera
//        {
//            get => (CameraLocation)this.GetValue(CameraProperty);
//            set => this.SetValue(CameraProperty, value);
//        }
//    }
//}
