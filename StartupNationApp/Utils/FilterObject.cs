using Common;
using StartupNationApp.Utils.FIlter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupNationApp.Utils
{
    public class FilterObject
    {
        public IEnumerable<FilterStage> Stages {get; set;}
        public int LastFundedBeforeMonths {get; set;}
        public double GotAtLeast { get; set; }
    }

    
}
