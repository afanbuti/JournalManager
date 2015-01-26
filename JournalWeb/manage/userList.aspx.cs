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
    public partial class userList : NetChina.Other.BasePage
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
                    string mid = Request.QueryString["mid"].ToString();
                    string bDate = Request.QueryString["bd"].ToString();
                    string eDate = Request.QueryString["ed"].ToString();
                    this.hid_bDate.Value = bDate;
                    this.hid_eDate.Value = eDate;
                    hidMissionID.Value = mid;
                    GetUserList();
                    GetMissionDetail(mid);

                }
            }
        }

        public void GetMissionDetail(string mid)
        {
            NetUserMission uMission = new NetUserMission();
            UserMission uinfo = new UserMission();
            uinfo=uMission.GetNetMissionInfo(mid);
            if (uinfo != null)
            {
                this.div_desc.InnerHtml = uinfo.MissionDesc;
                this.radio_UserList.SelectedValue = uinfo.UserID.ToString();
                hidSelectUser.Value = uinfo.UserID.ToString();
            }
            else
            {
                this.radio_UserList.SelectedValue = "0";
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        public void GetUserList()
        {
            NetUserInfo uInfo = new NetUserInfo();
            UserInfo info = new UserInfo();
            DataSet ds = uInfo.GetUserInfos("", "", 0, 1, true);
            this.radio_UserList.Items.Clear();           
            if (ds != null)
            {
                int dsCount = ds.Tables[0].Rows.Count;
                if (dsCount > 0)
                {
                    radio_UserList.Items.Add(new ListItem("空", "0"));
                    for (int i = 0; i < dsCount; i++)
                    {
                        string value = ds.Tables[0].Rows[i][0].ToString();
                        string text = ds.Tables[0].Rows[i][3].ToString();
                        radio_UserList.Items.Add(new ListItem(text, value));
                    }
                    //this.radio_UserList.SelectedValue = ds.Tables[0].Rows[0][0].ToString();
                }
            }            
        }
    }
}
