using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.JournalModel
{
    [Serializable]
    public class UserMission
    {
        #region 字段列表

  

        /// <summary>
        /// 任务ID
        /// </summary>
        private int _missionId;
        /// <summary>
        /// 用户ID        
        /// </summary>
        private int _userId;
        /// <summary>
        /// 任务标题
        /// </summary>
        private string _missionTitle;
        /// <summary>
        /// 任务描述
        /// </summary>
        private string _missionDesc;
        /// <summary>
        /// 写入时间
        /// </summary>
        private DateTime _writeDate;
        /// <summary>
        /// 开始执行日期
        /// </summary>
        private DateTime _execDate;
        /// <summary>
        /// 完成日期
        /// </summary>
        private DateTime _finishDate;
        /// <summary>
        /// 执行状态 0,表示待执行 1,表示执行中 2,表示任务已经完成
        /// </summary>
        private string _execStatus;
        /// <summary>
        /// 任务是否分发到人 0,表示没有分发 1,表示分发了
        /// </summary>
        private string _isAsigin;
        /// <summary>
        /// 任务类型 0,表示开发任务 1,表示维护任务
        /// </summary>
        private string _missionType;
        /// <summary>
        /// 文件路径
        /// </summary>
        private string _filePath;

        /// <summary>
        /// 任务编号
        /// </summary>
        public int MissionID
        {
            get { return _missionId; }
            set { _missionId = value; }
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
        /// 任务标题
        /// </summary>
        public string MissionTitle
        {
            get { return _missionTitle; }
            set { _missionTitle = value; }
        }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string MissionDesc
        {
            get { return _missionDesc; }
            set { _missionDesc = value; }
        }
        /// <summary>
        /// 写入日期
        /// </summary>
        public DateTime WriteDate
        {
            get { return _writeDate; }
            set { _writeDate = value; }
        }
        /// <summary>
        /// 开始执行日期
        /// </summary>
        public DateTime ExecDate
        {
            get { return _execDate; }
            set { _execDate = value; }
        }
        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime FinishDate
        {
            get { return _finishDate; }
            set { _finishDate = value; }
        }
        /// <summary>
        /// 执行状态 0,表示待执行 1,表示执行中 2,表示任务已经完成
        /// </summary>
        public string ExecStatus
        {
            get { return _execStatus; }
            set { _execStatus = value; }
        }
        /// <summary>
        /// 任务是否分发到人 0,表示没有分发 1,表示分发了
        /// </summary>
        public string IsAsigin
        {
            get { return _isAsigin; }
            set { _isAsigin = value; }
        }

        /// <summary>
        /// 任务类型 0,表示开发任务 1,表示维护任务
        /// </summary>
        public string MissionType
        {
            get { return _missionType; }
            set { _missionType = value; }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        private int m_WorkHour;
        /// <summary>
        /// 任务计划工时
        /// </summary>
        public int WorkHour
        {
            get { return m_WorkHour; }
            set { m_WorkHour = value; }
        }

        private string m_RealProcess;
        /// <summary>
        /// 实际工作进度
        /// </summary>
        public string RealProcess
        {
            get { return m_RealProcess; }
            set { m_RealProcess = value; }
        }

        #endregion

        #region 搜索用到的字段列表

        /// <summary>
        /// 每页显示大小
        /// </summary>
        private int _pageSize;
        /// <summary>
        /// 当前页
        /// </summary>
        private int _currentPage;

        /// <summary>
        /// 搜索开始日期
        /// </summary>
        private string _beginDate;
        /// <summary>
        /// 搜索结束日期
        /// </summary>
        private string _endDate;

        /// <summary>
        /// 每页显示大小
        /// </summary>
        public int PageSize
        {
            get{return _pageSize;}
            set{_pageSize=value;}
        }
        /// <summary>
        /// 当前第几页
        /// </summary>
        public int CurrentPage
        {
            get{return _currentPage;}
            set{_currentPage=value;}
        }
        /// <summary>
        /// 搜索开始日期
        /// </summary>
        public string BeginDate
        {
            get{return _beginDate;}
            set{_beginDate=value;}
        }
        /// <summary>
        /// 搜索结束日期
        /// </summary>
        public string EndDate
        {
            get{return _endDate;}
            set{_endDate=value;}
        }
        #endregion

        #region 搜索用到的构造函数

        /// <summary>
        /// 初始化搜索的构造函数
        /// </summary>
        /// <param name="_userIdX">用户ID</param>
        /// <param name="_execStatusX">执行状态</param>
        /// <param name="_beginDateX">搜索开始日期</param>
        /// <param name="_endDateX">搜索结束日期</param>
        /// <param name="_pageSizeX">每页显示大小</param>
        /// <param name="_currentPageX">当前页</param>
        public UserMission(int _userIdX, string _execStatusX, string _beginDateX,string _endDateX,int _pageSizeX,int _currentPageX)
        {
            _userId = _userIdX;
            _execStatus = _execStatusX;
            _beginDate = _beginDateX;
            _endDate = _endDateX;
            _pageSize = _pageSizeX;
            _currentPage = _currentPageX;
        }

        #endregion

        #region 构造函数

        public UserMission()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_missionIdX">任务编号</param>
        /// <param name="_userIdX">用户编号</param>
        /// <param name="_missionTitleX">任务标题</param>
        /// <param name="_missionDescX">任务描述</param>
        /// <param name="_writeDateX">写入日期</param>
        /// <param name="_execDateX">执行日期</param>
        /// <param name="_finishDateX">完成日期</param>
        /// <param name="_execStatusX">执行状态 0,表示待执行 1,表示执行中 2,表示任务已经完成</param>
        /// <param name="_missionTypeX">任务类型 0,表示开发任务 1,表示维护任务</param>
        public UserMission
            (
            int _missionIdX,
            int _userIdX,
            string _missionTitleX,
            string _missionDescX,
            int _workHourX,
            string _realProcessX,
            DateTime _writeDateX,
            DateTime _execDateX,
            DateTime _finishDateX,
            string _execStatusX,
            string _missionTypeX,
            string _filePathX
            )
        {
            _missionId = _missionIdX;
            _userId = _userIdX;
            _missionTitle = _missionTitleX;
            _missionDesc = _missionDescX;
            m_WorkHour = _workHourX;
            m_RealProcess = _realProcessX;
            _writeDate = _writeDateX;
            _execDate = _execDateX;
            _finishDate = _finishDateX;
            _execStatus = _execStatusX;
            _missionType = _missionTypeX;
            _filePath = _filePathX;
        }
        #endregion
    }
}
