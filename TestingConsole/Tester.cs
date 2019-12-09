using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CuttingTask;

namespace TestingConsole
{
    class Tester
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            #region Считывание Ширины Количества Имён и проверка
            List<int> desired = new List<int>(150);
            using (StreamReader reader = new StreamReader("widthes.txt"))
            {
                string strNum;
                while ((strNum = reader.ReadLine()) != null)
                {
                    desired.Add(Convert.ToInt32(strNum));
                }
            }

            List<string> names = new List<string>(desired.Count);
            using (StreamReader reader = new StreamReader("names.txt"))
            {
                string name;
                while ((name = reader.ReadLine()) != null)
                {
                    names.Add(name);
                }
            }

            List<decimal> counts = new List<decimal>(desired.Count);
            using (StreamReader reader = new StreamReader("counts.txt"))
            {
                string strNum;
                while ((strNum = reader.ReadLine()) != null)
                {
                    counts.Add(Convert.ToDecimal(strNum));
                }
            }

            if (counts.Count != desired.Count || desired.Count != names.Count)
            {
                throw new Exception("Не равное количество параметров");
            }
            #endregion

            const int POSSIBLE_LENGTH = 6000;
            const int LIMIT_ON_CUTS = 6;
            List<CommonInfo> commonInfoList = new List<CommonInfo>(desired.Count);  
            for (var i = 0; i < desired.Count; i++)
            {
                commonInfoList.Add(new CommonInfo(names[i], desired[i], counts[i]));                
            }
            
            #region Агрегация материалов с одинаковым Width
            var elementsCount = commonInfoList.Select(x => x.Width).Distinct();
            var resultList = new List<CommonInfo>();

            foreach (var item in elementsCount)
            {
                var tmp = commonInfoList.Where(x => x.Width == item);
                resultList.Add(tmp.Aggregate((prev, next) => {
                    prev.Name += ", " + next.Name;
                    prev.Count += next.Count;
                    return prev;
                }));
            }
            #endregion

            Solver solver = new Solver();
            List<Plank> planks = solver.CalculateCuts(POSSIBLE_LENGTH, LIMIT_ON_CUTS, resultList);
{
                foreach (var plank in planks)
            {
                Console.WriteLine(plank);
            }
            }

            Console.ReadKey(true);
        }
    }
}
