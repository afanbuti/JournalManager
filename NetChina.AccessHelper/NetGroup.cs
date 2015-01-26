using System;
using System.Collections.Generic;
using System.Text;
using NetChina.JournalModel;
using System.Data.OleDb;
using System.Data;

namespace NetChina.AccessHelper
{
    public class NetGroup
    {
        #region SQL变量

        private const string SQL_Select_GroupInfo = "select groupid,groupname from tbl_NetGroupInfo";

        #endregion

        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <returns></returns>
        public GroupInfo GetGroupInfo()
        {
            GroupInfo info = null;
            try
            {                
                using (OleDbDataReader sdr = AccessHelper.ExecuteReader(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_GroupInfo, null))
                {
                    while (sdr.Read())
                    {
                        info = new GroupInfo(
                            int.Parse(sdr["GroupId"].ToString()),
                            sdr["GroupName"].ToString()                            
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info;
        }

        /// <summary>
        /// 获取用户的集合信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="inUse"></param>
        /// <returns></returns>
        public DataSet GetGroupList()
        {
            DataSet objDs = null;
            try
            {

                //objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_GroupInfo, null);
                objDs = AccessHelper.GetDataSet(SQL_Select_GroupInfo);
            }
            catch (Exception ex)
            {
            }
            return objDs;
        }       
    }   
}
