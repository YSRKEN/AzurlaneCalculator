using System;
using System.Collections.Generic;
using System.Text;

namespace AzurlaneCalculator.Stores
{
	static class CalcProb
	{
		// 計算用メソッド
		//確率Aのベルヌーイ試行にて、1回以上成功する確率がBになる
		//試行回数Xを求める。このXは切り上げ前とする
		public static double CalcBernoulliProb(double a, double b)
			=> Math.Log(1.0 - b) / Math.Log(1.0 - a);
		//範囲[a,b]においてシンプソン積分を行う
		public static double SimpsonMethod(double a, double b, int n, Func<double, double> func) {
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
		public static double BetaFunction(double a, double b) {
			// 内部の関数をラムダ式で定義する
			Func<double, double> func = (double t) =>
				(Math.Pow(t, a - 1.0) * Math.Pow(1.0 - t, b - 1.0));
			// シンプソン積分した結果を返す
			return SimpsonMethod(0.0, 1.0, 100, func);
		}
		//ベータ分布の累積密度関数(BetaCDF(x,a,b))を求める
		public static double BetaCDF(double x, double a, double b) {
			Func<double, double> func = (double t) =>
				(Math.Pow(t, a - 1.0) * Math.Pow(1.0 - t, b - 1.0));
			return SimpsonMethod(0.0, x, 100, func) / BetaFunction(a, b);
		}
		//ベータ分布の累積密度関数の逆関数(BetaCDF^-1(p,a,b))を求める
		public static double InverseBetaCDF(double p, double a, double b) {
			// 前提となる関数を定義する
			Func<double, double> func = (double x) => (BetaCDF(x, a, b) - p);
			// 初期値を設定する
			double x1 = 0.0, f1 = func(x1);
			double x2 = 1.0, f2 = func(x2);
			double x3 = 0.0;
			double eps = 1.0e-6;
			//二分法で探索する
			while (x2 - x1 > eps) {
				x3 = (x1 + x2) / 2;
				double f3 = func(x3);
				if (f1 * f3 < 0.0) {
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
		public static double[] ClopperPearsonMethod(int n, int k, decimal x) {
			decimal a = 1.0M - x;
			double lb = 1.0 - InverseBetaCDF((double)(1.0M - a / 2), n - k + 1, k);
			double ub = 1.0 - InverseBetaCDF((double)(a / 2), n - k, k + 1);
			return new double[] { lb, ub };
		}
		//N回試行してK回成功した際、確率Xと見なす際のp値を求める(Sterneの手法)
		public static double SterneMethod(int n, int k, decimal x) {
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
			for (int i = 0; i <= n; ++i) {
				double temp = prob(n, i, x_);
				if (limitprob >= temp)
					sum += temp;
			}
			return sum;
		}
	}
}
