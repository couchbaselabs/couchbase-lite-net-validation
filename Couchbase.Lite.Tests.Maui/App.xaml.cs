﻿using Application = Microsoft.Maui.Controls.Application;

namespace Couchbase.Lite.Tests.Maui
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new ContentPage();
		}
    }
}
