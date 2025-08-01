<%@ Control Language="C#" CodeBehind="DateTime_Edit.ascx.cs" Inherits="LabExtim.DynamicData.FieldTemplates.DateTime_EditField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:TextBox ID="TextBox1" runat="server" CssClass="droplist" Text='<%# FieldValueEditString %>'
    Columns="10" ></asp:TextBox>
<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
    ImageAlign="Middle" />
<cc1:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" 
    FirstDayOfWeek="Monday" CssClass="MyCalendar"
    PopupButtonID="ImageButton1" TargetControlID="TextBox1" 
    PopupPosition="Right" 
    SelectedDate='<%# (FieldValueEditString == String.Empty) ? DateTime.Today : Convert.ToDateTime(FieldValueEditString) %>'  
    >
</cc1:CalendarExtender>
<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist"
    ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="droplist"
    ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="TextBox1"
    Display="Dynamic" />
