using System;
using System.Collections.Generic;
using System.Text;
using NetChina.JournalModel;
using System.Data.OleDb;
using System.Data;

namespace NetChina.AccessHelper
{
    public class NetUserJournal
    {
        #region SQL 变量
        //根据时间获取用户当天第一页的日志信息
        private const string SQL_Select_JournalInfoFirst = "Select top {0} * From tbl_select where format(writetime,'yyyy/mm/dd')>=@WriteTime and format(writetime,'yyyy/mm/dd')<=@EndTime {1} Order By userOrder desc,jourId desc";//现在的页码*每页显示的条数
        //private const string SQL_Select_JournalInfoAll = "Select * From tbl_NetUserJournal where format(writetime,'yyyy/mm/dd')>=@WriteTime and format(writetime,'yyyy/mm/dd')<=@EndTime and userName like '%@KeyWord%' Order By jourId Desc";//现在的页码*每页显示的条数
        private const string SQL_Select_JournalInfoAll = "Select * From tbl_NetUserJournal where format(writetime,'yyyy/mm/dd')=@WriteTime Order By jourId Desc";
        private const string SQL_Select_JournalCount = "Select count(*) as resultCount From tbl_NetUserJournal where format(writetime,'yyyy/mm/dd')>=@WriteTime and format(writetime,'yyyy/mm/dd')<=@EndTime {0}";
        private const string SQL_Select_JournalList = "Select *  From tbl_select where format(writetime,'yyyy/mm/dd')>=@WriteTime and format(writetime,'yyyy/mm/dd')<=@EndTime {0} Order By userOrder Desc,jourId desc";
        private const string SQL_Select_JournalInfo = "Select top {0} * From tbl_select where jourId not in (select top {1} jourId from tbl_select where format(writetime,'yyyy/mm/dd')>=@WriteTime and format(writetime,'yyyy/mm/dd')<=@EndTime {2} order by userOrder desc,jourId desc) and format(writetime,'yyyy/mm/dd')>=@WriteTime and format(writetime,'yyyy/mm/dd')<=@EndTime {3} Order By userOrder Desc,jourId desc";

        //获取没有写当前日志的名单
        private const string SQL_Select_JournalIsWC = "select userid,realName from tbl_NetUserInfo where userid not in(select userid from tbl_select where format(writetime,'yyyy/mm/dd')=@WriteTime) and groupid='1'";



        //根据用户编号获取所有该用户信息
        private const string SQL_Select_UserJournal = "Select jourId,userid,userName,jourDesc,writeTime,modifytime,evalContent From tbl_NetUserJournal where userid=@UserId Order By writetime Desc";
        //根据时间获取用户当天的日志信息
        //private const string SQL_Select_JournalByDate = "Select jourId,userid,jourDesc,writeTime,modifytime From tbl_NetUserJournal where format(writetime,'yyyy/mm/dd')=@WriteTime Order By writetime Desc";
        //根据用户编号获取单个用户日志信息
        private const string SQL_Select_UserJournalPrimaryKey = "Select jourId,userid,userName,jourDesc,writeTime,modifytime,evalContent From tbl_NetUserJournal where userId=@UserId and format(writetime,'yyyy/mm/dd')=@WriteTime";

        private const string SQL_Select_UserJournalByJourId = "Select jourId,userid,userName,jourDesc,writeTime,modifytime,evalContent From tbl_NetUserJournal where jourId=@JourId";
        //插入用户日志信息
        private const string SQL_Insert_UserJournal = "insert into tbl_NetUserJournal(userid,userName,jourDesc,writeTime,modifytime) values (@UserId,@UserName,@JourDesc,@WriteTime,@ModifyTime)";
        //修改用户日志信息
        private const string SQL_Update_UserJournal = "Update tbl_NetUserJournal set userid=@UserId,userName=@UserName,jourDesc=@JourDesc,modifytime=@ModifyTime Where jourId={0}";

