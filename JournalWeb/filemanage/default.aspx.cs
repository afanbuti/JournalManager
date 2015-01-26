using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using NetChina.Common;
using NetChina.Other;
using NetChina.AccessHelper;
using NetChina.JournalModel;

namespace JournalWeb.filemanage
{
    public partial class _default : BasePage
    {
        int PageSize = 20;
        protected string tempTitle = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["groupId"].ToString() == "1")
            {
                Response.Redirect("/user/viewList.aspx");
            }
            else
            {
                if (Session["groupId"].ToString() == "2")
                {
                    string tempStr = string.Empty;           

                    tempStr += "<a href=\"/manage/JournalList.aspx\" target=\"_self\" class=\"top_a\">总结查看</a><br /><br />";
                    tempStr += "<a href=\"/manage/userManager.aspx\" target=\"_self\"  class=\"top_a\">用户管理</a><br /><br />";
                    tempStr += "<a href=\"/manage/MissionList.aspx\" target=\"_self\"  class=\"top_a\">任务管理</a><br /><br />";
                    tempStr += "<a href=\"/filemanage/default.aspx\" target=\"_self\"  class=\"top_b\">文档管理</a><br /><br />";
                    tempStr += "<a href=\"/manage/systemManage.aspx\" target=\"_self\"  class=\"top_a\">假日管理</a><br /><br />";
                    span_Manager.InnerHtml = tempStr;
                }
                KeyWord = Request.QueryString["tl"];//标题关键字
                DocType = Request.QueryString["dty"];//文档类型

                if (!IsPostBack)
                {
                    GetTypeList();
                    GetFileList(false);
                }
            }
        }

        /// <summary>
        /// 获取类别列表
        /// </summary>
        public void GetTypeList()
        {
            string file = Server.MapPath("/Inc/docType.xml");
            DataSet objDs = operateXml.ConvertXMLFileToDataSet(file);
            this.drop_DocType.Items.Clear();
            if (objDs != null)
            {
                int objCount = objDs.Tables[0].Rows.Count;
                if (objCount > 0)
                {      
                    
                    this.drop_DocType.Items.Add(new ListItem("全部","-1"));
                    for (int i = 0; i < objCount; i++)
                    {
                        string value = objDs.Tables[0].Rows[i]["key"].ToString();
                        string text = objDs.Tables[0].Rows[i]["name"].ToString();

                        this.drop_DocType.Items.Add(new ListItem(text, value));
                        if (text == DocType)
                        {
                            this.drop_DocType.SelectedValue = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取列表信息
        /// </summary>
        /// <param name="IsSearch">是否为搜索</param>
        public void GetFileList(bool IsSearch)
        {
            NetFileManager fileBll = new NetFileManager();
            FileManager fileInfo = new FileManager();

            #region 分页处理

            if (this.beginTime.Value.Trim() == "" || this.endTime.Value.Trim() == "")
            {
                this.endTime.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                this.beginTime.Value = System.DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                beginSearchDate = this.beginTime.Value;
                endSearchDate = this.endTime.Value;
                DocType = this.drop_DocType.SelectedItem.Text;

            }
            if (IsSearch)
            {
                beginSearchDate = Convert.ToDateTime(this.beginTime.Value).ToString("yyyy-MM-dd");
                endSearchDate = Convert.ToDateTime(this.endTime.Value).ToString("yyyy-MM-dd");
                DocType = this.drop_DocType.SelectedItem.Text;
                KeyWord = this.txt_KeyWord.Value.Trim();
            }
            else
            {
                if (Request.QueryString["bd"] != null && Request.QueryString["ed"] != null)
                {
                    try
                    {
                        beginSearchDate = Convert.ToDateTime(Request.QueryString["bd"]).ToString("yyyy-MM-dd");
                        endSearchDate = Convert.ToDateTime(Request.QueryString["ed"]).ToString("yyyy-MM-dd");

                    }
                    catch
                    {
                        beginSearchDate = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                        endSearchDate = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    beginSearchDate = Convert.ToDateTime(this.beginTime.Value).ToString("yyyy-MM-dd");
                    endSearchDate = Convert.ToDateTime(this.endTime.Value).ToString("yyyy-MM-dd");
                    DocType = this.drop_DocType.SelectedItem.Text;
                }
            }
            this.beginTime.Value = beginSearchDate;
            this.endTime.Value = endSearchDate;

            int userCount = fileBll.GetFileFYCount(DocType, beginSearchDate, endSearchDate, KeyWord);//获取分页总数
            int currentPage = 1;
            try
            {
                currentPage = Convert.ToInt32(Request.QueryString["page"]);
                if (currentPage > (userCount + PageSize - 1) / PageSize)
                {
                    currentPage = (userCount + PageSize - 1) / PageSize;
                }

                if (currentPage <= 0) currentPage = 1;
            }
            catch
            {
                currentPage = 1;
            }

            #endregion

            DataSet ds = fileBll.GetFileDataSet(DocType, beginSearchDate, endSearchDate, PageSize, currentPage, KeyWord);
            if (ds != null)
            {
                HtmlTableRow hrow = null;
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        hrow = new HtmlTableRow();
                        int tempBH = (currentPage - 1) * PageSize + i + 1;
                        string tempOpreator = string.Empty;
                        tempOpreator = "<div><a href='" + ds.Tables[0].Rows[i]["FilePath"].ToString() + "' target='_blank'>下载</a> | <a href='javascript:void(0);' onclick=\"del('" + ds.Tables[0].Rows[i]["FileId"].ToString() + "','" + ds.Tables[0].Rows[i]["UserId"].ToString() + "','" + ds.Tables[0].Rows[i]["FilePath"].ToString() + "')\">删除</a></div>";
                        hrow.Cells.Add(GetHCell(tempOpreator, false));//操作
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[i]["FileTitle"].ToString(), false));
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[i]["FileType"].ToString(), false));
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[i]["realName"].ToString(), false));
                        hrow.Cells.Add(GetHCell(Convert.ToDateTime(ds.Tables[0].Rows[i]["WriteDate"].ToString()).ToString("MM/dd H:mm"), false));
                        tbl_list.Rows.Add(hrow);
                    }
                    div_noInfo.Style.Add("display", "none");
                }
                else
                {
                    div_noInfo.Attributes.Add("display", "block");
                }
            }
            else
            {
                div_noInfo.Attributes.Add("display", "none");
            }

            string parms = "&bd=" + beginSearchDate + "&ed=" + endSearchDate + "&tl=" + Server.UrlEncode(KeyWord) + "&dty=" + Server.UrlEncode(DocType);
            GetPage(userCount, PageSize, currentPage, parms);//获取分页文字
        }

        /// <summary>
        /// 创建td
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        private HtmlTableCell GetHCell(string strHtml, bool isTh)
        {
            HtmlTableCell hcell = null;
            if (isTh)
            {
                hcell = new HtmlTableCell("th");
            }
            else
            {
                hcell = new HtmlTableCell();
            }
            hcell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            hcell.InnerHtml = strHtml;
            return hcell;
        }

        public void GetPage(int RecordCount, int pageSize, int currentPage, string otherId)
        {

            this.tbl_td_page.InnerHtml = PageNavigator.Pagination(RecordCount, pageSize, currentPage, "default.aspx", otherId);
        }

        #region 属性列表

        /// <summary>
        /// 文档类型
        /// </summary>
        public string DocType
        {
            get { return (string)ViewState["doctype"]; }
            set { ViewState["doctype"] = value; }
        }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord
        {
            get { return (string)ViewState["keyword"]; }
            set { ViewState["keyword"] = value; }
        }

        /// <summary>
        /// 开始日期属性
        /// </summary>
        public string beginSearchDate
        {
            set { ViewState["beginSearchDate"] = value; }
            get { return (string)ViewState["beginSearchDate"]; }
        }

        /// <summary>
        /// 结束日期属性
        /// </summary>
        public string endSearchDate
        {
            set { ViewState["endSearchDate"] = value; }
            get { return (string)ViewState["endSearchDate"]; }
        }

        #endregion

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            GetFileList(true);
        }
    }
}
