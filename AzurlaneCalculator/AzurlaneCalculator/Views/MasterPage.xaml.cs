using AzurlaneCalculator.ViewModels;
using Xamarin.Forms;

namespace AzurlaneCalculator.Views
{
	public partial class MasterPage : ContentPage
	{
		public MasterPage()
		{
			InitializeComponent();
			this.BindingContext = new MasterPageViewModel();
		}
	}
}
