using System;
using System.Collections.Generic;
using System.Text;
using NetChina.JournalModel;
using NetChina.AccessHelper;
using System.Data.OleDb;
using System.Data;
using NetChina.Common;

namespace NetChina.AccessHelper
{
   public class NetRestDate
    {
        #region SQL 变量



        //获取所以假日所有
        private const string SQL_Select_RestDateAll = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where ( format(RestDate,'yyyy/mm/dd')>=@BeginDate and format(RestDate,'yyyy/mm/dd')<=@EndDate )  Order By RestDate asc";
        //获取所以假日byType
        private const string SQL_Select_RestDateByType = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where ( format(RestDate,'yyyy/mm/dd')>=@BeginDate and format(RestDate,'yyyy/mm/dd')<=@EndDate ) and Type=@Type Order By RestDate asc";
       //获取所以假日ByUserId
       private const string SQL_Select_RestDateByUserId = "SELECT RestID, RestDate, CycLeNum, RestDetail, type, remark, realname, userid FROM tbl_RestDateList WHERE RestDate Between @BeginDate And @EndDate And UserId=@UserId ORDER BY RestDate";
       //获取所以假日ByType and UserId
       private const string SQL_Select_RestDateAllByTypeAndUserId = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where ( format(RestDate,'yyyy/mm/dd')>=@BeginDate and format(RestDate,'yyyy/mm/dd')<=@EndDate ) and Type=@Type and UserId=@UserId  Order By RestDate asc";

       //获取所有假日By Beginand UserId
       private const string SQL_Select_RestDateAllByBeginAndUserId = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where format(RestDate,'yyyy/mm/dd')>=@BeginDate and ( UserId=@AllPerson or UserId=@UserId ) Order By RestDate asc";
       //获取所有假日by begin end
       private const string SQL_Select_RestDateAllByBegin_End = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where ( RestDate Between @BeginDate And @EndDate ) and {0} Order By RestDate asc";
       //判断假日是否存在 ，根据类型
       private const string SQL_Select_RestDateCountByRestDateAndType = "Select count(tbl_NetRestDate.RestID) as resultCount From tbl_RestDateList where format(RestDate,'yyyy/mm/dd')=@RestDate and ( Type='周六' or Type='周日' or Type='法定节假' or Type='其他节假')";
       //判断假日是否存在 ，根据用户id
       private const string SQL_Select_RestDateCountByRestDateAndUserId = "Select count(tbl_NetRestDate.RestID) as resultCount From tbl_RestDateList where format(RestDate,'yyyy/mm/dd')=@RestDate and  UserId =@UserId ";

       //获取所有假日By Begin and end , UserId
       private const string SQL_Select_RestDateAllByBeginAndEndAndUserId = "Select RestDate From tbl_RestDateList  where (RestDateBetween @BeginDate And @EndDate)  and ( UserId=@UserId ) Order By RestDate asc";
       //添加数据的SQL
        private const string SQL_Insert_RestDate = "insert into tbl_NetRestDate(UserId,RestDate,CycleNum,RestDetail,Type,Remark) values (@UserId,@RestDate,@CycleNum,@RestDetail,@Type,@Remark)";

        //更新假日
       private const string SQL_Update_RestDate = "Update tbl_NetRestDate set UserId=@UserId,RestDate=@RestDate,CycleNum=@CycleNum,RestDetail=@RestDetail,Type=@Type,Remark=@Remark Where RestID=@RestID";


       //删除全部假日
       private const string SQL_Delete_RestDate = "delete from tbl_NetRestDate";
       //删除某个假日 by RestID
       private const string SQL_Delete_RestDateByRestID = "delete from tbl_NetRestDate Where RestID=@RestID";

        

        #endregion

        #region 参数变量
       private const string Parm_RestId = "@RestID";                  //假日id
        private const string Parm_UserId = "@UserId";               //人员Id
        private const string Parm_RestDate = "@RestDate";         //假期日期
        private const string Parm_CycleNum = "@CycleNum";   //循环周期天数
        private const string Parm_RestDetail = "@RestDetail";     //假期细节:全天、上午、下午
        private const string Parm_Type = "@Type";           //假期类型
        private const string Parm_Remark = "@Remark";     //备注


