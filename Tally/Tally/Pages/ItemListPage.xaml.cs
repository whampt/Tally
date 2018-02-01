using FreshMvvm;
using System;
using Tally.PageModels;
using Xamarin.Forms.Xaml;

namespace Tally.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemListPage : FreshBaseContentPage
	{
		public ItemListPage ()
		{
			InitializeComponent ();
		}

        //private async void ClearClicked(object sender, EventArgs e)
        //{
        //    //why is this so damn hard?
        //    //await ItemListPageModel.DeleteAll();
        //}
    }
}