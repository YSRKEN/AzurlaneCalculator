using AzurlaneCalculator.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzurlaneCalculator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
		public MasterPage()
		{
			InitializeComponent();
			this.BindingContext = new MasterPageViewModel();
		}
	}
}
