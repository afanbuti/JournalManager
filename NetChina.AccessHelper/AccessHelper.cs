using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Configuration;

namespace NetChina.AccessHelper
{
    /// <summary>
    /// AccessHelper 的摘要说明。
    /// </summary>
    public abstract class AccessHelper
    {

        #region 连接字符串
        private static readonly string ACCESS_PROVIDER = ConfigurationManager.AppSettings["AccessProvider"];
        private static readonly string ACCESS_DATA_SOURCE = ConfigurationManager.AppSettings["WebAccess"];
        public static readonly string CONN_STRING = "Provider=" + ACCESS_PROVIDER + ";" + "Data Source=" + System.Web.HttpContext.Current.Server.MapPath(ACCESS_DATA_SOURCE);

        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #endregion

        public AccessHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 执行一条SQL语句 
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static OleDbDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(connString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                OleDbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //				cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connString))
            {               
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();                 
                return val;
            }
        }

        /// <summary>
        /// 返回DataSet 集合
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet ExcuteDataSet(string connString, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            
            OleDbDataAdapter adp = new OleDbDataAdapter();
            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                DataSet ds = new DataSet();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);

                adp.SelectCommand = cmd;
                adp.Fill(ds, "result1");
                cmd.Parameters.Clear();
                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(CONN_STRING))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(SQLString, connection);
                    adapter.Fill(ds, "ds");
                    connection.Close();
                    return ds;
                }
                catch (OleDbException ex)
                {
                    connection.Close();
                    connection.Dispose();                   
                    throw ex;
                }
            }
        }


        public static void CacheParameters(string cacheKey, params OleDbParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// 查找缓存参数
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static OleDbParameter[] GetCachedParameters(string cacheKey)
        {
            OleDbParameter[] cachedParms = (OleDbParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            OleDbParameter[] clonedParms = new OleDbParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OleDbParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        public static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText, OleDbParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
