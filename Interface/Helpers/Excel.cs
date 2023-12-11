using DataPlayer;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System;
using System.Data;

namespace Interface.Helpers
{
    public class Excel
    {
        public Excel() { }
        public List<Product> FileEXProduct(string excelFilePath)
        {

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            List<Product> products = new List<Product>();

            FileInfo excelFile = new FileInfo(excelFilePath);

            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    Product product = new Product();

                    product.ProductId = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                    product.ProductName = worksheet.Cells[row, 2].Text;
                    product.Description = worksheet.Cells[row, 3].Text;
                    product.Price = double.Parse(worksheet.Cells[row, 4].Text);
                    product.Quantity = Convert.ToInt32(worksheet.Cells[row, 5].Text);

                    products.Add(product);
                }
            }

            return products;
        }
        public void ExportFile(DataTable dataTable, string sheetName, string title)
        {
            //Tạo các đối tượng Excel

            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbooks oBooks;

            Microsoft.Office.Interop.Excel.Sheets oSheets;

            Microsoft.Office.Interop.Excel.Workbook oBook;

            Microsoft.Office.Interop.Excel.Worksheet oSheet;

            //Tạo mới một Excel WorkBook 

            oExcel.Visible = true;

            oExcel.DisplayAlerts = false;

            oExcel.Application.SheetsInNewWorkbook = 1;

            oBooks = oExcel.Workbooks;

            oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add(Type.Missing));

            oSheets = oBook.Worksheets;

            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(1);

            oSheet.Name = sheetName;

            // Tạo phần Tiêu đề
            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "G1");

            head.MergeCells = true;

            head.Value2 = title;

            head.Font.Bold = true;

            head.Font.Name = "Times New Roman";

            head.Font.Size = "20";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range cl1 = oSheet.get_Range("A3", "A3");

            cl1.Value2 = "Số thứ tự";

            cl1.ColumnWidth = 12;

            Microsoft.Office.Interop.Excel.Range cl2 = oSheet.get_Range("B3", "B3");

            cl2.Value2 = "Họ tên";

            cl2.ColumnWidth = 35.0;

            Microsoft.Office.Interop.Excel.Range cl3 = oSheet.get_Range("C3", "C3");

            cl3.Value2 = "Chức vụ";
            cl3.ColumnWidth = 12.0;

            Microsoft.Office.Interop.Excel.Range cl4 = oSheet.get_Range("D3", "D3");

            cl4.Value2 = "Lương cơ bản";

            cl4.ColumnWidth = 10.5;

            Microsoft.Office.Interop.Excel.Range cl5 = oSheet.get_Range("E3", "E3");

            cl5.Value2 = "Ngày nghỉ";

            cl5.ColumnWidth = 20.5;

            Microsoft.Office.Interop.Excel.Range cl6 = oSheet.get_Range("F3", "F3");

            cl6.Value2 = "Ngày công";

            cl6.ColumnWidth = 18.5;

            Microsoft.Office.Interop.Excel.Range cl7 = oSheet.get_Range("G3", "G3");

            cl7.Value2 = "Trợ cấp";

            cl7.ColumnWidth = 13.5;
            Microsoft.Office.Interop.Excel.Range cl8 = oSheet.get_Range("H3", "H3");

            cl8.Value2 = "Thực lãnh";

            cl8.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "H3");

            rowHead.Font.Bold = true;

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 6;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Tạo mảng theo datatable

            object[,] arr = new object[dataTable.Rows.Count, dataTable.Columns.Count];

            //Chuyển dữ liệu từ DataTable vào mảng đối tượng

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                DataRow dataRow = dataTable.Rows[row];

                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    arr[row, col] = dataRow[col];
                }
            }

            //Thiết lập vùng điền dữ liệu

            int rowStart = 4;

            int columnStart = 1;

            int rowEnd = rowStart + dataTable.Rows.Count - 2;

            int columnEnd = dataTable.Columns.Count;


            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowStart, columnStart];

            // Ô kết thúc điền dữ liệu

            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnEnd];

            // Lấy về vùng điền dữ liệu

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);

            //Điền dữ liệu vào vùng đã thiết lập

            range.Value2 = arr;

            // Kẻ viền

            range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Căn giữa cột mã nhân viên

            //Microsoft.Office.Interop.Excel.Range c3 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnStart];

            //Microsoft.Office.Interop.Excel.Range c4 = oSheet.get_Range(c1, c3);

            //Căn giữa cả bảng 
            oSheet.get_Range(c1, c2).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        }

        public void ExportFileTimeKeeping(DataTable dataTable, string sheetName, string title)
        {
            //Tạo các đối tượng Excel

            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbooks oBooks;

            Microsoft.Office.Interop.Excel.Sheets oSheets;

            Microsoft.Office.Interop.Excel.Workbook oBook;

            Microsoft.Office.Interop.Excel.Worksheet oSheet;

            //Tạo mới một Excel WorkBook 

            oExcel.Visible = true;

            oExcel.DisplayAlerts = false;

            oExcel.Application.SheetsInNewWorkbook = 1;

            oBooks = oExcel.Workbooks;

            oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add(Type.Missing));

            oSheets = oBook.Worksheets;

            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(1);

            oSheet.Name = sheetName;

            // Tạo phần Tiêu đề
            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "G1");

            head.MergeCells = true;

            head.Value2 = title;

            head.Font.Bold = true;

            head.Font.Name = "Times New Roman";

            head.Font.Size = "20";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range cl1 = oSheet.get_Range("A3", "A3");

            cl1.Value2 = "Mã nhân viên";

            cl1.ColumnWidth = 12;

            Microsoft.Office.Interop.Excel.Range cl2 = oSheet.get_Range("B3", "B3");

            cl2.Value2 = "Họ tên";

            cl2.ColumnWidth = 35.0;

            Microsoft.Office.Interop.Excel.Range cl3 = oSheet.get_Range("C3", "C3");

            cl3.Value2 = "D1";
            cl3.ColumnWidth = 12.0;

            Microsoft.Office.Interop.Excel.Range cl4 = oSheet.get_Range("D3", "D3");

            cl4.Value2 = "D2";

            cl4.ColumnWidth = 10.5;

            Microsoft.Office.Interop.Excel.Range cl5 = oSheet.get_Range("E3", "E3");

            cl5.Value2 = "D3";

            cl5.ColumnWidth = 20.5;

            Microsoft.Office.Interop.Excel.Range cl6 = oSheet.get_Range("F3", "F3");

            cl6.Value2 = "D4";

            cl6.ColumnWidth = 18.5;

            Microsoft.Office.Interop.Excel.Range cl7 = oSheet.get_Range("G3", "G3");

            cl7.Value2 = "D5";

            cl7.ColumnWidth = 13.5;
            Microsoft.Office.Interop.Excel.Range cl8 = oSheet.get_Range("H3", "H3");

            cl8.Value2 = "D6";

            cl8.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range cl9 = oSheet.get_Range("I3", "I3");

            cl9.Value2 = "D7";

            cl9.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c20 = oSheet.get_Range("J3", "J3");

            c20.Value2 = "D8";

            c20.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c21 = oSheet.get_Range("K3", "K3");

            c21.Value2 = "D9";

            c21.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c22 = oSheet.get_Range("L3", "L3");

            c22.Value2 = "D10";

            c22.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c23 = oSheet.get_Range("M3", "M3");

            c23.Value2 = "D11";

            c23.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c24 = oSheet.get_Range("N3", "N3");

            c24.Value2 = "D12";

            c24.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c25 = oSheet.get_Range("O3", "O3");

            c25.Value2 = "D13";

            c25.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c26 = oSheet.get_Range("P3", "P3");

            c26.Value2 = "D14";

            c26.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c27 = oSheet.get_Range("Q3", "Q3");

            c27.Value2 = "D15";

            c27.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c28 = oSheet.get_Range("R3", "R3");

            c28.Value2 = "D16";

            c28.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c29 = oSheet.get_Range("S3", "S3");

            c29.Value2 = "D17";

            c29.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c30 = oSheet.get_Range("T3", "T3");

            c30.Value2 = "D18";

            c30.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c31 = oSheet.get_Range("U3", "U3");

            c31.Value2 = "D19";

            c31.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c32 = oSheet.get_Range("V3", "V3");

            c32.Value2 = "D20";

            c32.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c33 = oSheet.get_Range("W3", "W3");

            c33.Value2 = "D21";

            c33.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c34 = oSheet.get_Range("X3", "X3");

            c34.Value2 = "D22";

            c34.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c35 = oSheet.get_Range("Y3", "Y3");

            c35.Value2 = "D23";

            c35.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c36 = oSheet.get_Range("Z3", "Z3");

            c36.Value2 = "D24";

            c36.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c37 = oSheet.get_Range("AA3", "AA3");

            c37.Value2 = "D25";

            c37.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c38 = oSheet.get_Range("AB3", "AB3");

            c38.Value2 = "D26";

            c38.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c39 = oSheet.get_Range("AC3", "AC3");

            c39.Value2 = "D27";

            c39.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c40 = oSheet.get_Range("AD3", "AD3");

            c40.Value2 = "D28";

            c40.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c41 = oSheet.get_Range("AE3", "AE3");

            c41.Value2 = "D29";

            c41.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c42 = oSheet.get_Range("AF3", "AF3");

            c42.Value2 = "D30";

            c42.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range c43 = oSheet.get_Range("AG3", "AG3");

            c43.Value2 = "D31";

            c43.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "AG3");

            rowHead.Font.Bold = true;

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 6;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Tạo mảng theo datatable

            object[,] arr = new object[dataTable.Rows.Count, dataTable.Columns.Count];

            //Chuyển dữ liệu từ DataTable vào mảng đối tượng

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                DataRow dataRow = dataTable.Rows[row];

                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    arr[row, col] = dataRow[col];
                }
            }

            //Thiết lập vùng điền dữ liệu

            int rowStart = 4;

            int columnStart = 1;

            int rowEnd = rowStart + dataTable.Rows.Count - 2;

            int columnEnd = dataTable.Columns.Count;


            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowStart, columnStart];

            // Ô kết thúc điền dữ liệu

            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnEnd];

            // Lấy về vùng điền dữ liệu

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);

            //Điền dữ liệu vào vùng đã thiết lập

            range.Value2 = arr;

            // Kẻ viền

            range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Căn giữa cột mã nhân viên

            //Microsoft.Office.Interop.Excel.Range c3 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnStart];

            //Microsoft.Office.Interop.Excel.Range c4 = oSheet.get_Range(c1, c3);

            //Căn giữa cả bảng 
            oSheet.get_Range(c1, c2).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        }
    }
}
