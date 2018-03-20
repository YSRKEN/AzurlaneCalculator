using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AzurlaneCalculator.Stores
{
	static class CalcExp
	{
		// レベルに応じた経験値表
		private static List<int> levelExpList = ReadLevelExpList();
		// 経験値表を初期化する
		private static List<int> ReadLevelExpList(){
			// 出力先を用意する
			var output = new List<int>();
			// 1行づつ読み込み、出力先に追記していく
			using (var stream = Utility.GetResourceStream("AzurlaneCalculator", "Resources.LevelExpList.txt"))
			using (var reader = new StreamReader(stream, Encoding.UTF8))
			{
				string line;
				while ((line = reader.ReadLine()) != null) {
					int exp;
					if(int.TryParse(line, out exp)) {
						output.Add(exp);
					}
				}
			}
			return output;
		}

		// 最大レベル・最小レベル
		public static int MinLevel { get; } = 1;
		public static int MaxLevel { get; } = levelExpList.Count;
		// あるレベルにおける総経験値を取得する
		public static int LevelExp(int level)
		{
			
			if (level < MinLevel || level > MaxLevel)
				return -1;
			return levelExpList[level - 1];
		}
	}
}
