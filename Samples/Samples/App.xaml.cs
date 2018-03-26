using System;
using Xamarin.Forms;


namespace Samples
{
	public partial class App : Application
	{
		public App ()
		{
			this.InitializeComponent();
            this.MainPage = new Samples.MainPage();
		}
	}
}
