<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewMission.aspx.cs" Inherits="JournalWeb.user.viewMission" %>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=tempTitle %></title>
        <script src="../inc/js/datePicker.js" type="text/javascript"></script>
        <link rel="StyleSheet" href="../inc/css/dtree.css" type="text/css" />    
    <script src="../inc/js/dtree.js" type="text/javascript"></script>
    <script src="../inc/js/jquery.js" type="text/javascript"></script>
        <script src="../inc/js/jquery.interface.js" type="text/javascript"></script>
    <script src="../inc/js/jquery.jwindow.js" type="text/javascript"></script>   
    
    <link  rel="Stylesheet" href="../inc/css/jwindowUser.css" />
    
    <style type="text/css">
    body
    {
        margin:0px;
        padding:0px;
        font-size:14px;
        background-color:#EEEEFF;
    }
    #div_top{height:30px;height:100%;text-align:left;padding-left:12px;padding-top:5px;line-height:22px;}
    #leftBodyer { 
        width:12%; /*设定宽度*/
        height:100%;
        text-align:left; /*文字左对齐*/
        float:left; /*浮动居左*/
        clear:left; /*不允许左侧存在浮动*/
        /*overflow:hidden;超出宽度部分隐藏overflow-y:scroll;*/
        overflow-x:hidden;
        
        background-color:#F7F3F7;/*#E5EEF7border:1px solid #A2BDE0; */
        padding-bottom:12px; 
        padding-left:10px;
        margin-left:5px;   
        border:1px solid #666666;
           
    }
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
	width:96%;
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

    #div_page
    {
        margin-bottom:5px;
    }
    .tr_row1
    {
        /*background-color:#E5EEF7;*/
    }
    #mainBodyer
    {
        width: 85%;
        height:100%;
        text-align: left;
        float: left; /*浮动居右*/
        clear: right; /*不允许右侧存在浮动*/
        /*overflow: hidden;超出宽度部分隐藏*/
        overflow-x:hidden;
        /*overflow-y:scroll;border:1px solid #A2BDE0;*/
        border:1px solid #666666;
        background-color:#FFFFFF;
        margin-left:5px;         
    }
    #topBodyer
    {
        width:100%;
        height:20px;
        text-align:center; ;/*#E5EEF7 #CCE8F7 */
        padding-top:15px;
        padding-bottom:10px;
        background-color:#FFFFFF;
    }  
    #div_body
    {
        border-collapse:collapse;
        padding-top:5px;
    } .top_a
      {
         text-decoration: none; 
         color:#000;              
      }
      .top_b
      {
        color:red;
        text-decoration: none;        
      }
    
     /* #tbl_list{width:100%;border-collapse:collapse;font-size:14px;text-align:center}/*border:1px solid #EEE;*/
    /*  #tbl_list th{ solid #CCC;padding:4px;}/*background:#EEE;border-bottom:1px*/ 
      #tbl_list{width:100%;border-collapse:collapse; border:1px solid #3A6EA5;font-size:12px;text-align:center}
      #tbl_list th{background:#EEE;padding:4px;border:1px solid #CCC}/*E5EEF7 F7F3F7border-bottom:1px solid #CCC;*/
      #tbl_list td{border-bottom:1px solid #EEE;padding:4px;}/*border:1px solid #EEE;*/
      #tdOther{text-align:left;}
      
    </style>
    <script type="text/javascript">
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
        
            function setTxt(objValue) 
        { 
            var t=document.getElementById(objValue);         
            if(!window.clipboardData.setData('text',t.innerText))
            {
                alert("温馨提示：确实允许网页访问剪切板\n\n刷新页面，点复制时允许！");
            }                
        } 
        
         function showDiv2()
        {
            var ud=$("#hid_uId").val();
            showDiv(ud);
        }
        //弹出div层 
        function showDiv(uid)
        {
            document.getElementById("div_content").innerHTML="<iframe src='userModify.aspx?uid="+uid+"' frameborder='0' width='100%' height='150'/>";
            $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
        }
        //做一些事情
        function doSomething(tp,uid,mid)
        {
            var bDate=$("#beginTime").val();
            var eDate=$("#endTime").val();
            var obj=document.getElementById("drop_Status");
            var index=obj.selectedIndex; 
            var status=obj.options[index].value
            
            var obj1=document.getElementById("drop_type");
            var index1=obj1.selectedIndex; 
            var typeMission=obj1.options[index].value
            var tempalert="";
            if(tp=="second")
            {
                tempalert="确认完成了么？\n\n一旦点击将不可更改！";
            }
            else if(tp=="first")
            {
                tempalert="确认要执行么？\n\n一旦执行将不可更改！";
            }
            else
            {
                return false;
            }
            if(confirm(tempalert))
        {          
           $.ajax({
            type:"post",
            url:"/handler/doMission.ashx",
            data:"uid="+uid+"&mid="+mid+"&tp="+tp,
            success:function(msg){
                if(msg=="1")
                {
                    if(tp=="second")
                    {
                        alert("温馨提示：--该任务已经完成!请继续其它任务！");    
                    }
                    else if(tp=="first")
                    {
                        alert("温馨提示：--该任务已经开始执行!请把握好时间\!");    
                    }                
                    //location=location.href;
                    location="?page=1&bd="+bDate+"&ed="+eDate+"&st="+status+"&uid="+uid+"&to="+typeMission;
                }   
                else
                {
                    alert("温馨提示：操作失败！");
                }      
            }
           });
       }                 
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
            <div id="div_body">
            
            <div id="leftBodyer">
                <div id="div_top">
                     <a href="viewMission.aspx" target="_self" class="top_b">任务浏览</a><br />
                    <a href="viewList.aspx" target="_self" class="top_a">总结浏览</a><br />
                    <a href="javascript:void(0);" name="modify" onclick="showDiv2()"  class="top_a">密码设置</a><br /> 
                    <a href="../loginout.aspx" target="_self"  class="top_a">退出管理</a><br />
                </div>               
                <div class="dtree" id="navi" runat="server">
                </div>
            </div>
            <div id="mainBodyer">            
            <div id="div_list">  
                <div id="topBodyer">
                    <table cellpadding="0" cellspacing="0" border="0" align="center" id="tbl_Search">
                        <tr>
                            <td height="20px">
                                <span>任务人：<asp:DropDownList runat="server" ID="drop_UserList">
                                </asp:DropDownList></span>
                                类型<asp:DropDownList runat="server" ID="drop_type">
                                <asp:ListItem Value="-1">不限</asp:ListItem>
                                        <asp:ListItem Value="0">维护</asp:ListItem>
                                        <asp:ListItem Value="1">开发</asp:ListItem>
                                       <asp:ListItem Value="2">需求分析</asp:ListItem>
                                       <asp:ListItem Value="3">系统设计</asp:ListItem>  
                                    </asp:DropDownList>
                            </td>
                            
                            <td height="20px">
                                <span>&nbsp;&nbsp;从
                                    <input style="width: 80px; height: 17px" id="beginTime" readonly onfocus="HS_setDate(this);"
                                        type="text" name="beginTime" runat="server" />
                                    <img onclick="selectDate();" alt="" src="/Inc/images/calendar.gif" /></span>&nbsp;
                            </td>
                            <td height="20px">
                                <span>至
                                    <input style="width: 80px; height: 17px" id="endTime" readonly onfocus="HS_setDate(this);"
                                        type="text" name="endTime" runat="server" />
                                    <img onclick="selectDate1();" alt="" src="/Inc/images/calendar.gif" /></span>&nbsp;
                            </td>
                             <td height="20px">
                                    <span>状态：<asp:DropDownList runat="server" ID="drop_Status">                                        
                                        <asp:ListItem Value="0">待执行</asp:ListItem>
                                        <asp:ListItem Value="1">执行中</asp:ListItem>
                                        <asp:ListItem Value="2">已完成</asp:ListItem>
                                       <asp:ListItem Value="-1">-全部-</asp:ListItem> 
                                    </asp:DropDownList></span>
                                </td>
                            <td>
                                &nbsp;&nbsp;<asp:Button ID="btn_search" runat="server" Text="立即查找" CssClass="btn" OnClick="btn_search_Click" /></td>
                            <td>
                                &nbsp;&nbsp;</td>
                        </tr>
                    </table>
                </div>                         
                <div style="width:100%">
                <table cellpadding="0" cellspacing="0" align="center" border="0" id="tbl_list" runat="server" style="width:100%"
                    class="sortable">
                </table>
                </div>
            </div>
            <div id="div_page" style="width: 100%;">
                <table cellpadding="0" cellspacing="0" border="0" align="center" id="tbl_page">
                    <tr>
                        <td height="10px;">
                        </td>
                    </tr>
                    <tr>
                        <td id="tbl_td_page" runat="server">
                        </td>
                    </tr>
                </table>
            </div>
            
            </div>
        </div>
                <div class="window " id="panelWindow">
	<div class="title">修改用户信息<span class="buttons"><span class="close"></span>&nbsp;&nbsp;</span></div>
	<div class="content" id="div_content">		   
	</div>
	<div class="status"><span class="resize"></span></div>
</div>
        <input id="hid_uId" runat="server" style="width: 68px" type="hidden" />
    </form>
</body>
</html>
