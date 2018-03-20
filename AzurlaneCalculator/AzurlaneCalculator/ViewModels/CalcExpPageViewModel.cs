using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Reactive.Linq;
using System.Linq;
using AzurlaneCalculator.Stores;

namespace AzurlaneCalculator.ViewModels
{
	class CalcExpPageViewModel : INotifyPropertyChanged
	{
		#pragma warning disable 0067
		public event PropertyChangedEventHandler PropertyChanged;

		// レベル一覧
		private List<int> levelList;

		// 必要経験値を計算する
		private string ExpLevelToLevelStr()
		{
			// レベルを取得
			int startLevel = levelList[StartLevelIndex.Value];
			int goalLevel = levelList[GoalLevelIndex.Value];
			// 入力チェック
			if (startLevel < CalcExp.MinLevel || startLevel > CalcExp.MaxLevel)
				return "<None>";
			if (goalLevel < CalcExp.MinLevel || goalLevel > CalcExp.MaxLevel)
				return "<None>";
			// 結果を返す
			return $"{CalcExp.LevelExp(goalLevel) - CalcExp.LevelExp(startLevel)}";
		}

		// ReactiveProperty
		public ReactiveProperty<int> StartLevelIndex { get; }
			= new ReactiveProperty<int>(0);
		public ReactiveProperty<int> GoalLevelIndex { get; }
			= new ReactiveProperty<int>(0);
		public ReadOnlyReactiveProperty<string> OutputText { get; }
		// ReactiveCollection
		public ReadOnlyReactiveCollection<string> LevelList { get; }

		// コンストラクタ
		public CalcExpPageViewModel()
		{
			// ReactiveCollectionを設定
			{
				levelList = new List<int>();
				for(int i = CalcExp.MinLevel; i <= CalcExp.MaxLevel; ++i)
				{
					levelList.Add(i);
				}
				var oc = new ObservableCollection<string>(levelList.Select(x => $"{x}").ToList());
				LevelList = oc.ToReadOnlyReactiveCollection();
			}
			// ReadOnlyReactivePropertyを設定
			OutputText = StartLevelIndex.CombineLatest(
				GoalLevelIndex,
				(s, g) => $"必要経験値：{ExpLevelToLevelStr()}"
			).ToReadOnlyReactiveProperty();
		}
	}
}
