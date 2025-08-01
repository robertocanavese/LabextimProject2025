using System;

namespace CMLabExtim.CustomClasses
{
    public enum ComboListItemSource
    {
        PikingItem,
        MacroItem,
    }
    public class ComboListItem
    {
        private int _mCommonKey;
        private string _mPrefix;
        private string _mText;

        public string Prefix
        {
            get { return _mPrefix; }
            set { _mPrefix = value; }
        }

        public ComboListItemSource PrefixSource
        {
            set
            {
                switch (value)
                {
                    case ComboListItemSource.MacroItem:
                        _mPrefix = "M";
                        break;
                    case ComboListItemSource.PikingItem:
                        _mPrefix = "P";
                        break;
                }
            }
        }

        public int CommonKey
        {
            get { return _mCommonKey; }
            set { _mCommonKey = value; }
        }

        public string Text
        {
            set { _mText = value; }
        }

        public string ComboText
        {
            get
            {
                return (_mPrefix == "M"
                    ? string.Format("{0}{1}{2}", string.IsNullOrEmpty(_mText) ? "" : "[", _mText,
                    string.IsNullOrEmpty(_mText) ? "" : "]") : _mText);
            }
        }

        public string ComboValue
        {
            get { return (_mPrefix + _mCommonKey.ToString()); }
            set
            {
                _mPrefix = value[0].ToString();
                _mCommonKey = Convert.ToInt32(value.Substring(1));
            }
        }

        public string Order { get; set; }
        public int Hits { get; set; }

        private string _unit;

        public string IdUnit
        {
            get
            {
                return string.Format("{0}{2}{1} ({3})", _mPrefix == "M" ? "[" : "", _mPrefix == "M" ? "]" : "", (string.IsNullOrEmpty(_mText) ? "":_mText.Trim()), _unit.Trim());
            }
            set { _unit = value; }
        }
    }
}