using DataPlayer;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System;

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


    }
}
