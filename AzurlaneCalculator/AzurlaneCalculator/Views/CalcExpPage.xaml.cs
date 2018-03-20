using AzurlaneCalculator.ViewModels;
using Xamarin.Forms;

namespace AzurlaneCalculator.Views
{
	public partial class CalcExpPage : ContentPage
	{
		public CalcExpPage()
		{
			InitializeComponent();
			this.BindingContext = new CalcExpPageViewModel();
		}
	}
}
