using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomExtensions
{
    public static class StringExtension
    {
        public static bool ContainsCaseInsensitive(this String stringToSearchInto, String stringToSearch)
        {
            return stringToSearchInto.ToLowerInvariant().Contains(stringToSearch.ToLowerInvariant());
        }
    }
}
