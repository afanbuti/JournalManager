<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MissionList.aspx.cs" Inherits="JournalWeb.manage.MissionList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
<head runat="server">
    <title><%= tempTitle %></title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <script src="../inc/js/datePicker.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.interface.js" type="text/javascript"></script>

    <script src="../inc/js/jquery.jwindow.js" type="text/javascript"></script>

    <script src="../inc/js/innerSupport.js" type="text/javascript"></script>

    <script src="../inc/js/common.js" type="text/javascript"></script>
    <link rel="Stylesheet" href="../inc/css/jwindow.css" />

    <style type="text/css">
    body{font-size:14px;margin:0px;padding:0px;background-color:#EEEEFF;}
    
    #div_top{

     width: 100%;
        height:26px;
        text-align: left;
        padding-top:8px;
        margin-bottom:5px;
        border:1px solid #666666;
        background-color:#F7F3F7;
    }/*background-color:#E5EEF7;*/
    #tbl_list{border-collapse:collapse;border:0px solid #EEE;font-size:12px;text-align:center}
      #tbl_list th{background:#EEE;padding:4px;border:1px solid #CCC}/*E5EEF7 F7F3F7border-bottom:1px solid #CCC;*/
      #tbl_list td{border-bottom:1px solid #EEE;padding:4px;}
      .input1{font-family: verdana;background-color: #ffffff;border-bottom: #FFFFFF 1px solid;border-left: #CCCCCC 1px solid;border-right: #FFFFFF 1px solid;border-top: #CCCCCC 1px solid;font-size: 12px;}
    .input1-bor {font-family:verdana;background-color:#F0F8FF;font-size: 12px;border: 1px solid #333333;cursor:pointer;}    
    .hover{background-color:#E5EEF7;cursor:pointer}  /*#FFD900这里是鼠标经过时的颜色*/       
      .btn{
        padding:2px 2px 0px 2px;
        border:#2C59AA 1px solid;         
        font-size: 12px; 
        filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#ffffff, EndColorStr=#D7E7FA);         
        cursor: hand; 
        color: black;         
        text-align:center;
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
     .posttitle
{
	font-size: 13px;	
	margin-right: 10px;
	margin-top:2px;
	background-color: #E5EEF7;/*E5EEF7*/
	border-top: 1px solid #666;
	border-bottom: 1px dashed #666;
	padding : 2px;
	margin-bottom:0px;
	font-family: Verdana;
	font-weight:bold;
	width:96%;
	height:20px;
}
    
     #div_content2{
margin:10px 10px 10px 10px;
overflow-y:auto;
scrollbar-face-color:#ffffff;
scrollbar-highlight-color:#ffffff;
overflow:auto;
width:410;
scrollbar-shadow-color:#919192;
scrollbar-3dlight-color:#ffffff;
line-height:18px;
scrollbar-arrow-color:#919192;
scrollbar-track-color:#ffffff;
scrollbar-darkshadow-color:#ffffff;
height:160px;
}
     .posttitle
{
	font-size: 13px;	
	margin-right: 10px;
	margin-top:2px;
	background-color: #E5EEF7;
	border-top: 1px solid #666;
	border-bottom: 1px dashed #666;
	padding : 2px;
	margin-bottom:0px;
	font-family: Verdana;
	font-weight:bold;
	width:100%;
}
    #mainBodyer
    {
        width: 100%;
        height:100%;
        text-align: left;
         left:0px;
        float: left; /*浮动居右*/
        clear: right; /*不允许右侧存在浮动*/
        /*overflow: hidden;超出宽度部分隐藏*/
        overflow-x:hidden;
        /*overflow-y:scroll;*/
                border:1px solid #666666;
           background-color: #ffffff;     
     }
    #div_page
    {
        margin-bottom:5px;
    }
     #div_body
    {
     
        border-collapse:collapse;
        padding-top:5px;        
        padding-left:10px;
        padding-right:10px;
    } 
   
 /*  
   #out{width:300px;height:20px;background:#ccc; position:relative}
#in{width:10px; height:20px;background:#03f;color:white;text-align:center;overflow:hidden;}
#in_0{text-align:center; width:300px; font-weight:bold; height:20px; line-height:20px; position:absolute; z-index:-1;}
#in_1{text-align:center; width:300px; font-weight:bold; height:20px; line-height:20px; color:#fff;}
#font_color{background:yellow;text-align:center;color:white;}

*/

 
    </style>
   
   
   
   

   
    

    <script type="text/javascript">
    
//      $(document).ready(function(){
//        rowColor();        
//      });  
      
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
    
    function ShowText()
    {
        var obj=document.getElementById("div_addMission");
        var obj2=document.getElementById("btn_add");
        var drop1=document.getElementById("drop_Status");
        var drop2=document.getElementById("drop_UserList");
        var   index   =   drop2.selectedIndex; 
        
        if(obj.style.display=='none')
            {
                obj.style.display='block';
                obj2.value='取消';
                document.title="添加--["+drop2.options[index].text+"]--的任务";
                $("#txt_desc").focus();
                document.getElementById("div_MissPerson").innerHTML= drop2.options[index].text;
                drop1.disabled =true;
                
            }
    } 
      
      //显示添加内容
      function ShowAddText()
      {
        var obj=document.getElementById("div_addMission");
        var obj2=document.getElementById("btn_add");
        var drop1=document.getElementById("drop_Status");
        var drop2=document.getElementById("drop_UserList");
        var   index   =   drop2.selectedIndex; 
//        if(drop2.options[index].value=="-1"  )
//        {
//             alert("温馨提示：添加任务时，请先选择确定的一个人员！");
//        }
//        else
//        {
            if(obj.style.display=='none')
            {
                obj.style.display='block';
                obj2.value='取消';
                document.title="添加--["+drop2.options[index].text+"]--的任务";
                $("#txt_desc").focus();
                document.getElementById("div_MissPerson").innerHTML= drop2.options[index].text;
                drop1.disabled =true;
                
            }
            else if(obj.style.display=='block')
            {
                obj.style.display='none';            
                obj2.value='添加';  
                $("#txt_desc").val(''); 
                document.title="["+drop2.options[index].text+"]--的任务列表";
                document.getElementById("div_MissPerson").innerHTML=""; 
                drop1.disabled =false;          
            }    
        //}
      }
      
      //查询取消添加
      function checkAdd()
      {
        var obj=document.getElementById("div_addMission");
        var obj2=document.getElementById("btn_add");
        var drop1=document.getElementById("drop_Status");
        var drop2=document.getElementById("drop_UserList");
        var   index   =   drop2.selectedIndex;   
                
        if(obj.style.display=='block')
        {
            obj.style.display='none';            
            obj2.value='添加';  
            $("#txt_desc").val('');             
            document.getElementById("div_MissPerson").innerHTML=""; 
            drop1.disabled =false;          
        }    
        document.title="["+drop2.options[index].text+"]--的任务列表";   
      }
      //核对是否填写任务
      function checkForm()
      {
        //var tempDesc=$("#txt_desc").val();        
        var tempDesc=document.getElementById("txt_desc").value;
        var tempWorkHour=document.getElementById("Text_workHour").value;
        var tempPerson=document.getElementById("div_MissPerson").innerHTML; 
        var drop2=document.getElementById("drop_UserList");
        var drop_type=document.getElementById("drop_typeList");
        var txt_path=document.getElementById("txt_Path").value;
        var   index   =   drop2.selectedIndex;
        var index_type=drop_type.selectedIndex;
        
        var drop_value= drop2.options[index].value;
        var drop_text=drop2.options[index].text;
        
        var type_value=drop_type.options[index_type].value;//类型编号
        var type_text=drop_type.options[index_type].text;//类型文本
        
        if(trim(tempDesc).length<=0)
        {
            alert("温馨提示：请输入要分配给--["+tempPerson+"]--的任务内容！");
            $("#txt_desc").focus();
            return false;
        }   
        if(trim(tempWorkHour).length<=0)
        {
            alert("温馨提示：请输入要分配给--["+tempPerson+"]--的任务工时！");
            $("#Text_workHour").focus();
            return false;
        }     
        addMission(drop_value,drop_text,tempDesc,tempWorkHour,type_value,txt_path);
      }
      
      //addMission
    function addMission(uid,uName,udesc,uWorkHour,typeValue,txtpath)
    {   
        var beginDate=document.getElementById("beginTime").value;
        var endDate=document.getElementById("endTime").value;
        var isa=document.getElementById("hid_IsAsign").value;
        //if(confirm("确认要添加--["+uName+"]--的任务么？"))
        //{          
           $.ajax({
            type:"post",
            url:"/handler/doMission.ashx",
            data:"uid="+uid+"&desc="+escape(udesc)+"&workHour="+escape(uWorkHour)+"&tp=add&tpValue="+escape(typeValue)+"&path="+escape(txtpath),
            success:function(msg){
                if(msg=="1")
                {
                    //if(confirm("温馨提示：--["+uName+"]--的任务添加成功!\n\n点击确认将关闭添加对话框!"))
                    //{
                        ShowAddText();
                    //}
                    location="?opt=on&page=1&uid="+uid+"&to="+escape(typeValue)+"&bd="+beginDate+"&ed="+endDate+"&isa="+isa;
                }   
                else
                {
                    alert("温馨提示：--["+uName+"]--的任务添加失败!");
                }      
            }
           });
       //}     
    }
    
    function deldata(mid,uid,path)
    {        
        if(confirm("温馨提示：确认要删除这项任务么？"))
        {
            $.ajax({
                type:"post",
                url:"/handler/doMission.ashx",
                data:"mid="+mid+"&tp=del&uid="+uid+"&path="+escape(path),
                success:function(msg){
                    if(msg=="1") //表示删除成功
                    {
                        //alert("温馨提示：该任务已经删除成功！\n\n任务无法挽回，只又重写！谢谢合作！"); 
                        var drop_type=document.getElementById("drop_type");       
                        var index_type=drop_type.selectedIndex;
                        var type_value=drop_type.options[index_type].value;//类型编号
                        var chkType="";
                        if(document.getElementById("chk_IsAsigin").checked==true)
                        {
                            chkType="1";
                        }
                        else
                        {
                            chkType="0";
                        }                        
                        location="?opt=off&page=1&uid="+uid+"&to="+escape(type_value)+"&isa="+chkType;
                    }
                    else
                    {
                        alert("温馨提示：任务删除失败！");
                    }
                }            
            });
        }
        else
        {
            return false;
        }
    }
    
    //选择日期
    function selectDate()
    {
        var obj=document.getElementById("beginTime");        
        HS_setDate(obj);
    }
    //选择日期
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
    
    function selectData()
    {
        var isAsign=document.getElementById("hid_IsAsign");
        var chkAsign=document.getElementById("chk_IsAsigin"); 
               
        if(chkAsign.checked)
        {
           isAsign.value="1";
        }
        else
        {
           isAsign.value="0";
        }
    }
    
    //弹出div层 
    function showDiv(mid)
    {
        var bDate=$("#beginTime").val();
        var eDate=$("#endTime").val();
        $("#panelWindow").css("width","500");
        document.getElementById("div_WinTitle").innerHTML="用户列表";
        document.getElementById("div_content").innerHTML="<iframe src='userList.aspx?mid="+mid+"&bd="+bDate+"&ed="+eDate+"' frameborder='0' width='100%' height='280'/>";
        $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				   			
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
    }
    //显示任务
    function showMission(mid,cbh)
    {
        $("#panelWindow").css("width","500");
        document.getElementById("div_WinTitle").innerHTML="修改任务";
        document.getElementById("div_content").innerHTML="<iframe src='showMission.aspx?mid="+mid+"&bh="+cbh+"' frameborder='0' width='100%' height='300'/>";
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
            if (event.ctrlKey &&event.keyCode == 13)
            {
                event.returnValue=false;
                event.cancel = true;
                var obj=document.getElementById("div_addMission");
                if(obj.style.display=='block')
                {
                    form1.btn_addMission.click();
                }
            }
        }
        function openUpload()
        {
            //var arr=window.showModalDialog("FileUpload.htm","window","dialogWidth:630px;dialogHeight:300px;help:no;scroll:no;status:no;");
            window.open('FileUpload.htm','_blank','height=300,width=630,top=70,status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=yes');
        }
        
        function ShowExport()
        {
            //var arr=window.showModalDialog("ExportTo.aspx","window","dialogWidth:630px;dialogHeight:300px;help:no;scroll:no;status:no;");
            alert("温馨提示：导出内容是已经完成的任务！");
        }
        
        function setTxt(objValue) 
        { 
            var t=document.getElementById(objValue);         
            if(!window.clipboardData.setData('text',t.innerText))
            {
                alert("温馨提示：确实允许网页访问剪切板\n\n刷新页面，点复制时允许！");
            }                
        } 
        
        
        
        function showFullName()
        {
               show
        }        
    </script>

</head>
<body onkeydown="KeyDown(event);">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div id="div_body">
            <div  id="div_top" >&nbsp;&nbsp;
                    <a href="JournalList.aspx" target="_self" class="top_a">总结查看</a>&nbsp;&nbsp;
                <a href="userManager.aspx" target="_self"  class="top_a">用户管理</a>&nbsp;&nbsp;
                <a href="MissionList.aspx" target="_self"  class="top_b">任务管理</a>&nbsp;&nbsp;
                 <a href="/filemanage/default.aspx" target="_self"  class="top_a">文档管理</a>&nbsp;&nbsp;
                 <a href="systemManage.aspx" target="_self"  class="top_a">假日管理</a>&nbsp;&nbsp;
                <a href="../loginout.aspx" target="_self"  class="top_a">退出管理</a>    
                </div>
            <div id="mainBodyer">
             <div id="div_list" style="width: 100%; left:0px;">
                <%--       <div style="width: 100%">
                        <table cellpadding="0" cellspacing="0" align="center" border="0">
                            <tr>
                                <td height="10px;">
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                    <div style="width: 100%;padding-bottom:10px;">
                        <table cellpadding="0" cellspacing="0" border="0" align="center" id="tbl_Search" style="width:100%">
                            <tr>
                                <td colspan="7" align="center" style="height: 20px">
                                    任务人：<asp:DropDownList runat="server" ID="drop_UserList" onchange="chageValue();" Width="68px">
                                    </asp:DropDownList>类型<asp:DropDownList runat="server" ID="drop_type">
                                    <asp:ListItem Value="-1">不限</asp:ListItem>
                                        <asp:ListItem Value="0">维护</asp:ListItem>
                                        <asp:ListItem Value="1">开发</asp:ListItem>
                                        <asp:ListItem Value="2">需求分析</asp:ListItem>
                                         <asp:ListItem Value="3">系统设计</asp:ListItem>
                                    </asp:DropDownList>已分配：<asp:CheckBox runat="server" ID="chk_IsAsigin" />
                                    <span>从<input style="width: 68px; height: 17px" id="beginTime" readonly onfocus="HS_setDate(this);"
                                            type="text" name="beginTime" runat="server" /></span>到<input style="width: 66px; height: 17px" id="endTime" readonly onfocus="HS_setDate(this);"
                                            type="text" name="endTime" runat="server" />状态：<asp:DropDownList runat="server" ID="drop_Status" Width="62px">
                                        <asp:ListItem Value="-1">-全部-</asp:ListItem>
                                        <asp:ListItem Value="0">待执行</asp:ListItem>
                                        <asp:ListItem Value="1">执行中</asp:ListItem>
                                        <asp:ListItem Value="2">已经完成</asp:ListItem>
                                    </asp:DropDownList>内容：<input type="text" id="txt_content" value="" style="width: 80px" runat="server" />
                                    <input runat="server" id="btn_Search" type="button" value="查询" class="btn" onclick="checkAdd();" onserverclick="btn_Search_ServerClick" />&nbsp;<input runat="server" id="btn_add" type="button" value="添加" class="btn"
                                        onclick="ShowAddText();" />&nbsp;<input runat="server" id="btn_Export" type="button" value="导出" class="btn"
                                        onclick="ShowExport();" onserverclick="btn_Export_ServerClick" /></td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%;display:none; " id="div_addMission">
                        <table cellpadding="0" cellspacing="0" align="center" border="0" width="100%" style="height: 210px">
                            <tr>
                                <td height="10px;">
                                </td>
                            </tr>
                            <tr>
                                <td height="10px;">
                                    <div id="div_noWriteTitle" runat="server" class="posttitle">
                                        添加任务&gt;&gt;&nbsp;任务人：&nbsp;<span id="div_MissPerson" style="color:Red;"></span></div>
                                    <div id="div_NoWrite" runat="server" style="padding: 5px 0px 5px 5px">
                                      
                                       
                                              <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr>
                                            <td style="width: 50%;" valign="top">
                                        <textarea name="message" runat="server" id="txt_desc" class="input1-bor" style="width: 490px;
                                            height: 125px" /></td>
                                            <td style="width: 50%; height: 141px;" valign="top">
                                             <span style="padding-left: 5px;"></span>
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100%; height: 30px">
                                                            &nbsp; &nbsp;任务类型：<asp:DropDownList runat="server" ID="drop_typeList" Width="110px"><asp:ListItem Text="维护" Value="0"></asp:ListItem><asp:ListItem Text="开发" Value="1"></asp:ListItem>
                                                    <asp:ListItem Value="2">需求分析</asp:ListItem>
                                                    <asp:ListItem Value="3">系统设计</asp:ListItem>
                                                </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 30px">
                                                           &nbsp; &nbsp;计划工时：<asp:TextBox  id="Text_workHour"  runat="server" style="width: 103px" ToolTip="只能输入整数，如 20"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 30px">
                                                            &nbsp; &nbsp;<input type="button" value="添加附件" onclick="openUpload();" id="btn_addDoc" class="btn" style="width: 56px"/>
                                                            <input type="text" id="txt_Path" readonly runat="server" style="width: 110px" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 50px">
                                                            &nbsp; &nbsp;<input class="btn" runat="server"
                                                type="button" id="btn_addMission" value="添加任务" onclick="return checkForm();" style="width: 179px; height: 29px" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>   
                                    </div>
                                   
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%;margin-bottom:20px; border-top-width: 1px; border-top-color: #00ccff;">
            
                        <table cellpadding="0" cellspacing="0" align="center" id="tbl_list" runat="server" style="width:100%;"
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
            <div style="width:100%;text-align:center;margin-bottom:5px;display:none;">注：红灯&nbsp;<img src="../inc/images/red.gif" border='0'/>[待执行]&nbsp;黄灯&nbsp;<img src="../inc/images/yellow.gif" border='0'/>[执行中]&nbsp;绿灯&nbsp;<img src="../inc/images/green.gif" border='0'/>[已经完成]</div>
         </div>   </div>
           
        <input id="hid_uId" runat="server" style="width: 68px" type="hidden" /><input id="hid_IsAsign" runat="server" style="width: 68px" type="hidden" />
         <div class="window " id="panelWindow"  onmouseover="">
            <div class="title">
                <span id="div_WinTitle">用户列表</span><span class="buttons"><span class="close"></span>&nbsp;&nbsp;</span></div>
            <div class="content" id="div_content">
            </div>
            <div class="status">
                <span class="resize"></span>
            </div>
        </div>
        
 
<%-- <div id='div_real1' style='width:50px; height:20px; left: 0px; top: 0px; position:relative; background-color:#EEEEEE;   border-color:#1E6BB2;  border-width:1px; border-style:solid;'>
         <div id='div_real2' style='width:30%; height:100%;background:#4AAF4F;color:#0D00EF;text-align:center; font-weight:bold; line-height:20px; position:absolute; z-index:-1;'></div><div style="position:relative; z-index:2; text-align:center;padding-top:3px;">10%</div>
         </div>
         
            
       <div id='div_plan1' style='width:50px; height:20px; left: 0px; top: 0px; position:relative; background-color:#EEEEEE;   border-color:#1E6BB2;  border-width:1px; border-style:solid;'>
         <div id='div_plan2' style='width:10%; height:100%;background:#FF6600;color:#0D00EF;text-align:center; font-weight:bold; line-height:20px; position:absolute; z-index:-1;'>10%</div></div>
         </div>
       --%>  

     
    </form>
</body>
</html>
