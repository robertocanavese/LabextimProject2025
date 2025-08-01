using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CMLabExtim;

namespace DailyManager
{
    public class DBU
    {
        public static SqlDataReader DBUReader(string sqlQuery, object[] parameterValues, CommandType sqlComType, SqlConnection conn, SqlTransaction trans = null, int commandTimeOut = 60)
        {

            string _parList = string.Empty;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand _cmd = new SqlCommand(sqlQuery, conn, trans);
                _cmd.CommandTimeout = commandTimeOut;
                _cmd.CommandType = sqlComType;
                if ((parameterValues != null))
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
                        {
                            _cmd.Parameters.Add(new SqlParameter("@p" + i.ToString(), _parameterValue));
                        }
                        _parList += _parameterValue.ToString() + ", ";
                        i += 1;
                    }
                }
                return _cmd.ExecuteReader();

            }
            catch (Exception _ex)
            {
                Log.Write(string.Format("LabExtim (Comando fallito: {0} - Valori parametri: {1}) - ", sqlQuery, _parList), _ex);
                throw _ex;
            }

        }

        public static int DBUNonQuery(string sqlQuery, object[] parameterValues, CommandType sqlComType, SqlConnection conn, SqlTransaction trans = null, Int32 commandTimeout = 60)
        {


            string _parList = string.Empty;

            try
            {
                bool _wasClosed = false;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    _wasClosed = true;
                }
                SqlCommand _cmd = new SqlCommand(sqlQuery, conn, trans);
                _cmd.CommandTimeout = commandTimeout;
                _cmd.CommandType = sqlComType;
                if ((parameterValues != null))
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
                        {
                            _cmd.Parameters.Add(new SqlParameter("@p" + i.ToString(), _parameterValue));
                        }
                        _parList += _parameterValue.ToString() + ", ";
                        i += 1;
                    }
                }
                int _result = _cmd.ExecuteNonQuery();
                if (_wasClosed == true)
                {
                    conn.Close();
                }
                return _result;

            }
            catch (Exception _ex)
            {
                Log.Write(string.Format("WarehouseManager (Comando fallito: {0} - Valori parametri: {1}) - ", sqlQuery, _parList), _ex);
                throw _ex;
            }

        }



    }
}
