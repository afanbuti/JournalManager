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
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using System.Text;

namespace JournalWeb.user
{
    public partial class viewList : NetChina.Other.BasePage
    {
        int PageSize = 3;
        protected string tempTitle = "";
        //public static int m_Mission = 0;
       

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
                Uid = Session["userid"].ToString();
                if (string.IsNullOrEmpty(UserId))
                {
                    UserId = Uid;
                }

                hid_uId.Value = Uid;
                if (!IsPostBack)
                {
                    this.IsPostBackOperation();                 
                }
                if (Uid == this.drop_UserList.SelectedValue)
                {
                    div_IsSelf.Style.Add("display", "block");
                    GetTodayContent();
                }
                else
                {
                    div_IsSelf.Style.Add("display", "none");
                }
            }

            //this.div_noWriteDate.Visible = false;
        }

        //在页面回传时进行的操作
        private void IsPostBackOperation()
        {
            GetUserList();
            BuildTree();//建立树状内容
            GetInfoList(false);
            GetExecingMission();
            GetNoWriteList();
        }

        /// <summary>
        /// 获取当前执行的任务
        /// </summary>
        public void GetExecingMission()
        {
            NetUserMission bll = new NetUserMission();
            DataSet objDs=bll.GetDataSetByStatus(UserId, beginSearchDate, endSearchDate);
            HtmlTable ht = new HtmlTable();
            HtmlTableRow hrow = null;
            if (objDs != null)
            {
                int dsCount = objDs.Tables[0].Rows.Count;
                this.hid_missionCount.Value = dsCount.ToString();
                if (dsCount > 0)
                {
                    Session["MissionId"] = objDs.Tables[0].Rows[0]["MissionId"].ToString();

                    //原来的代码
                    //sp_bpCount.InnerHtml = "(" + dsCount + ")";
                    //string tempString = string.Empty;
                    //string tempStr = string.Empty;
                    
                    //for (int i = 0; i < dsCount; i++)
                    //{

                    //    string time = objDs.Tables[0].Rows[i]["writedate"].ToString();
                    //    string tempMissStr = string.Empty;
                    //    Session["MissionId"] = objDs.Tables[0].Rows[0]["MissionId"].ToString();
                    //    if (objDs.Tables[0].Rows[i]["MissionType"].ToString() == "0")
                    //    {
                    //        tempMissStr = "<span style='padding-left:20px;'>任务类型：维护</span>";
                    //    }
                    //    else
                    //    {
                    //        tempMissStr = "<span style='padding-left:20px;'>任务类型：开发</span>";
                    //    }
                    //    if (Session["userid"].ToString() == UserId)
                    //    {
                    //        tempString = "<span style=\"padding-left:200px;\"><input class=\"btn\" type=\"button\" value=\"我完成了！\" onclick=\"doSomething('second','" + objDs.Tables[0].Rows[i]["UserId"].ToString() + "','" + objDs.Tables[0].Rows[i]["MissionId"].ToString() + "');\" /></span>";
                    //    }

                    //    tempStr += "<div class='posttitle' style='text-align:left;'>任务发放日期：" + Convert.ToDateTime(time).ToString("yyyy/MM/dd H:mm") + "<span style='padding-left:30px;'>" + commonFun.getDate(time) + "</span>" + tempString + tempMissStr + "</div>";
                    //    tempStr += "<div style='text-align:left;padding-left:20px;padding-top:5px;'>" + objDs.Tables[0].Rows[i]["MissionDesc"].ToString() + "</div>";
                    //    tempStr += "<div style='padding-right: 20px; font-size: 13px; text-align: right'>状态：【执行中】执行时间：" + objDs.Tables[0].Rows[i]["ExecDate"].ToString() + "&nbsp;&nbsp;任务人：" + objDs.Tables[0].Rows[i]["realName"].ToString() + "</div>";
                    //}
                    //this.div_MissionList.InnerHtml = tempStr;


              
                    hrow = new HtmlTableRow();

                HtmlTableCell cell6 = GetHCell("正在执行的任务内容", true);
                if (Session["userid"].ToString() == UserId)
                {
                    cell6.Width = "450";
                }
                else
                {
                    cell6.Width = "490";
                }
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
                HtmlTableCell cel3 = GetHCell("<div  title='此刻计划应该完成的任务进度'>计划</div>", true);
                cel3.Width = "50";
                hrow.Cells.Add(cel3);
                HtmlTableCell cel4 = GetHCell("<div  title='计划完成任务的时间'>计划</div>", true);
                cel4.Width = "40";
                hrow.Cells.Add(cel4);
                //HtmlTableCell cel8 = GetHCell("实际结束", true);
                //cel3.Width = "50";
                //hrow.Cells.Add(cel8);

                //HtmlTableCell cel6 = GetHCell("状态", true);
                //cel6.Width = "40";
                //hrow.Cells.Add(cel6);
                if (Session["userid"].ToString() == UserId)
                {
                    HtmlTableCell cel60 = GetHCell("操作", true);
                    cel60.Width = "40";
                    hrow.Cells.Add(cel60);
                }
                tbl_mission_list.Rows.Add(hrow);


                    for (int i = 0; i < dsCount; i++)
                            {
                                   DataTable dt=objDs.Tables[0];

                                hrow = new HtmlTableRow();
                                string tempBH = (1+i).ToString();
                                string tempMissAttachment = string.Empty;
                                if (!string.IsNullOrEmpty(dt.Rows[i]["FilePath"].ToString()))
                                {
                                    tempMissAttachment = "<span style='padding-left:10px;'><a href='" + dt.Rows[i]["FilePath"].ToString() + "' target='_blank'><img src='/inc/images/download.jpg' border='0'/></a></span>";
                                }


                                string _content = "<div style='text-align:left;'><span id='div_mission_" + tempBH + "'>" + commonFun.replaceStr(dt.Rows[i]["MissionDesc"].ToString()) + tempMissAttachment + "</span>&nbsp;<span style='color:red;cursor:pointer;' onclick=\"setTxt('div_mission_" + tempBH + "')\">[复制]</span></div>"; 
                                //string _content ="<div style='text-align:left;'>"+ dt.Rows[i]["MissionDesc"].ToString().Trim()+ "</div>";
                                string _missionType = dt.Rows[i]["MissionType"].ToString().Trim();
                                string _workHour = dt.Rows[i]["WorkHour"].ToString().Trim();
                                 string _writedate = dt.Rows[i]["WriteDate"].ToString().Trim();
                                  
                                string _execDate = dt.Rows[i]["ExecDate"].ToString().Trim();
                                string _realProcess = dt.Rows[i]["RealProcess"].ToString().Trim();
                                string _planFinishProcess = dt.Rows[i]["PlanFinishProcess"].ToString().Trim();
                                string _planFinishDate = dt.Rows[i]["PlanFinishDate"].ToString().Trim();
                                //string finishDate = dt.Rows[i]["FinisthDate"].ToString();
                                //string _Status = dt.Rows[i]["ExecStatus"].ToString().Trim();
                              
                                  // string _realProcess = "30%";
                                //string _planFinishProcess = "50%";
                                if (!string.IsNullOrEmpty(_realProcess))
                                {
                                    this.hid_realProcess.Value = _realProcess.Replace("%", "");
                                }

                                string _operation = "<input class=\"btn\" type=\"button\" value=\"完成\" onclick=\"doSomething('second','" + objDs.Tables[0].Rows[i]["UserId"].ToString() + "','" + objDs.Tables[0].Rows[i]["MissionId"].ToString() + "');\" />";


                                 if (_realProcess != string.Empty && _realProcess != "0")
                                 {
                                     _realProcess = " <div id='div_real1' style='width:50px; height:20px; left: 0px; top: 0px; position:relative; background-color:#EEEEEE;   border-color:#1E6BB2;  border-width:1px; border-style:solid;'> <div id='div_real2' style='width:" + _realProcess + "; height:100%;background:#4AAF4F;color:#0D00EF;text-align:center;  line-height:20px; position:absolute; z-index:-1;left:0px; '></div><div style='position:relative;font-weight:bold; z-index:2; text-align:center;padding-top:3px;'>" + _realProcess + "</div> </div>";
                                 }
                                 else _realProcess = string.Empty;

                                if (_planFinishProcess != string.Empty && _planFinishProcess != "0")
                                {
                                    _planFinishProcess = "<div id='div_plan1' style='width:50px; height:20px; left: 0px; top: 0px; position:relative; background-color:#EEEEEE;   border-color:#1E6BB2;  border-width:1px; border-style:solid;'><div id='div_plan2' style='width:" + _planFinishProcess + "; height:100%;background:#FF6600;color:#0D00EF;text-align:center; line-height:20px; position:absolute;left:0px; z-index:-1;'></div> <div style='position:relative; font-weight:bold; z-index:2; text-align:center;padding-top:3px;'>" + _planFinishProcess + "</div></div>";
                                }
                                else _planFinishProcess = string.Empty;

                                if (!string.IsNullOrEmpty(_execDate))
                                {
                                    _execDate = Convert.ToDateTime(_execDate).ToString("M-dd H:mm");
                                }
                                //if (!string.IsNullOrEmpty(finishDate))
                                //{
                                //    finishDate = Convert.ToDateTime(finishDate).ToString("M-dd H:mm");
                                //}
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
                             
                                hrow.Cells.Add(GetHCell(_content,false));// 任务内容
                                hrow.Cells.Add(GetHCell(_missionType, false));//任务类型
                                hrow.Cells.Add(GetHCell(_writedate, false)); //入库日期
                                hrow.Cells.Add(GetHCell(_workHour, false));//计划工时
                                hrow.Cells.Add(GetHCell(_execDate, false)); //执行日期
                                hrow.Cells.Add(GetHCell(_realProcess, false));//实际进度
                                hrow.Cells.Add(GetHCell(_planFinishProcess, false));//计划工作进度
                                
                                hrow.Cells.Add(GetHCell(_planFinishDate, false));//计划结束时间
                                //hrow.Cells.Add(GetHCell(_Status, false));  //执行状态
                                if (Session["userid"].ToString() == UserId)
                                {
                                    hrow.Cells.Add(GetHCell(_operation, false));//操作
                                }
                                tbl_mission_list.Rows.Add(hrow);
                            }
                }
                else
                {
                    hrow = new HtmlTableRow();
                    HtmlTableCell hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
                        //hcell.Attributes.Add("colspan", "9");
                        hcell.Height = "35px;";
                        hrow.Cells.Add(hcell);

                        tbl_mission_list.Rows.Add(hrow);
                }
            }
            else
            {
                hrow = new HtmlTableRow();
                HtmlTableCell hcell = null;

                hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
                hcell.Attributes.Add("colspan", "10");
                hrow.Cells.Add(hcell);

                tbl_mission_list.Rows.Add(hrow); 
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
                        htmlstr += "d.add(" + (i + 1) + ",0,'" + ds.Tables[0].Rows[i]["realName"] + "','viewList.aspx?uid=" + ds.Tables[0].Rows[i]["userId"] + "','分组列表','_parent');";
                    }

                    htmlstr += "document.write(d);//--></script>";
                }
            }
            this.navi.InnerHtml = htmlstr;//赋值到前台
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="isSearch"></param>
        public void GetInfoList(bool isSearch)
        {
            NetUserJournal uJourl = new NetUserJournal();
            UserJournal joul = new UserJournal();

            #region 分页处理

            if (this.beginTime.Value.Trim() == "" || this.endTime.Value.Trim() == "")
            {
                this.endTime.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                this.beginTime.Value = System.DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");

            }

            if (isSearch)
            {
                beginSearchDate = Convert.ToDateTime(this.beginTime.Value).ToString("yyyy-MM-dd");
                endSearchDate = Convert.ToDateTime(this.endTime.Value).ToString("yyyy-MM-dd");
                UserId = this.drop_UserList.SelectedValue;
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

            int userCount = uJourl.GetJournalFYCount(UserId, beginSearchDate, endSearchDate);//获取分页总数
            int currentPage = 1;
            try
            {
                if (!isSearch)
                {
                    currentPage = Convert.ToInt32(Request.QueryString["page"]);
                    if (currentPage > (userCount + PageSize - 1) / PageSize)
                    {
                        currentPage = (userCount + PageSize - 1) / PageSize;
                    }

                    if (currentPage <= 0) currentPage = 1;
                }
            }
            catch
            {
                currentPage = 1;
            }

            #endregion

            if (beginSearchDate == endSearchDate)
            {
                tempTitle = beginSearchDate;
            }
            else
            {
                tempTitle = "从" + beginSearchDate + "到" + endSearchDate;
            }
            tempTitle += "&nbsp;[" + this.drop_UserList.SelectedItem.Text + "]&nbsp;&nbsp;的总结列表";

            DataSet ds = uJourl.GetJournalDataSet(UserId, beginSearchDate, endSearchDate, PageSize, currentPage, false);
            HtmlTableRow hrow = null;
            if (ds != null)
            {
                int uCount = ds.Tables[0].Rows.Count;
                if (uCount > 0)
                {
                    for (int i = 0; i < uCount; i++)
                    {
                        hrow = new HtmlTableRow();
                        string tempEval="";
                        hrow.Attributes.Add("class", "tr_row1");
                        string time = ds.Tables[0].Rows[i]["writetime"].ToString();
                        hrow.Cells.Add(GetHCell("<div class='posttitle' style='text-align:left;'>日期：" + Convert.ToDateTime(time).ToString("yyyy/MM/dd H:mm") + "<span style='padding-left:30px;'>" + commonFun.getDate(time) + "</span></div>", false));
                        tbl_list.Rows.Add(hrow);
                        hrow = new HtmlTableRow();
                        if(!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["evalContent"].ToString()))
                        {
                            tempEval += "<div id='div_EvalContent'><div>总结小评：" + commonFun.replaceStr(ds.Tables[0].Rows[i]["evalContent"].ToString()) + "</div></div>";
                        }
                        hrow.Cells.Add(GetHCell("<div style='text-align:left;padding-left:20px;padding-top:5px;'>" + commonFun.replaceStr(ds.Tables[0].Rows[i]["jourDesc"].ToString()) + "</div>" + tempEval, false));

                        tbl_list.Rows.Add(hrow);
                    }
                }
                else
                {
                    hrow = new HtmlTableRow();
                    string tempNo = "温馨提示：没有总结信息!";
                    HtmlTableCell cell = GetHCell(tempNo, false);
                    hrow.Cells.Add(cell);
                    tbl_list.Rows.Add(hrow);
                }
            }

            string parms = "&bd=" + beginSearchDate + "&ed=" + endSearchDate + "&uid=" + UserId;
            GetPage(userCount, PageSize, currentPage, parms);//获取分页文字
        }

        public void GetPage(int RecordCount, int pageSize, int currentPage, string otherId)
        {

            this.tbl_td_page.InnerHtml = PageNavigator.Pagination(RecordCount, pageSize, currentPage, "viewList.aspx", otherId);
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

        protected void btn_search_Click(object sender, EventArgs e)
        {
            GetInfoList(true);
        }

        protected void btn_Add_Click(object sender, EventArgs e)
        {
            if (Session["userid"] != null)
            {
                DateTime _now = DateTime.Now;
                if (_now.Hour<17 || (_now.Hour==17 && _now.Minute<=25))
                {
                    Response.Write("<script> alert(\"17：25以后才能填写总结！\");</script>");
                    this.IsPostBackOperation();
                    return;
                }

                if (Session["MissionId"] != null && Session["MissionId"].ToString() != string.Empty)
                {
                    string _process = this.TextBox_realProcess.Text.Trim();
                    int _process_int = 0;
                    if (_process == string.Empty)
                    {
                        Response.Write("<script> alert(\"请填写实际任务进度！\");</script>");
                        this.IsPostBackOperation();
                        return;
                    }
                    //if (!_process.Contains("%"))
                    //{
                    //    Response.Write("<script> alert(\"填写的任务进度不是一个有效的百分数！\");</script>");
                    //    return;
                    //}
                    try
                    {
                        _process_int = int.Parse(_process);
                        _process = _process + "%";
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script> alert(\"填写的任务进度不是一个有效的百分数！\");</script>");
                        return;
                    }
                
                    if (!this.UpdateRealProcess(_process, Convert.ToInt32(Session["MissionId"])))
                    {
                        Response.Write("<script> alert(\"修改实际任务进度失败！\");</script>");
                    }

                }
            
                    addJoual();
             
                
            }
            else
            {
                Response.Redirect("/login.aspx");
            }
        }
        
        //修改实际任务进度
        private bool UpdateRealProcess(string pProcess,int pMissionId)
        {
            NetUserMission _net = new NetUserMission();
           return _net.UpdateRealProcess(pMissionId, pProcess);
        }

        /// <summary>
        /// 获取当日详细内容
        /// </summary>
        public void GetTodayContent()
        {
            //string uid = string.Empty;
            string tempTime = System.DateTime.Now.ToString("yyyy-MM-dd");

            NetUserJournal uJourl = new NetUserJournal();
            UserJournal joulInfo = new UserJournal();

            joulInfo = uJourl.GetNetUserInfoByIds(UserId, tempTime);           
            if (joulInfo != null)
            {
                this.txt_desc.Value = joulInfo.JourDesc;
                hid_isHave.Value = joulInfo.JourID.ToString();//有值
                btn_Add.Enabled = false;
                btn_reset.Disabled = true;
                txt_desc.Disabled = true;
            }
            else
            {
                btn_Add.Enabled = true;
                btn_reset.Disabled = false;
                txt_desc.Disabled = false;
                hid_isHave.Value = "0";//没有值
            }
        }

        /// <summary>
        /// 添加日志内容
        /// </summary>
        public void addJoual()
        {
            NetUserJournal uJoul = new NetUserJournal();
            UserJournal joulInfo = new UserJournal();

            joulInfo.JourDesc = this.txt_desc.Value;
            joulInfo.UserName = "";
            joulInfo.UserID = int.Parse(Session["userid"].ToString());
            joulInfo.WriteTime = System.DateTime.Now;
            joulInfo.ModifyTime = System.DateTime.Now;
            System.Threading.Thread.Sleep(1000);

            if (uJoul.Insert(joulInfo))
            {
                string tempStr="";
                tempStr+="<script>";
                tempStr+="location='?uid="+Session["userid"].ToString()+"';";;
                tempStr+="</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "startup", tempStr);
            }
        }

        private void GetNoWriteList()
        {
            DateTime _begin=DateTime.Today.AddDays(-DateTime.Today.Day+1);
            DateTime _end = DateTime.Today.AddDays(-1);
            NetUserJournal _net = new NetUserJournal();
            List<DateTime> _noWriteDateList = _net.GetNoWriteDateList(_begin.ToString("yyyy-MM-dd"), _end.ToString("yyyy-MM-dd"), int.Parse(UserId));

            this.lbl_noWriteCount.Text = _noWriteDateList.Count.ToString() ;
            if (_noWriteDateList.Count == 0)
            {
                this.div_noWriteDate.Visible = false;
            }
            else
            {
                StringBuilder _strDateList = new StringBuilder("具体日期是：");
                foreach (DateTime _date in _noWriteDateList)
                {
                    _strDateList.Append(_date.ToString("yyyy-M-d") + "，");
                }
                _strDateList=_strDateList.Remove(_strDateList.Length - 1, 1);
                _strDateList = _strDateList.Append(" 。");
                this.TD_noWriteList.InnerHtml = _strDateList.ToString();
                this.div_noWriteDate.Visible = true;
            }
        }
    }
}
