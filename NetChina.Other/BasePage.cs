using System;
using System.Collections.Generic;
using System.Text;
using NetChina.AccessHelper;
using NetChina.JournalModel;

namespace NetChina.Other
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (!IsSession())
            {
                //指定登陆页
                Response.Redirect("/login.aspx");
            }            
        }

        private bool IsSession()
        {
            if (Session["uname"] != null || Session["groupId"] != null || Session["userid"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
