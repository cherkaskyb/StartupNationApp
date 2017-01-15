using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader
{
    public static class HashSetConverter
    {
        public static HashSet<string> FromCompanies(ICollection<Company> collection)
        {
            var hash = new HashSet<string>();
            foreach (var c in collection)
            {
                hash.Add(c.LinkToFinder);
            }

            return hash;
        }
    }
}
