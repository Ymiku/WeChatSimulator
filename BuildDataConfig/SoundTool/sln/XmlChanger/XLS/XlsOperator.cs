using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System;

//=== exapmle====

// XlsOperator oer = new XlsOperator();
// oer.Open(Application.dataPath + "/ability.xls");
// int rowNum = oer.AddNewRow();
// if (rowNum >=0)
// {
//     oer.SetCellValue(rowNum, "id", 4001);
//     oer.SetCellValue(rowNum, "prefab_path", "4001_test");
// }
// oer.Save();

public class SheetInfo
{
    public ISheet Sheet;
    public Dictionary<string, int> DicColumnIndex = new Dictionary<string,int>();
}

public class XlsOperator
{
    private string m_path;

    private Dictionary<string, SheetInfo> m_dicSheet = new Dictionary<string,SheetInfo>();
    private string m_firstSheetName;

    private IWorkbook m_curBook;

    private const int ROW_BEGIN = 0;
    private const int COL_BEGIN = 2;

    public class Debug
    {
        public static void Log(object msg)
        {
            Console.WriteLine(msg.ToString());
        }

        public static void LogError(object msg)
        {
            Console.WriteLine("ERR: " + msg.ToString());
        }
    }

    public bool Open(string path)
    {
        Debug.Log("打开xls:" + path);

        if (File.Exists(path) == false)
        {
            Debug.LogError("xls文件不在存:" + path);
            return false;
        }

        //paser the xls
        FileStream fs = null;
        try
        {
            fs = File.Open(path, FileMode.Open, FileAccess.Read);
        }
        catch(Exception e)
        {
            Debug.LogError("读取xls文件发生错误：" + e.Message);
            return false;
        }

        if(fs!=null)
        {
            m_path = path;

            try
            {
                m_curBook = new HSSFWorkbook(fs);
            }
            catch(Exception e)
            {
                Debug.LogError("解析xls文件发生错误：" + e.Message);
                return false;
            }

            if (m_curBook == null)
            {
                Debug.LogError("xls加载失败:" + path);
            }

            if (m_curBook.NumberOfSheets == 0)
            {
                Debug.LogError("xls内容为空:" + path);
                m_curBook = null;
            }

            for (int i = 0; i < m_curBook.NumberOfSheets; ++i)
            {
                ISheet s = m_curBook.GetSheetAt(i);
                IRow row = s.GetRow(ROW_BEGIN);
                if (row == null)
                {
                    continue;
                }

                Debug.Log("解析sheet:" + s.SheetName + " 总行数:" + s.PhysicalNumberOfRows);

                SheetInfo sheetInfo = new SheetInfo();
                sheetInfo.Sheet = s;

                if (i == 0)
                {
                    m_firstSheetName = s.SheetName;
                }

                m_dicSheet.Add(s.SheetName, sheetInfo);

                
                for (int j = COL_BEGIN; j < row.LastCellNum; j++)
                {
                    ICell cell = row.GetCell(j);
                    
                    if (sheetInfo.DicColumnIndex.ContainsKey(cell.StringCellValue))
                    {
                        //Debug.LogWarning("已经存在相同的列名:" + cell.StringCellValue);
                        continue;
                    }

                    sheetInfo.DicColumnIndex.Add(cell.StringCellValue,cell.ColumnIndex);
                }
            }

            fs.Close();
        }

        return true;
    }

    public int AddNewRow(int copyRowNum = -1, string sheetName = null)
    {
        SheetInfo sheetInfo = GetSheet(sheetName);
        if (sheetInfo != null)
        {
            
            IRow row;
            if (copyRowNum >= 0)
            {
                row = sheetInfo.Sheet.CopyRow(copyRowNum, sheetInfo.Sheet.LastRowNum + 1);
            }
            else
            {
                row = sheetInfo.Sheet.CreateRow(sheetInfo.Sheet.LastRowNum + 1);
            }

            return row.RowNum;
        }
        return -1;
    }

    public void SetCellValue(int rowNum, string columnName, bool value, string sheetName = null)
    {
        ICell cell = CreateCell(rowNum, columnName, sheetName);
        if (cell != null)
        {
			cell.SetCellType(CellType.Boolean);
            cell.SetCellValue(value);
        }
    }

    public void SetCellValue(int rowNum, string columnName, double value, string sheetName = null)
    {
        ICell cell = CreateCell(rowNum, columnName, sheetName);
        if (cell != null)
        {
			cell.SetCellType(CellType.Numeric);
            cell.SetCellValue(value);
        }
    }

    public void SetCellValue(int rowNum, string columnName, string value, string sheetName = null)
    {
        ICell cell = CreateCell(rowNum, columnName, sheetName);
        if (cell!=null)
        {
			cell.SetCellType(CellType.String);
            cell.SetCellValue(value);
        }
    }

    public bool Save(string path = null)
    {
        if(string.IsNullOrEmpty(path))
        {
            path = m_path;
        }

        if (m_curBook==null)
        {
            return false;
        }


        // 写入到客户端  
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        {
            m_curBook.Write(ms);
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();

                fs.Close();
            }
        }

        return true;
    }

    public SheetInfo GetSheet(string sheetName = null)
    {
        if (string.IsNullOrEmpty(sheetName))
        {
            sheetName = m_firstSheetName;
        }

        if (m_curBook == null)
        {
            return null;
        }

        if (m_dicSheet.ContainsKey(sheetName))
        {
            SheetInfo sheetInfo = m_dicSheet[sheetName];
            return sheetInfo;
        }

        return null;
    }

    private ICell CreateCell(int rowNum, string columnName, string sheetName = null)
    {
        SheetInfo sheetInfo = GetSheet(sheetName);
        if (sheetInfo != null)
        {
            if (sheetInfo.DicColumnIndex.ContainsKey(columnName))
            {
                int colIndex = sheetInfo.DicColumnIndex[columnName];
                IRow row = sheetInfo.Sheet.GetRow(rowNum);
                if (row != null)
                {
                    return row.CreateCell(colIndex);
                }
            }
        }

        return null;
    }
}
