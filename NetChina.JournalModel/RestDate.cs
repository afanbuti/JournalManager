using System;
using System.Collections.Generic;
using System.Text;

namespace NetChina.JournalModel
{
    [Serializable]
   public class RestDate
    {

        #region �ֶ��б�
        private int m_RestID;
        /// <summary>
        /// ����ID
        /// </summary>
        public int RestID
        {
            get { return m_RestID; }
            set { m_RestID = value; }
        }


        private int m_UserId;
        /// <summary>
        /// Ա��ID,-1ָ������
        /// </summary>
        public int UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        private DateTime m_RestDate;
        /// <summary>
        /// ��������
        /// </summary>
        public DateTime Rest_Date
        {
            get { return m_RestDate; }
            set { m_RestDate = value; }
        }

        private string m_RestDetail;
        /// <summary>
        /// ȫ�죬���磬����
        /// </summary>
        public string RestDetail
        {
            get { return m_RestDetail; }
            set { m_RestDetail = value; }
        }

        private int m_CycleNum;
        /// <summary>
        /// ѭ������
        /// </summary>
        public int CycleNum
        {
            get { return m_CycleNum; }
            set { m_CycleNum = value; }
        }

        private string m_Type;
        /// <summary>
        /// �������ͣ����������գ������ڼ٣������ڼ٣�������٣����˳����������
        /// </summary>
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        private string m_Remark;
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region �����ֶ��б�
        private int ms_UserId;
        /// <summary>
        /// �û�Id
        /// </summary>
        public int S_UserId
        {
            get { return ms_UserId; }
            set { ms_UserId = value; }
        }

        private string ms_Type;
        /// <summary>
        /// ��������
        /// </summary>
        public string S_Type
        {
            get { return ms_Type; }
            set { ms_Type = value; }
        }

        private int ms_PageSize;
        /// <summary>
        /// ÿҳ��ʾ��С
        /// </summary>
        public int S_PageSize
        {
            get { return ms_PageSize; }
            set { ms_PageSize = value; }
        }

        private int ms_CurrentPage;
        /// <summary>
        /// ��ǰ�ڼ�ҳ
        /// </summary>
        public int S_CurrentPage
        {
            get { return ms_CurrentPage; }
            set { ms_CurrentPage = value; }
        }


        private string ms_BeginDate;
        /// <summary>
        /// ������ʼ����
        /// </summary>
        public string S_BeginDate
        {
            get { return ms_BeginDate; }
            set { ms_BeginDate = value; }
        }

        private string ms_EndDate;
        /// <summary>
        /// ������������
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

        #region �����õ��Ĺ��캯��

        /// <summary>
        ///�����õ��Ĺ��캯��
        /// </summary>
     /// <param name="pUserId">��ԱId</param>
     /// <param name="pType">��������</param>
     /// <param name="pBegionDate">������ʼ����</param>
     /// <param name="pEndDate">������������</param>
     /// <param name="pPageSize">ÿҳ��ʾ��С</param>
     /// <param name="pCurrentPage">��ǰҳ</param>
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

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="pUserId">��Աid</param>
        /// <param name="pRestDate">��������</param>
        /// <param name="pCycleNum">ѭ������</param>
        /// <param name="pRestDetail">ȫ�죬���磬����</param>
        /// <param name="pType">��������</param>
        /// <param name="pRemark">��ע</param>
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
