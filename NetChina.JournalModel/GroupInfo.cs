using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.JournalModel
{
    [Serializable]
    public class GroupInfo
    {
        #region 字段列表
        /// <summary>
        /// 分组ID
        /// </summary>
        private int _groupId;
        /// <summary>
        /// 分组名称
        /// </summary>
        private string _groupName;

        /// <summary>
        /// 分组ID
        /// </summary>
        public int GroupID
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        #endregion

        #region  构造函数

        public GroupInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_groupIdX">分组Id</param>
        /// <param name="_groupNameX">分组名称</param>
        public GroupInfo(int _groupIdX,string _groupNameX)
        {
            _groupId = _groupIdX;
            _groupName = _groupNameX;
        }

        #endregion
    }
}
