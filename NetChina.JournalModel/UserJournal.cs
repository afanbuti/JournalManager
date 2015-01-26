using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.JournalModel
{
    [Serializable]
    public class UserJournal
    {
        #region 字段列表
        /// <summary>
        /// 日志编号
        /// </summary>
        private int _jourId;
        /// <summary>
        /// 用户编号
        /// </summary>
        private int _userId;
        /// <summary>
        /// 用户名称
        /// </summary>
        private string _userName;
        /// <summary>
        /// 日志描述
        /// </summary>
        private string _jourDesc;
        /// <summary>
        /// 写入时间
        /// </summary>
        private DateTime _writeTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime _modifyTime;

        /// <summary>
        /// 评价内容
        /// </summary>
        private string _evalContent;

        /// <summary>
        /// 日志编号
        /// </summary>
        public int JourID
        {
            get { return _jourId; }
            set { _jourId = value; }
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        /// <summary>
        /// 日志描述
        /// </summary>
        public string JourDesc
        {
            get { return _jourDesc; }
            set { _jourDesc = value; }
        }

        /// <summary>
        /// 写入时间
        /// </summary>
        public DateTime WriteTime
        {
            get { return _writeTime; }
            set { _writeTime = value; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return _modifyTime; }
            set { _modifyTime = value; }
        }
        /// <summary>
        /// 评价内容
        /// </summary>
        public string EvalContent
        {
            get { return _evalContent; }
            set { _evalContent = value; }
        }

        #endregion 

        #region 搜索字段设置

        private DateTime beginDate; //开始时间
        private DateTime endDate;   //结束时间
        private string keyWord;     //关键字

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord
        {
            get { return keyWord; }
            set { keyWord = value; }
        }

        #endregion

        #region 构造函数列表

        public UserJournal()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_jourIdX">日志编号</param>
        /// <param name="_userIdX">用户编号</param>
        /// /// <param name="_userIdX">用户姓名</param>
        /// <param name="_jourDescX">日志内容</param>
        /// <param name="_writeTimeX">写入时间</param>
        /// <param name="_modifyTimeX">修改时间</param>
        public UserJournal
        (
            int _jourIdX,
            int _userIdX,
            string _userNameX,
            string _jourDescX,
            DateTime _writeTimeX,
            DateTime _modifyTimeX,
            string _evalContentX
        )
        {
            _jourId = _jourIdX;
            _userId = _userIdX;
            _userName = _userNameX;
            _jourDesc = _jourDescX;
            _writeTime = _writeTimeX;
            _modifyTime = _modifyTimeX;
            _evalContent = _evalContentX;
        }       
        #endregion
    }
}
