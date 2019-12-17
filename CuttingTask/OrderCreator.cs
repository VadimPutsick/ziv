using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingTask
{
    public class OrderCreator
    {
        private const double MAX_WIDTH = 6000.0;
        private const double MAX_WASTE = 4;
        private const int ORDER_COUNT = 6;
        private List<Order> orders = new List<Order>();
        public List<Order> createOrder(List<CommonInfo> commonInfos) 
        {

            bool hasMembraneToCut = true;
            while (hasMembraneToCut)
            {                
                commonInfos.Sort((prev, next) => next.Count.CompareTo(prev.Count));
                List<CommonInfo> commonInfoForOrder = new List<CommonInfo>();
                commonInfoForOrder.AddRange(commonInfos.GetRange(0, ORDER_COUNT));

                double waste = (1 - commonInfoForOrder.Sum(x => x.Width) / MAX_WIDTH) * 100;
                var newCommonInfos = commonInfos.Skip(ORDER_COUNT).ToList();

                while (waste > MAX_WASTE || waste < 0)
                {
                    int commonInfoIndex = 0;
                    if (waste > 0)
                    {
                        commonInfoForOrder.Sort((prev, next) => prev.Width.CompareTo(next.Width));
                        commonInfoIndex = newCommonInfos.FindIndex(item => 
                            commonInfoForOrder[0].Width < item.Width &&
                            item.Width - commonInfoForOrder[0].Width < MAX_WIDTH * waste / 100 && 
                            item.Count >= 1);
                    }
                    else
                    {
                        commonInfoForOrder.Sort((prev, next) => next.Width.CompareTo(prev.Width));
                        commonInfoIndex = newCommonInfos.FindIndex(item => 
                            commonInfoForOrder[0].Width > item.Width &&
                            commonInfoForOrder[0].Width - item.Width > MAX_WIDTH * waste / 100 &&
                            item.Count >= 1);
                    }

                    if (commonInfoIndex >= 0)
                    {
                        CommonInfo commonInfo = commonInfoForOrder[0];
                        commonInfoForOrder[0] = newCommonInfos[commonInfoIndex];
                        newCommonInfos[commonInfoIndex] = commonInfo;
                    }
                    else
                    {
                        return null;
                    } 
                    waste = (1 - commonInfoForOrder.Sum(x => x.Width) / MAX_WIDTH) * 100;
                }
                orders.Add(new Order(commonInfoForOrder));
                hasMembraneToCut = commonInfos.First().Count > 0;
            }
            Array.ForEach(commonInfos.ToArray(), Console.WriteLine);
            return new List<Order>();
        }

    }
}
