using System;
using System.Diagnostics;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace XmlChanger
{
    class Program
    {
        public static string PATH = 
            @"DataConfig\attack_sound.xls";

        public static string PATH_TMP =
            @"DataConfig\attack_sound_tmp.xls";

        public static string ENUM_DIR =
            @"..\Assets\Scripts\Sounds\";

        public static string BAT_PATH =
            @"xlsc_attack_sound.bat";

        private static IWorkbook srcBook;
        private static IWorkbook destBook;
        private static ISheet srcSheet;
        private static ISheet destSheet;

        private const int CONTENT_BEGIN = 2;
        private const int DEST_BEGIN = 5;

        static void Main(string[] args)
        {
            //Directory.SetCurrentDirectory("..\\");

            srcBook = GetSrcBook();
            srcSheet = srcBook.GetSheetAt(0);
            Log("读取文件...done");

            CreateDestBook(srcSheet.SheetName);

            // 生成新的文件
            int rowCount = srcSheet.PhysicalNumberOfRows;
            int destRowNum = DEST_BEGIN;

            for (int i = CONTENT_BEGIN; i < rowCount; i++)
            {
                IRow row = srcSheet.GetRow(i);
                var checkCell = row.GetCell(0);
                if (checkCell == null)
                {
                    continue;
                }
                for (int j = CONTENT_BEGIN; j < row.LastCellNum; j++)
                {
                    ICell cell = row.GetCell(j);

                    IRow destRow = destSheet.CreateRow(destRowNum);
                    ICell destCell = destRow.CreateCell(0);
                    int num = (i - CONTENT_BEGIN + 1) * 100 + (j - CONTENT_BEGIN + 1);
                    destCell.SetCellValue(num);
                    destCell = destRow.CreateCell(1);
                    destCell.SetCellValue(cell.StringCellValue);
                    destRowNum++;
                }
            }
            Log("创建 TMP...done");

            SaveDestBook();
            Log("Save TMP...done");
            
            WriteEnumFile();
            Log("写 enum cs...done");
            Process proc = new Process();
            proc.StartInfo.FileName = BAT_PATH;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
            proc.WaitForExit();

            // delete files
        }

        private static void WriteEnumFile()
        {
            // 写ArmorType
            string armorPath = Path.Combine(ENUM_DIR, "ArmorType.cs");
            var row = srcSheet.GetRow(0);
            using (FileStream fs = new FileStream(armorPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("namespace TheNextMoba.Sounds");
                    sw.WriteLine("{");
                    sw.WriteLine("\tpublic enum ArmorType");
                    sw.WriteLine("\t{");

                    for (int i = CONTENT_BEGIN; i < row.LastCellNum; i++)
                    {
                        var cell = row.GetCell(i);
                        if (cell != null)
                        {
                            sw.WriteLine("\t\t" + cell.StringCellValue + " = " + (i - CONTENT_BEGIN + 1) + ",");
                        }
                    }

                    sw.WriteLine("\t}");
                    sw.WriteLine("}");
                }
            }

            // 写WeaponType
            string weaponPath = Path.Combine(ENUM_DIR, "WeaponType.cs");
            using (FileStream fs = new FileStream(weaponPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("namespace TheNextMoba.Sounds");
                    sw.WriteLine("{");
                    sw.WriteLine("\tpublic enum WeaponType");
                    sw.WriteLine("\t{");

                    for (int i = CONTENT_BEGIN; i < srcSheet.PhysicalNumberOfRows; i++)
                    {
                        row = srcSheet.GetRow(i);
                        var cell = row.GetCell(0);
                        if (cell != null)
                        {
                            sw.WriteLine("\t\t" + cell.StringCellValue + " = " + (i - CONTENT_BEGIN + 1) + ",");
                        }
                    }

                    sw.WriteLine("\t}");
                    sw.WriteLine("}");
                }
            }
        }

        private static void SaveDestBook()
        {
            using (FileStream fs = new FileStream(PATH_TMP, FileMode.Create))
            {
                destBook.Write(fs);
            }
        }

        private static void CreateDestBook(string sheetName)
        {
            destBook = new HSSFWorkbook();
            destSheet = destBook.CreateSheet(sheetName.ToUpper());

            var row = destSheet.CreateRow(0);
            SetCell(row, 0, "optional");
            SetCell(row, 1, "optional");

            row = destSheet.CreateRow(1);
            SetCell(row, 0, "uint32");
            SetCell(row, 1, "string");

            row = destSheet.CreateRow(2);
            SetCell(row, 0, "key");
            SetCell(row, 1, "sound");

            row = destSheet.CreateRow(4);
            SetCell(row, 0, "key");
            SetCell(row, 1, "sound");
        }

        private static void SetCell(IRow row, int col, string value)
        {
            var cell = row.CreateCell(col);
            cell.SetCellValue(value);
        }

        private static IWorkbook GetSrcBook()
        {
            try
            {
                using (FileStream file = new FileStream(PATH, FileMode.Open, FileAccess.Read))
                {
                    return new HSSFWorkbook(file);
                }
            }
            catch (Exception e)
            {
                LogError(@"请确保attack_sound.xls存在且处于关闭状态，详细错误为" + e);
                throw;
            }
        }

        public static void LogError(object msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg.ToString());
        }

        public static void Log(object msg)
        {
            Console.WriteLine(msg.ToString());
        }
    }
}
