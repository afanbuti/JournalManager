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
    public partial class showEval : NetChina.Other.BasePage
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
                    string jid = Request.QueryString["jid"];

                    if (!string.IsNullOrEmpty(jid))
                    {
                        hid_jid.Value = jid;
                        string bh = Request.QueryString["cbh"];
                        this.hid_BH.Value = bh;
                        GetData(jid);

                    }
                }
            }
        }

        /// <summary>
        /// 获取数据信息
        /// </summary>
        /// <param name="jid"></param>
        public void GetData(string jid)
        {
            NetUserJournal nJourl = new NetUserJournal();
            UserJournal uInfo = new UserJournal();

            uInfo=nJourl.GetNetUserInfo(jid);
            if (uInfo !=null)
            {
                this.txt_EvalDesc.Value = uInfo.EvalContent;
            }
        }
    }
}
