using AzurlaneCalculator.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzurlaneCalculator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalcSkillPage : ContentPage
	{
		public CalcSkillPage ()
		{
			InitializeComponent ();
			this.BindingContext = new CalcSkillPageViewModel();
		}
	}
}