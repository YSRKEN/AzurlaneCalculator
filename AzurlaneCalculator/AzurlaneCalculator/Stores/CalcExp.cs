using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			using (var stream = Utility.GetResourceStream("LevelExpList.txt"))
			using (var reader = new StreamReader(stream, Encoding.UTF8))
			{
				string line;
				while ((line = reader.ReadLine()) != null) {
					if (int.TryParse(line, out int exp)) {
						output.Add(exp);
					}
				}
			}
			return output;
		}
		// 海域の経験値情報
		private struct MapExp
		{
			// 海域名
			public string StageName;
			// 小型艦隊の基礎経験値
			public int EnemyExpS;
			// 中型艦隊の基礎経験値
			public int EnemyExpM;
			// 大型艦隊の基礎経験値
			public int EnemyExpL;
			// ボス艦隊の基礎経験値
			public int EnemyExpB;
			// ボス艦隊を出すまでの道中戦闘回数
			public int EnemyCount;
			// 海域1周における平均的な敵経験値
			public int AverageEnemyExp {
				get {
					// 平均的な道中経験値を、
					// ・中型艦隊がいなければ小型艦隊
					// ・大型艦隊がいなければ小型/中型艦隊の平均
					// ・それ以外なら中型艦隊
					// と定義する
					int enemyExp = (
						EnemyExpM <= 0
							? EnemyExpS
							: EnemyExpL <= 0
							? (EnemyExpS + EnemyExpM) / 2
							: EnemyExpM);
					return enemyExp * EnemyCount + EnemyExpB;
				}
			}
		}
		// 海域の経験値表
		private static List<MapExp> mapExpList = ReadMapExpList();
		// 海域の経験値表を初期化する
		private static List<MapExp> ReadMapExpList() {
			// 出力先を用意する
			var output = new List<MapExp>();
			// 1行づつ読み込み、出力先に追記していく
			using (var stream = Utility.GetResourceStream("MapExpList.txt"))
			using (var reader = new StreamReader(stream, Encoding.UTF8)) {
				string line;
				while ((line = reader.ReadLine()) != null) {
					// データ行かを判定する
					string[] temp = line.Split("\t".ToCharArray());
					if (temp[0] == "海域")
						continue;
					// 数値を読み取り、海域の経験値表に追記する
					var mapExp = new MapExp {
						StageName = temp[0],
						EnemyExpS = int.Parse(temp[1]),
						EnemyExpM = int.Parse(temp[2]),
						EnemyExpL = int.Parse(temp[3]),
						EnemyExpB = int.Parse(temp[4]),
						EnemyCount = int.Parse(temp[5]),
					};
					output.Add(mapExp);
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
		// 海域名のリスト
		public static List<string> StageNameList {
			get {
				return mapExpList.Select(m => m.StageName).ToList();
			}
		}
		// ある海域における獲得経験値を取得する
		public static int StageExp(string stageName, string enemyType) {
			// 海域情報を取得する
			var mapInfo = mapExpList.Where(m => m.StageName == stageName).First();
			// 敵の種類によって場合分けする
			switch (enemyType) {
			case "小型":
				return mapInfo.EnemyExpS;
			case "中型":
				return mapInfo.EnemyExpM;
			case "大型":
				return mapInfo.EnemyExpL;
			case "ボス":
				return mapInfo.EnemyExpB;
			case "周回":
				return mapInfo.AverageEnemyExp;
			default:
				return 0;
			}
		}
	}
}
