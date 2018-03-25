using System;
using Xamarin.Forms;
using AzurlaneCalculator.ViewModels;
using Xamarin.Forms.Xaml;

namespace AzurlaneCalculator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : MasterDetailPage
	{
		public DetailPage()
		{
			InitializeComponent();
			// 選択を切り替えた際の動きを記述する
			var masterPage = this.Master as MasterPage;
			var masterPageViewModel = masterPage.BindingContext as MasterPageViewModel;
			masterPageViewModel.SelectedMenuItem.Subscribe(item => {
				// アイテムがnullでなければ、それに合わせて表示するページを切り替える
				if (item != null) {
					// itemに登録したTargetTypeから表示ページのインスタンスを作成し、代入する
					Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
					// 選択を解除する
					masterPageViewModel.SelectedMenuItem.Value = null;
					// 選択ページを引っ込める
					IsPresented = false;
				}
			});
		}
	}
}