        private const string SQL_Update_UserJournalSome = "Update tbl_NetUserJournal set jourDesc=@JourDesc,modifytime=@ModifyTime Where jourId={0} and format(writetime,'yyyy/mm/dd')=@WriteTime";

        private const string SQL_Update_JourlEvalJourId = "Update tbl_NetUserJournal set EvalContent=@EvalContent Where jourId={0}";

        private const string SQL_Delete_UserJournal = "delete from tbl_NetUserJournal Where jourId={0} and userid={1}";

        private const string SQL_Select_SingleJournal = "select evalContent from tbl_NetUserJournal Where jourId={0} and userid={1}";

        //根据用户id查询在某个时间段内的日志填写日期
        private const string SQL_Select_SingleJournalByUserId = "select writetime from tbl_NetUserJournal where  (format(writetime,'yyyy/mm/dd') Between @BeginTime And @EndTime) and Userid=@UserId ";
        #endregion

        #region 参数变量

        private const string Parm_UserId = "@UserId";           //用户ID
        private const string Parm_UserName = "@UserName";       //用户ID
        private const string Parm_JourId = "@JourId";           //用户ID
        private const string Parm_JourDesc = "@JourDesc";       //总结内容
        private const string Parm_WriteTime = "@WriteTime";     //开始时间或者写入日期
        private const string Parm_ModifyTime = "@ModifyTime";   //修改时间
        private const string Parm_EndTime = "@EndTime";       //结束时间
        private const string Parm_BeginTime = "@BeginTime";  //开始时间
        private const string Parm_EvalContent = "@EvalContent";      //评价内容

        private const string Parm_KeyWord = "@KeyWord";         //关键字

        #endregion

