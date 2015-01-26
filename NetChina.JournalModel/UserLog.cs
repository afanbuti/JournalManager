using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.JournalModel
{
    [Serializable]
    public class UserLog
    {
        #region 字段列表
        /// <summary>
        /// 登陆ID
        /// </summary>
        private int _logId;
        /// <summary>
        /// 登陆用户ID
        /// </summary>
        private string _userId;
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        private DateTime _loginDate;

        /// <summary>
        /// 登陆ID
        /// </summary>
        public int LogID
        {
            get { return _logId; }
            set { _logId = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime LoginDate
        {
            get { return _loginDate; }
            set { _loginDate = value; }
        }

        #endregion 

        #region 构造函数

        public UserLog()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_logIdX">日志ID</param>
        /// <param name="_userIdX">用户ID</param>
        /// <param name="_loginDateX">登陆最后日期</param>
        public UserLog(int _logIdX, string _userIdX, DateTime _loginDateX)
        {
            _logId = _logIdX;
            _userId = _userIdX;
            _loginDate = _loginDateX;
        }

        #endregion
    }
}
