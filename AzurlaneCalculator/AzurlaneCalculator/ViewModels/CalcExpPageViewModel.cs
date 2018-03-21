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

		// ReactiveProperty
		public ReactiveProperty<int> StartLevel { get; }
			= new ReactiveProperty<int>(1);
		public ReactiveProperty<int> GoalLevel { get; }
			= new ReactiveProperty<int>(1);
		public ReadOnlyReactiveProperty<string> OutputText { get; }
		public ReactiveProperty<string> StageName { get; }
			= new ReactiveProperty<string>("1-1");
		public ReactiveProperty<string> EnemyType { get; }
			= new ReactiveProperty<string>("周回");
		// コレクション
		public List<int> LevelList { get; }
		public List<string> StageNameList { get; }
		public List<string> EnemyTypeList { get; }
			= new List<string> { "小型", "中型", "大型", "ボス", "周回" };

		// コンストラクタ
		public CalcExpPageViewModel()
		{
			// コレクションを設定
			{
				LevelList = new List<int>();
				for(int i = CalcExp.MinLevel; i <= CalcExp.MaxLevel; ++i)
				{
					LevelList.Add(i);
				}
			}
			{
				StageNameList = CalcExp.StageNameList;
			}
			// ReadOnlyReactivePropertyを設定
			OutputText = StartLevel.CombineLatest(
				GoalLevel, StageName, EnemyType, (sl, gl, sn, et) => {
					string output = "";
					int startExp = CalcExp.LevelExp(sl);
					int goalExp = CalcExp.LevelExp(gl);
					output += $"必要経験値：{goalExp - startExp}";
					int wantExp = goalExp - startExp;
					int getExp = CalcExp.StageExp(sn, et);
					if (getExp != 0) {
						output += $"\n取得経験値：{getExp}";
						output += $"\n必要回数：{Math.Ceiling(1.0 * wantExp / getExp)}";
					}
					else {
						output += "\n取得経験値：―";
						output += "\n必要回数：―";
					}
					return output;
				}
			).ToReadOnlyReactiveProperty();
		}
	}
}
