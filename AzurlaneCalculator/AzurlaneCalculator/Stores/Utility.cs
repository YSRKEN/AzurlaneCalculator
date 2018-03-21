using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AzurlaneCalculator.Stores
{
	static class Utility
	{
		// リソースのStreamをファイル名から取得する
		public static Stream GetResourceStream(string name) {
			// リフレクションにより、アセンブリ情報を取得する
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();
			// アセンブリ情報から、リソース名の一覧を取得する
			string[] resnames = assembly.GetManifestResourceNames();
			// リソース名の一覧を検索し、条件に合ったものを返す
			var regex = new Regex($"Resources.{name}$");
			foreach(string resname in resnames) {
				if (regex.IsMatch(resname)) {
					return assembly.GetManifestResourceStream(resname);
				}
			}
			return null;
		}
	}
}
