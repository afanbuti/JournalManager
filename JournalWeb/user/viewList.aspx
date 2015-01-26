<%@ Page Language="C#" AutoEventWireup="true" Codebehind="viewList.aspx.cs" Inherits="JournalWeb.user.viewList" %>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%= tempTitle %>
    </title>

    <script src="../inc/js/datePicker.js" type="text/javascript"></script>

    <style type="text/css">
    body
    {
        margin:0px;
        padding:0px;
        font-size:14px;
        background-color:#EEEEFF;
    }
    #div_top{height:30px;height:100%;text-align:left;padding-left:12px;padding-top:5px;line-height:22px;}
    #div_IsSelf{display:none;}
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
    #div_bp{
        margin:0px 0px 10px 10px;
        border:1px solid #666666;
        width:160px;
        background-color:#EEEEFF;
        text-align:center;       
        cursor:pointer; 
    }
    .posttitle
{
	font-size: 13px;
	padding-left: 10px;	
	
	margin-top:2px;
	background-color: #E5EEF7;
	border-top: 1px solid #666;
	border-bottom: 1px dashed #666;
	padding : 2px;
	margin-bottom:0px;
	font-family: Verdana;
	font-weight:bold;
	width:96%;
	    margin-right: auto;
        margin-left: auto;
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
        margin-bottom:10px;
        padding-bottom:5px;       
        border-bottom:1px dashed #666;
        width:98%;     
        margin-right: auto;
        margin-left: auto; 
 
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
    } 
    .input1{font-family: verdana;background-color: #ffffff;border-bottom: #FFFFFF 1px solid;border-left: #CCCCCC 1px solid;border-right: #FFFFFF 1px solid;border-top: #CCCCCC 1px solid;font-size: 12px;}
    .input1-bor {font-family:verdana;background-color:#F0F8FF;font-size: 12px;border: 1px solid #333333;cursor:pointer;}    
      #tbl_list{width:100%;border-collapse:collapse;font-size:14px;text-align:center}/*border:1px solid #EEE;*/
       #tbl_list th{background:#EEE;padding:4px;border:1px solid #CCC}/*E5EEF7 F7F3F7border-bottom:1px solid #CCC;*/
      #tbl_list td{border-bottom:1px solid #EEE;padding:4px;}
       #tbl_mission_list{width:100%;border-collapse:collapse; border:1px solid #3A6EA5;font-size:12px;text-align:center}
      #tbl_mission_list th{background:#EEE;padding:4px;border:1px solid #3A6EA5}
      #tbl_mission_list td{border-bottom:1px solid #EEE;padding:4px;}
      #tdOther{text-align:left;}
      #div_EvalContent
      {
        padding-right: 3px;         
        padding-left:10px;        
        margin-top:6px;
        padding-bottom: 3px; 
        overflow: hidden;
        text-align: left;
        width:96%;
        color:#0A246A;/*#999999*/    
      }.top_a
      {
         text-decoration: none; 
         color:#000;              
      }
      .top_b
      {
        color:red;
        text-decoration: none;        
      }
      .disable
      {
        border-style:none; 
        border-width: thin; 
        background-color:Transparent; 
        color: #CCCCCC; 
        cursor:wait;
      }      
    </style>
    
    <link rel="StyleSheet" href="../inc/css/dtree.css" type="text/css" />

    <script src="../inc/js/dtree.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.interface.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.jwindow.js" type="text/javascript"></script>

    <link rel="Stylesheet" href="../inc/css/jwindowUser.css" />

    <script type="text/javascript">
    $(document).ready(function(){
        initData();        
    }); 
    
    function initData()
    {
        $("#txt_desc").focus();                    
    }
    
       function setTxt(objValue) 
        { 
            var t=document.getElementById(objValue);         
            if(!window.clipboardData.setData('text',t.innerText))
            {
                alert("温馨提示：确实允许网页访问剪切板\n\n刷新页面，点复制时允许！");
            }                
        } 
        
    
    function showHiddenDiv()
    {
        //var obj=document.getElementById("div_addContent");
        var obj=document.getElementById("div_MissionList");
        if(obj.style.display=='none')
        {
            obj.style.display='block';
        }
        else
        {
            obj.style.display='none';
        }
    }   
    function clearData()
    {
        $("#txt_desc").val("");    
        $("#txt_desc").focus();
      
    }
    //检测是否为空
    function checkForm()
    {
        var desc=$("#txt_desc").val();
        if(trim(desc).length<=0||trim(desc).length<=15)
        {
           alert("温馨提示：今天的总结内容不能小于15个字符!");
           $("#txt_desc").focus();                              
           return false;
        }
        
        var lastCount=$("#hid_missionCount").val();
        var lastProcess=$("#hid_realProcess").val();
        var nowProcess=$("#TextBox_realProcess").val();
       
        if(trim(lastCount)!="0") 
        {
            if(trim(nowProcess)=="")
            {
                alert("温馨提示：实际进度不能为空！");
               $("#TextBox_realProcess").focus(); 
               return false;
            }
            if(isNaN(trim(nowProcess)))
            {
                alert("温馨提示：实际进度必须为整数");
               $("#TextBox_realProcess").focus(); 
               return false;
            }
            
            if(trim(lastProcess)!="")
            {
              
                if(isNaN(trim(lastProcess))) 
               {
                    parent.location.reload();
                     return false;
                }
                lastProcess=parseInt(lastProcess);
               nowProcess=parseInt(nowProcess); 
                if(nowProcess<lastProcess)
               {
                    alert("温馨提示：本日任务进度不能小于上次进度！");
                     $("#TextBox_realProcess").focus(); 
                     return false;
               } 
            }
        }
          
        var tempObj=document.getElementById("btn_Add");
        tempObj.className  = "disable";        
        tempObj.value = '正在提交.';
        tempObj.onclick=Function("return false;");            
    }
    
     //做一些事情
        function doSomething(tp,uid,mid)
        {
            var bDate=$("#beginTime").val();
            var eDate=$("#endTime").val(); 
            
            var tempalert="";
            if(tp=="second")
            {
                tempalert="确认完成了么？\n\n一旦点击将不可更改！";
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
                    //location=location.href;
                    location="?page=1&bd="+bDate+"&ed="+eDate+"&uid="+uid;
                }   
                else
                {
                    alert("温馨提示：操作失败！");
                }      
            }
           });
       }                 
        }

    //删除左右两端的空格
    function trim(str){  
        return str.replace(/(^\s*)|(\s*$)/g, "");
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
        //enter提交
        function KeyDown(event)
        {
            if (event.keyCode == 13)
            {
                event.returnValue=false;
                event.cancel = true;
                form1.btn_search.click();
            }
        }  
        
        document.onkeydown=mykeydown;   
    function   mykeydown()
    {   
        if(event.keyCode==116) //屏蔽F5刷新键   
        {   
            window.event.keyCode=0;   
            return   false;   
        }   
   }       
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="div_body">
            <div id="leftBodyer">
                <div id="div_top">
                    <a href="viewMission.aspx" target="_self" class="top_a">任务浏览</a><br />
                    <a href="viewList.aspx" target="_self" class="top_b">总结浏览</a><br />
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
                                    <span>提交人：<asp:DropDownList runat="server" ID="drop_UserList">
                                    </asp:DropDownList></span>
                                </td>
                                <td height="20px">
                                    <span>开始日期：
                                        <input style="width: 80px; height: 17px" id="beginTime" readonly onfocus="HS_setDate(this);"
                                            type="text" name="beginTime" runat="server" />
                                        <img onclick="selectDate();" alt="" src="/Inc/images/calendar.gif" /></span>&nbsp;
                                </td>
                                <td height="20px">
                                    <span>结束日期：
                                        <input style="width: 80px; height: 17px" id="endTime" readonly onfocus="HS_setDate(this);"
                                            type="text" name="endTime" runat="server" />
                                        <img onclick="selectDate1();" alt="" src="/Inc/images/calendar.gif" /></span>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btn_search" runat="server" Text="立即查找" CssClass="btn" OnClick="btn_search_Click" /></td>
                                <td>
                                    &nbsp;&nbsp;</td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%">
                        <table cellpadding="0" cellspacing="0" align="center" border="0" id="tbl_list" runat="server"
                            class="sortable">
                        </table>
                    </div>
                </div>
                <div id="div_page">
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
                
                <%--<div id="div_bp" onclick="showHiddenDiv();">
                    显示/隐藏执行中任务<span runat="server" id="sp_bpCount"></span></div>
                    <div id="div_MissionList" runat="server" style="padding-bottom:5px;padding-left:10px;">
                    这一时间段没有执行中任务
                    </div>--%>
                    
                    
                    
                     <div  id="div_noWriteDate" runat="server" style="width: 100%; text-align:center;">
                        <table cellpadding="0" cellspacing="0"  border="0" id="tbl_noWriteDate" runat="server" style="width:100%"
                            class="sortable">
                           <tr> 
                                <td style="font-size: 14px; font-weight: bold; border-top: #3A6EA5 1px solid;  border-bottom: #3A6EA5 1px dashed; background-color:#E5EEF7; height:25px;">
                                    本月未写总结
                                    <asp:Label ID="lbl_noWriteCount" runat="server" Font-Bold="True" Font-Size="Medium"
                                        ForeColor="Red" Text="0"></asp:Label>
                                    次</td>
                           </tr>
                             <tr> 
                                <td style=" border-bottom: #3A6EA5 1px solid; height: 25px; color: #3300ff;" id="TD_noWriteList" runat="server">
                                </td>
                           </tr> 
                            <tr> 
                                <td style="  height: 3px; " >&nbsp;
                                </td>
                           </tr> 
                        </table>
                    </div>
                    
                      <div style="width: 100%; text-align:left;">
                        <table cellpadding="0" cellspacing="0" align="center" border="0" id="tbl_mission_list" runat="server" style="width:100%"
                            class="sortable">
                        </table>
                    </div>
                    
                    
         <div id="div_IsSelf" runat="server">
                <div id="div_addContent">
              <table width="100%" border="0" cellpadding="0" cellspacing="0" align="center" id="tbl_UserLog">
                        <tr>
                            <td colspan="3" height="5px">&nbsp;                                </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center" style="font-size: 14px; font-weight: bold;">
                                今日总结内容：</td>
                        </tr>
                        <tr>
                            <td colspan="2" rowspan="3" align="left">
                                <textarea name="message" runat="server" id="txt_desc" class="input1-bor" onblur="this.className='input1-bor'"
                                    onfocus="this.className='input1-bor'"  style="width: 498px; height: 162px" title="温馨提示：每天17：25以后才能填写总结。" /></td>
                            <td align="left">&nbsp;任务进度：<asp:TextBox ID="TextBox_realProcess" runat="server" ToolTip="只能输入整数,例如 50"
                                    Width="122px"></asp:TextBox>
                                 <br />   <span style="color:Red;font-size:12px">温馨提示 1：任务进度只能输入整数,<br />
                                     &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 例如若任务进度是 50%，则只需输入 50 。</span></td>
                        </tr>
                        <tr>
                          <td align="left"><span style="color:Red;font-size:12px">温馨提示 2：每天17：25至24：00可以填写总结，<br />
                                     &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;且只能提交一次数据，提交后将不可修改。</span></td>
                        </tr>
                        <tr>
                          <td align="left">&nbsp;<asp:Button ID="btn_Add" CssClass="btn" runat="server" Text="我的总结" OnClick="btn_Add_Click"
                                    OnClientClick="return checkForm();" Height="40px" Width="90px" />&nbsp;&nbsp;
                                <input id="btn_reset" type="button" value="写错了！" class="btn" onclick="clearData();"
                                    runat="server" style="width: 90px; height: 40px" /></td>
                        </tr>
                       
                        <tr>
                            <td width="252" style="height: 20px">&nbsp;                                </td>
                            <td width="299" style="width: 191px; height: 20px;">
                            &nbsp;<input id="hid_isHave" runat="server" type="hidden" style="width: 66px" />                            </td>
                            <td style="height: 20px">&nbsp;       <input id="hid_missionCount" runat="server" type="hidden" style="width: 66px" />                            <input id="hid_realProcess" runat="server" type="hidden" style="width: 66px" /></td>
                            <td style="height: 20px">                         </td>
                        </tr>
                    </table>
                </div>
            </div>
            
            
               
                    
                    
            </div>
        </div>
        <div class="window " id="panelWindow">
            <div class="title">
                修改用户信息<span class="buttons"><span class="close" ></span>&nbsp;&nbsp;</span></div>
            <div class="content" id="div_content">
            </div>
            <div class="status">
                <span class="resize"></span>
            </div>
        </div>
        <input id="hid_uId" runat="server" style="width: 68px" type="hidden" />
    </form>
</body>
</html>
