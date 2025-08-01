using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CMLabExtim;

namespace DailyManager
{
    public class DBReindex
    {

        public static bool DbReindex(string dbLabel)
        {

            SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[dbLabel].ToString());
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["User ID"] = "sa";
            builder["Password"] = "7800500+";
            builder["Initial Catalog"] = _conn.Database;
            builder["Data Source"] = _conn.DataSource;
            SqlConnection _conn0 = new SqlConnection(builder.ConnectionString);
            SqlConnection _conn1 = new SqlConnection(builder.ConnectionString);
            SqlConnection _conn2 = new SqlConnection(builder.ConnectionString);
            StringBuilder _sb = new StringBuilder();
            bool _error = false;
            _sb.AppendLine(string.Format("MANUTENZIONE INDICI database '{0}' su istanza '{1}' - PROCEDURA AVVIATA ({2:dd/MM/yyyy hh:mm:ss}) - ", _conn.Database.ToUpperInvariant(), _conn.DataSource.ToUpperInvariant(), DateTime.Now));

            try
            {
                SqlDataReader _dr0 = DBU.DBUReader("Select * From sys.tables", null, CommandType.Text, _conn0, null, 90);


                while (_dr0.Read())
                {
                    string _sql1 = string.Format("SELECT a.index_id, name, isnull(avg_fragmentation_in_percent,0.0) avg_fragmentation_in_percent, isnull(fragment_count,0) fragment_count, STATS_DATE(b.OBJECT_ID, b.index_id) AS StatsUpdated FROM sys.dm_db_index_physical_stats (DB_ID(N'{0}'), OBJECT_ID(N'{1}'), NULL, NULL, NULL) AS a " + "JOIN sys.indexes AS b ON a.object_id = b.object_id AND a.index_id = b.index_id where b.is_disabled = 0;", _conn.Database, _dr0["name"]);
                    SqlDataReader _dr1 = DBU.DBUReader(_sql1, null, CommandType.Text, _conn1, null, 120);
                    while (_dr1.Read())
                    {
                        if (_dr1["name"] != null)
                        {
                            double _frag = Convert.ToDouble(_dr1["avg_fragmentation_in_percent"]);
                            double _fragCount =  Convert.ToDouble( _dr1["fragment_count"]);
                            if (_frag >= 5.0 || _fragCount > 200)
                            {
                                try
                                {
                                    string _sql2 = string.Empty;
                                    //if (_frag >= 40.0 || _fragCount > 2000)
                                    //{
                                        _sql2 = string.Format("ALTER INDEX [{0}] ON [{1}] REBUILD; UPDATE STATISTICS [{1}] [{0}]", _dr1["name"], _dr0["name"]);
                                    //}
                                    //else
                                    //{
                                    //    _sql2 = string.Format("ALTER INDEX [{0}] ON [{1}] REORGANIZE; UPDATE STATISTICS [{1}] [{0}]", _dr1["name"], _dr0["name"]);
                                    //}
                                    DateTime _start = DateTime.Now;
                                    int _res2 = DBU.DBUNonQuery(_sql2, null, CommandType.Text, _conn2, null, 120);
                                    TimeSpan _elapsed = DateTime.Now - _start;
                                    //{0:MM/dd/yy H:mm:ss zzz}", 
                                    //_sb.AppendLine(string.Format("{2} indice '{1}' tabella '{0}' perchè frammentato per il {3:n0}% o per {5:n0} record (tempo esecuzione {4:c}) - ", _dr0["name"], _dr1["name"], (_frag >= 40.0 ? "RICHIESTO REBUILD" : "RICHIESTO REORGANIZE"), _frag, _elapsed, _fragCount));
                                    _sb.AppendLine(string.Format("{2} indice '{1}' tabella '{0}' perchè frammentato per il {3:n0}% o per {5:n0} record (tempo esecuzione {4:c}) - ", _dr0["name"], _dr1["name"], (_frag >= 5.0 ? "RICHIESTO REBUILD" : ""), _frag, _elapsed, _fragCount));
                                }
                                catch (Exception exi)
                                {
                                    _error = true;
                                    Log.Write(string.Format("MANUTENZIONE FALLITA INDICE '{1}' sul database '{0}'- ", _conn.Database.ToUpperInvariant(), _dr1["name"]), exi);
                                    _sb.AppendLine(string.Format("MANUTENZIONE FALLITA INDICE '{1}' sul database '{0}'- Errore: {2}", _conn.Database.ToUpperInvariant(), _dr1["name"], exi.Message));
                                }
                            }
                        }
                    }
                    _dr1.Close();
                    _conn1.Close();
                }

                _dr0.Close();
                _conn0.Close();
                if (!_error)
                {
                    _sb.AppendLine(string.Format("MANUTENZIONE INDICI database '{0}' su istanza '{1}' - PROCEDURA COMPLETATA CON SUCCESSO ({0:dd/MM/yyyy hh:mm:ss})", _conn.Database.ToUpperInvariant(), _conn.DataSource.ToUpperInvariant(), DateTime.Now));
                    Log.WriteMessage(_sb.ToString());
                }
                else
                {
                    _sb.AppendLine(string.Format("MANUTENZIONE INDICI database '{0}' su istanza '{1}' - PROCEDURA COMPLETATA CON ERRORI (vedi errori precedenti) ({0:dd/MM/yyyy hh:mm:ss})", _conn.Database.ToUpperInvariant(), _conn.DataSource.ToUpperInvariant(), DateTime.Now));
                    Log.WriteMessage(_sb.ToString());
                }

            }
            catch (Exception ex)
            {
                Log.Write("",ex);
            }
            finally
            {
                if (_conn1.State != ConnectionState.Closed)
                {
                    _conn1.Close();
                }
                if (_conn0.State != ConnectionState.Closed)
                {
                    _conn0.Close();
                }
                _sb = null;
            }

            return true;

        }

        public static void DBReindex_mm_lotto(string gestConnectionKey)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[gestConnectionKey].ToString()))
            {
                string command = 
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[movmag]') AND name = N'mm_lotto') " +
                    "    DROP INDEX [mm_lotto] ON [dbo].[movmag] WITH ( ONLINE = OFF ) " +
                    "CREATE NONCLUSTERED INDEX [mm_lotto] ON [dbo].[movmag] " +
                    "( " +
	                "[mm_lotto] ASC " +
                    ") " +
                    "INCLUDE ( [mm_valore], [mm_vprovv], " +
                    "[mm_vprovv2]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]";


                DBU.DBUNonQuery(command, null, CommandType.Text, cn);
            }

        }


    }
}
