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

using NetChina.Other;
using NetChina.Common;

namespace JournalWeb.filemanage
{
    public partial class AddDoc : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["groupId"].ToString() == "1")
            {
                Response.Redirect("/user/viewList.aspx");
            } 
        }

        /// <summary>
        /// 获取类别列表
        /// </summary>
        public void GetTypeList()
        {
            //string file = Server.MapPath("/Inc/docType.xml");
            //DataSet objDs = operateXml.ConvertXMLFileToDataSet(file);
            //if (objDs != null)
            //{
            //    int objCount = objDs.Tables[0].Rows.Count;
            //    if (objCount > 0)
            //    {
            //        for (int i = 0; i < objCount; i++)
            //        {
            //            string value = objDs.Tables[0].Rows[i]["key"].ToString();
            //            string text = objDs.Tables[0].Rows[i]["name"].ToString();
            //            this.drop_DocType.Items.Add(new ListItem(text, value));                        
            //        }
            //        this.typeId.Value = drop_DocType.SelectedItem.Text;
            //    }
            //}
        }
    }
}
