using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using System.Windows.Forms;

namespace WinMembership
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            UsersListView.Items.Clear();
            var users = Membership.GetAllUsers();
            foreach (MembershipUser user in users)
            {
                var lvi = new ListViewItem();
                lvi.Text = user.UserName;
                lvi.SubItems.Add(user.Email);
                lvi.SubItems.Add(user.CreationDate.ToString());
                lvi.SubItems.Add(user.ProviderUserKey.ToString());

                UsersListView.Items.Add(lvi);
            }
            using (
                var _ctx =
                    new LabExtimDataContext(
                        ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ToString()))
            {
                var _employees = new List<Employee>();
                _employees.Insert(0, new Employee());
                _employees.AddRange(_ctx.Employees.OrderBy(e => e.UniqueName).ToList());
                cboEmployees.DataSource = _employees;
                cboEmployees.DisplayMember = "UniqueName";
                cboEmployees.ValueMember = "UserGUID";
            }
            UsersListView_SelectedIndexChanged(null, null);
        }

        private void AddCommand_Click(object sender, EventArgs e)
        {
            try
            {
                var _newUser = Membership.CreateUser(UserNameText.Text, PasswordText.Text, EmailText.Text);
                MessageBox.Show("User created!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to create user: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (UsersListView.SelectedItems.Count > 0)
                {
                    using (
                        var _ctx =
                            new LabExtimDataContext(
                                ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ToString()))
                    {
                        var _employees =
                            _ctx.Employees.Where(
                                emp =>
                                    emp.UserGUID ==
                                    (Guid?) Membership.GetUser(UsersListView.SelectedItems[0].Text).ProviderUserKey)
                                .ToList();
                        foreach (var _employee in _employees)
                        {
                            _employee.UserGUID = null;
                        }
                        _ctx.SubmitChanges();
                    }
                }
                Membership.DeleteUser(UsersListView.SelectedItems[0].Text);
                MessageBox.Show("User deleted!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete user: " + ex.Message);
            }
        }

        private void UsersListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UsersListView.SelectedItems.Count > 0)
            {
                using (
                    var _ctx =
                        new LabExtimDataContext(
                            ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ToString()))
                {
                    var _employee =
                        _ctx.Employees.SingleOrDefault(
                            emp => emp.UserGUID.ToString() == UsersListView.SelectedItems[0].SubItems[3].Text);
                    if (_employee != null)
                    {
                        cboEmployees.SelectedValue = _employee.UserGUID.GetValueOrDefault();
                    }
                    else
                        cboEmployees.SelectedIndex = 0;
                }
            }
        }

        private void btnLinkToEmployee_Click(object sender, EventArgs e)
        {
            if (UsersListView.SelectedItems.Count > 0)
            {
                using (
                    var _ctx =
                        new LabExtimDataContext(
                            ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ToString()))
                {
                    var _employees =
                        _ctx.Employees.Where(
                            emp =>
                                emp.UserGUID ==
                                (Guid?) Membership.GetUser(UsersListView.SelectedItems[0].Text).ProviderUserKey)
                            .ToList();
                    foreach (var _employee in _employees)
                    {
                        _employee.UserGUID = null;
                    }
                    _ctx.SubmitChanges();
                }
                using (
                    var _ctx =
                        new LabExtimDataContext(
                            ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ToString()))
                {
                    var _employee =
                        _ctx.Employees.SingleOrDefault(
                            emp => emp.UniqueName == ((Employee) cboEmployees.SelectedItem).UniqueName);
                    _employee.UserGUID = new Guid(UsersListView.SelectedItems[0].SubItems[3].Text);
                    _ctx.SubmitChanges();
                }
                LoadData();
            }
        }
    }
}