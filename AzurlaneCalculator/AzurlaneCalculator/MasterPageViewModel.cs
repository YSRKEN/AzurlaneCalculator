using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace AzurlaneCalculator
{
	class MasterPageViewModel : INotifyPropertyChanged
	{
#pragma warning disable 0067
		public event PropertyChangedEventHandler PropertyChanged;

		public ReadOnlyReactiveCollection<MenuItem> MenuList { get; }
		public ReactiveProperty<MenuItem> SelectedMenuItem { get; set; }
		 = new ReactiveProperty<MenuItem>();

		public MasterPageViewModel()
		{
			var menuList = new List<MenuItem> {
				new MenuItem {Title = "経験値計算機", TargetType = typeof(MainPage) },
				new MenuItem {Title = "このアプリについて", TargetType = typeof(MainPage) },
			};
			var oc = new ObservableCollection<MenuItem>(menuList);
			MenuList = oc.ToReadOnlyReactiveCollection();
		}
	}
}
