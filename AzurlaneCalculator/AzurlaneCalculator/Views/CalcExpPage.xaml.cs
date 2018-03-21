using AzurlaneCalculator.ViewModels;
using Xamarin.Forms;

namespace AzurlaneCalculator.Views
{
	public partial class CalcExpPage : TabbedPage
	{
		public CalcExpPage()
		{
			InitializeComponent();
			this.BindingContext = new CalcExpPageViewModel();
		}
	}
}
