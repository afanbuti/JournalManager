using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.JournalModel
{
    [Serializable]
    public class UserInfo
    {

        #region 字段列表
        /// <summary>
        /// 用户ID
        /// </summary>
        private int _userId;
        /// <summary>
        /// 用户所属分组ID
        /// </summary>
        private string _groupId;
        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;
        /// <summary>
        /// 真实姓名
        /// </summary>
        private string _realName;
        /// <summary>
        /// 用户密码
        /// </summary>
        private string _netPass;
        /// <summary>
        /// 用户修改信息时间
        /// </summary>
        private DateTime _userModTime;
        /// <summary>
        /// 是否为管理员
        /// </summary>
        private string _isManager;

        /// <summary>
        /// 序号
        /// </summary>
        private int _userOrder;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 分组ID
        /// </summary>
        public string GroupID
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName
        {
            get { return _realName; }
            set { _realName = value; }
        }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string NetPass
        {
            get { return _netPass; }
            set { _netPass = value; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UserModTime
        {
            get { return _userModTime; }
            set { _userModTime = value; }
        }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public string IsManager
        {
            get { return _isManager; }
            set { _isManager = value; }
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int UserOrder
        {
            get { return _userOrder; }
            set { _userOrder = value; }
        }

        #endregion

        #region 构造函数

        public UserInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_userIdx">用户ID</param>
        /// <param name="_groupIdX">用户所属分组</param>
        /// <param name="_userNameX">用户名/登录名</param>
        /// <param name="_realNameX">用户真实姓名</param>
        /// <param name="_netPassX">用户密码</param>
        /// <param name="_userModTimeX">最后修改日期</param>
        /// <param name="_isManagerX">是否为管理员</param>
        public UserInfo
        (
            int _userIdx,
            string _groupIdX, 
            string _userNameX, 
            string _realNameX, 
            string _netPassX, 
            DateTime _userModTimeX, 
            string _isManagerX,
            int _userOrderX
        )
        {
            _userId = _userIdx;
            _groupId = _groupIdX;
            _userName = _userNameX;
            _realName = _realNameX;
            _netPass = _netPassX;
            _userModTime = _userModTimeX;
            _isManager = _isManagerX;
            _userOrder=_userOrderX;
        }
        #endregion
    }
}
