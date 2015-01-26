using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using NetChina.AccessHelper;
using NetChina.JournalModel;
using NetChina.Common;
using System.Web.SessionState;

namespace JournalWeb.handler
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class doMission : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["userId"] != null)
            {
                context.Response.ContentType = "text/plain";
                int uid = 0;
                if (context.Request.Params["uid"] != null)
                {
                    if (context.Request.Params["uid"] != "-1")
                    {
                        uid = Convert.ToInt32(context.Request.Params["uid"].ToString());
                    }
                }
                string tp = context.Request.Params["tp"].ToString();

                string returnStr = string.Empty;                
                switch (tp)
                {
                    case "add":
                        string desc = context.Request.Params["desc"].ToString();
                        int workHour = int.Parse(context.Request.Params["workHour"].ToString());
                        string tempValue = context.Request.Params["tpValue"].ToString();
                        string path = context.Request.Params["path"].ToString();
                        returnStr = AddMission(uid, desc,workHour, tempValue, path);
                        break;
                    case "edit":
                        string missid = context.Request.Params["mid"].ToString();
                        string content = context.Request.Params["content"].ToString();
                        string stype = context.Request.Params["stype"].ToString();
                        string sworkHour = context.Request.Params["sworkHour"].ToString();
                        string sRealProcess=context.Request.Params["sRealProcess"].ToString();
                        returnStr = UpdateMissionDesc(content, missid, stype, sworkHour, sRealProcess);
                        break;
                    case "first":
                    case "second":
                        int mid = Convert.ToInt32(context.Request.Params["mid"].ToString());
                        returnStr = updateMission(tp, uid, mid);

                        break;

                    case "del":
                        int mid2 = Convert.ToInt32(context.Request.Params["mid"].ToString());
                        string path2 = context.Request.Params["path"].ToString();
                        returnStr = DeleteMission(uid, mid2, path2);
                        break;
                    case "update":
                        int mid3 = Convert.ToInt32(context.Request.Params["mid"].ToString());
                        
                        returnStr = updateMission(tp, uid, mid3);
                        break;
                }
                context.Response.Write(returnStr);
            }
        }

        /// <summary>
        /// 是否可重用的
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string UpdateMissionDesc(string desc, string missionId, string sType, string sworkHour,string sRealProcess)
        {
            string tempStr = "0";
            if (sRealProcess.Trim() == "0") 
            {
                sRealProcess = string.Empty;
            }
            if (sRealProcess.Trim() != string.Empty)
            {
                sRealProcess = sRealProcess.Trim() + "%";
            }

            NetUserMission netBll = new NetUserMission();
            if (netBll.UpdateDesc(missionId, desc, sType, sworkHour, sRealProcess))
            {
                tempStr = "1";
            }
            return tempStr;
        }

        /// <summary>
        /// 根据用户ID和任务ID删除
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="missionId"></param>
        /// <returns></returns>
        public string DeleteMission(int uid, int missionId,string path)
        {
            string tempStr = "0";
            NetUserMission netBll = new NetUserMission();
            if (netBll.Delete(uid, missionId))
            {
                string fullpath = HttpContext.Current.Server.MapPath(path);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                tempStr = "1";
            }
            return tempStr;
        }
        /// <summary>
        /// 跟新任务完成进度
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="uid">用户编号</param>
        /// <param name="missionId">任务编号</param>
        /// <returns>返回值 1 表示更新成功，0 表示更新失败</returns>
        public string updateMission(string type, int uid, int missionId)
        {
            string tempStr = "0";
            NetUserMission netBll = new NetUserMission();
            if (type == "first")
            {
                if (netBll.UpdateSome(missionId, uid, false))
                {
                    tempStr = "1";
                }
            }
            else if (type == "second")
            {
                if (netBll.UpdateSome(missionId, uid, true))
                {
                    tempStr = "1";
                }
            }
            else if (type == "update")
            {
                if(netBll.UpdateUser(missionId,uid))
                {
                    tempStr="1";
                }
            }           
            return tempStr;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="uname"></param>
        /// <returns></returns>
        public string AddMission(int uid, string desc,int workHour,string type,string path)
        {
            string tempstr = string.Empty;
            NetUserMission MissionBLL = new NetUserMission();
            UserMission MissionInfo = new UserMission();

            
            
            MissionInfo.UserID = uid;
            MissionInfo.MissionDesc = desc;
            MissionInfo.WorkHour = workHour;
            MissionInfo.WriteDate = System.DateTime.Now;
            MissionInfo.MissionTitle = "";
            MissionInfo.ExecStatus = "0";
            MissionInfo.MissionType = type;
            MissionInfo.FilePath = path;

            if (MissionBLL.Insert(MissionInfo))
            {
                tempstr = "1";
            }
            else
            {
                tempstr = "0";
            }
            return tempstr;
        }
    }
}
