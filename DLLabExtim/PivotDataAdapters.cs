using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CMLabExtim;

namespace DLLabExtim
{
    public class PivotDataAdapters
    {
        public static List<object[]> GetEmployeesWorkingDayHoursPivot(int companyId, int customerId, string startDate, string endDate)
        {
            List<object[]> _result = new List<object[]>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ToString()))
            {
                SqlDataReader readSchema = null/* TODO Change to default(_) if this is not a reference type */;
                SqlDataReader readTab = null/* TODO Change to default(_) if this is not a reference type */;
                DataTable _schema = null/* TODO Change to default(_) if this is not a reference type */;


                readSchema = null/* TODO Change to default(_) if this is not a reference type */;
                readSchema = DBUReader("prc_LAB_MGet_LAB_EmployeesWorkingDayHoursPivot", new object[] { new SqlParameter("@IDCompany", companyId), new SqlParameter("@IDCustomer", customerId), new SqlParameter("@DataDa", startDate), new SqlParameter("@DataA", endDate) }, CommandType.StoredProcedure, conn, null/* TODO Change to default(_) if this is not a reference type */);
                _schema = readSchema.GetSchemaTable();
                readSchema.Close();
                readTab = null/* TODO Change to default(_) if this is not a reference type */;
                readTab = DBUReader("prc_LAB_MGet_LAB_EmployeesWorkingDayHoursPivot", new object[] { new SqlParameter("@IDCompany", companyId), new SqlParameter("@IDCustomer", customerId), new SqlParameter("@DataDa", startDate), new SqlParameter("@DataA", endDate) }, CommandType.StoredProcedure, conn, null/* TODO Change to default(_) if this is not a reference type */);



                if (readTab.HasRows)
                {
                    object[] _rowHeader = new object[readTab.FieldCount - 1 + 1];
                    for (int i = 0; i <= readTab.FieldCount - 1; i++)
                        _rowHeader[i] = _schema.Rows[i][0];
                    _result.Add(_rowHeader);
                    while (readTab.Read())
                    {
                        object[] _row = new object[readTab.FieldCount - 1 + 1];
                        for (int i = 0; i <= readTab.FieldCount - 1; i++)
                            if (i == 0)
                            {
                                _row[i] = readTab[i];
                            }
                            else
                            {
                                _row[i] = (readTab[i] == DBNull.Value ? 0 : Convert.ToInt64(readTab[i]));
                            }
                        _result.Add(_row);
                    }
                    readTab.Close();
                }
            }

            return _result;
        }

        public static List<object[]> GetEmployeesWorkingDayHoursByEmployeeIdPivot(int userId, string startDate, string endDate)
        {
            List<object[]> _result = new List<object[]>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ToString()))
            {
                SqlDataReader readSchema = null/* TODO Change to default(_) if this is not a reference type */;
                SqlDataReader readTab = null/* TODO Change to default(_) if this is not a reference type */;
                DataTable _schema = null/* TODO Change to default(_) if this is not a reference type */;


                readSchema = null/* TODO Change to default(_) if this is not a reference type */;
                readSchema = DBUReader("prc_LAB_MGet_LAB_EmployeesWorkingDayHoursByEmployeeIdPivot", new object[] { new SqlParameter("@IDUser", userId), new SqlParameter("@DataDa", startDate), new SqlParameter("@DataA", endDate) }, CommandType.StoredProcedure, conn, null/* TODO Change to default(_) if this is not a reference type */);
                _schema = readSchema.GetSchemaTable();
                readSchema.Close();
                readTab = null/* TODO Change to default(_) if this is not a reference type */;
                readTab = DBUReader("prc_LAB_MGet_LAB_EmployeesWorkingDayHoursByEmployeeIdPivot", new object[] { new SqlParameter("@IDUser", userId), new SqlParameter("@DataDa", startDate), new SqlParameter("@DataA", endDate) }, CommandType.StoredProcedure, conn, null/* TODO Change to default(_) if this is not a reference type */);



                if (readTab.HasRows)
                {
                    object[] _rowHeader = new object[readTab.FieldCount - 1 + 1];
                    for (int i = 0; i <= readTab.FieldCount - 1; i++)
                        _rowHeader[i] = _schema.Rows[i][0];
                    _result.Add(_rowHeader);
                    while (readTab.Read())
                    {
                        object[] _row = new object[readTab.FieldCount - 1 + 1];
                        for (int i = 0; i <= readTab.FieldCount - 1; i++)
                            if (i == 0)
                            {
                                _row[i] = readTab[i];
                            }
                            else
                            {
                                _row[i] = (readTab[i] == DBNull.Value ? 0 : Convert.ToInt64(readTab[i]));
                            }
                        _result.Add(_row);
                    }
                    readTab.Close();
                }
            }

            return _result;
        }


        public static SqlDataReader DBUReader(string sqlQuery, object[] parameterValues, CommandType sqlComType, SqlConnection conn, SqlTransaction trans = null/* TODO Change to default(_) if this is not a reference type */, int commandTimeOut = 60)
        {
            string _parList = string.Empty;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand _cmd = new SqlCommand(sqlQuery, conn, trans);
                _cmd.CommandTimeout = commandTimeOut;
                if (sqlComType == null)
                    _cmd.CommandType = CommandType.Text;
                _cmd.CommandType = sqlComType;
                if (parameterValues != null)
                {
                    int i = 0;
                    foreach (object _parameterValue in parameterValues)
                    {
                        if (_parameterValue is SqlParameter)
                        {
                            SqlParameter _temp = (SqlParameter)_parameterValue;
                            _cmd.Parameters.Add(_parameterValue);
                        }
                        else
                            _cmd.Parameters.Add(new SqlParameter("@p" + i.ToString(), _parameterValue));
                        _parList += _parameterValue.ToString() + ", ";
                        i += 1;
                    }
                }
                return _cmd.ExecuteReader();
            }
            catch (Exception _ex)
            {
                Log.Write(string.Format("Labextim (Comando fallito: {0} - Valori parametri: {1})", sqlQuery, _parList), _ex);
                throw _ex;
            }
        }



    }
}
