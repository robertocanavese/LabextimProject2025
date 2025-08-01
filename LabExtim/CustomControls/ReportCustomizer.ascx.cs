using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;

namespace LabExtim.CustomControls
{
    public partial class ReportCustomizer : UserControl
    {
        public int IDReference { get; set; }
        public int ReportTypeCode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataList();
            }
        }

        protected void BindDataList()
        {
            //ReportDataContext _context = new ReportDataContext();
            //IEnumerable<ReportTextType> _reportTextTypes = _context.ReportTextTypes;
            dlsReportTexts.DataKeyField = "Key"; //"ID";
            dlsReportTexts.DataSource = Global.ReportTextTypes; //_reportTextTypes;
            dlsReportTexts.DataBind();
            btnSaveAsDefault.Enabled = IDReference == -1;
            btnUseCurrent.Enabled = IDReference != -1;
        }

        protected void dlsReportTexts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var _context = new ReportDataContext();
                var _idReportTextType = Convert.ToInt32(dlsReportTexts.DataKeys[e.Item.ItemIndex]);
                var _txtText = (TextBox) e.Item.FindControl("txtText");

                var _foundReportText = _context.ReportTexts.FirstOrDefault(rt => rt.ID_Ref01 == IDReference
                                                                                 &&
                                                                                 rt.ReportTypeCode ==
                                                                                 ReportTypeCode &&
                                                                                 rt.TextTypeCode ==
                                                                                 _idReportTextType);

                if (_foundReportText != null)
                {
                    _txtText.Text = _foundReportText.Text;
                }
                else
                {
                    _foundReportText = _context.ReportTexts.FirstOrDefault(rt => rt.ReportTypeCode == ReportTypeCode
                                                                                 && rt.TextTypeCode == _idReportTextType &&
                                                                                 rt.Standard);

                    if (_foundReportText == null)
                    {
                        _txtText.Text = string.Empty;
                    }
                    else
                    {
                        _txtText.Text = _foundReportText.Text;
                    }
                }
                var _ibtDefault = (ImageButton) e.Item.FindControl("ibtDefault");
                _ibtDefault.Enabled = IDReference != -1;
            }
        }

        protected void dlsReportTexts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "SaveAsDefault")
            {
                var _context = new ReportDataContext();
                var _idReportTextType = Convert.ToInt32(dlsReportTexts.DataKeys[e.Item.ItemIndex]);

                var _toUpdateReportText =
                    _context.ReportTexts.FirstOrDefault(rt => rt.ID_Ref01 == IDReference
                                                              && rt.ReportTypeCode == ReportTypeCode &&
                                                              rt.TextTypeCode == _idReportTextType);

                var _txtText = (TextBox) e.Item.FindControl("txtText");


                if (_toUpdateReportText == null && _txtText.Text != string.Empty)
                {
                    _toUpdateReportText = new ReportText();
                    _toUpdateReportText.ID_Ref01 = IDReference;
                    _toUpdateReportText.ReportTypeCode = ReportTypeCode;
                    _toUpdateReportText.TextTypeCode = _idReportTextType;
                    _toUpdateReportText.Text = _txtText.Text;
                    _toUpdateReportText.CreateDate = DateTime.Now;

                    _context.ReportTexts.InsertOnSubmit(_toUpdateReportText);
                }
                else
                {
                    _toUpdateReportText.Text = _txtText.Text;
                    if (_txtText.Text == string.Empty)
                    {
                        _context.ReportTexts.DeleteOnSubmit(_toUpdateReportText);
                    }
                }
                _context.SubmitChanges();

                //CustomersStandardReportText _customersStandardReportText = new CustomersStandardReportText();
                //_customersStandardReportText.CustomerCode = m_CustomerCode;
                //_customersStandardReportText.ReportTypeCode = _toUpdateReportText.ReportTypeCode;
                //_customersStandardReportText.TextTypeCode = _toUpdateReportText.TextTypeCode;
                //_customersStandardReportText.ID_ReportText = _toUpdateReportText.ID;
                //}
            }
        }

        protected void btnUseCurrent_Click(object sender, EventArgs e)
        {
            var _currentReportTexts = (List<ReportText>) Session["CurrentReportTexts"];
            _currentReportTexts.RemoveAll(
                delegate(ReportText _rt) { return _rt.ReportTypeCode == ReportTypeCode && _rt.ID_Ref01 == IDReference; });

            var _context = new ReportDataContext();

            foreach (DataListItem _item in dlsReportTexts.Items)
            {
                var _toSetReportText = new ReportText();
                _toSetReportText.ID = -1;
                _toSetReportText.ID_Ref01 = IDReference;
                _toSetReportText.TextTypeCode = Convert.ToInt32(dlsReportTexts.DataKeys[_item.ItemIndex]);
                _toSetReportText.ReportTypeCode = ReportTypeCode;
                _toSetReportText.Text = ((TextBox) dlsReportTexts.Items[_item.ItemIndex].FindControl("txtText")).Text;

                _currentReportTexts.Add(_toSetReportText);
            }
            Session["CurrentReportTexts"] = _currentReportTexts;
        }

        protected void btnSaveAsMainDefault_Click(object sender, EventArgs e)
        {
            var _context = new ReportDataContext();

            foreach (DataListItem _item in dlsReportTexts.Items)
            {
                var _idReportTextType = Convert.ToInt32(dlsReportTexts.DataKeys[_item.ItemIndex]);

                var _toUpdateReportText =
                    _context.ReportTexts.FirstOrDefault(rt => rt.ID_Ref01 == IDReference
                                                              && rt.ReportTypeCode == ReportTypeCode &&
                                                              rt.TextTypeCode == _idReportTextType);

                var _txtText = (TextBox) _item.FindControl("txtText");

                //if (_txtText.Text != string.Empty)
                //{
                if (_toUpdateReportText == null)
                {
                    _toUpdateReportText = new ReportText();
                    _toUpdateReportText.ID_Ref01 = IDReference;
                    _toUpdateReportText.ReportTypeCode = ReportTypeCode;
                    _toUpdateReportText.TextTypeCode = _idReportTextType;
                    _toUpdateReportText.Text = _txtText.Text;
                    _toUpdateReportText.CreateDate = DateTime.Now;
                    _toUpdateReportText.Standard = true;

                    _context.ReportTexts.InsertOnSubmit(_toUpdateReportText);
                }
                else
                {
                    _toUpdateReportText.Text = _txtText.Text;
                    _toUpdateReportText.CreateDate = DateTime.Now;
                    _toUpdateReportText.Standard = true;
                }
                _context.SubmitChanges();

                //}
            }
        }
    }
}