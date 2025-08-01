namespace CMLabExtim.Menuitems
{
    public class BnMenuItem
    {
        public BnMenuItem()
        {
            Visible = true;
            Enabled = true;
        }

        public BnMenuItem(string position, bool isStandard, bool selectable, string text, string value, string toolTip)
        {
            Position = position;
            IsStandard = isStandard;
            Selectable = selectable;
            Text = text;
            Value = value;
            ToolTip = toolTip;

            Visible = true;
            Enabled = true;
        }

        public string Position { get; set; }
        public bool IsStandard { get; set; }
        public bool Selectable { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string ToolTip { get; set; }
        public string ValuePath { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
    }
}