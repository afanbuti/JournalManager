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

namespace JournalWeb.manage
{
    public partial class systemManage :NetChina.Other.BasePage
    {
       
        protected string tempTitle = "";
        private static bool m_isUpdate = false;
        private static int m_RestID = 0;
        private static DataTable m_currentRestDateDataTable = null;
        protected void Page_Load(object sender, EventArgs e)
        {
             
                if (!IsPostBack)
                {
                    this.InitializePage();
                    this.ReflashRestDateList();
                }

                //this.btn_addDate.ServerClick += new EventHandler(btn_addDate_ServerClick);
            //this.Button_deleteList.Attributes.Add("onclick", "return confirm('确认删除列表中全部假日吗？');");
            //this.Button_deleteList.ServerClick += new EventHandler(Button_deleteList_ServerClick);
        }

        private void InitializePage()
        {
            m_isUpdate = false;
            m_RestID = 0;
            this.GetUserList();
            this.Text_allEndDate.Value = "2009-12-30";
            this.Text_s_beginDate.Value = DateTime.Today.ToShortDateString();
            this.Text_s_endDate.Value = this.Text_allEndDate.Value.Trim();

        }
        private void ReflashRestDateList()
        {
            string _today = DateTime.Today.ToShortDateString();
           
            this.GridView1.DataSource = this.GetRestDateList(_today,this.Text_allEndDate.Value.Trim(),"请选择",-2);
            this.GridView1.DataBind();
        }

        /// <summary>
        /// 传入的ID
        /// </summary>
        public string UserId
        {
            get { return (string)ViewState["userid"]; }
            set { ViewState["userid"] = value; }
        }

