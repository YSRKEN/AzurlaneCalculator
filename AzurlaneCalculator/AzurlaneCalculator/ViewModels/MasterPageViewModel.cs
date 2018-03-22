using AzurlaneCalculator.Models;
using AzurlaneCalculator.Views;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AzurlaneCalculator.ViewModels
{
	class MasterPageViewModel : INotifyPropertyChanged
	{
		#pragma warning disable 0067
		public event PropertyChangedEventHandler PropertyChanged;

		// ReactiveProperty
		public ReactiveProperty<MenuItem> SelectedMenuItem { get; set; }
			= new ReactiveProperty<MenuItem>();
		// ReactiveCollection
		public ReadOnlyReactiveCollection<MenuItem> MenuList { get; }

		// コンストラクタ
		public MasterPageViewModel()
		{
			#region ReactiveCollectionを設定
			// MenuList
			{
				var menuList = new List<MenuItem> {
				new MenuItem {Title = "経験値計算機", TargetType = typeof(CalcExpPage) },
				new MenuItem {Title = "スキル計算機", TargetType = typeof(CalcSkillPage) },
				new MenuItem {Title = "このアプリについて", TargetType = typeof(AboutPage) },
			};
				var oc = new ObservableCollection<MenuItem>(menuList);
				MenuList = oc.ToReadOnlyReactiveCollection();
			}
			#endregion
		}
	}
}
