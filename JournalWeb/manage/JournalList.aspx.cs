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
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;

namespace JournalWeb.manage
{
    public partial class JournalList : NetChina.Other.BasePage
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
                if (!IsPostBack)
                {
                    GetUserList();
                    
                    GetNoWrite(DateTime.Today.AddDays(-1));
                    GetJournalList(false);
                    this.div_noWriteTitle.Visible = true;
                    this.div_NoWrite.Visible = true;
                }
            }
            this.tbl_td_page.Visible = true;
        }

        /// <summary>
        /// 未写人名单
        /// </summary>
        public void GetNoWrite(DateTime pTime)
        {
            string time = pTime.ToString("yyyy-MM-dd");
            NetUserJournal netJourl = new NetUserJournal();
            List<string> _noWriteNameList = netJourl.GetNoWriteList(ref time);

            int _noCount = _noWriteNameList.Count;
            if (_noCount > 0)
            {
                string tempNoPerson = "<span style='color:red;'>";
                this.div_noWriteTitle.InnerHtml = time + " 未写总结名单如下：&nbsp;共" + _noCount + "个";
                for (int i = 0; i < _noCount; i++)
                {
                    tempNoPerson += _noWriteNameList[i].ToString() + "&nbsp;&nbsp;";
                }

                tempNoPerson += "</span>";
                this.div_NoWrite.InnerHtml = tempNoPerson;
            }
            this.beginTime.Value = time;
            this.endTime.Value = time;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        public void GetUserList()
        {
            this.drop_UserList.Items.Clear();

            NetUserInfo uInfo=new NetUserInfo();
            UserInfo info=new UserInfo();
            DataSet ds=uInfo.GetUserInfos("","",0,1,true);
            if(ds!=null)
            {
                int dsCount=ds.Tables[0].Rows.Count;
                if(dsCount>0)
                {
                   
                    for(int i=0;i<dsCount;i++)
                    {
                        string value=ds.Tables[0].Rows[i][0].ToString();
                        string text=ds.Tables[0].Rows[i][3].ToString();
                        drop_UserList.Items.Add(new ListItem(text,value));
                    }
                    drop_UserList.Items.Add(new ListItem("所有人","-1"));
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

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord
        {
            set { ViewState["keyword"] = value; }
            get { return (string)ViewState["keyword"]; }
        }

        /// <summary>
        /// 获取当天日志数据
        /// </summary>
        public void GetJournalList(bool IsSearch)
        {

            NetUserJournal uJourl = new NetUserJournal();
            UserJournal joul = new UserJournal();

            #region 分页判断处理
            if (IsSearch)
            {
                beginSearchDate = Convert.ToDateTime(this.beginTime.Value).ToString("yyyy-MM-dd");
                endSearchDate=Convert.ToDateTime(this.endTime.Value).ToString("yyyy-MM-dd");
                KeyWord = this.drop_UserList.SelectedValue;//用户ID
            }
            else
            {

                if (Request.QueryString["bd"] != null && Request.QueryString["ed"] != null && Request.QueryString["uid"] != null)
                {
                    KeyWord = Request.QueryString["uid"];
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
                    //beginSearchDate = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"); 
                    //endSearchDate = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    beginSearchDate = Convert.ToDateTime(this.beginTime.Value).ToString("yyyy-MM-dd");
                    endSearchDate = Convert.ToDateTime(this.endTime.Value).ToString("yyyy-MM-dd");
                    KeyWord = this.drop_UserList.SelectedValue;//用户ID
                }
            }
            this.beginTime.Value = beginSearchDate;
            this.endTime.Value=endSearchDate;
            this.drop_UserList.SelectedValue = KeyWord;
            int _currentPage = Convert.ToInt32(Request.QueryString["page"]); 
            if (!IsSearch && _currentPage<=0)
            {
                KeyWord = string.Empty;
            }
            if(beginSearchDate==endSearchDate)
            {
                //KeyWord = string.Empty;
               tempTitle = beginSearchDate;
             
            }
            else
            {
                tempTitle = "从" + beginSearchDate + "到" + endSearchDate;
              
            }
            tempTitle += "总结列表";
            
            int userCount = uJourl.GetJournalFYCount(KeyWord,beginSearchDate,endSearchDate);//获取分页总数
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

            DataSet ds = uJourl.GetJournalDataSet(KeyWord,beginSearchDate,endSearchDate,PageSize, currentPage,false);

            if (ds != null)
            {
                int uCount = ds.Tables[0].Rows.Count;
                HtmlTableRow hrow = null;
                string today=System.DateTime.Now.ToString("yyyy-MM-dd");

                hrow = new HtmlTableRow();
                HtmlTableCell cel1 = GetHCell("提交人",true);                
                cel1.Width = "70";
                hrow.Cells.Add(cel1);              
                
                hrow.Cells.Add(GetHCell("总结内容",true));
                if (beginSearchDate == today)
                {
                    HtmlTableCell cel2 = GetHCell("操作", true);
                    cel2.Width = "125";
                    hrow.Cells.Add(cel2);
                }
                //else
                //{
                //    HtmlTableCell cel3 = GetHCell("提交日期", true);
                //    cel3.Width = "125";
                //    hrow.Cells.Add(cel3);
                //}
                tbl_list.Rows.Add(hrow);

                if (uCount > 0)
                {                    
                    string tempOpreator = string.Empty;
                    for (int j = 0; j < uCount; j++)
                    {
                        hrow = new HtmlTableRow();
                        int tempBH = (currentPage - 1) * PageSize + j + 1;
                        //hrow.Cells.Add(GetHCell(tempBH.ToString()));//编号
                        hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[j]["realName"].ToString(), false));//真实姓名
                        //string tempDesc = commonFun.GetContentSummary(ds.Tables[0].Rows[j][3].ToString(), 40, true);
                        
                        //hrow.Cells.Add(GetHCell(ds.Tables[0].Rows[j][4].ToString()));//写入时间                        
                        tempOpreator = string.Empty;
                        string tempEval = "";
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[j]["evalContent"].ToString()))
                        {
                            tempEval += "<div id='div_EvalContent'><span id='sp_pjtitle" + tempBH + "'>总结小评：</span><span id='sp_tempPj" + tempBH + "'>" + commonFun.replaceStr(ds.Tables[0].Rows[j]["evalContent"].ToString()) + "</span></div>";
                        }
                        else
                        {
                            tempEval += "<div id='div_EvalContent'><span id='sp_pjtitle" + tempBH + "' style='display:none;'>总结小评：</span><span id='sp_tempPj" + tempBH + "'>" + commonFun.replaceStr(ds.Tables[0].Rows[j]["evalContent"].ToString()) + "</span></div>";
                        }
                        //tempOpreator = "<a href=\"javascript:void(0);\" name=\"view\" onclick=\"showDiv('" + ds.Tables[0].Rows[j][0].ToString() + "','0','" + tempBH + "')\">查看详细</a>";
                        if (beginSearchDate == today)
                        {
                            string tempDesc = commonFun.replaceStr(ds.Tables[0].Rows[j][3].ToString());


                            hrow.Cells.Add(GetHCell("<div style='text-align:left;line-height:22px;padding-left:5px;' id='div_tempContent" + tempBH + "'>" + tempDesc + "</div>" + tempEval + "<div style='padding-top:5px;font-size:14px;text-align:left;padding-left:5px;'><span style='color:red;' onclick=\"setTxt('div_tempContent" + tempBH + "')\">[复制]</span><span style='color:red;padding-left:10px;padding-right:10px;'><a href=\"javascript:void(0);\" onclick=\"showEvalDiv('" + ds.Tables[0].Rows[j][0].ToString() + "','" + tempBH + "')\">评论</a></span>提交日期:" + Convert.ToDateTime(ds.Tables[0].Rows[j][4].ToString()).ToString("yyyy/MM/dd H:mm") + "</div>", false));//总结描述

                            tempOpreator += "&nbsp;<a href=\"javascript:void(0);\" onclick=\"showDiv('" + ds.Tables[0].Rows[j][0].ToString() + "','1','" + tempBH + "')\">修改</a>";
                            tempOpreator += "&nbsp;<a href=\"javascript:void(0);\" onclick=\"deldata('" + ds.Tables[0].Rows[j][0].ToString() + "','" + ds.Tables[0].Rows[j][1].ToString() + "')\">删除</a>";
                            //tempOpreator += "&nbsp;<a href=\"javascript:void(0);\" onclick=\"evaldata('" + ds.Tables[0].Rows[j][0].ToString() + "','" + ds.Tables[0].Rows[j][1].ToString() + "')\">评论</a>";
                            hrow.Cells.Add(GetHCell(tempOpreator, false));//操作内容
                        }
                        else
                        {
                            string tempDesc = commonFun.replaceStr(ds.Tables[0].Rows[j][3].ToString());


                            hrow.Cells.Add(GetHCell("<div style='text-align:left;line-height:22px;padding-left:10px;' id='div_tempContent" + tempBH + "'>" + tempDesc + "</div>" + tempEval + "<div style='padding-top:5px;text-align:left;padding-left:5px;'><span style='padding-left:5px;color:red;' onclick=\"setTxt('div_tempContent" + tempBH + "')\">[复制]</span><span style='color:red;padding-left:10px;padding-right:10px;'><a href=\"javascript:void(0);\" onclick=\"showEvalDiv('" + ds.Tables[0].Rows[j][0].ToString() + "','" + tempBH + "')\">评论</a></span>提交日期:" + Convert.ToDateTime(ds.Tables[0].Rows[j][4].ToString()).ToString("yyyy/MM/dd H:mm") + "</div>", false));//总结描述
                            //tempOpreator = Convert.ToDateTime(ds.Tables[0].Rows[j][4].ToString()).ToString("yyyy/MM/dd hh:mm");
                            //hrow.Cells.Add(GetHCell(tempOpreator, false));//操作内容
                        }
                        

                        tbl_list.Rows.Add(hrow);
                    }
                }
                else
                {
                    hrow = new HtmlTableRow();
                    string tempNo = "温馨提示：没有总结信息!";
                    HtmlTableCell cell = GetHCell(tempNo,false);
                    if (beginSearchDate == today)
                    {
                        cell.Attributes.Add("colspan", "3");
                    }
                    else
                    {
                        cell.Attributes.Add("colspan", "2");
                    }
                    hrow.Cells.Add(cell);
                    tbl_list.Rows.Add(hrow);
                }
            }
            string parms = "&bd=" + beginSearchDate + "&ed=" + endSearchDate + "&uid=" + KeyWord;
            GetPage(userCount, PageSize, currentPage, parms);//获取分页文字
        }

        public void GetPage(int RecordCount, int pageSize, int currentPage, string otherId)
        {

            this.tbl_td_page.InnerHtml = PageNavigator.Pagination(RecordCount, pageSize, currentPage, "JournalList.aspx", otherId);
        }

        /// <summary>
        /// 创建td
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        private HtmlTableCell GetHCell(string strHtml,bool isTh)
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

        protected void Button1_Click(object sender, EventArgs e)
        {            
            GetJournalList(true);
            this.div_noWriteTitle.Visible = false;
            this.div_NoWrite.InnerHtml = "<span style='font-weight:bold;'>" + tempTitle + "</span>";
            this.btn_Export.Enabled = true;
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            ExportToDoc();
        }

        /// <summary>
        /// 导出到Word中
        /// </summary>
        public void ExportToDoc()
        {
            NetUserJournal uJoul = new NetUserJournal();
            UserJournal jourlInfo = new UserJournal();
                      
            System.Data.DataTable dt = uJoul.GetJournalDataSet(KeyWord,beginSearchDate,endSearchDate, 0, 1, true).Tables[0];            
            StringBuilder sbXml = new StringBuilder();
            string title=string.Empty;
            if (this.beginTime.Value == this.endTime.Value)
            {
                title = this.beginTime.Value + "工作总结内容列表";
            }
            else
            {
                title = this.drop_UserList.SelectedItem.Text + "从" + this.beginTime.Value + "到" + this.endTime.Value + "的工作总结内容列表";
            }
            sbXml.Append(title + "\t\t\t\t\n\n\n");
            foreach (DataRow row in dt.Rows)
            {
                sbXml.Append( row["realName"] + "\t" + Convert.ToDateTime(row["writetime"]).ToString("yyyy-MM-dd") + "\t\n");
                sbXml.Append(string.Format("{0}\n\n", row["jourDesc"]));
            }
            ExportData.ExportWebData("Word", title, sbXml);           
        }

        protected void btn_ShowNoWrite_Click(object sender, EventArgs e)
        {
            this.btn_Export.Enabled = false;
            this.div_NoWrite.Visible = true;
            this.div_noWriteTitle.Visible = true;

            string _beginTime = this.beginTime.Value.Trim();
            string _endTime = this.endTime.Value.Trim();
            string _userId = this.drop_UserList.SelectedValue.Trim();
            string _userName = this.drop_UserList.SelectedItem.Text.Trim();

            NetUserJournal _netJourl = new NetUserJournal();

            if (_userId == "-1")
            {
                List<NoWritePersonInfo> _noWrit_List = _netJourl.GetNoWriteDateList(_beginTime, _endTime);
                int _noWriteCount = _noWrit_List.Count;
                int _totalCount = 0;
                //if (_noWriteCount == 0)
                //{
                //    string _title = "<span style='font-weight:bold;font-size:small;'>本段时间内没有人缺写总结！</span>";
                //    this.div_noWriteTitle.InnerHtml = _title;
                //    this.div_NoWrite.InnerHtml = string.Empty;
                //}
                //else
                //{
                if (_noWriteCount > 0)
                {

                

                    HtmlTableRow hrow = null;

                    hrow = new HtmlTableRow();
                    HtmlTableCell _cel1 = GetHCell("人员", true);
                    _cel1.Width = "100";
                    hrow.Cells.Add(_cel1);

                    HtmlTableCell _cell2 = GetHCell("未写次数", true);
                    _cell2.Width = "100";
                    hrow.Cells.Add(_cell2);

                    HtmlTableCell _cell3 = GetHCell("具体日期", true);
                    _cell3.Width = "600";
                    hrow.Cells.Add(_cell3);

                    tbl_list.Rows.Add(hrow);

                    foreach (NoWritePersonInfo _person in _noWrit_List)
                    {
                        _totalCount += _person.NoWriteCount;

                        hrow = new HtmlTableRow();
                        HtmlTableCell _ce1 = GetHCell(_person.RealName, false);
                        HtmlTableCell _ce2 = GetHCell(_person.NoWriteCount.ToString(), false);
                        string _dateListstring = "<div style='text-align:left;'>" + _person.NoWriteDate + "</div>";
                        HtmlTableCell _ce3 = GetHCell(_dateListstring, false);
                        hrow.Cells.Add(_ce1);
                        hrow.Cells.Add(_ce2);
                        hrow.Cells.Add(_ce3);
                        this.tbl_list.Rows.Add(hrow);
                    }
                }

                    string _title = "<span style='font-weight:bold;font-size:small;'><span style='color:red;'></span>在 " + _beginTime + " 至 " + _endTime + " 时间段内所有人未写总结共 <span style='color:red;'>" + _totalCount + " </span>次</span> <br />";
                    this.div_noWriteTitle.InnerHtml = _title;
                    this.div_NoWrite.InnerHtml = string.Empty;
                
                //}
                
            }
            else
            {

               
                List<DateTime> _noWriteList = _netJourl.GetNoWriteDateList(_beginTime, _endTime, int.Parse(_userId));
                int _noWriteCount = _noWriteList.Count;
                
                string _title = "<span style='font-weight:bold;font-size:small;'><span style='color:red;'>" + _userName + "</span>在 " + _beginTime + " 至 " + _endTime + " 时间段内未写总结共 <span style='color:red;'>" + _noWriteCount + " </span>次</span> <br />";
                StringBuilder _strDateList = new StringBuilder();
                if (_noWriteCount > 0)
                {
                    _strDateList = _strDateList.Append("<span style='color:#3300ff;'>具体日期是：");
                    foreach (DateTime _date in _noWriteList)
                    {
                        _strDateList = _strDateList.Append(_date.ToString("yyyy-M-d") + "，");
                    }
                    _strDateList = _strDateList.Remove(_strDateList.Length - 1, 1);
                    _strDateList = _strDateList.Append(" 。</span>");
                }
                this.div_noWriteTitle.InnerHtml = _title;
                this.div_NoWrite.InnerHtml = _strDateList.ToString();
                this.tbl_list.Rows.Add(new HtmlTableRow());
            }
            this.tbl_td_page.Visible = false;
               
        }
    }
}