        #region 方法列表
        /// <summary>
        /// 添加用户日志信息
        /// </summary>
        /// <param name="info">传入UserJournal对象</param>
        /// <returns></returns>
        public bool Insert(UserJournal info)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_UserId,OleDbType.Integer),
                                                               new OleDbParameter(Parm_UserName,OleDbType.VarChar,50),
															   new OleDbParameter(Parm_JourDesc,OleDbType.LongVarWChar),
															   new OleDbParameter(Parm_WriteTime,OleDbType.Date),
															   new OleDbParameter(Parm_ModifyTime,OleDbType.Date)                                                               
														   };
                parms[0].Value = info.UserID;
                parms[1].Value = info.UserName;
                parms[2].Value = info.JourDesc;
                parms[3].Value = info.WriteTime;
                parms[4].Value = info.ModifyTime;                

                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text,SQL_Insert_UserJournal, parms);
            }
            catch (Exception ex)
            {
                _Result = false;
            }

            return _Result;
        }

        /// <summary>
        /// 更新用户日志信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Update(UserJournal info)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_UserId,OleDbType.Integer),
                                                               new OleDbParameter(Parm_UserName,OleDbType.VarChar,50),
															   new OleDbParameter(Parm_JourDesc,OleDbType.LongVarWChar),
															   //new OleDbParameter(Parm_WriteTime,OleDbType.Date),
															   new OleDbParameter(Parm_ModifyTime,OleDbType.Date)                                                               
														   };
                parms[0].Value = info.UserID;
                parms[1].Value = info.UserName;
                parms[2].Value = info.JourDesc;
                //parms[3].Value = info.WriteTime;
                parms[3].Value = info.ModifyTime;        

                string sql = string.Format(SQL_Update_UserJournal,info.JourID);
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
        public bool UpdateUserEval(string evalDesc, string jourId)
        {
            bool _Result = true;
            try
            {

                OleDbParameter[] parms = new OleDbParameter[]{															   
															   new OleDbParameter(Parm_EvalContent,OleDbType.LongVarWChar)													   
															                                                                                                    
														   };
                string sql = string.Format(SQL_Update_JourlEvalJourId, jourId);

                parms[0].Value = evalDesc;

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
        /// 更新用户日志信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateSome(string joulDesc, string jid, string writeTime)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{															   
															   new OleDbParameter(Parm_JourDesc,OleDbType.LongVarWChar),															   
															   new OleDbParameter(Parm_ModifyTime,OleDbType.Date),
                                                               new OleDbParameter(Parm_WriteTime,OleDbType.VarChar)                                           
														   };
                parms[0].Value = joulDesc;                
                parms[1].Value = System.DateTime.Now.ToString();
                parms[2].Value = writeTime;

                string sql = string.Format(SQL_Update_UserJournalSome, jid);
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
        /// 获取用户的单个日志信息
        /// </summary>
        /// <param name="teachId"></param>
        /// <returns></returns>
        public UserJournal GetNetUserInfo(string jourId)
        {

            UserJournal info = null;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{									
                                    new OleDbParameter(Parm_JourId,OleDbType.VarChar,50)
														   };                
                parms[0].Value = jourId;
                using (OleDbDataReader sdr = AccessHelper.ExecuteReader(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_UserJournalByJourId, parms))
                {
                    while (sdr.Read())
                    {
                        info = new UserJournal(
                            int.Parse(sdr["jourid"].ToString()),
                            int.Parse(sdr["userid"].ToString()),
                            sdr["username"].ToString(),
                            sdr["jourDesc"].ToString(),
                            DateTime.Parse(sdr["writetime"].ToString()),
                            DateTime.Parse(sdr["modifytime"].ToString()),
                            sdr["evalContent"].ToString()
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
        /// 获取用户的单个日志信息
        /// </summary>
        /// <param name="teachId"></param>
        /// <returns></returns>
        public UserJournal GetNetUserInfoByIds(string userId, string writetime)
        {

            UserJournal info = null;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
									new OleDbParameter(Parm_UserId,OleDbType.Integer),
                                    new OleDbParameter(Parm_WriteTime,OleDbType.VarChar,50)
														   };
                parms[0].Value = userId;
                parms[1].Value = writetime;
                using (OleDbDataReader sdr = AccessHelper.ExecuteReader(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_UserJournalPrimaryKey, parms))
                {
                    while (sdr.Read())
                    {
                        info = new UserJournal(
                            int.Parse(sdr["jourid"].ToString()),
                            int.Parse(sdr["userid"].ToString()),
                            sdr["username"].ToString(),
                            sdr["jourDesc"].ToString(),
                            DateTime.Parse(sdr["writetime"].ToString()),
                            DateTime.Parse(sdr["modifytime"].ToString()),
                            sdr["evalContent"].ToString()
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




        public List<string> GetNoWriteList(ref  string beginTime)
        {
            DateTime _begin = Convert.ToDateTime(beginTime);
            int i = 30;
            NetRestDate _netRestDate = new NetRestDate();
            while (i>0)
            {
                i--;

                int _dateCount = _netRestDate.GetRestDateCount(_begin.ToShortDateString());
                if (_dateCount > 0)
                {
                    _begin = _begin.AddDays(-1);
                }
                else
                    break;
            }
            beginTime = _begin.ToString("yyyy-MM-dd");
            

            DataSet objDs = new DataSet("reslut");
            List<string> _nameList = new List<string>();
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_WriteTime,OleDbType.VarChar)					                                          
					                                           				   
                                                           };
                parms[0].Value = _begin.ToString("yyyy-MM-dd");

                objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_JournalIsWC, parms);
            }
            catch (Exception ex)
            {
            }


            if (objDs==null || objDs.Tables.Count==0)
            {
               return  null;
            }

            int _count=objDs.Tables[0].Rows.Count;
            if (_count > 0)
            {
                //NetRestDate _netRestDate = new NetRestDate();
                DataSet _nameDS = _netRestDate.GetUserIdDataSetByRestDate(_begin.ToString("yyyy-MM-dd"));

                for (int j = 0; j < _count; j++)
                {
                    string _userId=objDs.Tables[0].Rows[j]["userid"].ToString();
                    
                    if (!this.IsNameInRestNameList(_userId, _nameDS))
                    {
                        _nameList.Add(objDs.Tables[0].Rows[j]["realName"].ToString());
                    }
                }
            }
            return _nameList;
        }

        //判断用户是否在传入的数据集里
        private bool IsNameInRestNameList(string pUserId,DataSet pRestNameDS)
        {
            if (pRestNameDS==null || pRestNameDS.Tables.Count==0 || pRestNameDS.Tables[0].Rows.Count==0)
            {
                return false;
            }
            if (pRestNameDS.Tables[0].Rows.Count>0)
            {
                for (int i = 0; i < pRestNameDS.Tables[0].Rows.Count; i++)
                {
                    if (pUserId.Trim()==pRestNameDS.Tables[0].Rows[i]["UserId"].ToString().Trim())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
       
        /// <summary>
        /// 获取用户的集合信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="inUse"></param>
        /// <returns></returns>
        public DataSet GetJournalDataSet(string keyword, string beginTime, string endTime, int pageSize, int currentPage, bool isExport)
        {
            if (keyword == "-1") keyword = string.Empty;

            DataSet objDs = new DataSet("result");
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
                                                               new OleDbParameter(Parm_WriteTime,OleDbType.VarChar),
					                                           new OleDbParameter(Parm_EndTime,OleDbType.VarChar)
					                                           				   
                                                           };
                parms[0].Value = beginTime;
                parms[1].Value = endTime;

                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = "and userid=" + keyword+"";
                }

                string sql = string.Empty;
                if (isExport)
                {
                    //sql = SQL_Select_JournalInfoAll;                    
                    sql = string.Format(SQL_Select_JournalList, keyword);
                }
                else
                {
                    if (currentPage == 1)
                    {
                        sql = string.Format(SQL_Select_JournalInfoFirst, pageSize,keyword);
                    }
                    else
                    {
                        sql = string.Format(SQL_Select_JournalInfo, pageSize, pageSize * (currentPage - 1),keyword,keyword);
                    }
                }
                objDs = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, sql, parms);
            }
            catch (Exception ex)
            {
            }
            return objDs;
        }

        /// <summary>
        /// 获取用户的集合信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="inUse"></param>
        /// <returns></returns>
        public int GetJournalFYCount(string keyword, string beginTime, string endTime)
        {
            if (keyword == "-1") keyword = string.Empty;
            int resultNum = 0;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_WriteTime,OleDbType.VarChar,200),                                                                              
                                                               new OleDbParameter(Parm_EndTime,OleDbType.VarChar,200)                                                               
														   };
                parms[0].Value = beginTime;
                parms[1].Value = endTime;

                if (!string.IsNullOrEmpty(keyword))
                {
                    keyword = "and userid=" + keyword+"";
                }

                string sql = string.Format(SQL_Select_JournalCount, keyword);
                
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

        /// <summary>
        /// 根据开始日期和结束日期，用户Id，查询期间的所有日志日期，里面只有一个属性是writetime
        /// </summary>
        /// <param name="pBeginTime"></param>
        /// <param name="pEndTime"></param>
        /// <param name="pUserid"></param>
        /// <returns></returns>
        public DataSet GetNoWriteJournal(string pBeginTime,string pEndTime,int pUserid)
        {
            DataSet _ds = new DataSet("Result");
            string _sqlstr = string.Empty;
            OleDbParameter[] _param = new OleDbParameter[] {
                                                                                        new OleDbParameter(Parm_BeginTime,OleDbType.VarChar,50),
                                                                                        new OleDbParameter(Parm_EndTime,OleDbType.VarChar,50),
                                                                                        new OleDbParameter(Parm_UserId,OleDbType.Integer)
                                                                                        };
            _param[0].Value = NetRestDate.GetFullDateString(pBeginTime);
            _param[1].Value = NetRestDate.GetFullDateString(pEndTime);
            _param[2].Value = pUserid;
            try
            {
                _sqlstr = SQL_Select_SingleJournalByUserId;
                _ds = AccessHelper.ExcuteDataSet(AccessHelper.CONN_STRING, CommandType.Text, _sqlstr, _param);
            }
            catch (Exception ex)
            {
            }
            return _ds;
        }


        public List<UserInfo> GetUserList()
        {


            NetUserInfo uInfo = new NetUserInfo();
            List<UserInfo> _UserList = null;
            DataSet ds = uInfo.GetUserInfos("", "", 0, 1, true);
            if (ds != null)
            {
                int dsCount = ds.Tables[0].Rows.Count;
                if (dsCount > 0)
                {
                    _UserList = new List<UserInfo>();
                    for (int i = 0; i < dsCount; i++)
                    {
                        UserInfo _info = new UserInfo();
                        _info.UserID = Convert.ToInt32( ds.Tables[0].Rows[i][0]);
                        _info.RealName = ds.Tables[0].Rows[i][3].ToString();
                        _UserList.Add(_info);
                    }
                }
            }
            return _UserList;
        }

        /// <summary>
        /// 根据开始和结束日期,计算所有人未写日志的日期列表
        /// </summary>
        public List<NoWritePersonInfo> GetNoWriteDateList(string pBeginTime, string pEndTime)
        {
            
            List<UserInfo> _userList = this.GetUserList();//查出所有人
            List<NoWritePersonInfo> _noWriteList = null;

            if (_userList.Count>0)
            {
                _noWriteList = new List<NoWritePersonInfo>();
                foreach (UserInfo _user in _userList)
                {
                    List<DateTime> _noWriteDateList = this.GetNoWriteDateList(pBeginTime, pEndTime, _user.UserID);
                    if (_noWriteDateList.Count > 0)
                    {
                        NoWritePersonInfo _noWritePerson = new NoWritePersonInfo();
                        _noWritePerson.UserId = _user.UserID;
                        _noWritePerson.RealName = _user.RealName;
                        _noWritePerson.NoWriteCount = _noWriteDateList.Count;
                        _noWritePerson.NoWriteDate = this.GetNoWriteDateListString(_noWriteDateList);
                        _noWriteList.Add(_noWritePerson);
                    }
                    
                }
            }
            return _noWriteList;
        }

        public string GetNoWriteDateListString(List<DateTime> pNoWriteDateList)
        {
            StringBuilder _strDateList = new StringBuilder();
            if (pNoWriteDateList.Count > 0)
            {
                //_strDateList.Append("<span style='color:#3300ff;'>");
                foreach (DateTime _date in pNoWriteDateList)
                {
                    _strDateList = _strDateList.Append(_date.ToString("yyyy-M-d") + "，");
                }
                _strDateList = _strDateList.Remove(_strDateList.Length - 1, 1);
                _strDateList = _strDateList.Append(" 。");
                //_strDateList = _strDateList.Append(" 。</span>");
            }
            return _strDateList.ToString();
        }

       /// <summary>
       /// 根据开始和结束日期,用户Id，计算未写日志的日期列表
       /// </summary>
       /// <param name="pBeginTime"></param>
       /// <param name="pEndTime"></param>
       /// <param name="pUserId"></param>
       /// <returns></returns>
        public List<DateTime> GetNoWriteDateList(string pBeginTime, string pEndTime, int pUserId)
        {
            NetRestDate _netRestDate = new NetRestDate();
            DataSet _restDateList = _netRestDate.GetRestDateDataSet(pBeginTime, pEndTime, pUserId,true);

            NetUserJournal _netJournal=new NetUserJournal();
            DataSet _journalDateList = _netJournal.GetNoWriteJournal(pBeginTime, pEndTime, pUserId);

            //DataTable _tableRestDate = null;
            //DataTable _tableJournal = null;
            //if (_restDateList!=null && _restDateList.Tables[0]!=null )
            //{
            //    _tableRestDate = _restDateList.Tables[0];
            //}
            //if (_journalDateList!=null && _journalDateList.Tables[0]!=null)
            //{
            //    _tableJournal = _journalDateList.Tables[0];
            //}

            DateTime _begin = Convert.ToDateTime(pBeginTime).Date;
            DateTime _end = Convert.ToDateTime(pEndTime).Date.AddDays(1);
            List<DateTime> _noWriteDateList = new List<DateTime>();

              while (_begin<_end)
            {
                if (this.IsInJournalDateList(_begin,_journalDateList) || this.IsRestDateList(_begin,_restDateList))
                {
                    _begin=_begin.AddDays(1);
                    continue;
                }
                else
                {
                    _noWriteDateList.Add(_begin);
                    _begin=_begin.AddDays(1);
                }
            }
            return _noWriteDateList;
        }

        //判断日期pKeyDate是否在传入的日志日期列表里
        private bool IsInJournalDateList(DateTime pKeyDate,DataSet pJournalDataSet)
        {
            if (pJournalDataSet == null || pJournalDataSet.Tables[0] == null || pJournalDataSet.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                DataTable _tableJournal = pJournalDataSet.Tables[0];
                for (int i = 0; i < _tableJournal.Rows.Count; i++)
                {
                    if (pKeyDate.Date == Convert.ToDateTime(_tableJournal.Rows[i]["writetime"].ToString()).Date)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //判断日期pKeyDate是否在传入的假日日期列表里
        private bool IsRestDateList(DateTime pKeyDate, DataSet pRestDataSet)
        {
            if (pRestDataSet == null || pRestDataSet.Tables[0] == null || pRestDataSet.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                DataTable _tableRestDate = pRestDataSet.Tables[0];
                for (int i = 0; i < _tableRestDate.Rows.Count; i++)
                {
                    if (pKeyDate.Date == Convert.ToDateTime(_tableRestDate.Rows[i]["RestDate"].ToString()).Date)
                    {
                        if (_tableRestDate.Rows[i]["RestDetail"].ToString()=="上午")
                        {
                            return false;
                        }
                        else
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="UserId">根据用户ID</param>
        /// <returns></returns>
        public bool Delete(string UserId, string jourId)
        {
            bool _Result = true;
            try
            {
                string sql = string.Format(SQL_Delete_UserJournal, jourId, UserId);
                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, sql, null);
            }
            catch (Exception ex)
            {
                _Result = false;
            }
            return _Result;
        }



        ///// <summary>
        ///// 将一个日期字符串，如果月份或日期小于10 ，将前面补一个0
        ///// </summary>
        ///// <param name="pDate"></param>
        ///// <returns></returns>
        //private string GetFullDate(string pDate)
        //{
        //    DateTime _date = Convert.ToDateTime(pDate);
        //    string _full = pDate;
        //    if (_date.Month < 10)
        //    {
        //        _full = _full.Insert(pDate.IndexOf("-") + 1, "0");
        //    }
        //    if (_date.Day < 10)
        //    {
        //        _full = _full.Insert(_full.LastIndexOf("-") + 1, "0");
        //    }
        //    return _full;
        //}
        #endregion
    }
}



public class NoWritePersonInfo
{
    private int m_UserId;

    public int UserId
    {
        get { return m_UserId; }
        set { m_UserId = value; }
    }


    private string m_RealName;

    public string RealName
    {
        get { return m_RealName; }
        set { m_RealName = value; }
    }

    private int m_NoWriteCount;

    public int NoWriteCount
    {
        get { return m_NoWriteCount; }
        set { m_NoWriteCount = value; }
    }

    private string m_NoWriteDate;

    public string NoWriteDate
    {
        get { return m_NoWriteDate; }
        set { m_NoWriteDate = value; }
    }

}