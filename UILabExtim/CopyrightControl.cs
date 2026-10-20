using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace UILabExtim
{
	/// <summary>
	/// Summary description for CopyrightControl.
	/// </summary>
	[DefaultProperty("Text"), ToolboxData("<{0}:CopyrightControl runat=server></{0}:CopyrightControl>")]
	public class CopyrightControl : System.Web.UI.WebControls.WebControl, INamingContainer
	{
	
		[Bindable(true), Category("Appearance"), DefaultValue("04413650013"),  PersistenceMode(PersistenceMode.Attribute),
		Description("La partita IVA della societa'")
		] 
		public string Piva 
		{
			get
			{
				object piva = ViewState["Piva"];
				if(piva == null){
                    return "01698090287";
				}
				return piva as string;

			}

			set
			{
				ViewState["Piva"] = value;
			}
		}
		[Bindable(true), Category("Appearance"), DefaultValue("BasicNet S.p.A."),  PersistenceMode(PersistenceMode.Attribute),
		Description("Il nome della societa' da visualizzare"), 
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
		public string Company 
		{
			get
			{
				object piva = ViewState["Company"];
				if(piva == null)
				{
					return "Labe S.r.l.";
				}
				return piva as string;
			}

			set
			{
				ViewState["Company"] = value;
			}
		}
		[Bindable(true), Category("Appearance"), DefaultValue("http://www.basic.net/Default.asp"),  PersistenceMode(PersistenceMode.Attribute),
		Description("Il link associato al nome della societa'"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
		public string CompanyUrl 
		{
			get
			{
				object piva = ViewState["CompanyUrl"];
				if(piva == null)
				{
					return "https://www.labe.it";
				}
				return piva as string;
			}

			set
			{
				ViewState["CompanyUrl"] = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("Copyright"),  PersistenceMode(PersistenceMode.Attribute),
		Description("CSS Class name applied to the control."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
		public override string CssClass
		{
			get
			{
				object piva = ViewState["CssClass"];
				if(piva == null)
				{
					return "Copyright";
				}
				return piva as string;
			}

			set
			{
				ViewState["CssClass"] = value;
			}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			string css = "";
			if(this.CssClass!=string.Empty){
				css = string.Format(@"class = ""{0}""", CssClass);
			}
            output.Write(@"<div {4}>Copyright © 2008-{0}<A target=_top {4} href=""{1}""> {2} </A> - P.I. {3} - All Rights Reserved | <a class=""Copyright"" href=""#"" title=""Company Info"" onclick=""javascript:window.open('https://www.labe.it/chi-siamo/', 'COMPANYINFO','width=550,height=350,resizable=yes,scrollbars=yes,status=0,screenX=100,screenY=100,left=100,top=100,toolbar=0,location=0,directories=0,status=0,menubar=0');"">Company Info</a></div>", DateTime.Now.Year, CompanyUrl, Company, Piva, css);
			
		}

		}
}
