using System;
using System.Collections.Generic;
using System.Text;
using NetChina.JournalModel;
using System.Data.OleDb;
using System.Data;

namespace NetChina.AccessHelper
{
    public class NetFileManager
    {
        #region SQL变量  

        //第一页的内容
        private const string SQL_Select_FileFirst = "Select top {0} * From tbl_FileList where format(WriteDate,'yyyy/mm/dd')>=@WriteDate and format(WriteDate,'yyyy/mm/dd')<=@EndTime {1} Order By userOrder desc,FileId desc";//现在的页码*每页显示的条数
        //根据条件获取分页总数
        private const string SQL_Select_FileCount = "Select count(*) as resultCount From tbl_FileList where format(WriteDate,'yyyy/mm/dd')>=@WriteDate and format(WriteDate,'yyyy/mm/dd')<=@EndTime {0}";
        //第二页以后的内容
        private const string SQL_Select_FileList = "Select top {0} * From tbl_FileList where FileId not in (select top {1} FileId from tbl_FileList where format(writedate,'yyyy/mm/dd')>=@WriteDate and format(writedate,'yyyy/mm/dd')<=@EndDate {2} order by userOrder desc,FileId desc) and format(writedate,'yyyy/mm/dd')>=@WriteDate and format(writedate,'yyyy/mm/dd')<=@EndDate {3} Order By userOrder Desc,FileId desc";
        //private const string SQL_Select_FileList = "Select *  From tbl_FileList where format(WriteDate,'yyyy/mm/dd')>=@WriteDate and format(WriteDate,'yyyy/mm/dd')<=@EndTime {0} Order By userOrder Desc,FileId desc";
        //删除文件
        private const string SQL_Delete_File = "delete from tbl_NetFileManager where FileId=@FileId and UserId=@UserId";
        //插入文件
        private const string SQL_Insert_File = "insert into tbl_NetFileManager (UserId,FileTitle,FileDesc,FileType,FilePath) values (@UserId,@FileTitle,@FileDesc,@FileType,@FilePath)";


        #endregion

        #region 参数变量

        private const string Parm_FileId = "@FileId";       //文件编号
        private const string Parm_UserId = "@UserId";       //用户编号
        private const string Parm_FileTitle = "@FileTitle"; //文件标题
        private const string Parm_FileDesc = "@FileDesc";   //文件备注
        private const string Parm_FileType = "@FileType";   //文件类型
        private const string Parm_FilePath = "@FilePath";   //文件存放路径
        private const string Parm_WriteDate = "@WriteDate"; //上传日期
        private const string Parm_EndTime = "@EndTime";     //结束日期

        private const string Parm_KeyWord = "@KeyWord";         //关键字
        private const string Parm_PageSize = "@PageSize";       //每页大小

        #endregion 

        #region 方法列表

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="info">传入FileManager对象</param>
        /// <returns></returns>
        public bool Insert(FileManager info)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_UserId,OleDbType.Integer),
															   new OleDbParameter(Parm_FileTitle,OleDbType.VarChar,255),
															   new OleDbParameter(Parm_FileDesc,OleDbType.LongVarWChar),
															   new OleDbParameter(Parm_FileType,OleDbType.VarChar,100),
                                                               new OleDbParameter(Parm_FilePath,OleDbType.VarChar,255)                                                             
														   };
                parms[0].Value = info.UserId;
                parms[1].Value = info.FileTitle;
                parms[2].Value = info.FileDesc;
                parms[3].Value = info.FileType;
                parms[4].Value = info.FilePath;

                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, SQL_Insert_File, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
            }

            return _Result;
        }


        /// <summary>
        /// 删除文件信息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="FileId">文件ID</param>
        /// <returns></returns>
        public bool Delete(string UserId, string FileId)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_FileId,OleDbType.VarChar),
															   new OleDbParameter(Parm_UserId,OleDbType.VarChar)															                                                             
														   };
                parms[0].Value = FileId;
                parms[1].Value = UserId;               

                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, SQL_Delete_File, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
            }
            return _Result;
        }

        /// <summary>
        /// 获取分页列表内容
        /// </summary>
        /// <param name="DocType">文档类型</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="pageSize">当前页大小</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="KeyWord">标题关键字</param>
        /// <returns></returns>
        public DataSet GetFileDataSet(string DocType, string BeginTime, string EndTime, int pageSize, int currentPage, string KeyWord)
        {
            string tempSqlStr = "";
            DataSet objDs = new DataSet("result");
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_WriteDate,OleDbType.VarChar),
					                                           new OleDbParameter(Parm_EndTime,OleDbType.VarChar)					                                                                                                                          				   
                                                           };
                parms[0].Value = BeginTime;
                parms[1].Value = EndTime;

                string sql = string.Empty;

                
                tempSqlStr += " and FileTitle like '%" + KeyWord + "%'";

                if (DocType != "全部")
                {
                    tempSqlStr += " and FileType='" + DocType + "'";
                }

                if (currentPage == 1)
                {
                    sql = string.Format(SQL_Select_FileFirst, pageSize, tempSqlStr, tempSqlStr);
                }
                else
                {
                    sql = string.Format(SQL_Select_FileList, pageSize, pageSize * (currentPage - 1), tempSqlStr, tempSqlStr);
                }

                objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);
            }
            catch (Exception ex)
            {
            }
            return objDs;
        }

        /// <summary>
        /// 获取分页的总个数
        /// </summary>
        /// <param name="DocType">文档类别</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="KeyWord">标题关键字</param>
        /// <returns></returns>
        public int GetFileFYCount(string DocType, string BeginTime, string EndTime, string KeyWord)
        {
            int resultNum = 0;
            string tempSqlStr = "";
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_WriteDate,OleDbType.VarChar,200),                                                                              
                                                               new OleDbParameter(Parm_EndTime,OleDbType.VarChar,200)                                                                                                       
														   };
                parms[0].Value = BeginTime;
                parms[1].Value = EndTime;

                tempSqlStr += " and FileTitle like '%" + KeyWord + "%'";

                if (DocType != "全部")
                {
                    tempSqlStr += " and FileType='"+DocType+"'";
                }

                string sql = string.Format(SQL_Select_FileCount, tempSqlStr);

                using (OleDbDataReader sdr = AccessHelper.ExecuteReader(AccessHelper.CONN_STRING, CommandType.Text, sql, parms))
                {
                    while (sdr.Read())
                    {
                        resultNum = int.Parse(sdr["resultCount"].ToString());  //表示普通                        
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return resultNum;
        }
        #endregion
    }
}
