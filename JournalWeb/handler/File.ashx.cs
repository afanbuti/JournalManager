using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using NetChina.Common;
using System.IO;
using System.Web.SessionState;
using NetChina.AccessHelper;
using NetChina.JournalModel;

namespace JournalWeb.handler
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class File : IHttpHandler, IRequiresSessionState
    {
        private const int UploadFileLimit = 1;//上传文件数量限制

        private string _msg = "上传成功！";//返回信息

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            if (context.Session["userId"] != null)
            {
                string user = context.Request.UrlReferrer.ToString();
                int iTotal = context.Request.Files.Count;
                string title = context.Request.Params["txt_title"].ToString();
                //string Type = context.Request.Params["txt_Type"].ToString();
                string desc = context.Request.Params["txt_Desc"].ToString();
                string userid = context.Session["userId"].ToString();
                string typeId = context.Request.Params["typeId"].ToString();

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
                            //保存文件
                            string FileDir = HttpContext.Current.Server.MapPath(commonFun.uploadFile + typeId+"/"+System.DateTime.Now.ToString("yyyyMMdd"));
                            if (!Directory.Exists(FileDir))
                            {
                                Directory.CreateDirectory(FileDir);
                            }
                            string filePath = commonFun.uploadFile +typeId+"/"+System.DateTime.Now.ToString("yyyyMMdd")+"/"+ Path.GetFileName(file.FileName);
                            string fullPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
                            file.SaveAs(fullPath);

                            //这里可以根据实际设置其他限制
                            if (++iCount > UploadFileLimit)
                            {
                                _msg = "超过上传限制：" + UploadFileLimit;
                                break;
                            }
                            NetFileManager fbll = new NetFileManager();
                            FileManager fInfo = new FileManager();
                            fInfo.UserId = Convert.ToInt32(userid);
                            fInfo.FileTitle = title;
                            fInfo.FilePath = filePath;
                            fInfo.FileDesc = desc;
                            fInfo.FileType = typeId;

                            try
                            {
                                if (!fbll.Insert(fInfo))
                                {
                                    _msg = "插入数据失败";
                                    System.IO.File.Delete(fullPath);
                                    break;
                                }
                            }
                            catch
                            {
                                _msg = "插入数据失败";
                                System.IO.File.Delete(fullPath);
                                break;
                            }                            
                        }
                    }
                }

                context.Response.Write("<script>window.parent.Finish('" + _msg + "');</script>");
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
