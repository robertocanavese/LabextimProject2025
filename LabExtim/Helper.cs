using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabExtim
{
    /// <summary>
    ///     Summary description for Helper
    /// </summary>
    public class Helper
    {
        public static void BindTooltip(Page p)
        {
            if (p == null || p.Form == null)
                return;
            BindTooltip(p.Form.Controls);
        }

        public static void BindTooltip(ControlCollection cc)
        {
            try
            {
                if (cc == null)
                    return;
                for (var i = 0; i < cc.Count; i++)
                {
                    try
                    {
                        var c = cc[i];
                        if (c.HasControls())
                        {
                            BindTooltip(c.Controls);
                        }
                        else
                        {
                            if (!c.GetType().IsSubclassOf(typeof (ListControl))) continue;
                            var lc = (ListControl) c;
                            BindTooltip(lc);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public static void BindTooltip(ListControl lc)
        {
            lc.Attributes.Remove("onfocus");
            lc.Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
            for (var i = 0; i < lc.Items.Count; i++)
            {
                lc.Items[i].Attributes.Add("title", lc.Items[i].Text);
            }
        }
    }
}