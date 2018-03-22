using AzurlaneCalculator.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzurlaneCalculator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalcExpPage : TabbedPage
	{
		public CalcExpPage()
		{
			InitializeComponent();
			this.BindingContext = new CalcExpPageViewModel();
		}
	}
}
