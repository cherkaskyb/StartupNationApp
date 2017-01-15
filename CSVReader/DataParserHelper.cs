using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader
{
    public static class DataParserHelper
    {
        private static char BlankChar = ' ';

        private static char[] DateTimeDelimiters = new[] { '/', '\\' };
        public static DateTime ParseMMYYDateTime(string value)
        {
            var parts = value.Split(DateTimeDelimiters);
            DateTime result;
            switch (parts.Length)
            {
                case 1:
                    result = new DateTime(int.Parse(parts[0]), 1, 1);
                    break;
                case 2:
                    result = new DateTime(int.Parse(parts[1]), int.Parse(parts[0]), 1);
                    break;
                default:
                    throw new ArgumentException("parts size");
            }

            return result;
        }

        private static char AmountK = 'k';
        private static char AmountM = 'm';
        private static char AmountB = 'b';
        private static char AmountUsd = '$';
        public static Single ParseAmount(string value)
        {
            float factor = 1;
            value = value.ToLower();
            if (value.Contains(AmountK))
            {
                factor = 0.001f;
            }
            if (value.Contains(AmountB))
            {
                factor = 1000f;
            }

            value = value.Replace(AmountUsd, BlankChar)
                .Replace(AmountM, BlankChar)
                .Replace(AmountK, BlankChar)
                .Replace(AmountB, BlankChar);
            return Single.Parse(value) * factor;
        }
    }
}
