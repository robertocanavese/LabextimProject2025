using System;
using System.Collections.Generic;

namespace CMLabExtim.CustomClasses
{
    public class ComboKey
    {
        public ComboKey(IList<string> menuPath)
        {
            Prefix = menuPath[menuPath.Count - 1].Substring(0, 1);
            CommonKey = Convert.ToInt32(menuPath[menuPath.Count - 1].Substring(1));
        }

        public string Prefix { get; set; }
        public int CommonKey { get; set; }
        public string Order { get; set; }

        public string MenuValue
        {
            get { return Prefix + CommonKey.ToString(); }
        }
    }
}