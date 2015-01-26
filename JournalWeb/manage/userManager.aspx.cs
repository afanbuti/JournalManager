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
using NetChina.AccessHelper;
using NetChina.JournalModel;
using NetChina.Common;

namespace JournalWeb.manage
{
    public partial class addUser : NetChina.Other.BasePage
    {
        int PageSize = 20;
        string uid = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["groupId"].ToString() == "1")
            {
                Response.Redirect("/user/viewList.aspx");
            }
            else if (Session["groupId"].ToString() == "4")
            {
                Response.Redirect("/filemanage/default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    BindDrop_Group();//加载分组
                    GetUserList(false);
                }
            }
        }



        /// <summary>
        /// 获取用户列表
        /// </summary>
        public void GetUserList(bool IsSearch)
        {
            NetUserInfo uInfo = new NetUserInfo();
            string currentGroupId = string.Empty;
            int currentPage = 1;
            int userCount = 0;

            #region 判断是否为搜索
            if (IsSearch)
            {
                currentGroupId = this.drop_Group.SelectedValue;//获取当前选中的分组
                this.drop_groupList.SelectedValue = currentGroupId;
                userCount = uInfo.GetUserFYCount("", currentGroupId);//获取分页总数
            }
            else
            {
                if (Request.QueryString["gid"] != null)
                {
                    currentGroupId = Request.QueryString["gid"];
                    this.drop_Group.SelectedValue = currentGroupId;
                    this.drop_groupList.SelectedValue = currentGroupId;
                }
                else
                {
                    currentGroupId = this.drop_Group.SelectedValue;//获取当前选中的分组
                }
                userCount = uInfo.GetUserFYCount("", currentGroupId);//获取分页总数
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
            }
            #endregion

            DataSet ds = uInfo.GetUserInfos("", currentGroupId, PageSize, currentPage, false);

            if (ds != null)
            {
                int uCount = ds.Tables[0].Rows.Count;
                if (uCount > 0)
                {
                    HtmlTableRow hrow = null;
                    string tempOpreator = string.Empty;
                    for (int j = 0; j < uCount; j++)
                    {
                        hrow = new HtmlTableRow();
                        int tempBH = (currentPage - 1) * PageSize + j + 1;
                        hrow.Cells.Add(GetHCell(tempBH.ToString()));//编号
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[j][2].ToString()));//用户名
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[j][3].ToString()));//真实姓名
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[j][4].ToString()));//密码
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[j][5].ToString()));//修改时间

                        string tempGroup = this.drop_Group.SelectedItem.Text;
                        hrow.Cells.Add(GetHCell(tempGroup));   //分组
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[j]["userOrder"].ToString()));//
                        tempOpreator = string.Empty;
                        tempOpreator = "<a href=\"javascript:void(0);\" name=\"modify\" onclick=\"showDiv('" + ds.Tables[0].Rows[j][0].ToString() + "')\">修改</a>&nbsp;|&nbsp;<a href=\"javascript:void(0)\" onclick=\"del('" + ds.Tables[0].Rows[j][0].ToString() + "','" + ds.Tables[0].Rows[j][1].ToString() + "')\">删除</a>";
                        hrow.Cells.Add(GetHCell(tempOpreator));//操作内容

                        tbl_list.Rows.Add(hrow);
                    }
                }
            }
            string parms = "&gid=" + currentGroupId;
            GetPage(userCount, PageSize, currentPage, parms);//获取分页文字
        }

        /// <summary>
        /// 绑定分组内容
        /// </summary>
        public void BindDrop_Group()
        {
            NetGroup nGoup = new NetGroup();
            DataSet ds = nGoup.GetGroupList();
            if (ds != null)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    drop_groupList.Items.Clear();
                    drop_Group.Items.Clear();

                    for (int i = 0; i < count; i++)
                    {
                        this.drop_groupList.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                        this.drop_Group.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                    }
                }
            }
        }

        protected void btn_AddUser_Click(object sender, EventArgs e)
        {
            AddUserInfo();
        }

        public void GetPage(int RecordCount, int pageSize, int currentPage, string otherId)
        {

            this.tbl_td_page.InnerHtml = PageNavigator.Pagination(RecordCount, pageSize, currentPage, "userManager.aspx", otherId);
        }

        /// <summary>
        /// 创建td
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        private HtmlTableCell GetHCell(string strHtml)
        {
            HtmlTableCell hcell = new HtmlTableCell();
            hcell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            hcell.InnerHtml = strHtml;
            return hcell;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        public void AddUserInfo()
        {
            NetUserInfo nUser = new NetUserInfo();
            UserInfo info = new UserInfo();

            info.GroupID = this.drop_groupList.SelectedValue;//分组
            info.UserName = this.txt_UserName.Text; //用户名
            info.RealName = this.txt_RealName.Text; //真实姓名
            info.NetPass = this.txt_Pass.Text;      //密码
            info.UserModTime = System.DateTime.Now;
            if (info.GroupID == "1")
            {
                info.IsManager = "0"; ;
            }
            else
            {
                info.IsManager = "1";
            }
            info.UserOrder = Convert.ToInt32(this.txt_XH.Text.ToString());

            if (!nUser.CheckUserByUserName(info.UserName))
            {
                this.lit_Message.Text = "<font color=red>该用户已经存在！</font>";
            }
            else
            {
                if (nUser.Insert(info))
                {
                    this.lit_Message.Text = "<font color=red>添加用户成功！</font>";
                }
                else
                {
                    this.lit_Message.Text = "<font color=red>添加用户失败！</font>";
                }
            }
            this.txt_UserName.Text = "";
            this.txt_RealName.Text = "";
            this.txt_Pass.Text = "";

            GetUserList(false);
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            GetUserList(true);
        }
    }
}