        private DataTable GetRestDateList(string pBegionDate,string pEndDate,string pType,int pUserId)
        {
            NetRestDate _netDate = new NetRestDate();
            DataSet _ds = new DataSet();

            _ds = _netDate.GetRestDateDataSet(pBegionDate,pEndDate,pType,pUserId);
            if (_ds.Tables.Count == 0)
            {
                return null;
            }
            else
            {
                m_currentRestDateDataTable = _ds.Tables[0];
                return _ds.Tables[0];
            }
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
            DropDownList_s_user.Items.Clear();

            drop_UserList.Items.Add(new ListItem("所有人", "-1"));
            DropDownList_s_user.Items.Add(new ListItem("请选择", "-2"));
            DropDownList_s_user.Items.Add(new ListItem("所有人","-1"));
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
                        DropDownList_s_user.Items.Add(new ListItem(text, value));
                    }
                }
            }
            this.drop_UserList.SelectedValue = UserId;
             this.DropDownList_s_user.SelectedValue = UserId;
        }

        protected void btn_addDate_ServerClick(object sender, EventArgs e)
        {
            //判断是否空值
            if (this.text_restDate.Value.Trim()==string .Empty || this.DropDownList_type.SelectedIndex==0)
            {
                Response.Write("<script> alert(\"输入假日日期或类型出错，请重试！\");</script>");
                return;
            }

            RestDate _date = new RestDate();

            _date.UserId =int.Parse( this.drop_UserList.SelectedValue.Trim());
            string _userName=this.drop_UserList.SelectedItem.Text.Trim();
            DateTime _restDate=DateTime.Today;
            DateTime _endDate=DateTime.Today;
            try
            {
                _restDate = Convert.ToDateTime(this.text_restDate.Value.Trim());
            }
            catch (Exception ex)
            {
                Response.Write("<script> alert(\"输入假日日期出错，请重试！\");</script>");
                return;
            }
            try
            {
                _endDate = Convert.ToDateTime(this.Text_allEndDate.Value.Trim());
            }
            catch (Exception ex)
            {
                Response.Write("<script> alert(\"输入截止日期出错，请重试！\");</script>");
                return;
            }
            if (_restDate>_endDate)
            {
                Response.Write("<script> alert (\"假日日期不能大于截止日期，请修改！\");</script>");
                return;
            }

            _date.Rest_Date = _restDate;
            _date.RestDetail = this.DropDownList_resetDetail.SelectedValue;
            _date.Remark = this.Text_remark.Value;
            _date.Type = this.DropDownList_type.SelectedValue;

            if (_date.UserId == -1 && (_date.Type == "个人请假" || _date.Type == "个人出差" || _date.Type== "个人其他"))
            {
                Response.Write("<script> alert(\"'所有人'不可能是'"+_date.Type+"'！\");</script>");
                return;
            }
            if (_date.UserId != -1 && (_date.Type == "周六" || _date.Type == "周日" || _date.Type == "法定节假" || _date.Type=="其他节假"))
            {
                Response.Write("<script> alert(\"'"+_userName+"'不可能是'" + _date.Type + "'！\");</script>");
                return;
            }

            if (this.DropDownList_type.SelectedValue.Trim() == "周六")
            {
                _date.CycleNum = 14;

                if (_restDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    Response.Write("<script> alert(\"'" + _date.Rest_Date.ToShortDateString() + "'不是周六！\");</script>");
                    return;
                }
            }
            else if (this.DropDownList_type.SelectedValue.Trim() == "周日")
            {
                _date.CycleNum = 7;

                if (_restDate.DayOfWeek!=DayOfWeek.Sunday)
                {
                    Response.Write("<script> alert(\"'" + _date.Rest_Date.ToShortDateString() + "'不是周日！\");</script>");
                    return;
                }
            }
            else if (this.DropDownList_type.SelectedValue.Trim()=="个人出差")
            {
                _date.CycleNum = 1;
            }
            else _date.CycleNum = 0;

         


            if (m_isUpdate)
            {
                _date.RestID = m_RestID;
                NetRestDate _net = new NetRestDate();
                if (_net.Update(_date))
                {
                    if (_date.UserId != -1 && _date.Type == "个人请假")
                        Response.Write("<script> alert(\"修改'" + _userName + "'请假成功！\");</script>");
                    else
                        Response.Write("<script> alert(\"修改'" + _date.Type + "'成功！\");</script>");
                   
                }
                else
                {
                    Response.Write("<script> alert(\"'" + _date.Rest_Date.ToShortDateString() + "'修改失败，请重试！\");</script>");
                }
            }
            else
            {
                if (_date.CycleNum == 0)     //法定假日和个人请假情况
                {
                    if (this.AddRestDate(_date))
                    {
                        if (_date.UserId != -1 && _date.Type == "个人请假")
                            Response.Write("<script> alert(\"'" + _userName + "'请假成功！\");</script>");
                        else
                            Response.Write("<script> alert(\"添加'" + _date.Type + "'成功！\");</script>");
                  
                    }
                    else
                    {
                        if (_date.UserId != -1 && _date.Type == "个人请假")
                            Response.Write("<script> alert(\"'" + _date.Rest_Date.ToShortDateString() + "'已经是假日，无需请假!\");</script>");
                        else
                            Response.Write("<script> alert(\"'" + _date.Rest_Date.ToShortDateString() + "'已经添加过!\");</script>");
                       
                    }
                }
                else       // 周六和周日情况
                {
                    DateTime _weekDate = _restDate;
                    while (_weekDate <= _endDate)
                    {
                        this.AddRestDate(_date);

                        _weekDate = _weekDate.AddDays(_date.CycleNum);
                        _date.Rest_Date = _weekDate;

                    }
                    Response.Write("<script> alert(\"添加''" + _date.Type + "成功！\");</script>");
                }
            }
            this.InitializePage();
            this.ReflashRestDateList();
        }
        /// <summary>
        /// 添加假期
        /// </summary>
        /// <param name="pDate">假期对象</param>
        /// <returns></returns>
        private bool AddRestDate(RestDate pDate)
        {
            if (pDate == null) return false ;
            NetRestDate _netRestDate = new NetRestDate();
            if (_netRestDate.Insert(pDate))
                return true;
            else return false;
        }

        protected void Button_reset_ServerClick(object sender, EventArgs e)
        {
            this.InitializePage();
            this.DropDownList_resetDetail.SelectedIndex = 0;
            this.DropDownList_type.SelectedIndex = 0;
            this.Text_remark.Value = string.Empty;
            this.text_restDate.Value = string.Empty;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int _restId = Convert.ToInt32(this.GridView1.DataKeys[e.RowIndex].Value);
            string _date = this.GridView1.Rows[e.RowIndex].Cells[1].Text.Trim();
            NetRestDate _net = new NetRestDate();
            int[] _ArrayId = new int[] { _restId };
            if (_net.Delete(_ArrayId))
            {
                Response.Write("<script> alert(\"删除'"+_date+"'成功！\");</script>");
            }
            else
            {
                Response.Write("<script> alert(\"删除失败，请重试！\");</script>");
            }
            this.ReflashRestDateList();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string _type=this.GridView1.Rows[e.NewEditIndex].Cells[5].Text.Trim();
            if (_type=="周六")
            {
                Response.Write("<script> alert(\"周六不能修改，必须删除全部的周六假日，再添加！\");</script>");
                e.Cancel = true;
                return;
            }
            if (_type == "周日")
            {
                Response.Write("<script> alert(\"周日不能修改，必须删除全部的周日假日，再添加！\");</script>");
                e.Cancel = true;
                return;
            }
            m_isUpdate = true;
            m_RestID= Convert.ToInt32(this.GridView1.DataKeys[e.NewEditIndex].Value);

            this.GetUserList();
            //DataTable _table = this.GridView1.DataSource as DataTable;

            this.drop_UserList.SelectedItem.Text = m_currentRestDateDataTable.Rows[e.NewEditIndex]["realname"].ToString();
            this.drop_UserList.SelectedItem.Value = m_currentRestDateDataTable.Rows[e.NewEditIndex]["userid"].ToString();
            this.text_restDate.Value =Convert.ToDateTime( m_currentRestDateDataTable.Rows[e.NewEditIndex]["RestDate"]).ToString("yyyy-M-d");
            this.DropDownList_type.SelectedItem.Text = m_currentRestDateDataTable.Rows[e.NewEditIndex]["Type"].ToString();
            this.DropDownList_type.SelectedItem.Value = m_currentRestDateDataTable.Rows[e.NewEditIndex]["Type"].ToString();
            //this.DropDownList_resetDetail.SelectedItem.Text = m_currentRestDateDataTable.Rows[e.NewEditIndex]["RestDetail"].ToString();
            //this.DropDownList_resetDetail.SelectedItem.Value = m_currentRestDateDataTable.Rows[e.NewEditIndex]["RestDetail"].ToString(); 
            this.Text_remark.Value = m_currentRestDateDataTable.Rows[e.NewEditIndex]["Remark"].ToString();

            //this.text_restDate.Value = this.GridView1  .Rows[e.NewEditIndex].Cells[1].Text.Trim();
           //this.drop_UserList.SelectedItem.Value = this.GridView1.Rows[e.NewEditIndex].Cells[7].Text.Trim();
           //this.DropDownList_resetDetail.SelectedItem.Text = this.GridView1.Rows[e.NewEditIndex].Cells[4].Text.Trim();
           //this.DropDownList_type.SelectedItem.Text = this.GridView1.Rows[e.NewEditIndex].Cells[5].Text.Trim();

            //string _remark = this.GridView1.Rows[e.NewEditIndex].Cells[6].Text.Trim();
            //if (_remark == "&nbsp;")
            //    this.Text_remark.Value = string.Empty;
            //else
            //    this.Text_remark.Value = _remark;
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Separator)
            {
                ((LinkButton)e.Row.Controls[9].Controls[0]).Attributes.Add("onclick", "return confirm('确认删除吗？');");
            }
        }

        protected void Button_search_ServerClick(object sender, EventArgs e)
        {
            string _begin = this.Text_s_beginDate.Value.Trim();
            string _end = this.Text_s_endDate.Value.Trim();
            string _type = this.DropDownList_s_type.SelectedValue.Trim();
            int _userId = int.Parse(this.DropDownList_s_user.SelectedValue);
            if (_userId == -1)
                _userId = -2;

            this.GridView1.DataSource = this.GetRestDateList(_begin, _end, _type, _userId);
            this.GridView1.DataBind();
        }

        protected void Button_deleteList_Click(object sender, EventArgs e)
        {
            int _count = this.GridView1.DataKeys.Count;
            if (_count == 0) return;
            int[] _arrayId = new int[_count];
            for (int i = 0; i < _count; i++)
            {
                _arrayId[i] = Convert.ToInt32(this.GridView1.DataKeys[i].Value);
            }
            NetRestDate _net = new NetRestDate();
            if (_net.Delete(_arrayId))
            {
                Response.Write("<script> alert(\"删除假日成功！\");</script>");
            }
            else
            {
                Response.Write("<script> alert(\"删除假日失败，请重试！\");</script>");
            }
            this.ReflashRestDateList();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            this.ReflashRestDateList(); //重新绑定GridView数据的函数

        }

    

 



  



 
    }
}
