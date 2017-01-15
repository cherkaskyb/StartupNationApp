using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupNationApp.Utils.FIlter
{
    public class FilterStage
    {
        public CompanyStage Stage { get; set; }
        public bool IsSelected { get; set; }

        public FilterStage(CompanyStage stage, bool isSelected)
        {
            Stage = stage;
            IsSelected = isSelected;
        }

        public static IEnumerable<FilterStage> CreateAll()
        {
            var result = new List<FilterStage>();
            var values = Enum.GetValues(typeof(CompanyStage));

            foreach (var v in values)
            {
                result.Add(new FilterStage((CompanyStage) v, true));
            }

            return result;
        }
    }
}
