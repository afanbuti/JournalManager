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

namespace JournalWeb
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            
        }

        protected void imgBtn_Login_Click(object sender, ImageClickEventArgs e)
        {
            //NetUserInfo user = new NetUserInfo();
            UserInfo info = NetUserInfo.CheckUserAndPass(this.txtUserName.Text, this.txtPwd.Text);
            if (info != null)
            {
                if (info.GroupID == "1")
                {
                    Session["uname"] = this.txtUserName.Text;
                    Session["groupId"] = info.GroupID;
                    Session["realName"] = info.RealName;
                    //Session["IsManager"] = info.IsManager;
                    Session["userid"] = info.UserID;                    
                    Response.Redirect("/user/viewList.aspx");
                }
                else if (info.GroupID == "2")
                {
                    Session["uname"] = this.txtUserName.Text;
                    Session["groupId"] = info.GroupID;
                    Session["realName"] = info.RealName;
                    //Session["IsManager"] = info.RealName;
                    Session["userid"] = info.UserID;                    
                    Response.Redirect("/manage/JournalList.aspx");
                }
                else if (info.GroupID == "4")
                {
                    Session["uname"] = this.txtUserName.Text;
                    Session["groupId"] = info.GroupID;
                    Session["realName"] = info.RealName;
                    //Session["IsManager"] = info.RealName;
                    Session["userid"] = info.UserID;
                    Response.Redirect("/filemanage/default.aspx");
                }
            }
            else
            {
                this.errMessage.Text = "<span style='color:red;font-size:12px;'>温馨提示：身份验证失败！</span>";
            }
        }              
    }
}
