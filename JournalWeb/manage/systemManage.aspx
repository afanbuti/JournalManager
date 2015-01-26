<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="systemManage.aspx.cs" Inherits="JournalWeb.manage.systemManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= tempTitle %>
    </title>

    <script src="../inc/js/datePicker.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.js" type="text/javascript"></script>

    <script src="../inc/js/sorttable.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.interface.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.jwindow.js" type="text/javascript"></script>
    <script src="../inc/js/innerSupport.js" type="text/javascript"></script>

    <link rel="Stylesheet" href="../inc/css/jwindow.css" />
    <style type="text/css">
      body{font-size:14px;margin:0px;padding:0px;background-color:#EEEEFF;}
      #div_top{width:100%;height:100%;text-align:center;padding-top:20px}
      /*#div_top{width:100%;background-color:#E5EEF7;height:100%;text-align:center;padding-top:20px}*/
      #tbl_list{width:870px;border-collapse:collapse;border:1px solid #EEE;font-size:14px;text-align:center}
      #tbl_list th{background:#EEE;border-bottom:1px solid #CCC;padding:4px;cursor:pointer;}
      #tbl_list td{border:1px solid #EEE;padding:4px;cursor:pointer;}
      #tdOther{text-align:left;}
      
      #div_editBtn{text-align:center;margin-top:10px;}
      .hover{background-color:#E5EEF7;}  /*#FFD900这里是鼠标经过时的颜色*/       
    .input1-bor {font-family:verdana;background-color:#F0F8FF;font-size: 12px;border: 1px solid #333333;cursor:pointer;}
    .posttitle
{
	font-size: 13px;
	padding-left: 10px;
	margin-left: 10px;
	margin-right: 10px;
	margin-top:2px;
	background-color: #E5EEF7;
	border-top: 1px solid #666;
	border-bottom: 1px dashed #666;
	padding : 2px;
	margin-bottom:0px;
	font-family: Verdana;
	font-weight:bold;
	width:97%;
	height:20px;
}
      #div_content{
margin:10px 10px 10px 10px;
overflow-y:auto;
scrollbar-face-color:#ffffff;
scrollbar-highlight-color:#ffffff;
overflow:auto;
width:430;
scrollbar-shadow-color:#919192;
scrollbar-3dlight-color:#ffffff;
line-height:18px;
scrollbar-arrow-color:#919192;
scrollbar-track-color:#ffffff;
scrollbar-darkshadow-color:#ffffff;
height:200px;
}
 .btn{
        padding:2px 2px 0px 2px;
        border:#2C59AA 1px solid;         
        font-size: 12px; 
        filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#ffffff, EndColorStr=#D7E7FA);         
        cursor: hand; 
        color: black;         
        text-align:center;
    }
    #leftBodyer { 
        width:10%; /*设定宽度*/
        height:100%;
        text-align:left; /*文字左对齐*/
        float:left; /*浮动居左*/
        clear:left; /*不允许左侧存在浮动*/
        /*overflow:hidden;超出宽度部分隐藏*/
        overflow-x:hidden;
        /*overflow-y:scroll;*/
        background-color:#F7F3F7; /*E5EEF7*/ 
        border:1px solid #666666;         
        margin-left:5px; 
    }
    #mainBodyer
    {
        width: 88%;
        height:100%;
        text-align: left;
        float: left; /*浮动居右*/
        clear: right; /*不允许右侧存在浮动*/
        /*overflow: hidden;超出宽度部分隐藏*/
        overflow-x:hidden;
        /*overflow-y:scroll;*/
                border:1px solid #666666;
        background-color:#FFFFFF;
        margin-left:5px;
    }
    #div_body
    {
        border-collapse:collapse;
        padding-top:5px;        
    } 
    
    #div_EvalContent
      {
        /*border: #999 1px solid; 
        background: #ffffee; 
        padding-top: 10px;  
        */
        padding-right: 3px;         
        padding-left:3px;        
        margin-top:3px;
        padding-bottom: 3px; 
        overflow: hidden;
        text-align: left;
        width:98%;
        color:#0A246A;/*#999999*/   
      }
      .top_a
      {
         text-decoration: none; 
         color:#000;              
      }
      .top_b
      {
        color:red;
        text-decoration: none;        
      }
      
    </style>

    <script type="text/javascript">
    $(document).ready(function(){
        rowColor();        
    });
    
 
  function repstr(str)    
  {    
    return str.replace(/\r\n/ig,"<br/>")    
  } 
    function selectDate()
    {
        var obj=document.getElementById("beginTime");        
        HS_setDate(obj);
    }
    function selectDate1()
    {
        var obj1=document.getElementById("endTime");        
        HS_setDate(obj1);
    }
    
       //改变值
    function chageValue()
    {
        var drop2=document.getElementById("drop_UserList");
        var   index   =   drop2.selectedIndex;   
        document.getElementById("div_MissPerson").innerHTML= drop2.options[index].text;
    } 
    //数据表格隔行换色
    function rowColor()
    {
        $("#tbl_list tr").hover( 
        function(){ 
            $(this).addClass("hover");      //鼠标经过添加hover样式 
        }, 
        function(){ 
            $(this).removeClass("hover");   //鼠标离开移除hover样式 
        })
    }
    
    //弹出div层 
    function showDiv(jid,isEdit,bh)//isEdit
    {
        $.ajax({
            type:"post",
            url:"/handler/dosome.ashx",
            data:"jid="+jid+"&tp=jView&tp2="+isEdit,
            success:function(msg){            
                if(isEdit=="1") //编辑
                {
                    document.getElementById("div_WinTitle").innerHTML="编辑用户总结信息";
                    document.getElementById("div_content").innerHTML="<textarea class=\"input1-bor\" name=\"txt_Desc\" id=\"txt_Desc\" cols=\"55\" rows=\"11\">"+msg+"</textarea><div id=\"div_editBtn\"><input type=\"button\" value=\"修改一下\" class='btn' onclick=\"changeData('"+jid+"','"+bh+"');\"/></div>";                    
                }
                else //查看
                {
                    document.getElementById("div_WinTitle").innerHTML="查看用户总结详细";
                    document.getElementById("div_content").innerHTML=msg;
                }
                $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				    			
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
            }
           }); 
    }
    
    
    //弹出div层 
    function showEvalDiv(jid,cBH)
    {
        document.getElementById("div_WinTitle").innerHTML="评价该任务";
        document.getElementById("div_content").innerHTML="<iframe src='showEval.aspx?jid="+jid+"&cbh="+cBH+"' frameborder='0' width='380' height='195'/>";
        $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				    			
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
    }
    
    //修改数据
    function changeData(jid,cBH)
    {       
        var tempContent=document.getElementById("txt_Desc").value;//$("#txt_Desc").val();
        if(trim(tempContent).length<=0)
        {
            alert("温馨提示：请输入总结内容！");
            $("#txt_Desc").focus();
            return false;
        }       
        $.ajax({
            type:"post",
            url:"/handler/dosome.ashx",
            data:"jid="+jid+"&tp=edit&content="+escape(tempContent),
            success:function(msg){
                if(msg!="0") //表示修改成功
                {                    
                    var temp1="div_tempContent"+cBH;
                    document.getElementById(temp1).innerHTML=repstr(msg);
                    closeWin();
                }
            }            
        });
    }
    
    function deldata(jid,uid)
    {
        if(confirm("温馨提示：确认要删除这条信息么？"))
        {
            $.ajax({
                type:"post",
                url:"/handler/dosome.ashx",
                data:"jid="+jid+"&tp=del&uid="+uid,
                success:function(msg){
                    if(msg=="1") //表示删除成功
                    {                    
                        location=location.href;
                    }
                }            
            });
        }
        else
        {
            return false;
        }
    }
    function evaldata(jid,uid)
    {
        
    }
    
    //删除左右两端的空格
    function trim(str){  
        return str.replace(/(^\s*)|(\s*$)/g, "");
    } 
    
    //关闭层
    function closeWin()
    {
        top.$("#panelWindow").hide();
        top.$("#panelWindow").DraggableDestroy();
        var overlayer = top.$("#_______overlayer");
		if(overlayer != null)
		{
			overlayer.remove();
		}	
    }
    //复制文本
    function setTxt(objValue) 
    { 
        var t=document.getElementById(objValue);         
        if(!window.clipboardData.setData('text',t.innerText))
        {
            alert("温馨提示：确实允许网页访问剪切板\n\n刷新页面，点复制时允许！");
        }                
    } 


   function checkForm()
      {
    //   var drop_User=document.getElementById("drop_UserList");
        var txt_Restdate=document.getElementById("text_restDate").value;
        var drop_type=document.getElementById("DropDownList_type");
//        var drop_user=document.getElementById("drop_UserList");
//        var _user=drop_user.SelectedValue;
        var _type=drop_type.selectedIndex;
        
        
        if(trim(txt_Restdate).length<=0)
        {
            alert("温馨提示：请输入假期日期！");
            $("#text_restDate").focus();
            return false;
        } 
          if(_type=="0")
        {
            alert("温馨提示：请输入假日类型！");
            $("#DropDownList_type").focus();
            return false;
        } 
        if(_type=="5")
        {
                if(confirm('选择个人出差将自动添加从“日期”到“所有假日截止日期”内的每一天，是否确认添加？'))
               {
                return true;
               } 
               else return false;
        }
        return true;
        
 
}
 
//function checkDelete()
//{
//    
//    if(confirm('确认删除列表中全部假日吗？'))
//   {
//        
//   } 
//}

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="div_body">
        <div id="leftBodyer">
            <div id="div_top">
                <a href="JournalList.aspx" target="_self" class="top_a">总结查看</a><br /><br />
                <a href="userManager.aspx" target="_self"  class="top_a">用户管理</a><br /><br />
                <a href="MissionList.aspx" target="_self"  class="top_a">任务管理</a><br /><br />
                <a href="/filemanage/default.aspx" target="_self"  class="top_a">文档管理</a><br />
                <br />
               <a href="systemManage.aspx" target="_self"  class="top_b">假日管理</a><br />
                <br />
                <a href="../loginout.aspx" target="_self"  class="top_a">退出管理</a>                
                <br />
                <br />
                <br />
            </div>
        </div>
        <div id="mainBodyer">
            <div id="div_operation" style="width: 100%; height: 140px">
                <div style="text-align: center">
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="width: 25%; height: 28px;" align="left">
                                &nbsp;
                                人员：<asp:DropDownList runat="server" ID="drop_UserList"  Width="120px">
                                    </asp:DropDownList></td>
                            <td style="width: 25%; height: 28px;" align="left">
                                日期：<input style="width: 110px; height: 17px" id="text_restDate" readonly onfocus="HS_setDate(this);"
                                            type="text" name="date_RestDate" runat="server" /></td>
                            <td rowspan="4" style="width: 50%" align="left">
                                <asp:Button  CssClass="btn"  runat="server"
                                                 id="btn_addDate" Text="确 定"  OnClientClick="return checkForm();"  OnClick="btn_addDate_ServerClick" style="width: 70px; height: 40px" /><br />
                                <br />
                                <input class="btn" runat="server"
                                                type="button" id="Button_reset" value="取 消" onserverclick="Button_reset_ServerClick" style="width: 70px; height: 40px" /></td>
                        </tr>
                        <tr>
                            <td style="width: 25%; height: 28px;" align="left">
                                &nbsp;
                                类型：<asp:DropDownList ID="DropDownList_type" runat="server" Width="120px">
                                    <asp:ListItem Selected="True">请选择</asp:ListItem>
                                    <asp:ListItem>周六</asp:ListItem>
                                    <asp:ListItem>周日</asp:ListItem>
                                    <asp:ListItem>法定节假</asp:ListItem>
                                    <asp:ListItem>其他节假</asp:ListItem>
                                    <asp:ListItem>个人出差</asp:ListItem> 
                                     <asp:ListItem>个人请假</asp:ListItem>
                                    <asp:ListItem>个人其他</asp:ListItem>
                                    
                                </asp:DropDownList></td>
                            <td style="width: 25%; height: 28px;" align="left">
                                细节：<asp:DropDownList ID="DropDownList_resetDetail" runat="server" Width="120px">
                                    <asp:ListItem Selected="True">全天</asp:ListItem>
                                    <asp:ListItem>上午</asp:ListItem>
                                    <asp:ListItem>下午</asp:ListItem>
                                </asp:DropDownList></td>
                                
                        </tr>
                        <tr>
                            <td colspan="2" align="left" style="height: 28px">
                                &nbsp;
                                    备注：<input  runat="server" id="Text_remark" type="text" style="width: 315px"  /></td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" style="height: 28px">
                                &nbsp;
                                所有假期截止日期：<input style="width: 230px; height: 17px" id="Text_allEndDate" readonly onfocus="HS_setDate(this);"
                                            type="text" name="date_RestDate" runat="server" /></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="div_restTimeList" style="width: 100%; height: 490px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 35px">
                    <tr>
                        <td style="border-right: #828282 2px solid; border-top: #828282 2px solid; border-left: #828282 2px solid;
                            width: 100%; border-bottom: #828282 2px solid; height: 30px; background-image: none; background-color: lightgrey;" valign="middle">
             &nbsp; 开始日期：<input style="width: 90px; height: 17px" id="Text_s_beginDate" readonly onfocus="HS_setDate(this);"
                                            type="text" name="date_RestDate" runat="server" />
                            &nbsp;
                            结束日期：<input style="width: 90px; height: 17px" id="Text_s_endDate" readonly onfocus="HS_setDate(this);"
                                            type="text" name="date_RestDate" runat="server" />
                            &nbsp; 
                    
                            人员：<asp:DropDownList runat="server" ID="DropDownList_s_user"  Width="90px">
                            </asp:DropDownList>&nbsp; 类型：<asp:DropDownList ID="DropDownList_s_type" runat="server" Width="90px">
                                <asp:ListItem Selected="True">请选择</asp:ListItem>
                                <asp:ListItem>周六</asp:ListItem>
                                    <asp:ListItem>周日</asp:ListItem>
                                    <asp:ListItem>法定节假</asp:ListItem>
                                    <asp:ListItem>其他节假</asp:ListItem>
                                    <asp:ListItem>个人出差</asp:ListItem> 
                                     <asp:ListItem>个人请假</asp:ListItem>
                                    <asp:ListItem>个人其他</asp:ListItem>
                            </asp:DropDownList>&nbsp;<input class="btn" runat="server"
                                                type="button" id="Button_search" value="查 询" onserverclick="Button_search_ServerClick"   />&nbsp;&nbsp;
                            <asp:Button ID="Button_deleteList"  CssClass="btn" runat="server" Text="删除列表" OnClientClick="return confirm('确认删除列表中全部假日吗？');" OnClick="Button_deleteList_Click" />
                            </td>
                    </tr>
                </table>
                <asp:GridView ID="GridView1" runat="server"  AllowPaging="True" AutoGenerateColumns="False" Width="100%"  PageSize="15" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowCreated="GridView1_RowCreated" DataKeyNames="RestID"   OnPageIndexChanging="GridView1_PageIndexChanging" >
                    <Columns>
                        <asp:BoundField DataField="RestID" HeaderText="RestID" InsertVisible="False" SortExpression="RestID" Visible="False" />
                        <asp:TemplateField HeaderText="假日日期" SortExpression="RestDate">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("RestDate") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle Width="150px" />
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("RestDate", "{0:yyyy-M-d}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="realName" HeaderText="人员名称" SortExpression="realName" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CycleNum" HeaderText="CycleNum" SortExpression="CycleNum" Visible="False" />
                        <asp:BoundField DataField="RestDetail" HeaderText="假期细节" SortExpression="RestDetail" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Type" HeaderText="假日类型" SortExpression="Type" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderText="备注" SortExpression="Remark" />
                        <asp:BoundField DataField="userid" HeaderText="userid" Visible="False" />
                        <asp:CommandField ShowEditButton="True" >
                            <ItemStyle Width="50px" />
                        </asp:CommandField>
                        <asp:CommandField ShowDeleteButton="True" >
                            <ItemStyle Width="50px" />
                        </asp:CommandField>
                    </Columns>
                    <RowStyle Height="22px" />
                    <HeaderStyle BackColor="#EEEEEE" BorderColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px" Height="25px" />
                    <FooterStyle BackColor="#EEEEEE" BorderColor="#E0E0E0" BorderStyle="Solid" BorderWidth="1px" Height="25px" />
                </asp:GridView>
    <%--            
           <asp:AccessDataSource ID="AccessDataSource1" runat="server" >
                </asp:AccessDataSource>--%>
            
            </div>
        </div>
        </div>
    </form>
</body>
</html>
