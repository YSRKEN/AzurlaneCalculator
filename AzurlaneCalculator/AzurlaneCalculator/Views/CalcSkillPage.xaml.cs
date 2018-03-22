using AzurlaneCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzurlaneCalculator.Views
{
	public partial class CalcSkillPage : ContentPage
	{
		public CalcSkillPage ()
		{
			InitializeComponent ();
			this.BindingContext = new CalcSkillPageViewModel();
		}
	}
}