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
        #region SQL ����



        //��ȡ���Լ�������
        private const string SQL_Select_RestDateAll = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where ( format(RestDate,'yyyy/mm/dd')>=@BeginDate and format(RestDate,'yyyy/mm/dd')<=@EndDate )  Order By RestDate asc";
        //��ȡ���Լ���byType
        private const string SQL_Select_RestDateByType = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where ( format(RestDate,'yyyy/mm/dd')>=@BeginDate and format(RestDate,'yyyy/mm/dd')<=@EndDate ) and Type=@Type Order By RestDate asc";
       //��ȡ���Լ���ByUserId
       private const string SQL_Select_RestDateByUserId = "SELECT RestID, RestDate, CycLeNum, RestDetail, type, remark, realname, userid FROM tbl_RestDateList WHERE RestDate Between @BeginDate And @EndDate And UserId=@UserId ORDER BY RestDate";
       //��ȡ���Լ���ByType and UserId
       private const string SQL_Select_RestDateAllByTypeAndUserId = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where ( format(RestDate,'yyyy/mm/dd')>=@BeginDate and format(RestDate,'yyyy/mm/dd')<=@EndDate ) and Type=@Type and UserId=@UserId  Order By RestDate asc";

       //��ȡ���м���By Beginand UserId
       private const string SQL_Select_RestDateAllByBeginAndUserId = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where format(RestDate,'yyyy/mm/dd')>=@BeginDate and ( UserId=@AllPerson or UserId=@UserId ) Order By RestDate asc";
       //��ȡ���м���by begin end
       private const string SQL_Select_RestDateAllByBegin_End = "Select RestID,RestDate,CycLeNum,RestDetail,type,remark,realname,userid From tbl_RestDateList  where ( RestDate Between @BeginDate And @EndDate ) and {0} Order By RestDate asc";
       //�жϼ����Ƿ���� ����������
       private const string SQL_Select_RestDateCountByRestDateAndType = "Select count(tbl_NetRestDate.RestID) as resultCount From tbl_RestDateList where format(RestDate,'yyyy/mm/dd')=@RestDate and ( Type='����' or Type='����' or Type='�����ڼ�' or Type='�����ڼ�')";
       //�жϼ����Ƿ���� �������û�id
       private const string SQL_Select_RestDateCountByRestDateAndUserId = "Select count(tbl_NetRestDate.RestID) as resultCount From tbl_RestDateList where format(RestDate,'yyyy/mm/dd')=@RestDate and  UserId =@UserId ";

       //��ȡ���м���By Begin and end , UserId
       private const string SQL_Select_RestDateAllByBeginAndEndAndUserId = "Select RestDate From tbl_RestDateList  where (RestDateBetween @BeginDate And @EndDate)  and ( UserId=@UserId ) Order By RestDate asc";
       //������ݵ�SQL
        private const string SQL_Insert_RestDate = "insert into tbl_NetRestDate(UserId,RestDate,CycleNum,RestDetail,Type,Remark) values (@UserId,@RestDate,@CycleNum,@RestDetail,@Type,@Remark)";

        //���¼���
       private const string SQL_Update_RestDate = "Update tbl_NetRestDate set UserId=@UserId,RestDate=@RestDate,CycleNum=@CycleNum,RestDetail=@RestDetail,Type=@Type,Remark=@Remark Where RestID=@RestID";


       //ɾ��ȫ������
       private const string SQL_Delete_RestDate = "delete from tbl_NetRestDate";
       //ɾ��ĳ������ by RestID
       private const string SQL_Delete_RestDateByRestID = "delete from tbl_NetRestDate Where RestID=@RestID";

        

        #endregion

        #region ��������
       private const string Parm_RestId = "@RestID";                  //����id
        private const string Parm_UserId = "@UserId";               //��ԱId
        private const string Parm_RestDate = "@RestDate";         //��������
        private const string Parm_CycleNum = "@CycleNum";   //ѭ����������
        private const string Parm_RestDetail = "@RestDetail";     //����ϸ��:ȫ�졢���硢����
        private const string Parm_Type = "@Type";           //��������
        private const string Parm_Remark = "@Remark";     //��ע


        private const string Parm_BeginDate = "@BeginDate";       //������ʼ����
        private const string Parm_EndDate = "@EndDate";     //������������
       private const string parm_AllPerson = "@AllPerson";//��ʾ�����ˣ���-2
       private const string parm_RestDate = "@RestDate";//Ҫ����ļ�������

        #endregion

        #region ��������

        /// <summary>
        /// ��Ӽ�����Ϣ
        /// </summary>
        /// <param name="info">����RestDate����</param>
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
       /// ��ȡ�����������жϼ����Ƿ����
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
                       resultNum = int.Parse(sdr["resultCount"].ToString());  //��ʾ��ͨ                        
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
       /// ��ȡ�����������жϼ����Ƿ����
    /// </summary>
    /// <param name="pRestDate">����</param>
    /// <param name="pUserId">�û�id</param>
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
                       resultNum = int.Parse(sdr["resultCount"].ToString());  //��ʾ��ͨ                        
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
    /// ���ݼ���ID�����¼���
    /// </summary>
    /// <param name="info">Ҫ���µļ��ն���</param>
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
        /// ɾ������
        /// </summary>
        /// <param name="pEnum">�������ͣ�ȫɾ����idɾ��������ɾ��</param>
        /// <param name="pParmValue">RestID ������Type</param>
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
      // /// ��ȡ����,��ͳ�������ˣ�ֻͳ�Ƶ�ǰ�û�
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
       /// ��ȡ����
       /// </summary>
       /// <param name="pBeginTime">��ʼ����</param>
       /// <param name="pEndTime">��������</param>
       /// <param name="pUserId">�û�Id</param>
       /// <param name="pIsAddAll">�Ƿ����ȫ�����ü���</param>
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
       ///  ��ȡ����
      /// </summary>
      /// <param name="pBeginTime">��ʼ����</param>
      /// <param name="pEndTime">��������</param>
      /// <param name="pType">�������ͣ������ȫ�������롰��ѡ��</param>
      /// <param name="pUserId">�û�Id������������ˣ����롰-2��</param>
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

           if (pType!="��ѡ��" && pUserId!=-2)
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
           else if (pType == "��ѡ��" && pUserId != -2)
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
           else if (pType != "��ѡ��" && pUserId == -2)
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
       /// ��һ�������ַ���������·ݻ�����С��10 ����ǰ�油һ��0
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
       /// ��һ�������ַ���������·ݻ�����С��10 ����ǰ�油һ��0
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
