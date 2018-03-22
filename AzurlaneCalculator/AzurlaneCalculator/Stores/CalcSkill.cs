using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzurlaneCalculator.Stores
{
	static class CalcSkill
	{
		// スキルレベルとそれに達するまでの経験値量
		private static List<int> skillExpList = new List<int> {
			0,100,300,700,1500,2900,5100,8300,12700,18500};
		// スキルレベルの上限・下限
		public static int MinLevel = 1;
		public static int MaxLevel = skillExpList.Count;
		// スキルレベルXにおける上限
		public static int LeaveExpMax(int level) {
			int nextLevel = level + 1;
			return skillExpList[nextLevel - 1] - skillExpList[level - 1];
		}
		// スキルレベルXにおける最低の経験値量
		public static int SkillExp(int level)
			=> skillExpList[level - 1];
		// 教科書の情報
		public struct BookInfo
		{
			public string Name;	//教科書名
			public int Hour;	//消費時間
			public int Exp;		//経験値
		}
		// 教科書一覧
		private static List<BookInfo> bookList
			= new List<BookInfo> {
				new BookInfo{ Name = "T1一致", Hour = 2, Exp = 150 },
				new BookInfo{ Name = "T1不一致", Hour = 2, Exp = 100 },
				new BookInfo{ Name = "T2一致", Hour = 4, Exp = 450 },
				new BookInfo{ Name = "T2不一致", Hour = 4, Exp = 300 },
				new BookInfo{ Name = "T3一致", Hour = 8, Exp = 1200 },
				new BookInfo{ Name = "T3不一致", Hour = 8, Exp = 800 },
		};
		// 教科書の名称一覧
		public static List<string> BookList {
			get => bookList.Select(x => x.Name).ToList();
		}
		// X番目の教科書の情報
		public static BookInfo BookInfoFromName(string name)
			=> bookList.First(x => x.Name == name);
	}
}
