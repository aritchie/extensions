//using System;
//using Foundation;
//using UIKit;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.iOS;
//using Acr.XamForms.Controls;


//[assembly: ExportRenderer(typeof(Button), typeof(ButtonDisabledTextColorRenderer))]
//namespace Acr.XamForms.Controls
//{
//    [Preserve(AllMembers = true)]
//    public class ButtonDisabledTextColorRenderer : ButtonRenderer
//    {
//        static UIColor DisabledColor => Color.FromHex("#faebd7").ToUIColor();


//        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);
//            if (e.PropertyName == nameof(Button.IsEnabled))
//                this.Element.Opacity = this.Element.IsEnabled ? 1 : .5;
//        }


//        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
//        {
//            base.OnElementChanged(e);
//            if (this.Control == null)
//                return;

//            var title = new NSAttributedString(Control.CurrentTitle, new UIStringAttributes { ForegroundColor = DisabledColor });
//            this.Control.SetAttributedTitle(title, UIControlState.Disabled);
//        }
//    }
//}
