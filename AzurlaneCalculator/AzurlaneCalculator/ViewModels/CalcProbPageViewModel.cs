using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AzurlaneCalculator.ViewModels
{
	class CalcProbPageViewModel : INotifyPropertyChanged
	{
		#pragma warning disable 0067
		public event PropertyChangedEventHandler PropertyChanged;

		// ReactiveProperty
		public ReactiveProperty<decimal> DropProb { get; }
			= new ReactiveProperty<decimal>(3.0M);
		public ReactiveProperty<int> DropCount { get; }
			= new ReactiveProperty<int>(100);
		public ReadOnlyReactiveProperty<string> Output1 { get; }
		public ReactiveProperty<int> SuccessDropCount { get; }
			= new ReactiveProperty<int>(10);
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
				if (x <= 0) x = 1;
				if (SuccessDropCount.Value > x)
					SuccessDropCount.Value = x;
			});
			SuccessDropCount.Subscribe(x => {
				if (x < 0) x = 0;
				if (DropCount.Value < x)
					DropCount.Value = x;
			});
			// ReadOnlyReactivePropertyを設定

		}
	}
}
