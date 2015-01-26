using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using NetChina.JournalModel;
using System.Data;

namespace NetChina.AccessHelper
{
    public class NetUserInfo
    {
        #region SQL 变量

        //根据分组编号获取所有用户信息
        private const string SQL_Select_UserInfoFirst = "Select top {0} * From tbl_NetUserInfo where groupId=@GroupId Order By userOrder desc, userId Desc";//现在的页码*每页显示的条数
        private const string SQL_Select_UserInfo = "Select top {0} * From tbl_NetUserInfo where userId not in (select top {1} userId from tbl_NetUserInfo where groupId=@GroupId order by userOrder desc, userId desc) and groupId=@GroupId Order By userOrder desc, userId Desc";

        private const string SQL_Select_UserAll = "Select * From tbl_NetUserInfo where groupId='1' Order By userOrder desc, userId Desc ";//现在的页码*每页显示的条数
        private const string SQL_Select_UserCount = "Select count(*) as resultCount From tbl_NetUserInfo where groupId=@GroupId";
        //根据用户编号获取单个用户信息
        private const string SQL_Select_UserInfoPrimaryKey = "Select * From tbl_NetUserInfo where userId=@UserId";
        //根据用户名来判断用户是否存在
        private const string SQL_Select_UserByUserName = "Select * From tbl_NetUserInfo where UserName=@UserName";
        //根据用户编号删除用户信息
        private const string SQL_Delete_UserInfoPrimaryKey = "Delete From tbl_NetUserInfo where userId=@UserId";
        //插入用户信息
        private const string SQL_Insert_UserInfo = "insert into tbl_NetUserInfo(groupId,userName,realName,netPass,userModTime,IsManager,userOrder) values (@GroupId,@UserName,@realName,@NetPass,@UserModTime,@IsManager,@UserOrder)";
        //修改用户信息
        private const string SQL_Update_UserInfo = "Update tbl_NetUserInfo set groupId=@GroupId,userName=@UserName,realName=@RealName,netPass=@NetPass,userModTime=@UserModTime,IsManager=@IsManager,userOrder=@UserOrder Where userId={0}";

        private const string SQL_CheckUser = "Select * From tbl_NetUserInfo where userName=@userName and netPass=@netPass";

        #endregion

        #region 参数变量

        private const string Parm_UserId = "@UserId";           //用户ID
        private const string Parm_GroupId = "@GroupId";         //分组ID
        private const string Parm_UserName = "@UserName";       //用户名
        private const string Parm_RealName = "@RealName";       //真是姓名
        private const string Parm_NetPass = "@NetPass";         //用户密码
        private const string Parm_UserModTime = "@UserModTime"; //最后修改时间
        private const string Parm_IsManager = "@IsManager";     //是否为管理员
        private const string Parm_UserOrder = "@UserOrder";     //排序编号

        private const string Parm_KeyWord = "@KeyWord";         //关键字
        private const string Parm_PageSize = "@PageSize";       //每页大小

        #endregion 

