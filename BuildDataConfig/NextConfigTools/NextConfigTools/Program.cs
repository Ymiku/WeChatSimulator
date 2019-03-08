using System;
using System.IO;
using OfficeOpenXml;

namespace NextConfigTools
{
	class MainClass
	{
		public static void Main (string[] args){
			Console.WriteLine ("Hello World!" );
			HeroDefs.InitPath (args [0]);
//			string heroName = args [1];
//			string skinPath = args [2];
		//	ReadExcel (HeroDefs.Stat_Path);
			ReadExcel (HeroDefs.Ability_Path);
		//	ReadExcel (HeroDefs.ComboAbility_Path);
		//	ReadExcel (HeroDefs.Attack_Path);

		}



		private static void ReadExcel(string path)
		{
			Console.WriteLine ("ReadExcel " + path);
			FileStream fs = new FileStream (path, FileMode.Open);
			ExcelPackage excel = new ExcelPackage ();
			excel.Load (fs);
			//Console.WriteLine ("size=" + exce ().Length);
			ExcelWorkbook workbook = excel.Workbook;
			ExcelWorksheets workSheets = workbook.Worksheets;
			int len = workSheets.Count;
			Console.WriteLine ("ReadExcel " + len);

			for (int i = 0; i < len; i++) {
				ExcelWorksheet sheet = workSheets [i];
				Console.WriteLine (sheet.Name + " " + sheet.Cells.Columns + "-" + sheet.Cells.Rows);
			} 
		}
	}



	public class HeroDefs
	{
		public static string XlsFolder_Path{ get; protected set;}
		public static string ScriptFolder_Path{ get; protected set;}
		public static string NextExcelTools_Path{ get; protected set;}

		public const string Xls = ".xls";
		public static string ScriptExt {get;protected set;}
		public const string ArenaObject = "arena_object";
		public const string Skin = "skin";
		public const string Attack = "attack";
		public const string Stat = "stat";
		public const string ComboAbility = "combo_ability";
		public const string Ability = "ability";


		public static string ArenaObject_Path{ get{ return XlsFolder_Path + ArenaObject+Xls;}}
		public static string Skin_Path{ get{ return XlsFolder_Path + Skin+Xls;}}
		public static string Stat_Path{ get{ return XlsFolder_Path + Stat+Xls;}}
		public static string Attack_Path{get{ return XlsFolder_Path + Attack+Xls;}}
		public static string ComboAbility_Path{ get{ return XlsFolder_Path + ComboAbility+Xls;}}
		public static string Ability_Path{ get{ return XlsFolder_Path + Ability+Xls;}}


		public static void InitPath(string dataPath){
			DirectoryInfo assetDir = new DirectoryInfo (dataPath);
			ScriptFolder_Path = assetDir.Parent.FullName + Path.DirectorySeparatorChar + "BuildDataConfig" + Path.DirectorySeparatorChar;
			XlsFolder_Path = ScriptFolder_Path + "DataConfig" + Path.DirectorySeparatorChar;
			NextExcelTools_Path = ScriptFolder_Path + "NextConfigTools" + Path.DirectorySeparatorChar + "Release" + Path.DirectorySeparatorChar + "NextConfigTools.exe";

			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;
			switch (pid) {
			case PlatformID.Unix:
				ScriptExt = ".py";
				break;
			default:
				ScriptExt = ".bat";
				break;
			}
		}


	}
}
