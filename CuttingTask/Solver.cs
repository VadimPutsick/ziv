using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingTask
{
    public class Solver
    {
        private void Sort(List<CommonInfo> commonInfo, Func<CommonInfo, CommonInfo, decimal> compareFunc)
        {
            for (var i = 0; i < commonInfo.Count; i++)
            {
                for (var j = i + 1; j < commonInfo.Count; j++)
                {
                    if (compareFunc(commonInfo[i], commonInfo[j]) > 0)
                    {
                        var temp = commonInfo[i];
                        commonInfo[i] = commonInfo[j];
                        commonInfo[j] = temp;
                    }
                }
            }
        }
        public List<Plank> CalculateCuts(int possibleLength, int limitOnCuts, List<CommonInfo> commonInfo, int diameter = 500)
        {
            Sort(commonInfo, (a, b) => b.Count - a.Count);

            List<Plank> planks = new List<Plank>(commonInfo.Count / limitOnCuts + 2);

            // пройдемся по всем ширинам 
            foreach (var desired in commonInfo)
            {
                // если не найдено подходящих досок то создади новую
                if (!planks.Any(plank => plank.FreeLength >= desired.Width && plank.CurrentCountDesiredInPlank < plank.CountDesiredInPlank))
                {
                    planks.Add(new Plank(possibleLength, limitOnCuts, diameter));
                }

                // режем (т. е. добавляем), где можем
                foreach (var plank in planks.Where(plank => plank.FreeLength >= desired.Width && plank.CurrentCountDesiredInPlank <= plank.CountDesiredInPlank))
                {
                    plank.Cut(desired.Width, desired.Name, desired.Count, desired.Sleeve);
                    break;
                }
            }

            foreach (var plank in planks)
            {
                int min = int.MaxValue;
                foreach (var count in plank.InitialCount)
                {
                    if (min > count)
                    {
                        min = (int)count;
                    }
                }
                plank.HowMany = min;
                for (var i = 0; i < plank.Cuts.Count; i++)
                {
                    plank.FinalCount.Add(plank.InitialCount[i] - min);
                }
            }

            return planks;
        }
    }
}
