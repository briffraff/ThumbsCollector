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

        public int Counter()
        {
            int counter = ws.UsedRange.Rows.Count - 2;
            return counter;
        }

        public void RenameSheet(string season)
        {
            ws.Name = season;
        }


        public void ClearSheet()
        {
            ws.UsedRange.Rows.ClearContents();
        }

        public int GetSheetId()
        {
            int id = this.ws.Index;
            return id;
        }

        public int WorksheetsCount()
        {
            return wb.Worksheets.Count;
        }

        public void AddNewWorksheet(string worksheetName)
        {

            if (ContainsSheet(worksheetName) == false)
            {
                ws = wb.Sheets.Add(After: wb.Sheets[wb.Sheets.Count]);
                RenameSheet(worksheetName);
                ws = wb.Sheets[worksheetName];
            }
        }

        public bool ContainsSheet(string worksheetName)
        {
            bool found = false;

            foreach (_Excel.Worksheet sheet in wb.Sheets)
            {
                // Check the name of the current sheet
                if (sheet.Name == worksheetName)
                {
                    found = true;
                    break; // Exit the loop now
                }
            }

            return found;
        }

        public void SelectSheet(string inputSeason)
        {
            //List<Worksheet> wsList = new List<Worksheet>();

            //foreach (Worksheet wbSheet in wb.Sheets)
            //{
            //    wsList.Add(wbSheet);
            //}

            //Worksheet wsIndex = wsList.Find(a => a.Name.Contains(inputSeason));

            ws = wb.Sheets[inputSeason];
        }

        public void Formatting()
        {
            //colors
            var white = _Excel.XlRgbColor.rgbWhite;
            var black = _Excel.XlRgbColor.rgbBlack;
            var gray = _Excel.XlRgbColor.rgbGray;
            var lightGray = _Excel.XlRgbColor.rgbLightGray;

            //merging cells
            ws.Range["A1:C1"].Merge();
            ws.Range["D1:G1"].Merge();

            //allignment
            ws.Cells.HorizontalAlignment = _Excel.XlHAlign.xlHAlignCenter;

            //season cell and counter cell
            //adjust font properties

            //season
            ws.Cells[1].EntireRow.Font.Name = "Bahnschrift";
            ws.Cells[1].EntireRow.Font.Size = 30;
            ws.Cells[1, "A"].EntireRow.Font.Color = white;
            ws.Rows[1].EntireRow.Interior.Color = black;

            //counter
            ws.Cells[1, "D"].Font.Color = gray;
            ws.Cells[1].EntireRow.Font.Size = 24;
            ws.Columns.AutoFit();

            //title bar
            ws.Range["A2:C2"].Font.Name = "Bahnschrift";
            ws.Range["A2:C2"].Font.Size = 14;
            ws.Range["A2:C2"].Font.Bold = true;
            ws.Range["A2:C2"].ColumnWidth = 15;
            ws.Rows[2].EntireRow.Interior.Color = lightGray;

        }


    }
}
