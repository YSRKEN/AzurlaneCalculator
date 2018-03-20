using Xamarin.Forms;

namespace AzurlaneCalculator
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
