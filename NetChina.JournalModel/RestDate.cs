using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.JournalModel
{
    [Serializable]
   public class RestDate
    {

        #region 字段列表
        private int m_RestID;
        /// <summary>
        /// 假日ID
        /// </summary>
        public int RestID
        {
            get { return m_RestID; }
            set { m_RestID = value; }
        }


        private int m_UserId;
        /// <summary>
        /// 员工ID,-1指所有人
        /// </summary>
        public int UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        private DateTime m_RestDate;
        /// <summary>
        /// 假日日期
        /// </summary>
        public DateTime Rest_Date
        {
            get { return m_RestDate; }
            set { m_RestDate = value; }
        }

        private string m_RestDetail;
        /// <summary>
        /// 全天，上午，下午
        /// </summary>
        public string RestDetail
        {
            get { return m_RestDetail; }
            set { m_RestDetail = value; }
        }

        private int m_CycleNum;
        /// <summary>
        /// 循环天数
        /// </summary>
        public int CycleNum
        {
            get { return m_CycleNum; }
            set { m_CycleNum = value; }
        }

        private string m_Type;
        /// <summary>
        /// 假日类型：周六，周日，法定节假，其他节假，个人请假，个人出差，个人其他
        /// </summary>
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        private string m_Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 搜索字段列表
        private int ms_UserId;
        /// <summary>
        /// 用户Id
        /// </summary>
        public int S_UserId
        {
            get { return ms_UserId; }
            set { ms_UserId = value; }
        }

        private string ms_Type;
        /// <summary>
        /// 假日类型
        /// </summary>
        public string S_Type
        {
            get { return ms_Type; }
            set { ms_Type = value; }
        }

        private int ms_PageSize;
        /// <summary>
        /// 每页显示大小
        /// </summary>
        public int S_PageSize
        {
            get { return ms_PageSize; }
            set { ms_PageSize = value; }
        }

        private int ms_CurrentPage;
        /// <summary>
        /// 当前第几页
        /// </summary>
        public int S_CurrentPage
        {
            get { return ms_CurrentPage; }
            set { ms_CurrentPage = value; }
        }


        private string ms_BeginDate;
        /// <summary>
        /// 搜索开始日期
        /// </summary>
        public string S_BeginDate
        {
            get { return ms_BeginDate; }
            set { ms_BeginDate = value; }
        }

        private string ms_EndDate;
        /// <summary>
        /// 搜索结束日期
        /// </summary>
        public string S_EndDate
        {
            get { return ms_EndDate; }
            set { ms_EndDate = value; }
        }

        #endregion


        public RestDate()
        {
        }

        #region 搜索用到的构造函数

        /// <summary>
        ///搜索用到的构造函数
        /// </summary>
     /// <param name="pUserId">人员Id</param>
     /// <param name="pType">假日类型</param>
     /// <param name="pBegionDate">搜索开始日期</param>
     /// <param name="pEndDate">搜索结束日期</param>
     /// <param name="pPageSize">每页显示大小</param>
     /// <param name="pCurrentPage">当前页</param>
        public RestDate(int pUserId,string pType, string pBegionDate,string pEndDate,int pPageSize,int pCurrentPage)
        {
            ms_UserId = pUserId;
            ms_Type = pType;
            ms_BeginDate = pBegionDate;
            ms_EndDate = pEndDate;
            ms_PageSize = pPageSize;
            ms_CurrentPage = pCurrentPage;
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pUserId">人员id</param>
        /// <param name="pRestDate">假期日期</param>
        /// <param name="pCycleNum">循环天数</param>
        /// <param name="pRestDetail">全天，上午，下午</param>
        /// <param name="pType">假日类型</param>
        /// <param name="pRemark">备注</param>
        public RestDate(int pUserId,DateTime pRestDate,int pCycleNum,string pRestDetail, string pType, string pRemark)
        {
            m_UserId = pUserId;
            m_RestDate = pRestDate;
            m_CycleNum = pCycleNum;
            m_RestDetail = pRestDetail;
            m_Type = pType;
            m_Remark = pRemark;
        }
        #endregion



    }
}
