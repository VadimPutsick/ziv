using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingTask
{
    public class Order
    {
        const int ITEMS_LIMIT = 6;
        private List<CommonInfo> _commonInfos = new List<CommonInfo>();
        public decimal RollCount { get; private set; }
        public double Waste { get; private set; }
        public Order(List<CommonInfo> commonInfos, double waste) 
        {
            Waste = waste;
            RollCount = commonInfos.Select(item => item.Count).Min();
            foreach (CommonInfo commonInfo in commonInfos) 
            {
                _commonInfos.Add(commonInfo.clone());
                commonInfo.Count = commonInfo.Count - RollCount;
            }
            Array.ForEach(_commonInfos.ToArray(), Console.WriteLine);
            Console.WriteLine();
            Console.WriteLine("Waste:" + waste);
            Console.WriteLine("Roll quantity: " + RollCount);
            Console.WriteLine("-------------");

        }

        public List<CommonInfo> getOrder() {
            return _commonInfos;
        }
    }
}
