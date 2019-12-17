using System;
using System.Collections.Generic;
using CuttingTask;
using Microsoft.Office.Interop.Excel;

namespace ExcelExporter
{
    public class Exporter
    {
        private readonly string _path;
        public Exporter(string path)
        {
            _path = path;
            
        }

        public void WriteFile(IEnumerable<Plank> data)
        {
            var app = new Application
            {
                Visible = true,
                DisplayAlerts = false,
            };
            var workbook = app.Workbooks.Add(Type.Missing);
            var worksheet = (Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Results";
            worksheet.Cells[2, 1] = "№";
            worksheet.Cells[2, 2] = "Заказ";
            worksheet.Cells[2, 3] = "Длина";
            worksheet.Cells[2, 4] = "Гильза";
            worksheet.Cells[2, 5] = "Было";
            worksheet.Cells[2, 5] = "Сколько изготовить";
            worksheet.Cells[2, 5] = "Отходы";
            worksheet.Cells[2, 5] = "Было";
            worksheet.Cells[2, 6] = "Сколько раз нужно изготовить";
            worksheet.Cells[2, 7] = "Отходы";
            var startFromRow = 3;
            var index = 0;
            foreach (var plank in data)
            {
                index++;
                worksheet.Cells[startFromRow, 1] = index;
                var i = 0;
                for (i = 0; i < plank.Cuts.Count; i++)
                {
                    worksheet.Cells[startFromRow + i, 2].NumberFormat = "@";
                    worksheet.Cells[startFromRow + i, 2] = plank.Names[i];
                    worksheet.Cells[startFromRow + i, 3] = plank.Cuts[i];
                    worksheet.Cells[startFromRow + i, 4] = plank.Sleeves[i];
                    worksheet.Cells[startFromRow + i, 5] = plank.InitialCount[i];
                }
                worksheet.Range[worksheet.Cells[startFromRow, 1], worksheet.Cells[startFromRow + i - 1, 1]].Merge();
                worksheet.Range[worksheet.Cells[startFromRow, 6], worksheet.Cells[startFromRow + i - 1, 6]].Merge();
                worksheet.Range[worksheet.Cells[startFromRow, 7], worksheet.Cells[startFromRow + i - 1, 7]].Merge();

                ((Style)worksheet.Range[worksheet.Cells[startFromRow, 6], worksheet.Cells[startFromRow, 7]].Style).VerticalAlignment = XlVAlign.xlVAlignCenter;
                ((Style)worksheet.Range[worksheet.Cells[startFromRow, 6], worksheet.Cells[startFromRow, 7]].Style).HorizontalAlignment =
                    XlHAlign.xlHAlignCenter;

                //((Style)((Range)worksheet.Cells[startFromRow, 6]).Style).VerticalAlignment = XlVAlign.xlVAlignCenter;
                //((Style) ((Range) worksheet.Cells[startFromRow, 6]).Style).HorizontalAlignment =
                //    XlHAlign.xlHAlignCenter;
                //((Style)((Range)worksheet.Cells[startFromRow, 7]).Style).VerticalAlignment = XlVAlign.xlVAlignCenter;
                //((Style)((Range)worksheet.Cells[startFromRow, 7]).Style).HorizontalAlignment =
                //    XlHAlign.xlHAlignCenter;
                worksheet.Cells[startFromRow, 6] = plank.HowMany;
                worksheet.Cells[startFromRow, 7] = plank.GetWasteInPersents();              



                startFromRow += i;
            }
        }
    }
}