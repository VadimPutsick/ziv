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
        private decimal performedCount = 0;
        public Order(List<CommonInfo> commonInfos) 
        {
            
            performedCount = commonInfos.Select(item => item.Count).Min();
            //Console.WriteLine("-------------");
            //Console.WriteLine(performedCount);
            //Array.ForEach(commonInfos.ToArray(), Console.WriteLine);
            //Console.WriteLine();
            foreach (CommonInfo commonInfo in commonInfos) 
            {
                _commonInfos.Add(commonInfo.clone());
                commonInfo.Count = commonInfo.Count - performedCount;
            }
            //Console.WriteLine("-------------");
            //Console.WriteLine("-------------");
            Array.ForEach(_commonInfos.ToArray(), Console.WriteLine);
            Console.WriteLine("-------------");

        }
    }
}
