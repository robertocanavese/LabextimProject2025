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
}