        private const string Parm_BeginDate = "@BeginDate";       //搜索开始日期
        private const string Parm_EndDate = "@EndDate";     //搜索结束日期
       private const string parm_AllPerson = "@AllPerson";//表示所有人，即-2
       private const string parm_RestDate = "@RestDate";//要保存的假日日期

        #endregion

        #region 操作方法

        /// <summary>
        /// 添加假日信息
        /// </summary>
        /// <param name="info">传入RestDate对象</param>
        /// <returns></returns>
        public bool Insert(RestDate info)
        {
            bool _Result = true;

           int _count= this.GetRestDateCount(info.Rest_Date.ToShortDateString());
           if (_count > 0) return false;

            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_UserId,OleDbType.Integer),
                                                               new OleDbParameter(Parm_RestDate,OleDbType.Date),
															   new OleDbParameter(Parm_CycleNum,OleDbType.Integer),
															   new OleDbParameter(Parm_RestDetail,OleDbType.VarChar,50),
															   new OleDbParameter(Parm_Type,OleDbType.VarChar,50),
                                                               new OleDbParameter(Parm_Remark,OleDbType.VarChar,100),
														   };
                parms[0].Value = info.UserId;
                parms[1].Value = info.Rest_Date;
                parms[2].Value = info.CycleNum ;
                parms[3].Value = info.RestDetail;
                parms[4].Value = info.Type;
                parms[5].Value = info.Remark;

                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, SQL_Insert_RestDate, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
            }

            return _Result;
        }


       /// <summary>
       /// 获取假日数量，判断假日是否存在
       /// </summary>
       /// <param name="pRestDate"></param>
       /// <returns></returns>
       public int GetRestDateCount(string pRestDate) 
       {
           int resultNum = 0;
           try
           {
               OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(parm_RestDate,OleDbType.VarChar,50)                                                                                                                               							   };
               parms[0].Value = this.GetFullDate(pRestDate);
               //parms[1].Value = pType;



               string sql = SQL_Select_RestDateCountByRestDateAndType;

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
               resultNum = 0;
           }
           return resultNum;
       }
       public DataSet GetUserIdDataSetByRestDate(string pRestDate)
       {
           DataSet objDs = new DataSet("result");
           string _sqlstr = string.Empty;
           OleDbParameter[] parms = null;
           /*parms = new OleDbParameter[] {
                                                               new OleDbParameter(Parm_BeginDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_EndDate,OleDbType.VarChar)
                                                                };
           parms[0].Value = this.GetFullDate(pBeginTime);
           parms[1].Value = this.GetFullDate(pEndTime);

           */
           /*
           if (pIsAddAll)
           {
               _sqlstr = string.Format(SQL_Select_RestDateAllByBegin_End, "( UserId= " + pUserId.ToString() + " or UserId=-1 )");
           }
           else
           {
               _sqlstr = string.Format(SQL_Select_RestDateAllByBegin_End, "( UserId= " + pUserId.ToString() + " )");
           }*/
           _sqlstr = string.Format(SQL_Select_RestDateAll, pRestDate);
           try
           {
               objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, _sqlstr, parms);

           }
           catch (Exception ex)
           {

           }

           return objDs;
       }

    /// <summary>
       /// 获取假日数量，判断假日是否存在
    /// </summary>
    /// <param name="pRestDate">假日</param>
    /// <param name="pUserId">用户id</param>
    /// <returns></returns>
 
       public int GetRestDateCountByUserId(string pRestDate,int pUserId)
       {
           int resultNum = 0;
           try
           {
               OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(parm_RestDate,OleDbType.VarChar,50),
                                                               new OleDbParameter(Parm_UserId,OleDbType.Integer),
                                                                };
               parms[0].Value = this.GetFullDate(pRestDate);
               parms[1].Value = pUserId;



               string sql = SQL_Select_RestDateCountByRestDateAndUserId;

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
               resultNum = 0;
           }
           return resultNum;
       }

    /// <summary>
    /// 根据假日ID，更新假日
    /// </summary>
    /// <param name="info">要更新的假日对象</param>
    /// <returns></returns>
       public bool Update(RestDate info)
       {
           bool _Result = true;
           try
           {
               OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_UserId,OleDbType.Integer),
                                                               new OleDbParameter(Parm_RestDate,OleDbType.Date),
															   new OleDbParameter(Parm_CycleNum,OleDbType.Integer),
															   new OleDbParameter(Parm_RestDetail,OleDbType.VarChar,50),
															   new OleDbParameter(Parm_Type,OleDbType.VarChar,50),
                                                               new OleDbParameter(Parm_Remark,OleDbType.VarChar,100),
                                                               new OleDbParameter(Parm_RestId,OleDbType.Integer),
														   };
               parms[0].Value = info.UserId;
               parms[1].Value = info.Rest_Date;
               parms[2].Value = info.CycleNum;
               parms[3].Value = info.RestDetail;
               parms[4].Value = info.Type;
               parms[5].Value = info.Remark;
               parms[6].Value = info.RestID;

               AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text,SQL_Update_RestDate, parms);
           }
           catch (Exception ex)
           {
               _Result = false;
               throw ex;
           }

           return _Result;
       }



        /// <summary>
        /// 删除假日
        /// </summary>
        /// <param name="pEnum">操作类型（全删，按id删，按类型删）</param>
        /// <param name="pParmValue">RestID 或者是Type</param>
        /// <returns></returns>
       public bool Delete(int[] pArrayId)
       {
           if (pArrayId.Length == 0) return false;

           bool _Result = true;

       
           try
           {
               for (int i = 0; i < pArrayId.Length; i++)
               {
                   OleDbParameter[] _param = new OleDbParameter[] { new OleDbParameter(Parm_RestId, OleDbType.Integer) };
                   _param[0].Value = pArrayId[i];
                   AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, SQL_Delete_RestDateByRestID, _param);
               }
           }
           catch (Exception ex)
           {
               _Result = false;
           }
           return _Result;
       }


      ///// <summary>
      // /// 获取假日,不统计所有人，只统计当前用户
      ///// </summary>
      ///// <param name="pBeginTime"></param>
      ///// <param name="pEndTime"></param>
      ///// <param name="pUserid"></param>
      ///// <returns></returns>
      // public DataSet GetRestDateDataSet(string pBeginTime, string pEndTime, int pUserid)
      // {
      //     DataSet _ds = new DataSet("result");
      //     OleDbParameter[] parms = new OleDbParameter[] {
      //                                                         new OleDbParameter(Parm_BeginDate,OleDbType.VarChar,50),
      //                                                          new OleDbParameter(Parm_EndDate,OleDbType.VarChar,50),
      //                                                         new OleDbParameter(Parm_UserId,OleDbType.Integer)
      //                                                          };
      //     parms[0].Value = this.GetFullDate(pBeginTime);
      //     parms[1].Value = this.GetFullDate(pEndTime);
      //     parms[2].Value = pUserId;
      // }

       /// <summary>
       /// 获取假日
       /// </summary>
       /// <param name="pBeginTime">开始日期</param>
       /// <param name="pEndTime">结束日期</param>
       /// <param name="pUserId">用户Id</param>
       /// <param name="pIsAddAll">是否包含全部公用假日</param>
       /// <returns></returns>
       public DataSet GetRestDateDataSet(string pBeginTime, string pEndTime, int pUserId,bool pIsAddAll)
       {
           DataSet objDs = new DataSet("result");
           string _sqlstr = string.Empty;
           OleDbParameter[] parms = null;
               parms = new OleDbParameter[] {
                                                               new OleDbParameter(Parm_BeginDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_EndDate,OleDbType.VarChar)
                                                                };
               parms[0].Value = this.GetFullDate(pBeginTime);
               parms[1].Value = this.GetFullDate(pEndTime);
             

               if (pIsAddAll)
               {
                   _sqlstr =string.Format(SQL_Select_RestDateAllByBegin_End , "( UserId= "+pUserId.ToString()+" or UserId=-1 )");
               }
               else
               {
                   _sqlstr = string.Format(SQL_Select_RestDateAllByBegin_End , "( UserId= " + pUserId.ToString() + " )");
               }
               try
               {
                   objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, _sqlstr, parms);

               }
               catch (Exception ex)
               {

               }

               return objDs;
         
       }
      /// <summary>
       ///  获取假日
      /// </summary>
      /// <param name="pBeginTime">开始日期</param>
      /// <param name="pEndTime">结束日期</param>
      /// <param name="pType">假日类型，若想查全部，输入“请选择”</param>
      /// <param name="pUserId">用户Id，若想查所有人，输入“-2”</param>
      /// <returns></returns>
       public DataSet GetRestDateDataSet(string pBeginTime,string pEndTime,string pType,int pUserId)
       {
           DataSet objDs = new DataSet("result");
           string _sqlstr = string.Empty;
           OleDbParameter[] parms = null;

           if (pEndTime==string.Empty )
           {
               parms = new OleDbParameter[] {
                                                               new OleDbParameter(Parm_BeginDate,OleDbType.VarChar),
                                                                new OleDbParameter(Parm_UserId,OleDbType.Integer),
                                                               new OleDbParameter(Parm_UserId,OleDbType.Integer)
                                                                };
               parms[0].Value=this.GetFullDate(pBeginTime);
               parms[1].Value = -1;
               parms[2].Value=pUserId;
               _sqlstr=SQL_Select_RestDateAllByBeginAndUserId;
               try
               {
                   objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, _sqlstr, parms);

               }
               catch (Exception ex)
               {

               }

               return objDs;
           }

           if (pType!="请选择" && pUserId!=-2)
           {
               parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_BeginDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_EndDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_Type,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_UserId,OleDbType.Integer)
                                                           };
               parms[0].Value = this.GetFullDate(pBeginTime);
               parms[1].Value = this.GetFullDate(pEndTime);
               parms[2].Value = pType;
               parms[3].Value = pUserId;
               _sqlstr = SQL_Select_RestDateAllByTypeAndUserId;

           }
           else if (pType == "请选择" && pUserId != -2)
           {
               parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_BeginDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_EndDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_UserId,OleDbType.Integer)
                                                           };
               parms[0].Value = this.GetFullDate(pBeginTime);
               parms[1].Value = this.GetFullDate(pEndTime);
               parms[2].Value = pUserId;
               _sqlstr = SQL_Select_RestDateByUserId;
           }
           else if (pType != "请选择" && pUserId == -2)
           {
               parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_BeginDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_EndDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_Type,OleDbType.VarChar)
                                                           };
               parms[0].Value = this.GetFullDate(pBeginTime);
               parms[1].Value = this.GetFullDate(pEndTime);
               parms[2].Value = pType;
               _sqlstr = SQL_Select_RestDateByType;
           }
           else
           {
               parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_BeginDate,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_EndDate,OleDbType.VarChar)
                                                           };
               parms[0].Value = this.GetFullDate(pBeginTime);
               parms[1].Value = this.GetFullDate(pEndTime);
               _sqlstr = SQL_Select_RestDateAll;
           }
           try
           {
               objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, _sqlstr, parms);
               
           }
           catch (Exception ex)
           {

           }
      
           return objDs;
       }


       /// <summary>
       /// 将一个日期字符串，如果月份或日期小于10 ，将前面补一个0
       /// </summary>
       /// <param name="pDate"></param>
       /// <returns></returns>
       public string GetFullDate(string pDate)
       {
           DateTime _date = Convert.ToDateTime(pDate);
           string _full = _date.ToString("yyyy-M-d");
           if (_date.Month<10)
           {
               _full = _full.Insert(pDate.IndexOf("-") + 1, "0");
           }
           if (_date.Day<10)
           {
               _full = _full.Insert(_full.LastIndexOf("-") + 1, "0");
           }
           return _full;
       }


       /// <summary>
       /// 将一个日期字符串，如果月份或日期小于10 ，将前面补一个0
       /// </summary>
       /// <param name="pDate"></param>
       /// <returns></returns>
       public static  string GetFullDateString(string pDate)
       {
           NetRestDate _net = new NetRestDate();
           return _net.GetFullDate(pDate);
       }
        #endregion
    }
} 
