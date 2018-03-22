using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Text;

namespace AzurlaneCalculator.ViewModels
{
	class CalcProbPageViewModel : INotifyPropertyChanged
	{
		#pragma warning disable 0067
		public event PropertyChangedEventHandler PropertyChanged;

		// 計算用メソッド
		//確率Aのベルヌーイ試行にて、1回以上成功する確率がBになる
		//試行回数Xを求める。このXは切り上げ前とする
		private double CalcBernoulliProb(double a, double b)
			=> Math.Log(1.0 - b) / Math.Log(1.0 - a);

		// ReactiveProperty
		public ReactiveProperty<decimal> DropProb { get; }
			= new ReactiveProperty<decimal>(3.0M);
		public ReactiveProperty<int> DropCount { get; }
			= new ReactiveProperty<int>(100);
		public ReadOnlyReactiveProperty<string> Output1 { get; }
		public ReactiveProperty<int> DropCount2 { get; }
			= new ReactiveProperty<int>(100);
		public ReactiveProperty<int> SuccessDropCount { get; }
			= new ReactiveProperty<int>(10);
		public ReactiveProperty<decimal> DropProb2 { get; }
			= new ReactiveProperty<decimal>(3.0M);
		public ReadOnlyReactiveProperty<string> Output2 { get; }

		// ReactiveCommand
		public ReactiveCommand DropCountSub10Command { get; } = new ReactiveCommand();
		public ReactiveCommand DropCountSub1Command { get; } = new ReactiveCommand();
		public ReactiveCommand DropCountAdd1Command { get; } = new ReactiveCommand();
		public ReactiveCommand DropCountAdd10Command { get; } = new ReactiveCommand();

		// コンストラクタ
		public CalcProbPageViewModel() {
			// ReactiveCommandを設定
			DropCountSub10Command.Subscribe(_ => {
				DropCount.Value = Math.Max(1, DropCount.Value - 10);
			});
			DropCountSub1Command.Subscribe(_ => {
				DropCount.Value = Math.Max(1, DropCount.Value - 1);
			});
			DropCountAdd1Command.Subscribe(_ => {
				DropCount.Value += 1;
			});
			DropCountAdd10Command.Subscribe(_ => {
				DropCount.Value += 10;
			});
			// ReactivePropertyを設定
			DropProb.Subscribe(x => {
				if (x > 100M) DropProb.Value = 100M;
				if (x < 0M) DropProb.Value = 0M;
			});
			DropCount.Subscribe(x => {
				if (x <= 0) DropCount.Value = 1;
			});
			DropCount2.Subscribe(x => {
				if (x <= 0) DropCount2.Value = 1;
				if (SuccessDropCount.Value > x)
					SuccessDropCount.Value = x;
			});
			SuccessDropCount.Subscribe(x => {
				if (x < 0) SuccessDropCount.Value = 0;
				if (DropCount2.Value < x)
					DropCount2.Value = x;
			});
			DropProb2.Subscribe(x => {
				if (x > 100M) DropProb2.Value = 100M;
				if (x < 0M) DropProb2.Value = 0M;
			});
			// ReadOnlyReactivePropertyを設定
			Output1 = DropProb.CombineLatest(DropCount, (prob, count) => {
				string output = "";
				if (prob == 0.0M) {
					// 特殊処理
					output += "1回以上成功：0％";
					output += "\n全て失敗：：100％";
					output += "\n50,70,90,95,99％成功：";
					output += "\n∞,∞,∞,∞,∞回";
					return output;
				}
				double prob_ = (double)prob / 100;	//％を通常の確率に変換
				//
				double anySuccessPer = (1.0 - Math.Pow(1.0 - prob_, count)) * 100;
				output += $"1回以上成功：{Math.Round(anySuccessPer, 1)}％";
				double allFailPer = Math.Pow(1.0 - prob_, count) * 100;
				output += $"\n全て失敗：{Math.Round(allFailPer, 1)}％";
				output += $"\n50,70,90,95,99％成功：\n";
				double per50Count = Math.Ceiling(CalcBernoulliProb(prob_, 0.5));
				double per70Count = Math.Ceiling(CalcBernoulliProb(prob_, 0.7));
				double per90Count = Math.Ceiling(CalcBernoulliProb(prob_, 0.9));
				double per95Count = Math.Ceiling(CalcBernoulliProb(prob_, 0.95));
				double per99Count = Math.Ceiling(CalcBernoulliProb(prob_, 0.99));
				output += $"{per50Count},";
				output += $"{per70Count},";
				output += $"{per90Count},";
				output += $"{per95Count},";
				output += $"{per99Count}回";
				//
				return output;
			}).ToReadOnlyReactiveProperty();
		}
	}
}
