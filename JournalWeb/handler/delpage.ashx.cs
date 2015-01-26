using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using NetChina.AccessHelper;
using log4net;
using System.Web.SessionState;

namespace JournalWeb.handler
{

    

    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class delpage : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["userId"] != null)
            {
                context.Response.ContentType = "text/plain";
                int id = Convert.ToInt32(context.Request.Params["id"].ToString());
                string tp = context.Request.Params["tp"].ToString();
                string returnStr = string.Empty;
                if (tp == "deluser")
                {
                    returnStr = delData(id);
                }
                else if (tp == "delfile")
                {
                    string userId =context.Request.Params["uid"].ToString();
                    string path = context.Request.Params["path"].ToString();
                    returnStr = DelFileData(userId,id.ToString(),path);
                }
                context.Response.Write(returnStr);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public string DelFileData(string UserId, string FileId, string path)
        {
            NetFileManager fileBll = new NetFileManager();
            if (fileBll.Delete(UserId, FileId))
            {
                string fullpath = HttpContext.Current.Server.MapPath(path);
                if(System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }

                log4net.Config.BasicConfigurator.Configure(new log4net.Appender.FileAppender(new log4net.Layout.PatternLayout("%d;%m;%p%n"), "log\\" + HttpContext.Current.Session["realName"].ToString() + "的日志记录.log"));
                log.Info(HttpContext.Current.Session["realName"].ToString() + "(" + HttpContext.Current.Session["uname"].ToString() + ")" + "删除文档文件" + HttpContext.Current.Request.UserHostAddress);
                log4net.LogManager.ResetConfiguration();

                return "1";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 删除用户操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string delData(int id)
        {
            NetUserInfo nUser = new NetUserInfo();
            if (nUser.Delete(id))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
    }
}
