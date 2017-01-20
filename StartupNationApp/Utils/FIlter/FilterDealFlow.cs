using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupNationApp.Utils.FIlter
{
    public class FilterDealFlow
    {
        public DealFlowType DealFlow { get; set; }
        public bool IsSelected { get; set; }

        public FilterDealFlow(DealFlowType dealFlow, bool isSelected)
        {
            DealFlow = dealFlow;
            IsSelected = isSelected;
        }

        public static IEnumerable<FilterDealFlow> CreateAll()
        {
            var result = new List<FilterDealFlow>();
            var values = Enum.GetValues(typeof(DealFlowType));

            foreach (var v in values)
            {
                var dealFlow = (DealFlowType)v;
                var dealflowFilterItem = new FilterDealFlow(dealFlow, dealFlow == DealFlowType.None);
                result.Add(dealflowFilterItem);
            }

            return result;
        }
    }
}
