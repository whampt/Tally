using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tally.PageModels;
using Xamarin.Forms;

namespace Tally
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            var page = FreshPageModelResolver.ResolvePageModel<ItemListPageModel>();
            var navContainer = new FreshNavigationContainer(page);
            MainPage = navContainer;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
