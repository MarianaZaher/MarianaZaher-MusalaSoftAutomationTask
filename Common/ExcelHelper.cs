using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;


namespace MusalaSoftAutomationTask.Common
{
    class ExcelHelper
    {
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        Excel.Range range;

        string str;
        int rCnt;
        int cCnt;
        int rw = 0;
        int cl = 0;
        public List<string> GetExcelSheetData( int rowNum)
        {
            xlApp = new Excel.Application();
            string filePath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName)+ "\\TestDataFiles\\ExcelFile.xlsx";
            xlWorkBook = xlApp.Workbooks.Open(filePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;
            List<string> rowData = new List<string>();

            for (int colNum = 1; colNum <= cl; colNum++)
            {
                
                    rowData.Add((string)(range.Cells[rowNum, colNum] as Excel.Range).Value2);
                    
                
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            return rowData;
        }

    }
}

