using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace PM.ExtClass
{
    public class DH : IDisposable
    {
        public static string ConStringPMdB = "Data Source=(DESCRIPTION =" + "(ADDRESS = (PROTOCOL = TCP)(HOST = 172.17.2.131)(PORT = 1521))" + "(CONNECT_DATA =" + "(SERVER = DEDICATED)" + "(SERVICE_NAME = PRAN)));" + "User Id=pm;Password=pm; pooling=false;";
        public static List<Dictionary<string, string>> GetTableRows(DataTable dtData)
        {
            List<Dictionary<string, string>> lstRows = new List<Dictionary<string, string>>();
            Dictionary<string, string> dictRow = null;
            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, string>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col].ToString());
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }


        public DataTable GetDataTable(string sqlCommand)
        {
            DataTable ds = new DataTable();

            DataTable dataTable = new DataTable();
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter adpt = new OracleDataAdapter();
            cmd.Connection = new OracleConnection(ConStringPMdB);
            cmd.CommandText = sqlCommand;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);

            return ds;
        }







        OracleConnection ConnectionNow = null;


        public static string CurrentUser = "PM";
        public static string CurrentUserPass = "pm";

        private static string _UserLoginID;
        private static string _UserLoginPassword;
        public static string UserLoginID
        {
            get { return _UserLoginID; }
            set { _UserLoginID = value; }
        }
        public static string UserLoginPassword
        {
            get { return _UserLoginPassword; }
            set { _UserLoginPassword = value; }
        }
        public bool OpenConection(string CurrentUser, string CurrentUserPass)
        {
            try
            {
                ConnectionNow = new OracleConnection(ConStringPMdB + "User Id=" + CurrentUser + ";Password=" + CurrentUserPass + ";");
                ConnectionNow.Open();           // test connection ok, close connection
                ConnectionNow.Close();
                return true;
            }
            catch (Exception)
            {
                throw;//Ex.Message.ToString();  eturn false;
            }
            finally
            {
                ConnectionNow.Close();
            }
        }

        public OracleDataReader DataReader(string Query_)
        {
            ConnectionNow = new OracleConnection(ConStringPMdB + "User Id=" + _UserLoginID + ";Password=" + _UserLoginPassword + ";");
            ConnectionNow.Open();
            OracleCommand cmd = new OracleCommand(Query_, ConnectionNow);
            OracleDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        public DataSet GetDataSet(string sqlCommand, string DSN, string DTN)
        {
            DataSet ds = new DataSet(DSN);
            using (OracleCommand cmd = new OracleCommand(
                sqlCommand, new OracleConnection(ConStringPMdB + "User Id=" + _UserLoginID + ";Password=" + _UserLoginPassword + ";")))
            {
                cmd.Connection.Open();
                DataTable table = new DataTable(DTN);
                table.Load(cmd.ExecuteReader());
                ds.Tables.Add(table);
            }
            return ds;
        }

        public object ShowDataInGridView(string Query_)
        {
            ConnectionNow = new OracleConnection(ConStringPMdB + "User Id=" + _UserLoginID + ";Password=" + _UserLoginPassword + ";");
            ConnectionNow.Open();
            OracleDataAdapter DA = new OracleDataAdapter(Query_, ConnectionNow);
            DataSet DS = new DataSet();
            DA.Fill(DS);
            object DataMembers = DS.Tables[0];
            ConnectionNow.Close();
            return DataMembers;
        }



        public void Dispose()
        {
            //  throw new NotImplementedException();
        }
    }

}