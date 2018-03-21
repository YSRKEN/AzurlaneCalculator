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

		public ReactiveProperty<string> StageName { get; }
			= new ReactiveProperty<string>("1-1");
		public ReactiveProperty<string> EnemyType { get; }
			= new ReactiveProperty<string>("周回");
		public ReactiveProperty<bool> LeaderFlg { get; }
			= new ReactiveProperty<bool>(false);
		public ReactiveProperty<bool> MvpFlg { get; }
			= new ReactiveProperty<bool>(false);
		public ReactiveProperty<bool> CondFlg { get; }
			= new ReactiveProperty<bool>(false);
		public ReactiveProperty<bool> RankSFlg { get; }
			= new ReactiveProperty<bool>(true);
		public ReadOnlyReactiveProperty<string> OutputText { get; }

		public ReactiveProperty<int> AdmiralLevel { get; }
			= new ReactiveProperty<int>(80);
		public ReactiveProperty<int> FleetCount { get; }
			= new ReactiveProperty<int>(5);
		public ReactiveProperty<int> RoomCond { get; }
			= new ReactiveProperty<int>(100);
		public ReactiveProperty<int> RoomBoost { get; }
			= new ReactiveProperty<int>(0);
		public ReadOnlyReactiveProperty<string> OutputText2 { get; }

		public ReactiveProperty<string> LongJob { get; }
			= new ReactiveProperty<string>("―");
		//XAMLのTextに「\n」が使えないとかクソすぎでは？
		public ReactiveProperty<string> ExtraOptionString { get; }
			= new ReactiveProperty<string>("寮舎経験値に\n長時間遠征の\n経験値を\n加算する");

		// コレクション
		public List<int> LevelList { get; }
		public List<string> StageNameList { get; }
		public List<string> EnemyTypeList { get; }
			= new List<string> { "小型", "中型", "大型", "ボス", "周回" };

		public List<int> AdmiralLevelList { get; }
		public List<int> FleetCountList { get; }
			= new List<int> { 1, 2, 3, 4, 5 };
		public List<int> RoomCondList { get; }
		public List<int> RoomBoostList { get; }
			= new List<int> { 0, 5, 10, 15, 20, 25, 30, 35 };

		public List<string> LongJobList { get; }
			= new List<string> { "―", "初級", "中級", "上級" };

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
			{
				AdmiralLevelList = new List<int>();
				for (int i = CalcExp.MinAdmiralLevel; i <= CalcExp.MaxAdmiralLevel; ++i) {
					AdmiralLevelList.Add(i);
				}
			}
			{
				RoomCondList = new List<int>();
				for (int i = CalcExp.MinRoomCond; i <= CalcExp.MaxRoomCond; ++i) {
					RoomCondList.Add(i);
				}
			}
			// ReadOnlyReactivePropertyを設定
			OutputText = StartLevel.CombineLatest(
				GoalLevel, StageName, EnemyType, LeaderFlg, MvpFlg,
				CondFlg, RankSFlg, (sl, gl, sn, et, lf, mf, cf, sf) => {
					string output = "";
					int startExp = CalcExp.LevelExp(sl);
					int goalExp = CalcExp.LevelExp(gl);
					output += $"必要経験値：{goalExp - startExp}";
					int wantExp = goalExp - startExp;
					int getExp = CalcExp.StageExp(sn, et);
					getExp = (int)(getExp * CalcExp.StageExpBoost(lf, mf, cf, sf));
					if (getExp > 0) {
						output += $"\n取得経験値：{getExp}";
						output += $"\n必要回数：{Math.Ceiling(1.0 * wantExp / getExp)}";
					}
					else if(getExp == 0){
						output += "\n取得経験値：―";
						output += "\n必要回数：―";
					}
					else {
						output += "\n取得経験値：不明";
						output += "\n必要回数：―";
					}
					return output;
				}
			).ToReadOnlyReactiveProperty();
			OutputText2 = StartLevel.CombineLatest(
				GoalLevel, AdmiralLevel, FleetCount, RoomCond,
				RoomBoost,　LongJob, (sl, gl, al, fc, rc, rb, lj) => {
					string output = "";
					int startExp = CalcExp.LevelExp(sl);
					int goalExp = CalcExp.LevelExp(gl);
					output += $"必要経験値：{goalExp - startExp}";
					int wantExp = goalExp - startExp;
					decimal getExp = CalcExp.RoomExp(al);
					getExp = (int)(getExp * CalcExp.RoomExpBoost(fc, rc, rb));
					getExp += CalcExp.JobExp(lj);
					if (getExp > 0) {
						output += $"\n取得経験値：{Math.Round(getExp, 1)}";
						decimal time = 1.0M * wantExp / getExp;
						output += $"\n必要時間：{Math.Round(time, 1)}時間";
						if(time >= 24.0M) {
							time /= 24.0M;
							output += $"\n＝{Math.Round(time, 1)}日";
							if(time >= 30.0M) {
								time /= 30.0M;
								output += $"\n＝{Math.Round(time, 1)}ヶ月";
							}
						}
					}
					else if (getExp == 0) {
						output += "\n取得経験値：―";
						output += "\n必要時間：―";
					}
					else {
						output += "\n取得経験値：不明";
						output += "\n必要回数：―";
					}
					return output;
				}
			).ToReadOnlyReactiveProperty();
		}
	}
}
