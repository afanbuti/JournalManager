<%@ Page Language="C#" AutoEventWireup="true" Codebehind="JournalList.aspx.cs" Inherits="JournalWeb.manage.JournalList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="div_body">
        <div id="leftBodyer">
            <div id="div_top">
                <a href="JournalList.aspx" target="_self" class="top_b">总结查看</a><br /><br />
                <a href="userManager.aspx" target="_self"  class="top_a">用户管理</a><br /><br />
                <a href="MissionList.aspx" target="_self"  class="top_a">任务管理</a><br /><br />
                <a href="/filemanage/default.aspx" target="_self"  class="top_a">文档管理</a><br /><br />
               <a href="systemManage.aspx" target="_self"  class="top_a">假日管理</a><br /><br /> 
                <a href="../loginout.aspx" target="_self"  class="top_a">退出管理</a>                
                <br />
                <br />
                <br />
            </div>
        </div>
        <div id="mainBodyer">
            <div id="div_list">
                <div style="width: 100%">
                    <table cellpadding="0" cellspacing="0" align="center" border="0">
                        <tr>
                            <td height="10px;">
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 100%">
                    <table cellpadding="0" cellspacing="0" border="0" align="center" id="tbl_Search">
                        <tr>
                         <td style="width: 137px; height: 35px">
                                <span>提交人：<asp:DropDownList runat="server" ID="drop_UserList">
                                </asp:DropDownList><%--<input style="width: 97px; height: 17px" id="txt_uName" readonly  type="text"/>--%>&nbsp;</span>
                            </td>
                            <td style="height: 35px">
                                <span>开始日期：
                                    <input style="width: 80px; height: 17px" id="beginTime" readonly onfocus="HS_setDate(this);"
                                        type="text" name="beginTime" runat="server" />
                                    <img onclick="selectDate();" alt="" src="/Inc/images/calendar.gif" /></span>&nbsp;
                            </td>
                            <td style="width: 212px; height: 35px">
                                <span>结束日期：
                                    <input style="width: 80px; height: 17px" id="endTime" readonly onfocus="HS_setDate(this);"
                                        type="text" name="endTime" runat="server" />
                                    <img onclick="selectDate1();" alt="" src="/Inc/images/calendar.gif" /></span>&nbsp;
                            </td>
                           
                            <td style="height: 35px">
                                <asp:Button ID="Button1" runat="server" Text="查询总结" OnClick="Button1_Click" CssClass="btn" /></td>
                            <td style="height: 35px">
                                &nbsp;&nbsp;<asp:Button ID="btn_Export" CssClass="btn" runat="server" Text="导出总结" OnClick="btn_Export_Click" /></td>
                                 <td style="height: 35px">
                                &nbsp;&nbsp;<asp:Button ID="btn_ShowNoWrite" CssClass="btn" runat="server" Text="总结统计" OnClick="btn_ShowNoWrite_Click"  /></td>
                        </tr>
                    </table>
                </div>
                <div style="width: 100%">
                    <table cellpadding="0" cellspacing="0" align="center" border="0" width="100%">
                        <tr>
                            <td height="10px;">
                            </td>
                        </tr>
                         <tr>
                            <td height="10px;"><div id="div_noWriteTitle" runat="server" class="posttitle"></div><div id="div_NoWrite" runat="server" style="padding:10px 0px 10px 10px"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width:100%">
                <table cellpadding="0" cellspacing="0" align="center" border="1" id="tbl_list" runat="server"
                    class="sortable">
                </table>
                </div>
            </div>
            <div id="div_page" style="width: 100%;margin-bottom:5px;">
                <table cellpadding="0" cellspacing="0" border="0" align="center" id="tbl_page">
                    <tr>
                        <td height="10px;">
                        </td>
                    </tr>
                    <tr>
                        <td id="tbl_td_page" runat="server" >
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        </div>
        <div class="window " id="panelWindow">
            <div class="title">
                <span id="div_WinTitle">查看用户总结详细</span><span class="buttons"><span class="close"></span>&nbsp;&nbsp;</span></div>
            <div class="content" id="div_content">
            </div>
            <div class="status">
                <span class="resize"></span>
            </div>
        </div>
    </form>
</body>
</html>
