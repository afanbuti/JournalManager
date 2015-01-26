using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using NetChina.JournalModel;
using NetChina.AccessHelper;
using NetChina.Common;

namespace JournalWeb.handler
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class dosome : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";         
            
            string tp = context.Request.Params["tp"].ToString();

            string returnStr = string.Empty;
            if (tp == "uMod")
            {
                string uid = context.Request.Params["uid"].ToString();
                string uName = context.Request.Params["uname"].ToString();
                string upass = context.Request.Params["pwd"].ToString();
                string gid = context.Request.Params["gid"].ToString();
                string rname = context.Request.Params["rname"].ToString();
                int bh = Convert.ToInt32(context.Request.Params["bh"].ToString());
                returnStr = ModifyUserInfo(uid, uName, upass, gid, rname,bh);
            }
            else if (tp == "jView")
            {
                string jid = context.Request.Params["jid"].ToString();
                string isEdit = context.Request.Params["tp2"].ToString();
                returnStr = GetJoulnalDetail(jid,isEdit);
            }
            else if (tp == "edit")
            {
                string jid = context.Request.Params["jid"].ToString();
                string content = context.Request.Params["content"].ToString();
                returnStr = ModifyJournal(jid, content);
            }
            else if (tp == "editpj")
            {
                string jid = context.Request.Params["jid"].ToString();
                string content = context.Request.Params["content"].ToString();
                returnStr = ModifySingleJournal(jid, content);
            }
            else if (tp == "del")
            {
                string jid = context.Request.Params["jid"].ToString();
                string uid = context.Request.Params["uid"].ToString();
                returnStr = DeleteJournal(jid, uid);
            }
            else
            {
                returnStr = "";
            }

            context.Response.Write(returnStr);
        }
        /// <summary>
        /// 修改总结内容
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string ModifyJournal(string jid, string content)
        {
            NetUserJournal uJourl = new NetUserJournal();
            string writetime=System.DateTime.Now.ToString("yyyy-MM-dd");
            if (uJourl.UpdateSome(content, jid, writetime))
            {
                return content;
            }
            else
            {
                return "0";
            }
        }

        public string ModifySingleJournal(string jid,string content)
        {
            NetUserJournal uJourl = new NetUserJournal();           
            if (uJourl.UpdateUserEval(content, jid))
            {
                return content;
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string DeleteJournal(string jid, string uid)
        {
            NetUserJournal uJourl = new NetUserJournal();
            if (uJourl.Delete(uid, jid))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        public string ModifyUserInfo(string uid,string uName,string uPass,string gid,string rName,int BH)
        {
            NetUserInfo uInfo = new NetUserInfo();
            UserInfo info = new UserInfo();
            info.UserID = int.Parse(uid);
            info.UserName = uName;
            info.RealName = rName;
            info.GroupID = gid;
            info.UserModTime = System.DateTime.Now;
            info.NetPass = uPass;

            if (info.GroupID == "1")
            {
                info.IsManager = "0"; ;
            }
            else
            {
                info.IsManager = "1";
            }
            info.UserOrder = BH;

            if (uInfo.Update(info))
            {
                return "1";
            }
            else
            {
                return "0"; 
            }
        }

        /// <summary>
        /// 获取总结详细
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public string GetJoulnalDetail(string jid,string isEdit)
        {
            UserJournal jInfo = new UserJournal();
            NetUserJournal uJourl = new NetUserJournal();
            if (isEdit == "1")
            {
                return uJourl.GetNetUserInfo(jid).JourDesc;
            }
            else
            {
                return "<p>" + commonFun.replaceStr(uJourl.GetNetUserInfo(jid).JourDesc) + "</p>";
            }            
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
