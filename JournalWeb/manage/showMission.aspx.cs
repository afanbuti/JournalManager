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
    public partial class showMission : NetChina.Other.BasePage
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
                    string mid = Request.QueryString["mid"];

                    if (!string.IsNullOrEmpty(mid))
                    {
                        hid_mid.Value = mid;
                        string bh = Request.QueryString["bh"];
                        this.hid_BH.Value = bh;
                        GetData(mid);
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据信息
        /// </summary>
        /// <param name="jid"></param>
        public void GetData(string mid)
        {
            NetUserMission uMission = new NetUserMission();
            UserMission uInfo = new UserMission();

            uInfo = uMission.GetNetMissionInfo(mid);
            if (uInfo != null)
            {
                this.txt_EvalDesc.Value = commonFun.EnReplaceStr(uInfo.MissionDesc);
                this.drop_type.SelectedValue = uInfo.MissionType;
                this.TextBox_workHour.Text = uInfo.WorkHour.ToString();
                this.TextBox_realProcess.Text = uInfo.RealProcess.Trim();
            }
        }
    }
}
