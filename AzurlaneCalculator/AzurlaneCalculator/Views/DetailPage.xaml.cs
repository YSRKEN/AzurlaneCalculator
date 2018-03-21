using System;
using Xamarin.Forms;
using AzurlaneCalculator.ViewModels;

namespace AzurlaneCalculator.Views
{
	public partial class DetailPage : MasterDetailPage
	{
		public DetailPage()
		{
			InitializeComponent();
			// 選択を切り替えた際の動きを記述する
			var masterPage = this.Master as MasterPage;
			var masterPageViewModel = masterPage.BindingContext as MasterPageViewModel;
			masterPageViewModel.SelectedMenuItem.PropertyChanged += (s, a) => {
				// MasterPageのViewModelで何かが変化した際の動作
				// まず選択されているアイテムを取得する
				var item = masterPageViewModel.SelectedMenuItem.Value;
				// アイテムがnullでなければ、それに合わせて表示するページを切り替える
				if (item != null)
				{
					// itemに登録したTargetTypeから表示ページのインスタンスを作成し、代入する
					Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
					// 選択を解除する
					masterPageViewModel.SelectedMenuItem.Value = null;
					// 選択ページを引っ込める
					IsPresented = false;
				}
			};
		}
	}
}
