﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CuttingTask;
using ExcelExporter;

namespace TestingConsole
{
    internal class Tester
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            #region Считывание Ширины Количества Имён и проверка

            var desired = new List<int>(150);
            using (var reader = new StreamReader("widthes.txt"))
            {
                string strNum;
                while ((strNum = reader.ReadLine()) != null) desired.Add(Convert.ToInt32(strNum));
            }

            var names = new List<string>(desired.Count);
            using (var reader = new StreamReader("names.txt"))
            {
                string name;
                while ((name = reader.ReadLine()) != null) names.Add(name);
            }

            var counts = new List<decimal>(desired.Count);
            using (var reader = new StreamReader("counts.txt"))
            {
                string strNum;
                while ((strNum = reader.ReadLine()) != null) counts.Add(Convert.ToDecimal(strNum));
            }

            if (counts.Count != desired.Count || desired.Count != names.Count)
            {
                throw new Exception("Не равное количество параметров");
            }

            #endregion

            var commonInfoList = desired.Select((t, i) => new CommonInfo(names[i], t, counts[i])).ToList();

            #region Агрегация материалов с одинаковым Width

            var elementsCount = commonInfoList.Select(x => x.Width).Distinct();
            var resultList = new List<CommonInfo>();

            foreach (var item in elementsCount)
            {
                var tmp = commonInfoList.Where(x => x.Width == item);
                resultList.Add(tmp.Aggregate((prev, next) =>
                {
                    prev.Name += ", " + next.Name;
                    prev.Count += next.Count;
                    return prev;
                }));
            }
            #endregion

            OrderCreator orderCreator = new OrderCreator();
            var result = orderCreator.CreateOrder(resultList);
            var exporter = new Exporter(@"result.xlxs");
            exporter.WriteFile(result);    

            //Console.ReadKey(true);
        }
    }
}