﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace JournalWeb
{
    public partial class loginout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["uname"] = null;
            Session["IsManager"] = null;
            Session["userid"] = null;
            Session["realName"] = null;
            Response.Redirect("/login.aspx");
        }
    }
}