        #region 方法列表
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="info">传入UserInfo对象</param>
        /// <returns></returns>
        public bool Insert(UserInfo info)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_GroupId,OleDbType.VarChar,50),
															   new OleDbParameter(Parm_UserName,OleDbType.VarChar,200),
															   new OleDbParameter(Parm_RealName,OleDbType.VarChar,200),
															   new OleDbParameter(Parm_NetPass,OleDbType.VarChar,100),
                                                               new OleDbParameter(Parm_UserModTime,OleDbType.Date),
                                                               new OleDbParameter(Parm_IsManager,OleDbType.VarChar,10),
                                                               new OleDbParameter(Parm_UserOrder,OleDbType.Integer)
														   };
                parms[0].Value = info.GroupID;
                parms[1].Value = info.UserName;
                parms[2].Value = info.RealName;
                parms[3].Value = info.NetPass;
                parms[4].Value = info.UserModTime;
                parms[5].Value = info.IsManager;
                parms[6].Value = info.UserOrder;

                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text, SQL_Insert_UserInfo, parms);
            }
            catch (Exception ex)
            {                
                _Result = false;
            }

            return _Result;
        }       
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="UserId">根据用户ID</param>
        /// <returns></returns>
        public bool Delete(int UserId)
        {
            bool _Result = true;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_UserId,OleDbType.Integer)															   
														   };
                parms[0].Value = UserId;
                AccessHelper.ExecuteNonQuery(AccessHelper.CONN_STRING, CommandType.Text,SQL_Delete_UserInfoPrimaryKey, parms);
            }
            catch(Exception ex)
            {
                _Result = false;
            }
            return _Result;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Update(UserInfo info)
        {
            bool _Result = true;
            try
            {
               OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_GroupId,OleDbType.VarChar,50),
															   new OleDbParameter(Parm_UserName,OleDbType.VarChar,200),
															   new OleDbParameter(Parm_RealName,OleDbType.VarChar,200),
															   new OleDbParameter(Parm_NetPass,OleDbType.VarChar,100),
                                                               new OleDbParameter(Parm_UserModTime,OleDbType.Date),
                                                               new OleDbParameter(Parm_IsManager,OleDbType.VarChar,10),
                                                               new OleDbParameter(Parm_UserOrder,OleDbType.Integer)
														   };
                parms[0].Value = info.GroupID;
                parms[1].Value = info.UserName;
                parms[2].Value = info.RealName;
                parms[3].Value = info.NetPass;
                parms[4].Value = info.UserModTime;
                parms[5].Value = info.IsManager;
                parms[6].Value = info.UserOrder;

                string sql = string.Format(SQL_Update_UserInfo, info.UserID);
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
        /// 获取单个用户信息
        /// </summary>
        /// <param name="teachId"></param>
        /// <returns></returns>
        public UserInfo GetNetUserInfo(string userid)
        {

            UserInfo info = null;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
									new OleDbParameter(Parm_UserId,OleDbType.VarChar)															   
														   };
                parms[0].Value = userid;
                using (OleDbDataReader sdr = AccessHelper.ExecuteReader(AccessHelper.CONN_STRING, CommandType.Text,SQL_Select_UserInfoPrimaryKey, parms))
                {
                    while (sdr.Read())
                    {
                        info = new UserInfo(
                            int.Parse(sdr["userId"].ToString()),
                            sdr["groupId"].ToString(),
                            sdr["userName"].ToString(),
                            sdr["realName"].ToString(),
                            sdr["netPass"].ToString(),
                            DateTime.Parse(sdr["userModTime"].ToString()),
                            sdr["IsManager"].ToString(),
                            int.Parse(sdr["userOrder"].ToString())
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
        /// 核对用户名和密码 0用户名或者密码错误 1 表示是管理员 2表示是普通人员
        /// </summary>
        /// <param name="useName"></param>
        /// <param name="Pass"></param>
        /// <returns></returns>
        public static UserInfo CheckUserAndPass(string useName, string Pass)
        {
            UserInfo info = null;           
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
									new OleDbParameter(Parm_UserName,OleDbType.VarChar,50)		,
					                new OleDbParameter(Parm_NetPass,OleDbType.VarChar,200)								   
														   };
                parms[0].Value = useName;
                parms[1].Value = Pass;
                using (OleDbDataReader sdr = AccessHelper.ExecuteReader(AccessHelper.CONN_STRING, CommandType.Text, SQL_CheckUser, parms))
                {
                    //while (sdr.Read())
                    //{
                    //    if (sdr["IsManager"].ToString() == "0")  //表示普通
                    //    {
                    //        _resultStr = "1";//普通人员
                    //    }
                    //    else
                    //    {
                    //        _resultStr = "2";//管理员
                    //    }
                    //}
                    while (sdr.Read())
                    {
                        info = new UserInfo(
                            int.Parse(sdr["userId"].ToString()),
                            sdr["groupId"].ToString(),
                            sdr["userName"].ToString(),
                            sdr["realName"].ToString(),
                            sdr["netPass"].ToString(),
                            DateTime.Parse(sdr["userModTime"].ToString()),
                            sdr["IsManager"].ToString(),
                            int.Parse(sdr["userOrder"].ToString())
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return info;

        }

        /// <summary>
        /// 核对用户名是否存在
        /// </summary>
        /// <param name="useName"></param>
        /// <returns></returns>
        public bool CheckUserByUserName(string useName)
        {
            bool _result = false;
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
									new OleDbParameter(Parm_UserName,OleDbType.VarChar,50)				                						   
														   };
                parms[0].Value = useName;

                object obj = AccessHelper.ExecuteScalar(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_UserByUserName, parms);
                if(obj==null)
                {
                    _result = true;//说明不存在
                }
            }
            catch (Exception ex)
            {
                _result = false;
            }
            return _result;
        }

        /// <summary>
        /// 获取用户的集合信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="inUse"></param>
        /// <returns></returns>
        public DataSet GetUserInfos(string keyword, string groupID, int pageSize, int currentPage, bool isView)
        {
            DataSet objDs = new DataSet("result");
            OleDbParameter[] parms = null;
            string sql = string.Empty;
            try
            {
              
                if (isView)
                {
                    sql = SQL_Select_UserAll;
                }
                else
                {
                    parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_GroupId,OleDbType.VarChar,200)                                                              

														   };
                    parms[0].Value = groupID;

                    if (currentPage == 1)
                    {
                        sql = string.Format(SQL_Select_UserInfoFirst, pageSize);
                    }
                    else
                    {
                        sql = string.Format(SQL_Select_UserInfo, pageSize, pageSize * (currentPage - 1));
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
        public int GetUserFYCount(string keyword, string groupID)
        {
            int resultNum = 0;            
            try
            {
                OleDbParameter[] parms = new OleDbParameter[]{
															   new OleDbParameter(Parm_GroupId,OleDbType.VarChar,200)                                                         

														   };
                parms[0].Value = groupID;                
                using (OleDbDataReader sdr = AccessHelper.ExecuteReader(AccessHelper.CONN_STRING, CommandType.Text, SQL_Select_UserCount, parms))
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
