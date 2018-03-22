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
		//範囲[a,b]においてシンプソン積分を行う
		private double SimpsonMethod(double a, double b, int n, Func<double, double> func) {
			// 分割幅hを求める
			double h = (b - a) / n;
			// ヘルパーメソッド
			Func<int, double> index = (int i) => a + i * h;
			// 計算を行う
			double sum = func(a) + func(b);
			for (int j = 1; j < n / 2; ++j) {
				sum += 2.0 * func(index(2 * j));
			}
			for (int j = 1; j <= n / 2; ++j) {
				sum += 4.0 * func(index(2 * j - 1));
			}
			sum *= h / 3;
			return sum;
		}
		//ベータ関数B(a,b)の値を求める
		private double BetaFunction(double a, double b) {
			// 内部の関数をラムダ式で定義する
			Func<double, double> func = (double t) =>
				(Math.Pow(t, a - 1.0) * Math.Pow(1.0 - t, b - 1.0));
			// シンプソン積分した結果を返す
			return SimpsonMethod(0.0, 1.0, 100, func);
		}
		//ベータ分布の累積密度関数(BetaCDF(x,a,b))を求める
		private double BetaCDF(double x, double a, double b) {
			Func<double, double> func = (double t) =>
				(Math.Pow(t, a - 1.0) * Math.Pow(1.0 - t, b - 1.0));
			return SimpsonMethod(0.0, x, 100, func) / BetaFunction(a, b);
		}
		//ベータ分布の累積密度関数の逆関数(BetaCDF^-1(p,a,b))を求める
		private double InverseBetaCDF(double p, double a, double b) {
			// 前提となる関数を定義する
			Func<double, double> func = (double x) => (BetaCDF(x, a, b) - p);
			// 初期値を設定する
			double x1 = 0.0, f1 = func(x1);
			double x2 = 1.0, f2 = func(x2);
			double x3 = 0.0;
			double eps = 1.0e-6;
			//二分法で探索する
			while(x2 - x1 > eps) {
				x3 = (x1 + x2) / 2;
				double f3 = func(x3);
				if(f1 * f3 < 0.0) {
					x2 = x3;
				}
				else {
					x1 = x3;
				}
			}
			// 結果を返す
			return x3;
		}
		//N回試行してK回成功した際、二項分布のX％信頼区間を求める(Clopper-Pearson method)
		private double[] ClopperPearsonMethod(int n, int k, decimal x) {
			decimal a = 1.0M - x;
			double lb = 1.0 - InverseBetaCDF((double)(1.0M - a / 2), n - k + 1, k);
			double ub = 1.0 - InverseBetaCDF((double)(a / 2), n - k, k + 1);
			return new double[] { lb, ub };
		}
		//N回試行してK回成功した際、確率Xと見なす際のp値を求める(Sterneの手法)
		private double SterneMethod(int n, int k, decimal x) {
			// 組み合わせaCbを実数で求める(※精度を犠牲にして楽な実装を取った)
			Func<int, int, double> comb = (int a, int b) => {
				double result = 1.0;
				for (int num1 = a, num2 = b, r = 0; r < b; --num1, --num2, ++r) {
					result *= 1.0 * num1;
					result /= 1.0 * num2;
				}
				return result;
			};
			// ベルヌーイ試行の確率を求める関数
			Func<int, int, double, double> prob = (int a, int b, double s) => {
				double result = comb(a, b);
				for (int i = 0; i < b; ++i)
					result *= s;
				for (int i = 0; i < a - b; ++i)
					result *= (1.0 - s);
				return result;
			};
			// 地道に計算する
			double sum = 0.0;
			double x_ = (double)x / 100.0;
			double limitprob = prob(n, k, x_);
			sum += limitprob;
			for (int i = 0; i < k; ++i) {
				sum += prob(n, i, x_);
			}
			for(int i = n; i >= 0; --i) {
				double temp = prob(n, i, x_);
				if (limitprob <= temp)
					break;
				sum += temp;
			}
			return sum;
		}

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
			Output2 = DropCount2.CombineLatest(
				SuccessDropCount, DropProb2, (all, success, prob) => {
					string output = "";
					output += $"成功確率：{Math.Round(100.0M * success / all, 1)}％";
					double[] ci = ClopperPearsonMethod(all, success, 0.95M);
					output += $"\n95％信頼区間：{Math.Round(100.0 * ci[0], 1)}～{Math.Round(100.0 * ci[1], 1)}％";
					output += $"\np値：{Math.Round(SterneMethod(all, success, prob), 6)}";
					return output;
				}
			).ToReadOnlyReactiveProperty();
		}
	}
}
