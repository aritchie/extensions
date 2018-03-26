﻿using System;
using System.Windows.Input;
using Xamarin.Forms;


namespace Acr.XamForms.Controls
{
    public class CameraView : View
    {
        // ideas
        // VideoStream binding so stream can be analyzed
        // Authorization check
        // light on/off binding

        public static readonly BindableProperty IsFeedEnabledProperty = BindableProperty.Create(
            nameof(IsFeedEnabled),
            typeof(bool),
            typeof(CameraView),
            null,
            BindingMode.OneWayToSource
        );
        public bool IsFeedEnabled
        {
            get => (bool)this.GetValue(IsFeedEnabledProperty);
            set => this.SetValue(IsFeedEnabledProperty, value);
        }


        public static readonly BindableProperty PhotoFilePathProperty = BindableProperty.Create(
            nameof(PhotoFilePath),
            typeof(string),
            typeof(CameraView),
            null,
            BindingMode.OneWayToSource
        );
        public string PhotoFilePath
        {
            get => (string)this.GetValue(PhotoFilePathProperty);
            set => this.SetValue(PhotoFilePathProperty, value);
        }


        public static readonly BindableProperty TakePictureCommandProperty = BindableProperty.Create(
            nameof(TakePictureCommand),
            typeof(ICommand),
            typeof(CameraView)
        );
        public ICommand TakePictureCommand
        {
            get => (ICommand)this.GetValue(TakePictureCommandProperty);
            set => this.SetValue(TakePictureCommandProperty, value);
        }


        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            nameof(Camera),
            typeof(CameraLocation),
            typeof(CameraView)
        );
        public CameraLocation Camera
        {
            get => (CameraLocation)this.GetValue(CameraProperty);
            set => this.SetValue(CameraProperty, value);
        }
    }
}
