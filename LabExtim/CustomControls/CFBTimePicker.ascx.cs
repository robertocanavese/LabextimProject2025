using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabExtim.CustomControls
{
    public partial class CFBTimePicker : UserControl
    {
        public enum TipoDropDown
        {
            Anno = 1,
            Periodo = 2,
            Mese = 3,
            Settimana = 4,
            Giorno = 5
        };

        private const int LAST_SELECTED_YEAR = 2020;
        private const int FIRST_SELECTED_YEAR = 2000;
        private const string _PREFISSO_CONTROLLO = "_BasicPanel_";
        private DateTime _datafine;
        private DateTime _datainizio;
        private DropDownList _ddlAnno;
        private DropDownList _ddlGiorno;
        private DropDownList _ddlMese;
        private DropDownList _ddlPeriodo;
        private DropDownList _ddlSettimana;
        private ArrayList _externalPanelList;
        private Label _lblAnno;
        private Label _lblPeriodo;
        public DateTime? DataPartenzaOnLoad = null;
        public bool forzaDataPartenza = false;
        // 0 -> Tutto chiuso
        // 1 -> Anno
        // 2 -> Anno, Periodo Mese, Mese, Anno
        public int LivelloDataPartenzaOnLoad = 0;

        [Category("Behavior")]
        [Description("Panel Esterno")]
        [DefaultValue(null)]
        public string ExternalPanelID
        {
            get
            {
                var o = ViewState["ExternalPanelID"];
                if (o == null)
                    return null;
                return (string) o;
            }
            set { ViewState["ExternalPanelID"] = value; }
        }

        [Category("Behavior")]
        [Description("Anno Iniziale")]
        [DefaultValue(2000)]
        public int AnnoIniziale
        {
            get
            {
                var o = ViewState["AnnoIniziale"];
                if (o == null)
                    return FIRST_SELECTED_YEAR;
                return (int) o;
            }
            set { ViewState["AnnoIniziale"] = value; }
        }

        [Category("Behavior")]
        [Description("Labels CssClass")]
        [DefaultValue("")]
        public string LabelCssClass
        {
            get
            {
                var o = ViewState["LabelCssClass"];
                if (o == null)
                    return String.Empty;
                return (string) o;
            }
            set { ViewState["LabelCssClass"] = value; }
        }

        [Category("Behavior")]
        [Description("DropDownList CssClass")]
        [DefaultValue("")]
        public string DropDownListCssClass
        {
            get
            {
                var o = ViewState["DropDownListCssClass"];
                if (o == null)
                    return String.Empty;
                return (string) o;
            }
            set { ViewState["DropDownListCssClass"] = value; }
        }

        [Category("Behavior")]
        [Description("Anno Finale")]
        [DefaultValue(2020)]
        public int AnnoFinale
        {
            get
            {
                var o = ViewState["AnnoFinale"];
                if (o == null)
                    return LAST_SELECTED_YEAR;
                return (int) o;
            }
            set { ViewState["AnnoFinale"] = value; }
        }

        [Category("Behavior")]
        [Description("Testo Label Anno")]
        [DefaultValue("Anno")]
        public string LabelAnno
        {
            get
            {
                var o = ViewState["LabelAnno"];
                if (o == null)
                    return "Anno";
                return (string) o;
            }
            set { ViewState["LabelAnno"] = value; }
        }

        [Category("Behavior")]
        [Description("Testo Label Periodo")]
        [DefaultValue("Periodo")]
        public string LabelPeriodo
        {
            get
            {
                var o = ViewState["LabelPeriodo"];
                if (o == null)
                    return "Periodo";
                return (string) o;
            }
            set { ViewState["LabelPeriodo"] = value; }
        }

        public DateTime DataInizio
        {
            get
            {
                GetDataTime();
                return _datainizio;
            }
        }

        public DateTime DataFine
        {
            get
            {
                GetDataTime();
                return _datafine;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitComponent();
        }

        public void SetData(DateTime? DataPartenzaOnLoad)
        {
            SetDataPartenza(DataPartenzaOnLoad);
        }

        protected void InitComponent()
        {
            _externalPanelList = GetPanelList(ExternalPanelID);

            var _di = CultureInfo.CreateSpecificCulture("IT-it").DateTimeFormat;
            BuildDropDownListAnno();
            BuildDropDownListPeriodo();
            BuildDropDownListSettimana();
            BuildDropDownListMese(_di);
            BuildDropDownListGiorno();

            //if (!IsPostBack)
            //{
            SetDataPartenza(DataPartenzaOnLoad);
            //}
        }

        private void SetDataPartenza(DateTime? DataPartenzaOnLoad)
        {
            if (DataPartenzaOnLoad != null)
            {
                ViewState["Anno"] = DataPartenzaOnLoad.Value.Year.ToString();
                ViewState["Mese"] = DataPartenzaOnLoad.Value.Month.ToString();
                ViewState["Giorno"] = DataPartenzaOnLoad.Value.Day.ToString();
                ViewState["Periodo"] = "MESE";
                ViewState["Settimana"] = null;

                _ddlAnno.SelectedValue = DataPartenzaOnLoad.Value.Year.ToString();
                _ddlPeriodo.SelectedValue = "MESE";
                _ddlSettimana.Style.Add("display", "none");
                _ddlMese.SelectedValue = DataPartenzaOnLoad.Value.Month.ToString();
                _ddlGiorno.SelectedValue = DataPartenzaOnLoad.Value.Day.ToString();

                _ddlPeriodo.Enabled = true;

                _ddlMese.Style.Clear();
                _ddlGiorno.Style.Clear();
                _ddlPeriodo.Style.Clear();
            }
        }

        private ArrayList GetPanelList(string ExternalPanelID)
        {
            var _newArrayList = new ArrayList();
            if (ExternalPanelID != null)
            {
                var _stringList = ExternalPanelID.Split(',');
                if (_stringList.Length > 0)
                {
                    foreach (var _panelId in _stringList)
                    {
                        _newArrayList.Add((Panel) Parent.FindControl(_panelId));
                    }
                }
            }

            return _newArrayList;
        }

        private void ReloadPanelList(ArrayList panelList)
        {
            foreach (Panel _item in panelList)
            {
                _item.Visible = true;
            }
        }

        private void BuildDropDownListGiorno()
        {
            _ddlGiorno = new DropDownList();
            _ddlGiorno.ID = ClientID + _PREFISSO_CONTROLLO + "ddlGiorno";
            _ddlGiorno.AutoPostBack = true;
            _ddlGiorno.SelectedIndexChanged += _ddlGiorno_SelectedIndexChanged;
            if (!IsPostBack || _ddlAnno.SelectedIndex == 0) _ddlGiorno.Style.Add("display", "none");
            else
            {
                _ddlGiorno.Style.Clear();
                _ddlGiorno.CssClass = DropDownListCssClass;
            }
            PopolaDropDownGiorni();
            _BasicPanel.Controls.Add(_ddlGiorno);
        }

        private void PopolaDropDownGiorni()
        {
            _ddlGiorno.Items.Clear();
            var tmp_Year = (string.IsNullOrEmpty(_ddlAnno.SelectedValue) ||
                            string.Compare(_ddlAnno.SelectedValue, "*") == 0)
                ? 2009
                : int.Parse(_ddlAnno.SelectedValue);
            var tmp_Month = (string.IsNullOrEmpty(_ddlMese.SelectedValue) ||
                             string.Compare(_ddlMese.SelectedValue, "*") == 0)
                ? 1
                : int.Parse(_ddlMese.SelectedValue);
            var tmp_LastDayOfMonth = DateTime.DaysInMonth(tmp_Year, tmp_Month);
            _ddlGiorno.Items.Add(new ListItem("Tutti", "*"));
            for (var giorno = 1; giorno <= tmp_LastDayOfMonth; giorno++)
            {
                _ddlGiorno.Items.Add(new ListItem(giorno.ToString(), giorno.ToString()));
            }

            //if (DataPartenzaOnLoad != null)
            //    _ddlGiorno.SelectedValue = DataPartenzaOnLoad.Value.Day.ToString();
            //_ddlGiorno.AutoPostBack = false;

            _ddlGiorno.SelectedValue = ViewState["Giorno"] != null ? ViewState["Giorno"].ToString() : "*";

            //if (!IsPostBack)
            //    {
            //        ViewState["Giorno"] = _ddlGiorno.SelectedValue;
            //    }
        }

        private void BuildDropDownListMese(DateTimeFormatInfo dtFormatInfo)
        {
            _ddlMese = new DropDownList();
            _ddlMese.ID = ClientID + _PREFISSO_CONTROLLO + "ddlMese";
            _ddlMese.AutoPostBack = true;
            if (!IsPostBack || _ddlAnno.SelectedIndex == 0) _ddlMese.Style.Add("display", "none");
            else
            {
                _ddlMese.Style.Clear();
                _ddlMese.CssClass = DropDownListCssClass;
            }
            for (var mese = 1; mese <= 12; mese++)
            {
                _ddlMese.Items.Add(new ListItem(new DateTime(1, mese, 1).ToString("MMMM", dtFormatInfo), mese.ToString()));
            }

            //if (DataPartenzaOnLoad != null)
            //    _ddlMese.SelectedValue = DataPartenzaOnLoad.Value.Month.ToString();

            _ddlMese.SelectedIndexChanged += _ddlMese_SelectedIndexChanged;

            if (!IsPostBack) ViewState["Mese"] = _ddlMese.SelectedValue;
            _BasicPanel.Controls.Add(_ddlMese);
        }

        private void BuildDropDownListSettimana()
        {
            _ddlSettimana = new DropDownList();
            _ddlSettimana.ID = ClientID + _PREFISSO_CONTROLLO + "ddlSettimana";
            _ddlSettimana.AutoPostBack = true;
            _ddlSettimana.SelectedIndexChanged += _ddlSettimana_SelectedIndexChanged;
            if (!IsPostBack || _ddlAnno.SelectedIndex == 0) _ddlSettimana.Style.Add("display", "none");
            else
            {
                _ddlSettimana.Style.Clear();
                _ddlSettimana.CssClass = DropDownListCssClass;
            }
            if (_ddlAnno.SelectedValue != "*" && _ddlPeriodo.SelectedValue == "SETT")
            {
                PopolaDropDownSettimana();
            }

            //if (DataPartenzaOnLoad != null)
            //    _ddlSettimana.Style.Add("display", "none");

            _BasicPanel.Controls.Add(_ddlSettimana);
        }

        private void BuildDropDownListPeriodo()
        {
            _lblPeriodo = new Label();
            _lblPeriodo.ID = ClientID + _PREFISSO_CONTROLLO + "lblPeriodo";
            _lblPeriodo.CssClass = LabelCssClass;
            _lblPeriodo.Text = LabelPeriodo;
            _ddlPeriodo = new DropDownList();
            _ddlPeriodo.ID = ClientID + _PREFISSO_CONTROLLO + "ddlPeriodo";
            _ddlPeriodo.CssClass = DropDownListCssClass;
            _ddlPeriodo.AutoPostBack = true;
            _ddlPeriodo.Enabled = (_ddlAnno.SelectedValue == "*") ? false : true;
            _ddlPeriodo.Items.Add(new ListItem("Tutto", "*"));
            _ddlPeriodo.Items.Add(new ListItem("Settimana", "SETT"));
            _ddlPeriodo.Items.Add(new ListItem("Mese", "MESE"));
            _ddlPeriodo.Items.Add(new ListItem("1° Quarto", "1QUARTO"));
            _ddlPeriodo.Items.Add(new ListItem("2° Quarto", "2QUARTO"));
            _ddlPeriodo.Items.Add(new ListItem("3° Quarto", "3QUARTO"));
            _ddlPeriodo.Items.Add(new ListItem("4° Quarto", "4QUARTO"));
            _ddlPeriodo.Items.Add(new ListItem("1° Semestre", "1SEMESTRE"));
            _ddlPeriodo.Items.Add(new ListItem("2° Semestre", "2SEMESTRE"));

            _ddlPeriodo.SelectedIndexChanged += _ddlPeriodo_SelectedIndexChanged;

            //if (DataPartenzaOnLoad != null)
            //    _ddlPeriodo.SelectedValue = "MESE";

            if (!IsPostBack) ViewState["Periodo"] = _ddlPeriodo.SelectedValue;
            _BasicPanel.Controls.Add(_lblPeriodo);
            _BasicPanel.Controls.Add(_ddlPeriodo);
        }

        private void BuildDropDownListAnno()
        {
            _lblAnno = new Label();
            _lblAnno.ID = ClientID + _PREFISSO_CONTROLLO + "lblAnno";
            _lblAnno.CssClass = LabelCssClass;
            _lblAnno.Text = LabelAnno;
            _ddlAnno = new DropDownList();
            _ddlAnno.ID = ClientID + _PREFISSO_CONTROLLO + "ddlAnno";
            _ddlAnno.CssClass = DropDownListCssClass;
            _ddlAnno.AutoPostBack = true;
            _ddlAnno.Items.Add(new ListItem("Tutti", "*"));
            //for (int anno = LAST_SELECTED_YEAR; anno >= FIRST_SELECTED_YEAR; anno--)
            for (var anno = DateTime.Now.AddYears(1).Year; anno >= 2010; anno--)
            {
                _ddlAnno.Items.Add(new ListItem(anno.ToString(), anno.ToString()));
            }

            //if (DataPartenzaOnLoad != null)
            //    _ddlAnno.SelectedValue = DataPartenzaOnLoad.Value.Year.ToString();

            _ddlAnno.SelectedIndexChanged += _ddlAnno_SelectedIndexChanged;

            if (!IsPostBack) ViewState["Anno"] = _ddlAnno.SelectedValue;
            _BasicPanel.Controls.Add(_lblAnno);
            _BasicPanel.Controls.Add(_ddlAnno);
        }

        private void _ddlAnno_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ddlPeriodo.SelectedIndex = 0;
            _ddlPeriodo.Enabled = (_ddlAnno.SelectedValue == "*") ? false : true;
            _ddlMese.Style.Add("display", "none");
            _ddlSettimana.Style.Add("display", "none");
            _ddlGiorno.Style.Add("display", "none");
            ViewState["Anno"] = _ddlAnno.SelectedValue;
            ViewState["Periodo"] = "*";
            ViewState["Mese"] = "1";
            ReloadPanelList(_externalPanelList);
        }

        private void _ddlMese_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["Giorno"] = "*";
            PopolaDropDownGiorni();
            ViewState["Mese"] = _ddlMese.SelectedValue;

            ReloadPanelList(_externalPanelList);
        }

        private void _ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (_ddlPeriodo.SelectedValue)
            {
                case "SETT":
                {
                    PopolaDropDownSettimana();
                    _ddlMese.Style.Add("display", "none");
                    _ddlGiorno.Style.Add("display", "none");
                    _ddlSettimana.Style.Clear();
                    break;
                }
                case "MESE":
                {
                    _ddlSettimana.Style.Clear();
                    _ddlSettimana.Style.Add("display", "none");
                    _ddlMese.Style.Clear();
                    _ddlMese.SelectedIndex = 0;
                    ViewState["Mese"] = "1"; // aggiunta canny
                    _ddlGiorno.Style.Clear();
                    _ddlGiorno.SelectedIndex = 0;
                    ViewState["Giorno"] = "*"; // aggiunta canny
                    break;
                }
                default:
                {
                    _ddlSettimana.Style.Add("display", "none");
                    _ddlMese.Style.Add("display", "none");
                    _ddlGiorno.Style.Add("display", "none");
                    break;
                }
            }
            ViewState["Periodo"] = _ddlPeriodo.SelectedValue;

            ReloadPanelList(_externalPanelList);
        }

        private void _ddlGiorno_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ddlAnno.SelectedValue = ViewState["Anno"].ToString();
            _ddlPeriodo.SelectedValue = ViewState["Periodo"].ToString();
            _ddlSettimana.Style.Add("display", "none");
            _ddlMese.SelectedValue = ViewState["Mese"].ToString();
            ViewState["Giorno"] = _ddlGiorno.SelectedValue;

            ReloadPanelList(_externalPanelList);
        }

        private void _ddlSettimana_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ddlAnno.SelectedValue = ViewState["Anno"].ToString();
            _ddlPeriodo.SelectedValue = ViewState["Periodo"].ToString();
            _ddlMese.Style.Add("display", "none");
            ViewState["Settimana"] = _ddlSettimana.SelectedValue;

            ReloadPanelList(_externalPanelList);
        }

        public string GetDropDoanClientId(TipoDropDown TipoDropDown)
        {
            switch (TipoDropDown)
            {
                case TipoDropDown.Anno:
                    return _ddlAnno.ClientID;

                case TipoDropDown.Giorno:
                    return _ddlGiorno.ClientID;

                case TipoDropDown.Mese:
                    return _ddlMese.ClientID;

                case TipoDropDown.Settimana:
                    return _ddlSettimana.ClientID;

                case TipoDropDown.Periodo:
                    return _ddlPeriodo.ClientID;

                default:
                    return "";
            }
        }

        private void PopolaDropDownSettimana()
        {
            _ddlSettimana.Items.Clear();
            var tmp_FistDayOfYear = new DateTime(int.Parse(_ddlAnno.SelectedValue), 1, 1);
            var DayOfFirstWeek = (int) tmp_FistDayOfYear.DayOfWeek;
            var tmp_FistDayOfNextYear = new DateTime(int.Parse(_ddlAnno.SelectedValue) + 1, 1, 1);
            var tmp_FirstDayOfFirstWeek = tmp_FistDayOfYear.AddDays(-(DayOfFirstWeek - 1));
            var tmp_LastDayOfFirstWeek = tmp_FirstDayOfFirstWeek.AddDays(6);
            var labelText = "1° - Dal " + tmp_FirstDayOfFirstWeek.ToString("dd/MM") + " Al " +
                            tmp_LastDayOfFirstWeek.ToString("dd/MM") + "";
            _ddlSettimana.Items.Add(new ListItem(labelText, tmp_FirstDayOfFirstWeek.ToString("dd/MM/yyyy")));
            for (var settimane = 2; settimane <= 54; settimane++)
            {
                tmp_FirstDayOfFirstWeek = tmp_FirstDayOfFirstWeek.AddDays(7);
                tmp_LastDayOfFirstWeek = tmp_FirstDayOfFirstWeek.AddDays(6);
                labelText = settimane + "° - Dal " + tmp_FirstDayOfFirstWeek.ToString("dd/MM") + " Al " +
                            tmp_LastDayOfFirstWeek.ToString("dd/MM") + "";
                if (tmp_FirstDayOfFirstWeek < tmp_FistDayOfNextYear)
                {
                    _ddlSettimana.Items.Add(new ListItem(labelText, tmp_FirstDayOfFirstWeek.ToString("dd/MM/yyyy")));
                }
            }
            if (ViewState["Settimana"] == null) ViewState["Settimana"] = _ddlSettimana.SelectedValue;
        }

        private void GetDataTime()
        {
            var Anno = (ViewState["Anno"] == null) ? String.Empty : ViewState["Anno"].ToString();
            var Mese = (ViewState["Mese"] == null) ? String.Empty : ViewState["Mese"].ToString();
            var Settimana = (ViewState["Settimana"] == null) ? String.Empty : ViewState["Settimana"].ToString();
            var Giorno = (ViewState["Giorno"] == null) ? String.Empty : ViewState["Giorno"].ToString();
            var Periodo = (ViewState["Periodo"] == null) ? String.Empty : ViewState["Periodo"].ToString();

            switch (Periodo)
            {
                case "*":
                    if (string.Compare(Anno, "*") == 0)
                    {
                        _datainizio = new DateTime(AnnoIniziale, 1, 1, 0, 0, 0);
                        _datafine = new DateTime(AnnoFinale, 12, 31, 23, 59, 59);
                    }
                    else
                    {
                        _datainizio = new DateTime(int.Parse(Anno), 1, 1, 0, 0, 0);
                        _datafine = new DateTime(int.Parse(Anno), 12, 31, 23, 59, 59);
                    }
                    break;

                case "SETT":
                    _datainizio = Convert.ToDateTime(Settimana);
                    _datafine = Convert.ToDateTime(Settimana).AddDays(7).AddSeconds(-1);
                    break;

                case "MESE":
                    if (string.Compare(Giorno, "*") == 0)
                    {
                        _datainizio = new DateTime(int.Parse(Anno), int.Parse(Mese), 01, 0, 0, 0);
                        _datafine = _datainizio.AddMonths(1).AddSeconds(-1);
                    }
                    else
                    {
                        _datainizio = new DateTime(int.Parse(Anno), int.Parse(Mese), int.Parse(Giorno), 0, 0, 0);
                        _datafine = new DateTime(int.Parse(Anno), int.Parse(Mese), int.Parse(Giorno), 23, 59, 59);
                    }
                    break;

                case "1QUARTO":
                    _datainizio = new DateTime(int.Parse(Anno), 01, 01, 0, 0, 0);
                    _datafine = new DateTime(int.Parse(Anno), 03, 31, 23, 59, 59);
                    break;

                case "2QUARTO":
                    _datainizio = new DateTime(int.Parse(Anno), 04, 01, 0, 0, 0);
                    _datafine = new DateTime(int.Parse(Anno), 06, 30, 23, 59, 59);
                    break;

                case "3QUARTO":
                    _datainizio = new DateTime(int.Parse(Anno), 07, 01, 0, 0, 0);
                    _datafine = new DateTime(int.Parse(Anno), 09, 30, 23, 59, 59);
                    break;

                case "4QUARTO":
                    _datainizio = new DateTime(int.Parse(Anno), 10, 01, 0, 0, 0);
                    _datafine = new DateTime(int.Parse(Anno), 12, 31, 23, 59, 59);
                    break;

                case "1SEMESTRE":
                    _datainizio = new DateTime(int.Parse(Anno), 01, 01, 0, 0, 0);
                    _datafine = new DateTime(int.Parse(Anno), 06, 30, 23, 59, 59);
                    break;

                case "2SEMESTRE":
                    _datainizio = new DateTime(int.Parse(Anno), 07, 01, 0, 0, 0);
                    _datafine = new DateTime(int.Parse(Anno), 12, 31, 23, 59, 59);
                    break;
            }
        }
    }
}