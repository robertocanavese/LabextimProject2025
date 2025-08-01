using System;
using System.Xml.Serialization;

namespace CMLabExtim.Menuitems
{
    [Serializable]
    public class BnMenuItemSwitch
    {
        private bool _mEnabled;
        private string _mValue;
        private bool _mVisible;

        [XmlElement("VALU")]
        public string Value
        {
            get { return _mValue; }
            set { _mValue = value; }
        }

        [XmlElement("ENAB")]
        public bool Enabled
        {
            get { return _mEnabled; }
            set { _mEnabled = value; }
        }

        [XmlElement("VISI")]
        public bool Visible
        {
            get { return _mVisible; }
            set { _mVisible = value; }
        }
    }
}