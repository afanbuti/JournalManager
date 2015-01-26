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

namespace JournalWeb.user
{
    public partial class userModify : NetChina.Other.BasePage //System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["groupId"].ToString() == "2")
            {
                Response.Redirect("/user/JournalList.aspx");
            }
            else if (Session["groupId"].ToString() == "4")
            {
                Response.Redirect("/filemanage/default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
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
                hidgid.Value = info.GroupID;
                this.txt_UserName.Text = info.UserName;
                this.txt_RealName.Text = info.RealName;
                hidOrder.Value = info.UserOrder.ToString();
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
