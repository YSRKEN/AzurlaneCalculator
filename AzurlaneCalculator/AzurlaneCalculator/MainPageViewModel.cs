using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace AzurlaneCalculator
{
	class MainPageViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ReadOnlyReactiveCollection<MenuItem> MenuList { get; }

		public MainPageViewModel()
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
