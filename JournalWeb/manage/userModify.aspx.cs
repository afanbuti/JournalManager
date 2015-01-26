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
    public partial class userModify : NetChina.Other.BasePage
    {
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
                    GetDetailByUid();
                }

            }
        }

        public void GetDetailByUid()
        {
            UserId = Request.QueryString["uid"];

            if (!string.IsNullOrEmpty(UserId))
            {
                this.hid_userid.Value = UserId;
                NetUserInfo uInfo = new NetUserInfo();
                UserInfo info = uInfo.GetNetUserInfo(UserId);
                this.drop_groupList.SelectedValue = info.GroupID;
                hidgid.Value = info.GroupID;
                this.txt_UserName.Text = info.UserName;
                this.txt_RealName.Text = info.RealName;
                this.txt_XH.Text = info.UserOrder.ToString();
            }
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

                    for (int i = 0; i < count; i++)
                    {
                        this.drop_groupList.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId
        {
            get { return (string)ViewState["userid"]; }
            set { ViewState["userid"] = value; }
        }
    }
}
