using System;
using System.Collections.Generic;
using System.Linq;
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

        public void WriteFile(List<Order> orders)
        {
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }

            var data = orders.Take(orders.Count() - 1).ToList();
            var remnants = orders.Last();
            var app = new Application
            {
                Visible = true,
                DisplayAlerts = false,
            };
            var workbook = app.Workbooks.Add(Type.Missing);
            var worksheet = (Worksheet)workbook.ActiveSheet;
            worksheet.Name = "Results";

            var startFromRow = 3;
            var index = 0;
            var orderIndex = 1;

            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 6]].Merge();
            worksheet.Cells[1, 1] = "Обработано";

            ((Style)worksheet.Range[worksheet.Cells[startFromRow, 6], worksheet.Cells[startFromRow, 7]].Style).VerticalAlignment = XlVAlign.xlVAlignCenter;
            ((Style)worksheet.Range[worksheet.Cells[startFromRow, 6], worksheet.Cells[startFromRow, 7]].Style).HorizontalAlignment =
                XlHAlign.xlHAlignCenter;
            
            worksheet.Cells[2, 1] = "№";
            worksheet.Cells[2, 2] = "Наименования";
            worksheet.Cells[2, 3] = "Ширина";
            worksheet.Cells[2, 4] = "Сколько изготовить";
            worksheet.Cells[2, 5] = "Отходы %";
            worksheet.Cells[2, 6] = "Сколько раз";
            foreach (var order in data)
            {
                index = 0;
                var o = order.getOrder();
                foreach (var commonInfo in o)
                {
                    index++;
                    worksheet.Cells[startFromRow + index - 1, 2].NumberFormat = "@";
                    worksheet.Cells[startFromRow + index - 1, 2] = commonInfo.Name;
                    worksheet.Cells[startFromRow + index - 1, 3] = commonInfo.Width;
                    worksheet.Cells[startFromRow + index - 1, 4] = commonInfo.Count;
                }
                worksheet.Range[worksheet.Cells[startFromRow, 1], worksheet.Cells[startFromRow + index - 1, 1]].Merge();
                worksheet.Range[worksheet.Cells[startFromRow, 5], worksheet.Cells[startFromRow + index - 1, 5]].Merge();
                worksheet.Range[worksheet.Cells[startFromRow, 6], worksheet.Cells[startFromRow + index - 1, 6]].Merge();
                worksheet.Cells[startFromRow, 5].NumberFormat = "@";
                worksheet.Cells[startFromRow, 1] = orderIndex;
                worksheet.Cells[startFromRow, 5] = Math.Round(order.Waste, 2);
                worksheet.Cells[startFromRow, 6] = order.RollCount;
                startFromRow += index;
                orderIndex++;
            }

            worksheet.Range[worksheet.Cells[startFromRow, 1], worksheet.Cells[startFromRow, 6]].Merge();
            worksheet.Cells[startFromRow, 1] = "Не обработано";
            startFromRow++;

            index = 0;
            var o2 = remnants.getOrder();
            foreach (var commonInfo in o2)
            {
                index++;
                worksheet.Cells[startFromRow + index - 1, 2].NumberFormat = "@";
                worksheet.Cells[startFromRow + index - 1, 2] = commonInfo.Name;
                worksheet.Cells[startFromRow + index - 1, 3] = commonInfo.Width;
                worksheet.Cells[startFromRow + index - 1, 4] = commonInfo.Count;
            }
            worksheet.Range[worksheet.Cells[startFromRow, 1], worksheet.Cells[startFromRow + index - 1, 1]].Merge();
            worksheet.Range[worksheet.Cells[startFromRow, 5], worksheet.Cells[startFromRow + index - 1, 5]].Merge();
            worksheet.Range[worksheet.Cells[startFromRow, 6], worksheet.Cells[startFromRow + index - 1, 6]].Merge();
            worksheet.Cells[startFromRow, 5].NumberFormat = "@";
            worksheet.Cells[startFromRow, 1] = orderIndex;
            worksheet.Range[worksheet.Cells[2, 2], worksheet.Cells[startFromRow, 6]].Columns.AutoFit();
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