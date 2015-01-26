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
using System.Text;


namespace JournalWeb.manage
{
    public partial class MissionList : NetChina.Other.BasePage
    {
        int PageSize = 20;
        protected string tempTitle = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["groupId"].ToString() == "1")
            {
                Response.Redirect("/user/viewList.aspx");
            }
            else if (Session["groupId"].ToString() == "4")
            {
                Response.Redirect("/filemanage/default.aspx");
            }
            else
            {
                UserId = Request.QueryString["uid"];
                Status = Request.QueryString["st"];
                IsGoOn = Request.QueryString["opt"];
                IsAsign = Request.QueryString["isa"];
                KeyWord = Request.QueryString["tl"];
                TypeMission = Request.QueryString["to"];//类型

                Uid = Session["userid"].ToString();
                if (string.IsNullOrEmpty(UserId))
                {
                    UserId = "-1";
                }
                if (string.IsNullOrEmpty(Status))
                {
                    Status = "-1";
                }
                if (string.IsNullOrEmpty(IsAsign))
                {
                    IsAsign = "1";
                }
                if (string.IsNullOrEmpty(KeyWord))
                {
                    KeyWord = "";
                }
                if (string.IsNullOrEmpty(TypeMission))
                {
                    TypeMission = "-1";
                }
                hid_uId.Value = Uid;
                if (!IsPostBack)
                {
                    if (IsAsign == "1")
                    {
                        this.chk_IsAsigin.Checked = true;
                    }
                    hid_IsAsign.Value = IsAsign;
                    this.drop_type.SelectedValue = TypeMission;

                    GetUserList();
                    GetMissionList(false);

                    if (!string.IsNullOrEmpty(IsGoOn))
                    {
                        if (IsGoOn == "on")
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "startUp", "<script type='text/javascript'>ShowText();</script>");
                        }
                    }
                }
                chk_IsAsigin.Attributes.Add("onclick", "selectData()");
            }
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
                if (chk_IsAsigin.Checked)
                {
                    IsAsign = "1";
                }
                else
                {
                    IsAsign = "0";
                }
                //IsAsign = this.drop_IsAssign.SelectedValue;
                KeyWord = this.txt_content.Value.Trim();
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

            int userCount = misBll.GetMissionFYCount(UserId, Status, beginSearchDate, endSearchDate, IsAsign, true, KeyWord, TypeMission);//获取分页总数
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

            if (IsAsign == "1")
            {
                tempTitle = "[" + this.drop_UserList.SelectedItem.Text + "]--已经分配的任务列表";
            }
            else
            {
                tempTitle = "[" + this.drop_UserList.SelectedItem.Text + "]--未分配的任务列表";
            }
            this.drop_Status.SelectedValue = Status;
            DataSet ds = misBll.GetMissionDataSet(UserId, Status, beginSearchDate, endSearchDate, PageSize, currentPage, IsAsign, true, KeyWord, TypeMission);

            #region 重新构造数据显示格式
            if (ds == null || ds.Tables.Count == 0)
            {
                //this.Page_Load(null, null);
                HtmlTableRow hrow=new HtmlTableRow();
                 HtmlTableCell hcell = GetHCell("温馨提示：该时间段没有任务！", false);
                //hcell.Attributes.Add("colspan", "12");
                
                hrow.Cells.Add(hcell);
                this.tbl_list.Rows.Add(hrow);
            }
            else
            {
                DataTableHelper dHelp = new DataTableHelper();
                DataRow[] dv = dHelp.SelectDistinct("result", ds.Tables[0], "userid").Select("1=1");
                if (dv.Length > 0)
                {
                    //创建表头
                    HtmlTableRow hrow = null;
                    hrow = new HtmlTableRow();

                    HtmlTableCell cel19 = GetHCell("人员", true);
                    cel19.Width = "50";
                    hrow.Cells.Add(cel19);

                    //if (Status == "0" || Status == "-1")
                    //{
                    HtmlTableCell cel5 = GetHCell("操作", true);
                    cel5.Width = "40";
                    hrow.Cells.Add(cel5);
                    //}

                    HtmlTableCell cell6 = GetHCell("任务内容", true);
                    //if (Status == "0" || Status == "-1")
                    //{
                    //    cell6.Width = "400";
                    //}
                    //else
                    //{
                    cell6.Width = "690";
                    //}
                    hrow.Cells.Add(cell6);

                    HtmlTableCell cell10 = GetHCell("类型", true);
                    cell10.Width = "70";
                    hrow.Cells.Add(cell10);

                    HtmlTableCell cel7 = GetHCell("<div  title='入库日期'>入库</div>", true);
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
                    HtmlTableCell cel8 = GetHCell("<div  title='实际完成任务的时间'>实际</div>", true);
                    cel8.Width = "40";
                    hrow.Cells.Add(cel8);

                    HtmlTableCell cel60 = GetHCell("状态", true);
                    cel60.Width = "30";
                    hrow.Cells.Add(cel60);

                    tbl_list.Rows.Add(hrow);


                    foreach (DataRow dr in dv)
                    {
                        DataTable dt = dHelp.GetNewDataTable(ds.Tables[0], "userid=" + dr[0].ToString());
                        int dtCount = dt.Rows.Count;
                        if (dtCount > 0)
                        {


                            int uCount = dt.Rows.Count;
                            if (uCount > 0)
                            {
                                for (int i = 0; i < uCount; i++)
                                {
                                    string tempBH = (currentPage - 1) * PageSize + i + 1 + dr[0].ToString();
                                    string tempStatus = string.Empty;//是否执行    
                                    string tempStatusStr = string.Empty;
                                    hrow = new HtmlTableRow();
                                    //string realname = dt.Rows[i]["realName"].ToString();
                                    //if (string.IsNullOrEmpty(realname))
                                    //{
                                    //realname = "<font color=red>未分配</font>";
                                    //}
                                    //hrow.Cells.Add(GetHCell("" + realname + "", false));
                                    string personName = dt.Rows[i]["realName"].ToString();
                                    hrow.Cells.Add(GetHCell(personName, false));
                                    tempStatus = dt.Rows[i]["ExecStatus"].ToString();

                                    if (tempStatus == "0")
                                    {
                                        //if (IsAsign == "0")
                                        //{
                                        //    hrow.Cells.Add(GetHCell("<div><img onclick=\"deldata('" + dt.Rows[i]["MissionId"].ToString() + "','" + dt.Rows[i]["UserId"].ToString() + "')\" style='cursor:pointer;' alt='删除当前任务' src='../inc/images/album_del.gif' border='0'>&nbsp;<img alt='分配当前任务' style='cursor:pointer;' onclick=\"showDiv('" + dt.Rows[i]["MissionId"].ToString() + "')\" src='../inc/images/a_edit.gif' border='0'><img alt='修改当前任务' style='cursor:pointer;' onclick=\"showMission('" + dt.Rows[i]["MissionId"].ToString() + "','" + tempBH + "')\" src='../inc/images/iedit1.gif' border='0'></div>", false));
                                        //}
                                        //else
                                        //{
                                        hrow.Cells.Add(GetHCell("<div><img onclick=\"deldata('" + dt.Rows[i]["MissionId"].ToString() + "','" + dt.Rows[i]["UserId"].ToString() + "','" + dt.Rows[i]["FilePath"].ToString() + "')\" style='cursor:pointer;' alt='删除当前任务' src='../inc/images/album_del.gif' border='0'>&nbsp;<img alt='分配当前任务' style='cursor:pointer;' onclick=\"showDiv('" + dt.Rows[i]["MissionId"].ToString() + "')\" src='../inc/images/a_edit.gif' border='0'>&nbsp;<img alt='修改当前任务' style='cursor:pointer;' onclick=\"showMission('" + dt.Rows[i]["MissionId"].ToString() + "','" + tempBH + "')\" src='../inc/images/iedit1.gif' border='0'></div>", false));
                                        //}
                                    }
                                    else
                                    {
                                        //if (Status == "-1")
                                        //{
                                        if (tempStatus == "1")
                                        {
                                            hrow.Cells.Add(GetHCell("<div><img alt='变更当前任务人' style='cursor:pointer;' onclick=\"showDiv('" + dt.Rows[i]["MissionId"].ToString() + "')\" src='../inc/images/a_edit.gif' border='0'>&nbsp;<img alt='修改当前任务' style='cursor:pointer;' onclick=\"showMission('" + dt.Rows[i]["MissionId"].ToString() + "','" + tempBH + "')\" src='../inc/images/iedit1.gif' border='0'></div>", false));
                                        }
                                        else
                                        {
                                            hrow.Cells.Add(GetHCell("<div>无</div>", false));
                                        }

                                    }

                                    string tempMissAttachment = string.Empty;
                                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["FilePath"].ToString()))
                                    {
                                        tempMissAttachment = "<span style='padding-left:10px;'><a href='" + dt.Rows[i]["FilePath"].ToString() + "' target='_blank'><img src='/inc/images/download.jpg' border='0'/></a></span>";
                                    }


                                    hrow.Cells.Add(GetHCell("<div style='text-align:left;'><span id='div_mission_" + tempBH + "'>" + commonFun.replaceStr(dt.Rows[i]["MissionDesc"].ToString()) + tempMissAttachment + "</span>&nbsp;<span style='color:red;cursor:pointer;' onclick=\"setTxt('div_mission_" + tempBH + "')\">[复制]</span></div>", false)); //描述 

                                    if (tempStatus == "0")
                                    {
                                        tempStatusStr = "<img alt='待执行' src='../inc/images/red.gif' border='0'>";
                                    }
                                    else if (tempStatus == "1")
                                    {
                                        tempStatusStr = "<img alt='执行中' src='../inc/images/yellow.gif' border='0'>";
                                    }
                                    else if (tempStatus == "2")
                                    {
                                        tempStatusStr = "<img alt='已经完成' src='../inc/images/green.gif' border='0'>";
                                    }
                                    string writedate = dt.Rows[i]["WriteDate"].ToString();

                                    string execDate = dt.Rows[i]["ExecDate"].ToString();
                                    string finishDate = dt.Rows[i]["FinisthDate"].ToString();
                                    string planFinishDate = dt.Rows[i]["PlanFinishDate"].ToString();
                                    string _workHour = dt.Rows[i]["WorkHour"].ToString();
                                    // int _realProcess = 0;
                                    // int _planFinishProcess = 0;
                                    // if (dt.Rows[i]["RealProcess"].ToString().Trim() != string.Empty)
                                    // {
                                    //     _realProcess =Convert.ToInt32( dt.Rows[i]["RealProcess"]);
                                    // }

                                    //if (dt.Rows[i]["PlanFinishProcess"].ToString().Trim()!=string.Empty)
                                    //{
                                    //    _planFinishProcess = Convert.ToInt32(dt.Rows[i]["PlanFinishProcess"]);
                                    //}

                                    string _realProcess = dt.Rows[i]["RealProcess"].ToString().Trim();

                                    string _planFinishProcess = dt.Rows[i]["PlanFinishProcess"].ToString().Trim();
                                    // string _realProcess = "30%";
                                    //string _planFinishProcess = "50%";
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

                                    if (!string.IsNullOrEmpty(execDate))
                                    {
                                        execDate = Convert.ToDateTime(execDate).ToString("M-dd H:mm");
                                    }
                                    if (!string.IsNullOrEmpty(finishDate))
                                    {
                                        finishDate = Convert.ToDateTime(finishDate).ToString("M-dd H:mm");
                                    }
                                    if (!string.IsNullOrEmpty(writedate))
                                    {
                                        writedate = Convert.ToDateTime(writedate).ToString("M-dd H:mm");
                                    }
                                    if (!string.IsNullOrEmpty(planFinishDate))
                                    {
                                        planFinishDate = Convert.ToDateTime(planFinishDate).ToString("M-dd H:mm");
                                    }
                                    string _missionType = string.Empty;
                                    if (dt.Rows[i]["MissionType"].ToString() == "0")
                                    {
                                        _missionType = "维护";
                                    }
                                    else if (dt.Rows[i]["MissionType"].ToString() == "1")
                                    {
                                        _missionType = "开发";
                                    }
                                    else if (dt.Rows[i]["MissionType"].ToString() == "2")
                                    {
                                        _missionType = "需求分析";
                                    }
                                    else if (dt.Rows[i]["MissionType"].ToString() == "3")
                                    {
                                        _missionType = "系统设计";
                                    }


                                    //显示数据
                                    hrow.Cells.Add(GetHCell(_missionType, false));//任务类型
                                    hrow.Cells.Add(GetHCell(writedate, false)); //入库日期
                                    hrow.Cells.Add(GetHCell(_workHour, false));//计划工时
                                    hrow.Cells.Add(GetHCell(execDate, false)); //执行日期
                                    hrow.Cells.Add(GetHCell(_realProcess, false));//实际进度
                                    hrow.Cells.Add(GetHCell(_planFinishProcess, false));//计划工作进度

                                    hrow.Cells.Add(GetHCell(planFinishDate, false));//计划结束时间
                                    hrow.Cells.Add(GetHCell(finishDate, false)); //结束日期

                                    hrow.Cells.Add(GetHCell(tempStatusStr, false)); //执行状态


                                    tbl_list.Rows.Add(hrow);
                                }
                            }
                            else
                            {
                                hrow = new HtmlTableRow();
                                HtmlTableCell hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
                                hcell.Attributes.Add("colspan", "12");
                                hrow.Cells.Add(hcell);
                    
                                tbl_list.Rows.Add(hrow);
                            }

                        }
                        else
                        {
                            hrow = new HtmlTableRow();
                            HtmlTableCell hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
                            hcell.Attributes.Add("colspan", "12");
                            hrow.Cells.Add(hcell);
                
                            tbl_list.Rows.Add(hrow);
                        }
                    }
                }
                else
                {
                    HtmlTableRow hrow = new HtmlTableRow();
                    HtmlTableCell hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
                    //hcell.Attributes.Add("colspan", "12");
                    hrow.Cells.Add(hcell);

                    tbl_list.Rows.Add(hrow);
                }
            }
            #endregion

            #region 数据显示方式 原来的

            //HtmlTableRow hrow = null;
            //if (ds != null)
            //{

            //    hrow = new HtmlTableRow();

            //    HtmlTableCell cel1 = GetHCell("任务人", true);
            //    cel1.Width = "60";
            //    hrow.Cells.Add(cel1);

            //    HtmlTableCell cell6 = GetHCell("任务内容", true);
            //    if (Status == "0" || Status == "-1")
            //    {                
            //        cell6.Width = "560";
            //    }
            //    else
            //    {
            //        cell6.Width = "600";
            //    }
            //    hrow.Cells.Add(cell6);

            //    HtmlTableCell cel7 = GetHCell("入库日期", true);
            //    cel7.Width = "70";
            //    hrow.Cells.Add(cel7);

            //    HtmlTableCell cel2 = GetHCell("执行时间", true);
            //    cel2.Width = "70";
            //    hrow.Cells.Add(cel2);

            //    HtmlTableCell cel3 = GetHCell("结束日期", true);
            //    cel3.Width = "70";
            //    hrow.Cells.Add(cel3);



            //    HtmlTableCell cel4 = GetHCell("状态", true);
            //    cel4.Width = "40";
            //    hrow.Cells.Add(cel4);

            //    if (Status == "0" || Status == "-1")
            //    {
            //        HtmlTableCell cel5 = GetHCell("操作", true);
            //        cel5.Width = "30";
            //        hrow.Cells.Add(cel5);
            //    }

            //    tbl_list.Rows.Add(hrow);

            //    int uCount = ds.Tables[0].Rows.Count;
            //    if (uCount > 0)
            //    {
            //        for (int i = 0; i < uCount; i++)
            //        {       

            //            string tempStatus = string.Empty;//是否执行    
            //            string tempStatusStr = string.Empty;
            //            hrow = new HtmlTableRow();
            //            string realname = ds.Tables[0].Rows[i]["realName"].ToString();
            //            if (string.IsNullOrEmpty(realname))
            //            {
            //                realname = "<font color=red>未分配</font>";
            //            }
            //            hrow.Cells.Add(GetHCell("" + realname + "", false));
            //            hrow.Cells.Add(GetHCell("<div style='text-align:left;'>" + commonFun.replaceStr(ds.Tables[0].Rows[i]["MissionDesc"].ToString()) + "</div>", false)); //描述 
            //            tempStatus = ds.Tables[0].Rows[i]["ExecStatus"].ToString();
            //            if (tempStatus == "0")
            //            {
            //                tempStatusStr = "<img alt='待执行' src='../inc/images/red.gif' border='0'>";
            //            }
            //            else if (tempStatus == "1")
            //            {
            //                tempStatusStr = "<img alt='执行中' src='../inc/images/yellow.gif' border='0'>";
            //            }
            //            else if (tempStatus == "2")
            //            {
            //                tempStatusStr = "<img alt='已经完成' src='../inc/images/green.gif' border='0'>";
            //            }
            //            string writedate = ds.Tables[0].Rows[i]["WriteDate"].ToString();
            //            string execDate=ds.Tables[0].Rows[i]["ExecDate"].ToString();
            //            string finishDate=ds.Tables[0].Rows[i]["FinisthDate"].ToString();

            //            if(!string.IsNullOrEmpty(execDate))
            //            {
            //                execDate=Convert.ToDateTime(execDate).ToString("MM-dd hh:mm");
            //            }
            //            if(!string.IsNullOrEmpty(finishDate))
            //            {
            //                finishDate=Convert.ToDateTime(finishDate).ToString("MM-dd hh:mm");
            //            }
            //            if(!string.IsNullOrEmpty(writedate))
            //            {
            //                writedate=Convert.ToDateTime(writedate).ToString("MM-dd hh:mm");
            //            }
            //            hrow.Cells.Add(GetHCell(writedate, false)); //入库日期
            //            hrow.Cells.Add(GetHCell(execDate, false)); //执行日期
            //            hrow.Cells.Add(GetHCell(finishDate, false)); //结束日期

            //            hrow.Cells.Add(GetHCell(tempStatusStr, false)); //执行状态
            //            if (tempStatus == "0")
            //            {
            //                if (IsAsign == "0")
            //                {
            //                    hrow.Cells.Add(GetHCell("<div><img onclick=\"deldata('" + ds.Tables[0].Rows[i]["MissionId"].ToString() + "','" + ds.Tables[0].Rows[i]["UserId"].ToString() + "')\" style='cursor:pointer;' alt='删除当前任务' src='../inc/images/album_del.gif' border='0'>&nbsp;<img alt='分配当前任务' onclick=\"showDiv('"+ds.Tables[0].Rows[i]["MissionId"].ToString()+"')\" src='../inc/images/a_edit.gif' border='0'></div>", false));
            //                }
            //                else
            //                {
            //                    hrow.Cells.Add(GetHCell("<div><img onclick=\"deldata('" + ds.Tables[0].Rows[i]["MissionId"].ToString() + "','" + ds.Tables[0].Rows[i]["UserId"].ToString() + "')\" style='cursor:pointer;' alt='删除当前任务' src='../inc/images/album_del.gif' border='0'>&nbsp;<img alt='修改当前任务' src='../inc/images/a_edit.gif' border='0'></div>", false));
            //                }
            //            }
            //            else
            //            {
            //                if (Status == "-1")
            //                {
            //                    hrow.Cells.Add(GetHCell("<div>无</div>", false));
            //                }
            //            }

            //            tbl_list.Rows.Add(hrow);
            //        }
            //    }
            //    else
            //    {
            //        hrow = new HtmlTableRow();
            //        HtmlTableCell hcell = null;
            //        if (Status == "0" || Status == "-1")
            //        {
            //            hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
            //            hcell.Attributes.Add("colspan", "7");
            //            hrow.Cells.Add(hcell);
            //        }
            //        else
            //        {
            //            hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
            //            hcell.Attributes.Add("colspan", "6");
            //            hrow.Cells.Add(hcell);
            //        }
            //        tbl_list.Rows.Add(hrow);
            //    }
            //}
            //else
            //{
            //    hrow = new HtmlTableRow();
            //    HtmlTableCell hcell = null;
            //    if (Status == "0" || Status == "-1")
            //    {
            //        hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
            //        hcell.Attributes.Add("colspan", "7");
            //        hrow.Cells.Add(hcell);
            //    }
            //    else
            //    {
            //        hcell = GetHCell("温馨提示：该时间段没有任何任务！", false);
            //        hcell.Attributes.Add("colspan", "6");
            //        hrow.Cells.Add(hcell);
            //    }
            //    tbl_list.Rows.Add(hrow);
            //}

            #endregion

            string parms = "&isa=" + IsAsign + "&opt=off&bd=" + beginSearchDate + "&ed=" + endSearchDate + "&uid=" + UserId + "&st=" + Status + "&tl=" + Server.UrlEncode(KeyWord) + "&to=" + TypeMission;
            GetPage(userCount, PageSize, currentPage, parms);//获取分页文字
        }

        public void GetPage(int RecordCount, int pageSize, int currentPage, string otherId)
        {

            this.tbl_td_page.InnerHtml = PageNavigator.Pagination(RecordCount, pageSize, currentPage, "MissionList.aspx", otherId);
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

        /// <summary>
        /// 创建td 
        /// </summary>
        /// <param name="strHtml">td里内容</param>
        /// <param name="isTh">td统一样式</param>
        /// <param name="pWidth">td 宽度</param>
        /// <returns></returns>
        private HtmlTableCell GetHCell(string strHtml, bool isTh, int pWidth)
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
            hcell.Style.Add(HtmlTextWriterStyle.Width, pWidth.ToString());
            hcell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            hcell.InnerHtml = strHtml;
            return hcell;
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
        /// 是否接续添加
        /// </summary>
        public string IsGoOn
        {
            get { return (string)ViewState["isgoon"]; }
            set { ViewState["isgoon"] = value; }
        }
        /// <summary>
        /// 是否分配
        /// </summary>
        public string IsAsign
        {
            get { return (string)ViewState["isasign"]; }
            set { ViewState["isasign"] = value; }
        }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord
        {
            get { return (string)ViewState["keyword"]; }
            set { ViewState["keyword"] = value; }
        }

        /// <summary>
        /// 类型
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
            drop_UserList.Items.Clear();

            drop_UserList.Items.Add(new ListItem("所有人", "-1"));
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

        protected void btn_Search_ServerClick(object sender, EventArgs e)
        {
            GetMissionList(true);
        }

        protected void btn_Export_ServerClick(object sender, EventArgs e)
        {
            ExportToDocByOffice();
            //ExportToDoc();
            GetMissionList(true);
        }


        public void ExportToDocByOffice()
        {
            NetUserMission uMission = new NetUserMission();

            string title = string.Empty;
            if (this.beginTime.Value == this.endTime.Value)
            {
                title = this.beginTime.Value + "工作任务列表";
            }
            else
            {
                title = "从" + this.beginTime.Value + "到" + this.endTime.Value + "的修改完成的列表";
            }

            DataTable dt = uMission.GetMissionDataSet("2", beginSearchDate, endSearchDate, KeyWord).Tables[0];

            NetChina.Common.ExportData.CreateWordFile(title, dt);
            
            string name = NetChina.Common.commonFun.uploadFile + "temp/已经完成任务列表.doc";
            NetChina.Common.ExportData.DoadLoadFile(Server.MapPath(name));
        }

        /// <summary>
        /// 导出到Word中
        /// </summary>
        public void ExportToDoc()
        {
            NetUserMission uMission = new NetUserMission();
            UserMission uinfo = new UserMission();

            DataTable dt = uMission.GetMissionDataSet("2", beginSearchDate, endSearchDate,KeyWord).Tables[0];
            StringBuilder sbXml = new StringBuilder();
            string title = string.Empty;
            if (this.beginTime.Value == this.endTime.Value)
            {
                title = this.beginTime.Value + "工作任务列表";
            }
            else
            {
                title = "从" + this.beginTime.Value + "到" + this.endTime.Value + "的修改完成的列表";
            }
            sbXml.Append(title + "\t\t\t\t\n\n\n");
            sbXml.Append("编号\t\t\t\t任务内容\n\n");
            foreach (DataRow row in dt.Rows)
            {
                sbXml.Append(string.Format("{0}","1"));

                sbXml.Append(string.Format("\t\t\t\t{0}\n\n", row["MissionDesc"]));
            }
            ExportData.ExportWebData("Word", title, sbXml);
        }

  
    }
}
