using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace LabExtim
{
    public class GlobalVariables
    {
        public static List<HyperLink> PickingSelectors;

        public static List<HyperLink> GetPickingSelectors()
        {
            var labels = new List<string>
            {
                "Generale",
                "Impianti",
                "Serigrafia",
                "Digitale",
                "Prespaziati",
                "Tipografia",
                "Alluminio",
                "Tessuto",
                "Laser",
                "Tampone",
                "Lavorazioni",
                "Materiali",
                "Plastificazione",
                "Trasporto",
                "Silicon",
                "Regolo"
            };
           // var links = new List<string> {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};

            var selectors = new List<HyperLink>(16);

            foreach (var t in labels)
            {
                var newHyperLink = new HyperLink {Text = t};
                newHyperLink.Attributes.Add("onclick", "javascript:OpenSelector('Home.aspx?P1=" + t + "')");
                newHyperLink.Attributes.Add("class", "selectors");
                newHyperLink.Attributes.Add("style", "cursor:pointer");
                selectors.Add(newHyperLink);
            }

            return selectors;
        }
    }

    // Colori del Tema V4 (vedi blocco "Tema V4" in Site.css)
    public static class ThemeColors
    {
        public static readonly System.Drawing.Color Danger = System.Drawing.ColorTranslator.FromHtml("#C62828");
        public static readonly System.Drawing.Color Success = System.Drawing.ColorTranslator.FromHtml("#2E7D32");
        public static readonly System.Drawing.Color Info = System.Drawing.ColorTranslator.FromHtml("#1F5FA8");
        public static readonly System.Drawing.Color WarningBack = System.Drawing.ColorTranslator.FromHtml("#FFF3A3");
        public static readonly System.Drawing.Color HighlightBack = System.Drawing.ColorTranslator.FromHtml("#FFF8D6");
        public static readonly System.Drawing.Color MutedBack = System.Drawing.ColorTranslator.FromHtml("#E9EEF5");
        public static readonly System.Drawing.Color DisabledBack = System.Drawing.ColorTranslator.FromHtml("#6B7280");
    }
}