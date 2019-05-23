using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace thumbsCollector.Excel
{
    class ExcelApp
    {
        //fields
        string path = "";
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;

        //ctor
        public ExcelApp()
        {

        }

        public ExcelApp(string path, int Sheet)
        {
            this.path = path;
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[Sheet];
        }

        //Methods

        public void CreateNewFile()
        {
            this.wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        }

        public string ReadCell(int row, int col)
        {
            row++;
            col++;

            if (ws.Cells[row, col].Value2 != null)
            {
                return ws.Cells[row, col].Value2.ToString();
            }
            else
            {
                return "Empty cell";
            }
        }

        public void Write(int row, int col, string value)
        {
            row++;
            col++;
            ws.Cells[row, col].Value2 = value;
        }

        public void Save()
        {
            wb.Save();
        }

        public void SaveAs(string path)
        {
            wb.SaveAs(path);
        }

        public void Close()
        {
            wb.Close(true, this.wb.Name);
        }

        public void Formatting()
        {
            //merging cells
            ws.Range["A1:C1"].Merge();

            //allignment
            ws.Cells.HorizontalAlignment = _Excel.XlHAlign.xlHAlignCenter;

            //season cell
            //adjust font properties
            ws.Cells[1, "A"].Font.Name = "Bahnschrift";
            ws.Cells[1, "A"].Font.Size = 24;
            ws.Cells[1, "A"].Font.Color = "255,0,0";



            //title bar
            ws.Range["A2:C2"].Font.Name = "Bahnschrift";
            ws.Range["A2:C2"].Font.Size = 14;
            ws.Range["A2:C2"].Font.Bold = true;
            ws.Range["A2:C2"].ColumnWidth = 15;

        }
    }
}
