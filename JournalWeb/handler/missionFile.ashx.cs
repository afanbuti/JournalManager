using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using NetChina.Common;
using System.IO;

namespace JournalWeb.handler
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class missionFile : IHttpHandler
    {
        private const int UploadFileLimit = 1;//上传文件数量限制

        private string _msg = "上传成功！";//返回信息
        

        private string _dirPath = commonFun.uploadMission;//上传任务文件夹

        private string FileDir = string.Empty;//目录名
        private string FullPath = string.Empty;//物理路径
        private string RealPath = string.Empty;//相对路径

        public void ProcessRequest(HttpContext context)
        {
            string user = context.Request.UrlReferrer.ToString();
            int iTotal = context.Request.Files.Count;

            string FileRelPath = string.Empty;

            if (iTotal == 0)
            {
                _msg = "没有数据";
            }
            else
            {
                int iCount = 0;
                
                for (int i = 0; i < iTotal; i++)
                {
                    HttpPostedFile file = context.Request.Files[i];

                    if (file.ContentLength > 0 || !string.IsNullOrEmpty(file.FileName))
                    {
                        FileRelPath = _dirPath + System.DateTime.Now.ToString("yyyyMMdd")  ;
                        FileDir = context.Server.MapPath(FileRelPath);
                        if (!Directory.Exists(FileDir))
                        {
                            Directory.CreateDirectory(FileDir);
                        }
                        RealPath = FileRelPath + "/" + Path.GetFileName(file.FileName);                       
                        
                        file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(RealPath));

                        //这里可以根据实际设置其他限制
                        if (++iCount > UploadFileLimit)
                        {
                            _msg = "超过上传限制：" + UploadFileLimit;
                            break;
                        }
                    }
                }
            }
            context.Response.Write("<script>window.parent.Finish('" + _msg + "','" + RealPath + "');</script>");
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
