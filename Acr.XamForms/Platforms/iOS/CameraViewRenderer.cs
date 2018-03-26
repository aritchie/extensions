﻿using System;
using Acr.XamForms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace Acr.XamForms.Controls
{
    public class CameraViewRenderer : ViewRenderer<CameraView, UICameraPreview>
    {
    }
}
/*
using System;
using CustomRenderer;
using CustomRenderer.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace CustomRenderer.iOS
{
	public class CameraPreviewRenderer : ViewRenderer<CameraPreview, UICameraPreview>
	{
		UICameraPreview uiCameraPreview;

		protected override void OnElementChanged (ElementChangedEventArgs<CameraPreview> e)
		{
			base.OnElementChanged (e);

			if (Control == null) {
				uiCameraPreview = new UICameraPreview (e.NewElement.Camera);
				SetNativeControl (uiCameraPreview);
			}
			if (e.OldElement != null) {
				// Unsubscribe
				uiCameraPreview.Tapped -= OnCameraPreviewTapped;
			}
			if (e.NewElement != null) {
				// Subscribe
				uiCameraPreview.Tapped += OnCameraPreviewTapped;
			}
		}

		void OnCameraPreviewTapped (object sender, EventArgs e)
		{
			if (uiCameraPreview.IsPreviewing) {
				uiCameraPreview.CaptureSession.StopRunning ();
				uiCameraPreview.IsPreviewing = false;
			} else {
				uiCameraPreview.CaptureSession.StartRunning ();
				uiCameraPreview.IsPreviewing = true;
			}
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				Control.CaptureSession.Dispose ();
				Control.Dispose ();
			}
			base.Dispose (disposing);
		}
	}
}
 */
