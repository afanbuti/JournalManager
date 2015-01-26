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
    public class NetUserMission
    {
        #region SQL 变量
        //获取总数
        private const string SQL_Select_MissionCount = "Select count(*) as resultCount From tbl_NetUserMission where format(writedate,'yyyy/mm/dd')>=@WriteDate and format(writedate,'yyyy/mm/dd')<=@EndDate {0}";
        //分页第一页的数据
        private const string SQL_Select_MissionFirst = "Select top {0} MissionId,realName,userOrder,UserId,MissionTitle, MissionDesc,WriteDate,ExecDate,FinisthDate,ExecStatus,IsAsigin,MissionType,FilePath,WorkHour,RealProcess From tbl_mission where format(writedate,'yyyy/mm/dd')>=@WriteDate and format(writedate,'yyyy/mm/dd')<=@EndDate {1} Order By userOrder desc,missionId desc";//现在的页码*每页显示的条数

        //private const string SQL_Select_MissionByStatus = "Select * From tbl_mission where format(writedate,'yyyy/mm/dd')>=@WriteDate and format(writedate,'yyyy/mm/dd')<=@EndDate and ExecStatus='1' and UserId=@UserId Order By userOrder desc,missionId desc";//现在的页码*每页显示的条数
        private const string SQL_Select_MissionByStatus = "Select  MissionId,realName,userOrder,UserId,MissionTitle, MissionDesc,WriteDate,ExecDate,FinisthDate,ExecStatus,IsAsigin,MissionType,FilePath,WorkHour,RealProcess From tbl_mission where ExecStatus='1' and UserId=@UserId Order By userOrder desc,missionId desc";//现在的页码*每页显示的条数


        private const string SQL_Select_MissionList = "Select MissionId,realName,userOrder,UserId,MissionTitle, MissionDesc,WriteDate,ExecDate,FinisthDate,ExecStatus,IsAsigin,MissionType,FilePath,WorkHour,RealProcess From tbl_mission where format(writedate,'yyyy/mm/dd')>=@WriteDate and format(writedate,'yyyy/mm/dd')<=@EndDate {0} Order By userOrder Desc,missionId desc";

        //分页第一页以后的数据
        private const string SQL_Select_MissionInfo = "Select top {0} MissionId,realName,userOrder,UserId,MissionTitle, MissionDesc,WriteDate,ExecDate,FinisthDate,ExecStatus,IsAsigin,MissionType,FilePath,WorkHour,RealProcess From tbl_mission where missionId not in (select top {1} missionId from tbl_mission where format(writedate,'yyyy/mm/dd')>=@WriteDate and format(writedate,'yyyy/mm/dd')<=@EndDate {2} order by userOrder desc,missionId desc) and format(writedate,'yyyy/mm/dd')>=@WriteDate and format(writedate,'yyyy/mm/dd')<=@EndDate {3} Order By userOrder Desc,missionId desc";
        //添加数据的SQL
        private const string SQL_Insert_Mission = "insert into tbl_NetUserMission(UserId,MissionTitle,MissionDesc,WriteDate,ExecStatus,MissionType,FilePath,WorkHour) values (@UserId,@MissionTitle,@MissionDesc,@WriteDate,@ExecStatus,@MissionType,@FilePath,@WorkHour)";

        //更新正在执行信息
        private const string SQL_Update_MissionDoing = "Update tbl_NetUserMission set ExecDate=@ExecDate,ExecStatus='1' Where MissionId={0} and UserId={1}";
        //更新执行完成信息
        private const string SQL_Update_MissionFinish = "Update tbl_NetUserMission set FinisthDate=@FinisthDate,ExecStatus='2' Where MissionId={0} and UserId={1}";

        private const string SQL_Update_MissionUserId = "Update tbl_NetUserMission set UserId=@UserId,writedate=@WriteDate {1} Where MissionId={0}";

        private const string SQL_Update_MissionDesc = "Update tbl_NetUserMission set MissionDesc=@MissionDesc,MissionType=@MissionType, WorkHour=@WorkHour, RealProcess=@RealProcess Where MissionId={0}";
        //更新计划工时
        private const string SQL_Update_WorkHour = "Update tbl_NetUserMission set WorkHour=@WorkHour Where MissionId=@MissionId";
        //更新实际工作进度
        private const string SQL_Update_RealProcess = "Update tbl_NetUserMission set RealProcess=@RealProcess Where MissionId={0}";



        //删除某个任务
        private const string SQL_Delete_UserJournal = "delete from tbl_NetUserMission Where MissionId={0} and UserId={1}";
        //获取单个任务信息
        private const string SQL_Select_MissionDetail = "select * from tbl_NetUserMission where MissionId=@MissionId";


        #endregion

        #region 参数变量

        private const string Parm_MissionId = "@MissionId";         //任务ID
        private const string Parm_UserId = "@UserId";               //用户ID
        private const string Parm_MissionTitle = "@MissionTitle";   //任务标题
        private const string Parm_MissionDesc = "@MissionDesc";     //任务描述
        private const string Parm_WorkHour = "@WorkHour";     //任务计划工时
        private const string Parm_RealProcess = "@RealProcess";     //实际工作进度
        private const string Parm_WriteDate = "@WriteDate";         //写入日期
        private const string Parm_ExecDate = "@ExecDate";           //开始执行日期
        private const string Parm_FinisthDate = "@FinisthDate";     //完成日期
        private const string Parm_ExecStatus = "@ExecStatus";       //执行状态
        private const string Parm_MissionType = "@MissionType";     //任务类型
        private const string Parm_FilePath = "@FilePath";           //文件路径

        private const string Parm_KeyWord = "@KeyWord";             //关键字
        private const string Parm_EndDate = "@EndDate";             //搜索结束时间

        #endregion

        #region 操作方法

        /// <summary>
        /// 添加用户任务信息
        /// </summary>
        /// <param name="info">传入UserMission对象</param>
        /// <returns></returns>
        public bool Insert(UserMission info)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_UserId,OleDbType.Integer),
                                                               new OleDbParameter(Parm_MissionTitle,OleDbType.VarChar,200),
															   new OleDbParameter(Parm_MissionDesc,OleDbType.LongVarWChar),
															   new OleDbParameter(Parm_WriteDate,OleDbType.Date),
															   new OleDbParameter(Parm_ExecStatus,OleDbType.VarChar,50),
                                                               new OleDbParameter(Parm_MissionType,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_FilePath,OleDbType.VarChar),
                                                               new OleDbParameter(Parm_WorkHour,OleDbType.Integer) 
														   };
                parms[0].Value = info.UserID;
                parms[1].Value = info.MissionTitle;
                parms[2].Value = info.MissionDesc;
                parms[3].Value = info.WriteDate;
                parms[4].Value = info.ExecStatus;
                parms[5].Value = info.MissionType;
                parms[6].Value = info.FilePath;
                parms[7].Value = info.WorkHour;
                //parms[8].Value = string.Empty;

                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, SQL_Insert_Mission, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
            }

            return _Result;
        }

        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="missionId">任务编号</param>
        /// <param name="userid">用户编号</param>
        /// <param name="isFinish">是否完成的任务</param>
        /// <returns></returns>
        public bool UpdateSome(int missionId, int userid, bool isFinish)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = null;
                string sql = string.Empty;

                if (isFinish)
                {
                    parms = new OleDbParameter[]{															   
															   new OleDbParameter(Parm_FinisthDate,OleDbType.Date)													   
															                                                                                                    
														   };
                    sql = string.Format(SQL_Update_MissionFinish, missionId, userid);
                }
                else
                {
                    parms = new OleDbParameter[]{															   
															   new OleDbParameter(Parm_ExecDate,OleDbType.Date)													   
															                                                                                                    
														   };
                    sql = string.Format(SQL_Update_MissionDoing, missionId, userid);
                }

                parms[0].Value = System.DateTime.Now;
                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
                throw ex;
            }

            return _Result;
        }

        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="missionId">任务编号</param>
        /// <param name="userid">用户编号</param>
        /// <param name="isFinish">是否完成的任务</param>
        /// <returns></returns>
        public bool UpdateUser(int missionId, int userid)
        {
            bool _Result = true;
            try
            {

                OleDbParameter[] parms = new OleDbParameter[]{															   
															   new OleDbParameter(Parm_UserId,OleDbType.Integer),
					                                           new OleDbParameter(Parm_WriteDate,OleDbType.Date)								   
															                                                                                                    
				
                };
                string sql=string.Empty;
                string tempUpdate=string.Empty;

                //if (userid.ToString() == "0")
                //{
                    tempUpdate = ",ExecDate=Null,FinisthDate=Null,ExecStatus='0'"; 
                //}                
                sql = string.Format(SQL_Update_MissionUserId, missionId, tempUpdate);

                parms[0].Value = userid;
                parms[1].Value = System.DateTime.Now.ToString();

                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
                throw ex;
            }

            return _Result;
        }

        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="missionId">任务编号</param>
        /// <param name="userid">用户编号</param>
        /// <param name="isFinish">是否完成的任务</param>
        /// <returns></returns>
        public bool UpdateDesc(string missionId, string Desc, string sType, string sWorkHour,string sRealProcess)
        {
            bool _Result = true;
            try
            {

                OleDbParameter[] parms = new OleDbParameter[]{															   
															   new OleDbParameter(Parm_MissionDesc,OleDbType.LongVarWChar),
					                                           new OleDbParameter(Parm_MissionType,OleDbType.VarChar),								   
															    new OleDbParameter(Parm_WorkHour,OleDbType.Integer),                                                                                                                                      new OleDbParameter(Parm_RealProcess,OleDbType.VarChar)
														   };
                string sql = string.Format(SQL_Update_MissionDesc, missionId);

                parms[0].Value = Desc;
                parms[1].Value = sType;
                parms[2].Value = int.Parse(sWorkHour);
                parms[3].Value = sRealProcess;

                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
                throw ex;
            }

            return _Result;
        }



        /// <summary>
        /// 更新计划工时
        /// </summary>
        /// <param name="pMissionId"></param>
        /// <param name="pWorkHour"></param>
        /// <returns></returns>
        public bool UpdateWorkHour(int pMissionId, int pWorkHour)
        {
            bool _Result = true;
            try
            {
                
                OleDbParameter[] parms = new OleDbParameter[]{															   
															   new OleDbParameter(Parm_MissionId,OleDbType.Integer),
					                                           new OleDbParameter(Parm_WorkHour,OleDbType.Integer)								   
														   };
                parms[0].Value = pMissionId;
                parms[1].Value = pWorkHour;

                string sql = SQL_Update_WorkHour;
                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
                throw ex;
            }

            return _Result;
        }



        /// <summary>
        /// 更新实际工作进度
        /// </summary>
        /// <param name="pMissionId"></param>
        /// <param name="pWorkHour"></param>
        /// <returns></returns>
        public bool UpdateRealProcess(int pMissionId, string pRealProcess)
        {
            bool _Result = true;
            OleDbParameter[] parms = null;
            try
            {

               parms = new OleDbParameter[]{															   
                                                               //new OleDbParameter(Parm_MissionId,OleDbType.Integer),
                                                               new OleDbParameter(Parm_RealProcess,OleDbType.VarChar,50)								   
                                                           };
                //parms[0].Value = pMissionId;
                parms[0].Value = pRealProcess;

                string sql = string.Format(SQL_Update_RealProcess, pMissionId) ;
             
                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
                throw ex;
            }

            return _Result;
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="UserId">根据用户ID</param>
        /// <returns></returns>
        public bool Delete(int UserId, int MissionId)
        {
            bool _Result = true;
            try
            {
                string sql = string.Format(SQL_Delete_UserJournal, MissionId, UserId);
                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, sql, null);
            }
            catch (Exception ex)
            {
                _Result = false;
            }
            return _Result;
        }

        /// <summary>
        /// 获取用户的单个日志信息
        /// </summary>
        /// <param name="teachId"></param>
        /// <returns></returns>
        public UserMission GetNetMissionInfo(string mid)
        {

            UserMission info = null;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{									
                                    new OleDbParameter(Parm_MissionId,OleDbType.VarChar,50)
														   };
                parms[0].Value = mid;
                using (OleDbDataReader sdr = AccessHelper.ExecuteReader(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_MissionDetail, parms))
                {
                    while (sdr.Read())
                    {
                        info = new UserMission();
                        info.MissionID = int.Parse(sdr["MissionId"].ToString());
                        info.UserID = int.Parse(sdr["userid"].ToString());
                        info.MissionDesc = commonFun.replaceStr(sdr["MissionDesc"].ToString());
                        info.MissionType = sdr["MissionType"].ToString();
                        info.FilePath = sdr["FilePath"].ToString();

                        if (sdr["WorkHour"] == null || sdr["WorkHour"].ToString() == string.Empty)
                            info.WorkHour = 0;
                        else
                        info.WorkHour =int.Parse(sdr["WorkHour"].ToString());

                        string _realProcess = sdr["RealProcess"].ToString();
                        if (_realProcess!=string.Empty)
                        {
                            _realProcess=_realProcess.Replace("%", "");
                        }
                        info.RealProcess = _realProcess;
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
        /// 根据用户编号获取执行中的任务
        /// </summary>
        /// <param name="userid">用户编号</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public DataSet GetDataSetByStatus(string userid, string beginTime, string endTime)
        {
            DataSet objDs = new DataSet("result");
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
                                                               //new OleDbParameter(Parm_WriteDate,OleDbType.VarChar),
                                                               //new OleDbParameter(Parm_EndDate,OleDbType.VarChar),
					                                           new OleDbParameter(Parm_UserId,OleDbType.VarChar)                                                                                                                          				   
                                                           };
                //parms[0].Value = beginTime;
                //parms[1].Value = endTime;
                parms[0].Value = userid;

                objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_MissionByStatus, parms);

            }
            catch (Exception ex)
            {
              
            }
            //return objDs;
            return this.CountFinishDateByWorkHour(objDs);

        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="uMission">传入UserMission对象</param>
        /// <returns></returns>
        public DataSet GetMissionDataSet(string userid, string status, string beginTime, string endTime, int pageSize, int currentPage, string IsAsgin,bool IsSearch,string KeyWord,string typeMission)
        {
            string tempSqlStr = "";
            DataSet objDs = new DataSet("result");
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_WriteDate,OleDbType.VarChar),
					                                           new OleDbParameter(Parm_EndDate,OleDbType.VarChar)					                                                                                                                          				   
                                                           };
                parms[0].Value = beginTime;
                parms[1].Value = endTime;

                string sql = string.Empty;

                if (IsAsgin == "1") //已经分配
                {
                    if (userid != "-1")
                    {
                        tempSqlStr += " and userid=" + userid + "";
                    }
                    else
                    {
                        tempSqlStr += " and userid<>0";
                    }
                }
                else
                {
                    tempSqlStr += " and userid=0";
                }
                if (status != "-1")
                {
                    tempSqlStr += " and ExecStatus='" + status + "'";
                }
                if (IsSearch)
                {
                    tempSqlStr += " and MissionDesc like '%" + KeyWord + "%'";
                }
                if (typeMission != "-1")
                {
                    tempSqlStr += " and MissionType='" + typeMission + "'";
                }
                if (currentPage == 1)
                {
                    sql = string.Format(SQL_Select_MissionFirst, pageSize, tempSqlStr, tempSqlStr);
                }
                else
                {
                    sql = string.Format(SQL_Select_MissionInfo, pageSize, pageSize * (currentPage - 1), tempSqlStr, tempSqlStr);
                }

                objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);
            }
            catch (Exception ex)
            {
                
            }
            //return objDs;
            return this.CountFinishDateByWorkHour(objDs);
        }

        /// <summary>
        /// 获取某个状态下的任务
        /// </summary>
        /// <param name="status"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public DataSet GetMissionDataSet(string status, string beginTime, string endTime, string KeyWord)
        {
            string tempSqlStr = string.Empty;

            DataSet objDs = new DataSet("result");
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_WriteDate,OleDbType.VarChar),
					                                           new OleDbParameter(Parm_EndDate,OleDbType.VarChar)					                                                                                                                          				   
                                                           };
                parms[0].Value = beginTime;
                parms[1].Value = endTime;

                string sql = string.Empty;

                tempSqlStr += " and ExecStatus='" + status + "'";
                tempSqlStr += " and MissionDesc like '%" + KeyWord + "%'";

                sql = string.Format(SQL_Select_MissionList, tempSqlStr);

                objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);

            }
            catch (Exception ex)
            {
                
            }
            //return objDs;
            return this.CountFinishDateByWorkHour(objDs);
        }

        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="userid">用户编号</param>
        /// <param name="status">执行状态 0，待执行 1，执行中 2，执行结束</param>
        /// <param name="beginTime">查询开始日期</param>
        /// <param name="endTime">查询结束日期</param>
        /// <returns>返回总记录数</returns>        
        public int GetMissionFYCount(string userid, string status, string beginTime, string endTime, string IsAsgin, bool IsSearch, string KeyWord, string typeMission)
        {
            int resultNum = 0;
            string tempSqlStr = "";
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_WriteDate,OleDbType.VarChar,200),                                                                              
                                                               new OleDbParameter(Parm_EndDate,OleDbType.VarChar,200)                                                                                                       
														   };
                parms[0].Value = beginTime;
                parms[1].Value = endTime;

                if (IsAsgin == "1") //已经分配
                {
                    if (userid != "-1")
                    {
                        tempSqlStr += " and userid=" + userid + "";
                    }
                    else
                    {
                        tempSqlStr += " and userid<>0";
                    }
                }
                else
                {
                    tempSqlStr += " and userid=0";
                }
                if (status != "-1")
                {
                    tempSqlStr += " and ExecStatus='" + status + "'";
                }
                if (IsSearch)
                {
                    tempSqlStr += " and MissionDesc like '%" + KeyWord + "%'";
                }
                if (typeMission != "-1")
                {
                    tempSqlStr += " and MissionType='" + typeMission + "'";
                }

                string sql = string.Format(SQL_Select_MissionCount, tempSqlStr);

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


        private DataSet CountFinishDateByWorkHour(DataSet pSet)
        {
           
            if (pSet==null || pSet.Tables.Count==0) return null;
            pSet.Tables[0].Columns.Add("PlanFinishDate");
            pSet.Tables[0].Columns.Add("PlanFinishProcess");
            int _rowCount = pSet.Tables[0].Rows.Count;
            for (int i = 0; i < _rowCount; i++)
            {
                if (pSet.Tables[0].Rows[i]["FinisthDate"] != null && pSet.Tables[0].Rows[i]["FinisthDate"].ToString() != string.Empty)
                    pSet.Tables[0].Rows[i]["RealProcess"] = "100%";
                if (pSet.Tables[0].Rows[i]["ExecDate"] == null || pSet.Tables[0].Rows[i]["ExecDate"].ToString()==string.Empty) continue;

                 DateTime _ExecDate=Convert.ToDateTime(pSet.Tables[0].Rows[i]["ExecDate"]);
                 _ExecDate = this.GetNormativeExcuteDate(_ExecDate);
                int _userId=(int)pSet.Tables[0].Rows[i]["UserId"];
                int _WorkHour = 0;
                if (pSet.Tables[0].Rows[i]["WorkHour"] == null || pSet.Tables[0].Rows[i]["WorkHour"].ToString() == string.Empty)
                {
                    _WorkHour = 0;
                }
               else {
                    _WorkHour=Convert.ToInt32( pSet.Tables[0].Rows[i]["WorkHour"]);
                }
              

                //根据开始执行日期和计划结束日期，将假期加进去
                if (pSet.Tables[0].Rows[i]["ExecDate"].ToString().Trim() != string.Empty &&_WorkHour!=0)
                {
                    List<RestDate> _restDateList = this.GetRestDateListByUserId(_ExecDate.ToShortDateString(), _userId);

                    DateTime _planFinishDate=this.GetPlanFinishDate(_ExecDate, _WorkHour);
                    if (_restDateList.Count > 0)
                    {
                        for (int j = 0; j < _restDateList.Count; j++)
                        {
                            if (_restDateList[j].Rest_Date.Date >= _ExecDate.Date && _restDateList[j].Rest_Date.Date<= _planFinishDate.Date)
                            {
                                if (_restDateList[j].RestDetail == "全天")
                                {
                                    _planFinishDate = _planFinishDate.AddDays(1);
                                }
                                else
                                {
                                    _planFinishDate = _planFinishDate.AddHours(4);
                                    if (_planFinishDate.Hour > 12 && (_planFinishDate.Hour<17||(_planFinishDate.Hour==17 && _planFinishDate.Minute<=30)))
                                    {
                                        _planFinishDate = _planFinishDate.AddHours(1.5);
                                        
                                    }
                                    else if ((_planFinishDate.Hour==17&&_planFinishDate.Minute>30) || _planFinishDate.Hour > 17)
                                    {
                                        _planFinishDate = _planFinishDate.AddHours(14.5);
                                    }
                                }
                            }
                            else
                                break;
                        }
                    }
                    pSet.Tables[0].Rows[i]["PlanFinishProcess"] =this.GetPlanFinishProcessString( this.GetPlanFinishProcess(_WorkHour, _ExecDate, _restDateList));
                    pSet.Tables[0].Rows[i]["PlanFinishDate"] = _planFinishDate;
                }
            }
            return pSet;
        }

         //将不规范的执行时间，转化到正常的上班时间段内，以便计划计划结束时间
        private DateTime GetNormativeExcuteDate(DateTime pExecuteDate)
        {
            if (pExecuteDate.Hour < 8)
            {
                pExecuteDate = pExecuteDate.AddHours(8 - pExecuteDate.Hour);
                pExecuteDate = pExecuteDate.AddMinutes(-pExecuteDate.Minute);
            }
            if (pExecuteDate.Hour == 12 || (pExecuteDate.Hour == 13 && pExecuteDate.Minute < 30))
            {
                pExecuteDate = pExecuteDate.AddHours(13 - pExecuteDate.Hour);
                pExecuteDate = pExecuteDate.AddMinutes(30 - pExecuteDate.Minute);
            }
            if ((pExecuteDate.Hour == 17 && pExecuteDate.Minute >= 30) || pExecuteDate.Hour > 17)
            {
                pExecuteDate = pExecuteDate.AddHours(24 - pExecuteDate.Hour + 8);
                pExecuteDate = pExecuteDate.AddMinutes(-pExecuteDate.Minute);
            }
            return pExecuteDate;
        }
        //将double类型的数据转换成含%的百分数字符串
        private string GetPlanFinishProcessString(double pProcess)
        {
            if (pProcess == 0) return string.Empty;
            if (pProcess == 1) return "100%";

            string _result = pProcess.ToString();
            _result = _result.Substring(_result.IndexOf(".") + 1);
            if (_result.Length > 2)
            {
                _result= _result.Substring(0, 2);
            }
            if (_result.Length==1)
            {
                return _result + "0%";
            }
            else if (_result.Length==2)
            {
                if (_result.Remove(1)=="0")
                {
                    return _result.Substring(1) + "%";
                }
                return _result + "%";
            }
            
            return string.Empty;
            
        }

       //计算计划进度

        private double GetPlanFinishProcess(int pWorkHour, DateTime pBeginDate, List<RestDate> pRestDateList)
        {
            if (pWorkHour == 0) return 0;
            if (DateTime.Compare(pBeginDate, DateTime.Now) > 0) return 0;

            DateTime _today = DateTime.Now;

            int _restHour = 0;
            //计算假期多少工时
            if (pRestDateList.Count > 0)
            {
                for (int i = 0; i < pRestDateList.Count; i++)
                {
                    if (pBeginDate.Date <= pRestDateList[i].Rest_Date.Date && pRestDateList[i].Rest_Date.Date <= _today.Date)
                    {
                        if (pRestDateList[i].RestDetail == "全天")
                        {
                            _restHour += 8;
                        }
                        else
                        {
                            _restHour += 4;
                        }
                    }
                }
            }

            int _day = 0;
            int _hour = 0;
            int _minute = 0;
            //计算已用多少天
            if (pBeginDate.Month < _today.Month)
            {
                int _month = _today.Month;
                while (pBeginDate.Month < _month)
                {
                    _day += this.GetDaysByMonth(_month - 1);
                    _month--;
                }
                _day += _today.Day - pBeginDate.Day-1;
            }
            else if (pBeginDate.Month == _today.Month)
            {
                _day = _today.Day - pBeginDate.Day-1;
            }
            else return 0;

            //计算开始执行当天的工时
            int _hour_b = 0;
            int _minute_b = 0;
            if (pBeginDate.Hour<8)
            {
                _hour_b = 8;
            }
            else if (pBeginDate.Hour>=8 && pBeginDate.Hour<=11)
            {
                _hour_b = 12 - pBeginDate.Hour + 4;
                _minute_b = -pBeginDate.Minute;
            }
            else if (pBeginDate.Hour>=12 && pBeginDate.Hour<13 )
            {
                _hour_b = 4;
            }
            else if (pBeginDate.Hour==13 && pBeginDate.Minute<=30)
            {
                _hour_b = 4;
            }
            else if (pBeginDate.Hour==13 && pBeginDate.Minute>30)
            {
                _hour_b = 3;
                _minute_b = 60 - pBeginDate.Minute;
            }
            else if (pBeginDate.Hour >13 &&pBeginDate.Hour<17)
            {
                _hour_b = 17 - pBeginDate.Hour;
                _minute_b = -pBeginDate.Minute+30;
            }
            else if (pBeginDate.Hour==17 && pBeginDate.Minute<=30)
            {
                _minute_b = 30-pBeginDate.Minute;
            }
            else 
            {
                _hour_b = 0;
                _minute_b = 0;
            }

            //计算登录当天的工时
            int _hour_t = 0;
            int _minute_t = 0;
            if (_today.Hour < 8)
            {
                _hour_t = 0;
            }
            else if (_today.Hour >= 8 && _today.Hour <= 11)
            {
                _hour_t = _today.Hour -8;
                _minute_t = _today.Minute;
            }
            else if (_today.Hour >= 12 && _today.Hour < 13)
            {
                _hour_t = 4;
            }
            else if (_today.Hour == 13 && _today.Minute <= 30)
            {
                _hour_t = 4;
            }
            else if (_today.Hour == 13 && _today.Minute > 30)
            {
                _hour_t = 4;
                _minute_t = _today.Minute - 30;
            }
            else if (_today.Hour > 13 && _today.Hour < 17)
            {
                _hour_t = _today.Hour-14+4;
                _minute_t = _today.Minute + 30;
            }
            else if (_today.Hour == 17 && _today.Minute <= 30)
            {
                _hour_t = 7;
                _minute_t =  _today.Minute+30;
            }
            else
            {
                _hour_t = 8;
                _minute_t = 0;
            }

            //计算百分比
            _hour = _hour_b + _hour_t;
            _minute = _minute_b + _minute_t;
             double _hour_fromMinute= (double)_minute / 60;
             double _usedHour = _day * 8 + _hour + _hour_fromMinute - _restHour;
            double _process = (double)_usedHour / pWorkHour;
            if (_process <= 0)
                return 0;
            if (_process >= 1)
                return 1;
            return _process;


        }
