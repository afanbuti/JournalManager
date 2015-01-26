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

namespace JournalWeb.user
{
    public partial class viewMission : NetChina.Other.BasePage
    {
        int PageSize = 20;
        protected string tempTitle = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["groupId"].ToString() == "2")
            {
                Response.Redirect("/user/JournalList.aspx");
            }
            else if (Session["groupId"].ToString() == "4")
            {
                Response.Redirect("/filemanage/default.aspx");
            }
            else
            {
                UserId = Request.QueryString["uid"];
                Status = Request.QueryString["st"];
                TypeMission = Request.QueryString["to"];

                Uid = Session["userid"].ToString();
                if (string.IsNullOrEmpty(UserId))
                {
                    UserId = Uid;
                }
                if (string.IsNullOrEmpty(Status))
                {
                    Status = "0";
                }
                if (string.IsNullOrEmpty(TypeMission))
                {
                    TypeMission = "-1";
                }

                hid_uId.Value = Uid;


                if (!IsPostBack)
                {
                    GetUserList();
                    BuildTree();//建立树状内容
                    GetMissionList(false);
                }
            }
        }

        /// <summary>
        /// 开始日期属性
        /// </summary>
        public string beginSearchDate
        {
            set { ViewState["beginSearchDate"] = value; }
            get { return (string)ViewState["beginSearchDate"]; }
        }

        /// <summary>
        /// 结束日期属性
        /// </summary>
        public string endSearchDate
        {
            set { ViewState["endSearchDate"] = value; }
            get { return (string)ViewState["endSearchDate"]; }
        }

        public string Uid
        {
            get { return (string)ViewState["uid"]; }
            set { ViewState["uid"] = value; }
        }

        /// <summary>
        /// 传入的ID
        /// </summary>
        public string UserId
        {
            get { return (string)ViewState["userid"]; }
            set { ViewState["userid"] = value; }
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        public string Status
        {
            get { return (string)ViewState["status"]; }
            set { ViewState["status"] = value; }
        }
        /// <summary>
        /// 任务类型
        /// </summary>
        public string TypeMission
        {
            get { return (string)ViewState["typemission"]; }
            set { ViewState["typemission"] = value; }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        public void GetUserList()
        {
            NetUserInfo uInfo = new NetUserInfo();
            UserInfo info = new UserInfo();
            DataSet ds = uInfo.GetUserInfos("", "", 0, 1, true);
            if (ds != null)
            {
                int dsCount = ds.Tables[0].Rows.Count;
                if (dsCount > 0)
                {
                    for (int i = 0; i < dsCount; i++)
                    {
                        string value = ds.Tables[0].Rows[i][0].ToString();
                        string text = ds.Tables[0].Rows[i][3].ToString();
                        drop_UserList.Items.Add(new ListItem(text, value));
                    }
                }
            }
            this.drop_UserList.SelectedValue = UserId;
        }

        /// <summary>
        /// 建立树状结构
        /// </summary>
        public void BuildTree()
        {
            NetUserInfo uInfo = new NetUserInfo();
            DataSet ds = uInfo.GetUserInfos("", "", 0, 1, true);

            string htmlstr = "";
            if (ds != null)
            {
                int dsCount = ds.Tables[0].Rows.Count;
                if (dsCount > 0)
                {
                    htmlstr += "<script type=\"text/javascript\"><!--\n d = new dTree('d');\n";
                    htmlstr += "d.add(0,-1,'&nbsp;分组列表(" + dsCount + ")');\n";

                    for (int i = 0; i < dsCount; i++)
                    {
                        htmlstr += "d.add(" + (i + 1) + ",0,'" + ds.Tables[0].Rows[i]["realName"] + "','viewMission.aspx?st=0&uid=" + ds.Tables[0].Rows[i]["userId"] + "','分组列表','_parent');";
                    }

                    htmlstr += "document.write(d);//--></script>";
                }
            }
            this.navi.InnerHtml = htmlstr;//赋值到前台
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="IsSearch"></param>
        public void GetMissionList(bool IsSearch)
        {
            NetUserMission misBll = new NetUserMission();
            UserMission misInfo = new UserMission();

            #region 分页处理

            if (this.beginTime.Value.Trim() == "" || this.endTime.Value.Trim() == "")
            {
                this.endTime.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                this.beginTime.Value = System.DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                beginSearchDate = this.beginTime.Value;
                endSearchDate = this.endTime.Value;
            }

            if (IsSearch)
            {
                beginSearchDate = Convert.ToDateTime(this.beginTime.Value).ToString("yyyy-MM-dd");
                endSearchDate = Convert.ToDateTime(this.endTime.Value).ToString("yyyy-MM-dd");
                Status = this.drop_Status.SelectedValue;
                UserId = this.drop_UserList.SelectedValue;
                TypeMission = this.drop_type.SelectedValue;
            }
            else
            {
                if (Request.QueryString["bd"] != null && Request.QueryString["ed"] != null)
                {
                    try
                    {
                        beginSearchDate = Convert.ToDateTime(Request.QueryString["bd"]).ToString("yyyy-MM-dd");
                        endSearchDate = Convert.ToDateTime(Request.QueryString["ed"]).ToString("yyyy-MM-dd");

                    }
                    catch
                    {
                        beginSearchDate = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                        endSearchDate = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    beginSearchDate = Convert.ToDateTime(this.beginTime.Value).ToString("yyyy-MM-dd");
                    endSearchDate = Convert.ToDateTime(this.endTime.Value).ToString("yyyy-MM-dd");
                }
            }

            this.beginTime.Value = beginSearchDate;
            this.endTime.Value = endSearchDate;

            int userCount = misBll.GetMissionFYCount(UserId, Status, beginSearchDate, endSearchDate,"1",false,"",TypeMission);//获取分页总数
            int currentPage = 1;
            try
            {
                currentPage = Convert.ToInt32(Request.QueryString["page"]);
                if (currentPage > (userCount + PageSize - 1) / PageSize)
                {
                    currentPage = (userCount + PageSize - 1) / PageSize;
                }

                if (currentPage <= 0) currentPage = 1;
            }
            catch
            {
                currentPage = 1;
            }
            #endregion
            tempTitle = "[" + this.drop_UserList.SelectedItem.Text + "]--"+this.drop_Status.SelectedItem.Text+"的任务列表";
            this.drop_Status.SelectedValue = Status;

             DataSet ds = misBll.GetMissionDataSet(UserId,Status, beginSearchDate, endSearchDate, PageSize, currentPage,"1",false,"",TypeMission);
            HtmlTableRow hrow = null;
            if (ds != null)
            {
                int uCount = ds.Tables[0].Rows.Count;
                if (uCount > 0)
                {
                    

                    if (Session["userid"] != null)
                    {
                        if (Session["userid"].ToString() == UserId && Status=="0")
                        {

                            //如果是用户本人并且显示待执行的任务时

                            hrow = new HtmlTableRow();
                            //HtmlTableCell cel19 = GetHCell("人员", true);
                            //cel19.Width = "50";
                            //hrow.Cells.Add(cel19);

                            HtmlTableCell cell6 = GetHCell("待执行的任务内容", true);
                            cell6.Width = "610";

                            hrow.Cells.Add(cell6);

                            HtmlTableCell cell10 = GetHCell("<div  title='任务类型'>类型</div>", true);
                            cell10.Width = "50";
                            hrow.Cells.Add(cell10);

                            HtmlTableCell cel7 = GetHCell("<div  title='任务入库日期'>入库</div>", true);
                            cel7.Width = "40";
                            hrow.Cells.Add(cel7);
                            HtmlTableCell cel17 = GetHCell("<div  title='计划完成任务所需工时'>工时</div>", true);
                            cel17.Width = "40";
                            hrow.Cells.Add(cel17);

                            HtmlTableCell cel60 = GetHCell("操作", true);
                            cel60.Width = "80";
                            hrow.Cells.Add(cel60);

                            tbl_list.Rows.Add(hrow);


                            for (int i = 0; i < uCount; i++)
                            {
                                DataTable dt = ds.Tables[0];
                                hrow = new HtmlTableRow();
                                //string _personName = dt.Rows[i]["realName"].ToString();
                                string tempBH =(1 + i).ToString();
                                string tempMissAttachment = string.Empty;
                                if (!string.IsNullOrEmpty(dt.Rows[i]["FilePath"].ToString()))
                                {
                                    tempMissAttachment = "<span style='padding-left:10px;'><a href='" + dt.Rows[i]["FilePath"].ToString() + "' target='_blank'><img src='/inc/images/download.jpg' border='0'/></a></span>";
                                }


                                string _content="<div style='text-align:left;'><span id='div_mission_" + tempBH + "'>" + commonFun.replaceStr(dt.Rows[i]["MissionDesc"].ToString()) + tempMissAttachment + "</span>&nbsp;<span style='color:red;cursor:pointer;' onclick=\"setTxt('div_mission_" + tempBH + "')\">[复制]</span></div>"; 
                                //string _content = "<div style='text-align:left;'>" + dt.Rows[i]["MissionDesc"].ToString().Trim() + "</div>";
                                string _missionType = dt.Rows[i]["MissionType"].ToString().Trim();
                                string _workHour = dt.Rows[i]["WorkHour"].ToString().Trim();
                                string _writedate = dt.Rows[i]["WriteDate"].ToString().Trim();


                                string _operation = "<input  class=\"btn\" type=\"button\" value=\"开始执行\" onclick=\"doSomething('first','" + ds.Tables[0].Rows[i]["UserId"].ToString() + "','" + ds.Tables[0].Rows[i]["MissionId"].ToString() + "');\" />";



                                if (!string.IsNullOrEmpty(_writedate))
                                {
                                    _writedate = Convert.ToDateTime(_writedate).ToString("M-dd H:mm");
                                }
                           
                                if (_missionType == "0")
                                {
                                    _missionType = "维护";
                                }
                                else if (_missionType == "1")
                                {
                                    _missionType = "开发";
                                }
                                else if (_missionType == "2")
                                {
                                    _missionType = "需求分析";
                                }
                                else if (_missionType == "3")
                                {
                                    _missionType = "系统设计";
                                }
                                string _personName = dt.Rows[i]["realName"].ToString();
                                hrow.Cells.Add(GetHCell(_content, false));// 任务内容
                                hrow.Cells.Add(GetHCell(_missionType, false));//任务类型
                                hrow.Cells.Add(GetHCell(_writedate, false)); //入库日期
                                hrow.Cells.Add(GetHCell(_workHour, false));//计划工时

                                hrow.Cells.Add(GetHCell(_operation, false));//操作


                                tbl_list.Rows.Add(hrow);
                            }

                        }
                        else 
                        {
                            //所有人情况


                            hrow = new HtmlTableRow();
                            HtmlTableCell cel19 = GetHCell("人员", true);
                            cel19.Width = "50";
                            hrow.Cells.Add(cel19);

                            HtmlTableCell cell6 = GetHCell("正在执行的任务内容", true);
                            cell6.Width = "360";

                            hrow.Cells.Add(cell6);

                            HtmlTableCell cell10 = GetHCell("<div  title='任务类型'>类型</div>", true);
                            cell10.Width = "50";
                            hrow.Cells.Add(cell10);

                            HtmlTableCell cel7 = GetHCell("<div  title='任务入库日期'>入库</div>", true);
                            cel7.Width = "40";
                            hrow.Cells.Add(cel7);
                            HtmlTableCell cel17 = GetHCell("<div  title='计划完成任务所需工时'>工时</div>", true);
                            cel17.Width = "40";
                            hrow.Cells.Add(cel17);
                            HtmlTableCell cel2 = GetHCell("<div  title='任务开始执行的时间'>执行</div>", true);
                            cel2.Width = "40";
                            hrow.Cells.Add(cel2);

                            HtmlTableCell cel27 = GetHCell("<div  title='实际完成的任务进度'>实际</div>", true);
                            cel27.Width = "50";
                            hrow.Cells.Add(cel27);
                            HtmlTableCell cel3 = GetHCell("<div  title='计划完成的任务进度'>计划</div>", true);
                            cel3.Width = "50";
                            hrow.Cells.Add(cel3);
                            HtmlTableCell cel4 = GetHCell("<div  title='计划完成任务的时间'>计划</div>", true);
                            cel4.Width = "40";
                            hrow.Cells.Add(cel4);
                            HtmlTableCell cel8 = GetHCell("<div  title='实际完成任务的时间'>实际</div>", true);
                            cel3.Width = "40";
                            hrow.Cells.Add(cel8);

                            //HtmlTableCell cel6 = GetHCell("<div  title='任务目前状态'>状态</div>", true);
                            //cel6.Width = "40";
                            //hrow.Cells.Add(cel6);
                            //HtmlTableCell cel60 = GetHCell("操作", true);
                            //cel60.Width = "40";
                            //hrow.Cells.Add(cel60);

                            tbl_list.Rows.Add(hrow);


                            for (int i = 0; i < uCount; i++)
                            {
                                DataTable dt = ds.Tables[0];





                                hrow = new HtmlTableRow();
                                string _personName = dt.Rows[i]["realName"].ToString();

                                string tempBH = (1+i).ToString();
                                string tempMissAttachment = string.Empty;
                                if (!string.IsNullOrEmpty(dt.Rows[i]["FilePath"].ToString()))
                                {
                                    tempMissAttachment = "<span style='padding-left:10px;'><a href='" + dt.Rows[i]["FilePath"].ToString() + "' target='_blank'><img src='/inc/images/download.jpg' border='0'/></a></span>";
                                }


                                string _content = "<div style='text-align:left;'><span id='div_mission_" + tempBH + "'>" + commonFun.replaceStr(dt.Rows[i]["MissionDesc"].ToString()) + tempMissAttachment + "</span>&nbsp;<span style='color:red;cursor:pointer;' onclick=\"setTxt('div_mission_" + tempBH + "')\">[复制]</span></div>"; 
                                //string _content = "<div style='text-align:left;'>" + dt.Rows[i]["MissionDesc"].ToString().Trim() + "</div>";
                                string _missionType = dt.Rows[i]["MissionType"].ToString().Trim();
                                string _workHour = dt.Rows[i]["WorkHour"].ToString().Trim();
                                string _writedate = dt.Rows[i]["WriteDate"].ToString().Trim();

                                string _execDate = dt.Rows[i]["ExecDate"].ToString().Trim();
                                string _realProcess = dt.Rows[i]["RealProcess"].ToString().Trim();
                                string _planFinishProcess = dt.Rows[i]["PlanFinishProcess"].ToString().Trim();
                                string _planFinishDate = dt.Rows[i]["PlanFinishDate"].ToString().Trim();
                                string _finishDate = dt.Rows[i]["FinisthDate"].ToString();
                                //string _Status = dt.Rows[i]["ExecStatus"].ToString().Trim();

                                // string _realProcess = "30%";
                                //string _planFinishProcess = "50%";

                                //string _operation = "<input class=\"btn\" type=\"button\" value=\"完成\" onclick=\"doSomething('second','" + ds.Tables[0].Rows[i]["UserId"].ToString() + "','" + ds.Tables[0].Rows[i]["MissionId"].ToString() + "');\" />";


                                if (_realProcess != string.Empty && _realProcess != "0")
                                {
                                    _realProcess = " <div id='div_real1' style='width:50px; height:20px; left: 0px; top: 0px; position:relative; background-color:#EEEEEE;   border-color:#1E6BB2;  border-width:1px; border-style:solid;'> <div id='div_real2' style='width:" + _realProcess + "; height:100%;background:#4AAF4F;color:#0D00EF;text-align:center;  line-height:20px; position:absolute; z-index:-1;left:0px; '></div><div style='position:relative;font-weight:bold; z-index:2; text-align:center;padding-top:3px;'>" + _realProcess + "</div> </div>";
                                }
                                else _realProcess = string.Empty;

                                if (_planFinishProcess != string.Empty && _planFinishProcess != "0")
                                {
                                    _planFinishProcess = " <div id='div_plan1' style='width:50px; height:20px; left: 0px; top: 0px; position:relative; background-color:#EEEEEE;   border-color:#1E6BB2;  border-width:1px; border-style:solid;'><div id='div_plan2' style='width:" + _planFinishProcess + "; height:100%;background:#FF6600;color:#0D00EF;text-align:center; line-height:20px; position:absolute;left:0px; z-index:-1;'></div> <div style='position:relative; font-weight:bold; z-index:2; text-align:center;padding-top:3px;'>" + _planFinishProcess + "</div></div>";
                                }
                                else _planFinishProcess = string.Empty;

                                if (!string.IsNullOrEmpty(_execDate))
                                {
                                    _execDate = Convert.ToDateTime(_execDate).ToString("M-dd H:mm");
                                }
                                if (!string.IsNullOrEmpty(_finishDate))
                                {
                                    _finishDate = Convert.ToDateTime(_finishDate).ToString("M-dd H:mm");
                                }
                                if (!string.IsNullOrEmpty(_writedate))
                                {
                                    _writedate = Convert.ToDateTime(_writedate).ToString("M-dd H:mm");
                                }
                                if (!string.IsNullOrEmpty(_planFinishDate))
                                {
                                    _planFinishDate = Convert.ToDateTime(_planFinishDate).ToString("M-dd H:mm");
                                }
                                if (_missionType == "0")
                                {
                                    _missionType = "维护";
                                }
                                else if (_missionType == "1")
                                {
                                    _missionType = "开发";
                                }
                                else if (_missionType == "2")
                                {
                                    _missionType = "需求分析";
                                }
                                else if (_missionType == "3")
                                {
                                    _missionType = "系统设计";
                                }

                                //if (_Status == "0")
                                //{
                                //    _Status = "待执行";
                                //}
                                //else if (_Status == "1")
                                //{
                                //    _Status = "执行中";
                                //}
                                //else if (_Status == "2")
                                //{
                                //    _Status = "已完成";
                                //}
                                hrow.Cells.Add(GetHCell(_personName, false)); //人员姓名
                                hrow.Cells.Add(GetHCell(_content, false));// 任务内容
                                hrow.Cells.Add(GetHCell(_missionType, false));//任务类型
                                hrow.Cells.Add(GetHCell(_writedate, false)); //入库日期
                                hrow.Cells.Add(GetHCell(_workHour, false));//计划工时
                                hrow.Cells.Add(GetHCell(_execDate, false)); //执行日期
                                hrow.Cells.Add(GetHCell(_realProcess, false));//实际进度
                                hrow.Cells.Add(GetHCell(_planFinishProcess, false));//计划工作进度

                                hrow.Cells.Add(GetHCell(_planFinishDate, false));//计划结束时间
                                hrow.Cells.Add(GetHCell(_finishDate, false));//实际结束时间
                                //hrow.Cells.Add(GetHCell(_Status, false));  //执行状态
                                //hrow.Cells.Add(GetHCell(_operation, false));//操作


                                tbl_list.Rows.Add(hrow);
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("/login.aspx");
                    }
                  
                       

                }
                else
                {
                    hrow = new HtmlTableRow();
                    string tempNo = "温馨提示：没有任务信息!";
                    HtmlTableCell cell = GetHCell(tempNo, false);
                    cell.Height = "35px;";
                    hrow.Cells.Add(cell);
                    tbl_list.Rows.Add(hrow);
                }
            }
            else
            {

            }

            string parms = "&bd=" + beginSearchDate + "&ed=" + endSearchDate + "&uid=" + UserId + "&st=" + Status+"&to="+TypeMission;
            GetPage(userCount, PageSize, currentPage, parms);//获取分页文字
        }

        /// <summary>
        /// 创建td
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        private HtmlTableCell GetHCell(string strHtml, bool isTh)
        {
            HtmlTableCell hcell = null;
            if (isTh)
            {
                hcell = new HtmlTableCell("th");
            }
            else
            {
                hcell = new HtmlTableCell();
            }
            hcell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            hcell.InnerHtml = strHtml;
            return hcell;
        }

        public void GetPage(int RecordCount, int pageSize, int currentPage, string otherId)
        {

            this.tbl_td_page.InnerHtml = PageNavigator.Pagination(RecordCount, pageSize, currentPage, "viewMission.aspx", otherId);
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            GetMissionList(true);
        }
    }
}
