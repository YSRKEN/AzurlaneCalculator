using AzurlaneCalculator.Views;
using Reactive.Bindings;
using System;
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
		public ReactiveProperty<MasterMenuItem> SelectedMenuItem { get; set; }
			= new ReactiveProperty<MasterMenuItem>();
		// ReactiveCollection
		public ReadOnlyReactiveCollection<MasterMenuItem> MenuList { get; }

		// コンストラクタ
		public MasterPageViewModel()
		{
			#region ReactiveCollectionを設定
			// MenuList
			{
				var menuList = new List<MasterMenuItem> {
				new MasterMenuItem {Title = "艦船経験値計算機", TargetType = typeof(CalcExpPage) },
				new MasterMenuItem {Title = "艦船スキル計算機", TargetType = typeof(CalcSkillPage) },
				new MasterMenuItem {Title = "確率計算機", TargetType = typeof(CalcProbPage) },
				new MasterMenuItem {Title = "このアプリについて", TargetType = typeof(AboutPage) },
			};
				var oc = new ObservableCollection<MasterMenuItem>(menuList);
				MenuList = oc.ToReadOnlyReactiveCollection();
			}
			#endregion
		}
	}
	class MasterMenuItem
	{
		public string Title { get; set; }
		public Type TargetType { get; set; }
	}
}