//2009-5-31完成，计算本日结束时的工时。测试正确。
        //private double GetPlanFinishProcess(int pWorkHour, DateTime pBeginDate, List<RestDate> pRestDateList)
        //{
        //    if (pWorkHour == 0) return 0;
        //    if (DateTime.Compare(pBeginDate, DateTime.Now) > 0) return 0;

        //    DateTime _today = DateTime.Now;

        //    int _restHour = 0;
        //    //计算假期多少工时
        //    if (pRestDateList.Count > 0)
        //    {
        //        for (int i = 0; i < pRestDateList.Count; i++)
        //        {
        //            if (pBeginDate < pRestDateList[i].Rest_Date && pRestDateList[i].Rest_Date <= _today)
        //            {
        //                if (pRestDateList[i].RestDetail == "全天")
        //                {
        //                    _restHour += 8;
        //                }
        //                else
        //                {
        //                    _restHour += 4;
        //                }
        //            }
        //        }
        //    }

        //    int _day = 0;
        //    int _hour = 0;
        //    //计算已用多少天
        //    if (pBeginDate.Month < _today.Month)
        //    {
        //        int _month=_today.Month;
        //        while (pBeginDate.Month<_month)
        //        {
        //            _day += this.GetDaysByMonth(_month-1);
        //            _month--;
        //        }
        //        _day += _today.Day - pBeginDate.Day;
        //    }
        //    else if (pBeginDate.Month == _today.Month)
        //    {
        //        _day = _today.Day - pBeginDate.Day;
        //    }
        //    else return 0;

        //    //计算开始执行当天的工时
        //    if (pBeginDate.Hour<=12)
        //    {
        //        _hour = 12 - pBeginDate.Hour + 4;
        //    }
        //    if (pBeginDate.Hour>=13)
        //    {
        //        _hour = 18 - pBeginDate.Hour;
        //    }
        //    if (_hour<0)
        //    {
        //        _hour = 0;
        //    }
           

        //    int _usedHour = _day * 8 + _hour - _restHour;
        //    double _process = (double)_usedHour / pWorkHour;
        //    if (_process <= 0)
        //        return 0;
        //    if (_process >= 1)
        //        return 1;
        //    return _process;


        //}

        //根据哪一月返回当月多少天
        private int GetDaysByMonth(int pMonth)
        {
            int _days=0;
            switch (pMonth)
            {
                case 1: _days = 31; break;
                case 2: _days = 28; break;
                case 3: _days = 31; break;
                case 4: _days = 30; break;
                case 5: _days = 31; break;
                case 6: _days = 30; break;
                case 7: _days = 31; break;
                case 8: _days = 31; break;
                case 9: _days = 30; break;
                case 10: _days = 31; break;
                case 11: _days = 30; break;
                case 12: _days = 31; break;
                default: _days = 0;  break;
            }
            return _days;
        }
        //private double GetPlanFinishProcess(int pWorkHour, DateTime pBeginDate, List<RestDate> pRestDateList)
        //{
        //    if (pWorkHour == 0) return 0;
        //    if (DateTime.Compare(pBeginDate,DateTime.Now)>0) return 0;

        //    DateTime _today=DateTime.Now;
          
        //    int _restHour = 0;
        //    if (pRestDateList.Count > 0)
        //    {
        //        for (int i = 0; i < pRestDateList.Count; i++)
        //        {
        //            if (pBeginDate <= pRestDateList[i].Rest_Date && pRestDateList[i].Rest_Date <= _today)
        //            {
        //                if (pRestDateList[i].RestDetail == "全天")
        //                {
        //                    _restHour += 8;
        //                }
        //                else
        //                {
        //                    _restHour += 4;
        //                }
        //            }
        //        }
        //    }
        //    int _day = 0;
        //    int _hour = 0;
        //    if (pBeginDate.Month < _today.Month)
        //    {
        //        //计算相差多少天
        //        _day = (_today.Month - pBeginDate.Month) * 30 + (_today.Day - pBeginDate.Day);
        //    }
        //    else if (pBeginDate.Month == _today.Month)
        //    {
        //        //计算相差多少天
        //        _day = DateTime.Today.Day - pBeginDate.Day;
        //    }
        //   else    return 0;


        //    if (_today.Hour<8)
        //    {
        //        if (pBeginDate.Hour <= 12)
        //            _hour = 8 - pBeginDate.Hour;
        //        else
        //            _hour = 8 - pBeginDate.Hour + 2;
        //    }
        //    else if (_today.Hour>=8 && _today.Hour<=12)
        //    {
        //        if (pBeginDate.Hour <= 12)
        //            _hour = _today.Hour - pBeginDate.Hour;
        //        else
        //            _hour = _today.Hour - pBeginDate.Hour + 2;
        //    }
        //    else if (_today.Hour>12 && _today.Hour<14)
        //    {
        //        if (pBeginDate.Hour <= 12)
        //            _hour = 12 - pBeginDate.Hour;
        //        else
        //            _hour = 14-pBeginDate.Hour;
        //    }
        //    else if (_today.Hour>=14 && _today.Hour<=18)
        //    {
        //        if (pBeginDate.Hour <= 12)
        //            _hour = _today.Hour - pBeginDate.Hour-2;
        //        else
        //            _hour = _today.Hour - pBeginDate.Hour ;
        //    }
        //    if (_today.Hour>18)
        //    {
        //        if (pBeginDate.Hour <= 12)
        //            _hour = 18 - pBeginDate.Hour-2;
        //        else
        //            _hour = 18 - pBeginDate.Hour ;
        //    }


        //    int _usedHour = _day * 8 + _hour-_restHour;
        //    double _process = (double)_usedHour / pWorkHour;
        //    if (_process <= 0)
        //        return 0;
        //    if (_process >= 1)
        //        return 1;
        //    return _process;
            
            
        //}
        //根据工时计算计划结束日期
        private DateTime GetPlanFinishDate(DateTime pBegin, int pWorkHour)
        {
            DateTime _oldTime = pBegin;
            int _wholeDate = pWorkHour / 8;
            int _residueDate = pWorkHour % 8;

            if (_wholeDate!=0)
            {
               pBegin= pBegin.AddDays(_wholeDate);
            }

            if (_residueDate!=0)
            {
                pBegin = pBegin.AddHours(_residueDate);
             
            }
            if (_oldTime.Hour < 12 && pBegin.Hour>=12)
            {
                pBegin = pBegin.AddHours(1.5);
            }
            if ((pBegin.Hour == 17 && pBegin.Minute >30) || pBegin.Hour > 17)
            {
                pBegin = pBegin.AddHours(14.5);
            }
            //if (pBegin.Hour > 12 &&(pBegin.Hour<17||(pBegin.Hour==17 && pBegin.Minute<=30)))
            //{
            //    pBegin = pBegin.AddHours(1.5);
            //}

            //if ((pBegin.Hour == 17 && pBegin.Minute >30) || pBegin.Hour > 17)
            //{
            //    pBegin = pBegin.AddHours(14.5);
            //}
            return pBegin;

        }

        private List<RestDate> GetRestDateListByUserId(string pBeginDate, int pUserId)
        {
            NetRestDate _net = new NetRestDate();
            DataSet _ds = _net.GetRestDateDataSet(pBeginDate, string.Empty, string.Empty, pUserId);
            if (_ds.Tables==null || _ds.Tables.Count == 0) return null; 
            int _count = _ds.Tables[0].Rows.Count;

            List<RestDate> _restDateList = new List<RestDate>();
            for (int i = 0; i < _count; i++)
            {
                RestDate _rest = new RestDate();
                _rest.Rest_Date = Convert.ToDateTime(_ds.Tables[0].Rows[i]["RestDate"]);
                _rest.RestDetail = _ds.Tables[0].Rows[i]["RestDetail"].ToString();
                _restDateList.Add(_rest);
            }
            return _restDateList;
        }
        #endregion
    }
}
