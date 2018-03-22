using AzurlaneCalculator.Stores;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace AzurlaneCalculator.ViewModels
{
	class CalcSkillPageViewModel
	{
		// プロパティ
		public ReactiveProperty<int> StartLevel { get; }
			= new ReactiveProperty<int>(1);
		public ReactiveProperty<int> GoalLevel { get; }
			= new ReactiveProperty<int>(2);
		public ReadOnlyReactiveProperty<int> LeaveExp { get; }
		public ReadOnlyReactiveProperty<int> LeaveExpMax { get; }
		public ReactiveProperty<int> LeaveExpRaw { get; }
			= new ReactiveProperty<int>(0);
		public ReactiveProperty<int> BookIndex { get; }
			= new ReactiveProperty<int>(4);
		public ReadOnlyReactiveProperty<string> OutputText { get; }

		// コレクション
		public List<int> StartLevelList { get; }
		public List<int> GoalLevelList { get; }
		public List<string> BookList { get; }

		// コンストラクタ
		public CalcSkillPageViewModel() {
			// コレクションを設定
			{
				StartLevelList = new List<int>();
				for (int i = CalcSkill.MinLevel; i <= CalcSkill.MaxLevel - 1; ++i) {
					StartLevelList.Add(i);
				}
			}
			{
				GoalLevelList = new List<int>();
				for (int i = CalcSkill.MinLevel + 1; i <= CalcSkill.MaxLevel; ++i) {
					GoalLevelList.Add(i);
				}
			}
			BookList = CalcSkill.BookList;
			// ReadOnlyReactivePropertyを設定
			LeaveExp = LeaveExpRaw.Select(x => (x / 50) * 50).ToReadOnlyReactiveProperty();
			LeaveExpMax = StartLevel.Select(x => CalcSkill.LeaveExpMax(x)).ToReadOnlyReactiveProperty();
			OutputText = StartLevel.CombineLatest(
				GoalLevel, LeaveExp, BookIndex, (sl, gl, le, bi) => {
					// 現在の経験値量を求める
					int allExp = CalcSkill.SkillExp(sl) + le;
					// 目標とする経験値量を求める
					int goalExp = CalcSkill.SkillExp(gl);
					// 稼ぎたい経験値量を求める
					int diffExp = goalExp - allExp;
					// 教科書から得られる経験値を求める
					string bookName = BookList[bi];
					var bookInfo = CalcSkill.BookInfoFromName(bookName);
					int bookExp = bookInfo.Exp;
					// 教科書の使用回数を求める
					int useBookCount = (diffExp + bookExp - 1) / bookExp;
					// 結果を返す
					string output = "";
					output += $"必要経験値：{diffExp}";
					output += $"\n取得経験値：{bookExp}";
					output += $"\n必要回数：{useBookCount}";
					decimal time = 1.0M * useBookCount * bookInfo.Hour;
					output += $"\n必要時間：{Math.Round(time, 1)}時間";
					if (time >= 24.0M) {
						time /= 24.0M;
						output += $"\n＝{Math.Round(time, 1)}日";
						if (time >= 30.0M) {
							time /= 30.0M;
							output += $"\n＝{Math.Round(time, 1)}ヶ月";
						}
					}
					return output;
				}
			).ToReadOnlyReactiveProperty();
			// ReactivePropertyを設定
			StartLevel.Subscribe(x => {
				if (GoalLevel.Value <= x)
					GoalLevel.Value = Math.Min(10, x + 1);
			});
			GoalLevel.Subscribe(x => {
				if (StartLevel.Value >= x)
					StartLevel.Value = Math.Max(1, x - 1);
			});
		}
	}
}
