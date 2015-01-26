<%@ Page Language="C#" AutoEventWireup="true" Codebehind="userManager.aspx.cs" Inherits="JournalWeb.manage.addUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户管理页面</title>

    <script src="../inc/js/jquery.js" type="text/javascript"></script>
    <script src="../inc/js/sorttable.js" type="text/javascript"></script>
    <script src="../inc/js/jquery.interface.js" type="text/javascript"></script>
    <script src="../inc/js/jquery.jwindow.js" type="text/javascript"></script>    
    
    <link rel="Stylesheet" href="../inc/css/jwindow.css" />
    
    <style type="text/css">
       body{font-size:14px;margin:0px;padding:0px;background-color:#EEEEFF;}
       #div_top{width:100%;height:100%;text-align:center;padding-top:20px}
       /*#div_top{width:100%;height:40px;background-color:#E5EEF7;height:100%;text-align:center;padding-top:20px}*/
       #div_add{width:100%;}    
       #div_list{width:100%;}
       #div_page{width:100%;}
       .hover{background-color:#E5EEF7;}  /*这里是鼠标经过时的颜色*/ 
       #tbl_page{width:700px;font-size:14px;text-align:center}
       #tbl_list{width:700px;border-collapse:collapse;border:1px solid #EEE;font-size:14px;text-align:center}
       #tbl_list th{background:#EEE;border-bottom:1px solid #CCC;padding:4px;cursor:pointer;}
       #tbl_list td{border:1px solid #EEE;padding:4px;cursor:pointer;}
       #tbl_Search{width:300px;font-size:14px;text-align:center;padding-bottom:10px;} 
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
        width:100px; /*设定宽度*/
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
        width: 890px;
        height:100%;
        text-align: left;
        float: right; /*浮动居右*/
        clear: right; /*不允许右侧存在浮动*/
        /*overflow: hidden;超出宽度部分隐藏*/
        overflow-x:hidden;
        /*overflow-y:scroll;*/
                border:1px solid #666666;
        background-color:#FFFFFF;
    }
    #div_page
    {
        margin-bottom:5px;
    }
     #div_body
    {
        border-collapse:collapse;
        padding-top:5px;        
    } 
    </style>

    <script type="text/javascript">
    
    $(document).ready(function(){
        rowColor();        
    }); 
       
    //弹出div层 
    function showDiv(uid)
    {
        document.getElementById("div_content").innerHTML="<iframe src='userModify.aspx?uid="+uid+"' frameborder='0' width='100%' height='200'/>";
        $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				    		
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
    }
    
    //现在不用了
    function regScript()
    {        
        $("#tbl_list a[name='modify']").each(function(){        
            $(this).bind("click",function(){
			    $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				    		
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
		    });
		});
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
    //核对表单输入
    function checkForm()
    {
        var user=$("#txt_UserName").val();
        var realName=$("#txt_RealName").val();      
        var pass=$("#txt_Pass").val(); 
        var txtXh=$("#txt_XH").val();
        if(trim(user).length<=0)
        {
            alert("温馨提示：请输入用户名!");
            $("#txt_UserName").focus();
            $("#txt_UserName").val("")                   
            return false;
        }
        
        if(trim(realName).length<=0)
        {
            alert("温馨提示：请输入真实姓名!");
            $("#txt_RealName").focus();
            $("#txt_RealName").val("");        
            return false;
        }
        
        if(trim(pass).length<=0)
        {
            alert("温馨提示：请输入登录密码!");
            $("#txt_Pass").focus();
            $("#txt_Pass").val(""); 
            return false;
        }  
        if(trim(txtXh).lenght<=0||!isNum(txtXh))  
        {
            alert("温馨提示：序号必须为数字");
            $("#txt_XH").focus();
            $("#txt_XH").val("0"); 
            return false;
        } 
    }
    //重置数据
    function clearData()
    {
        $("#txt_UserName").val("")
        $("#txt_RealName").val("");      
        $("#txt_Pass").val(""); 
    }
    
    //删除左右两端的空格
    function trim(str){  
        return str.replace(/(^\s*)|(\s*$)/g, "");
    }
    
    //删除数据
    function del(id,gid)
    {   
        if(confirm("确认要删除这个用户么？"))
        {          
           $.ajax({
            type:"post",
            url:"/handler/delpage.ashx",
            data:"id="+id+"&tp=deluser",
            success:function(msg){
                if(msg=="1")
                {
                    alert("温馨提示：删除数据成功!");
                    location="?page=1&gid="+gid;
                }   
                else
                {
                    alert("温馨提示：删除数据失败!");
                }      
            }
           });
       }     
    }
    
    //判断是否为数字
	    function isNum(inputStr)
	    {
	        if(inputStr.length>1)
	        {
	            if(inputStr.charAt(0)=="0")
	            {
	                return false;
	            }
	        }	    
		    for(var i=0;i<inputStr.length;i++)
		    {
			    var oneChar=inputStr.charAt(i);
			    if(oneChar < "0" ||oneChar>"9")
			    {
				    //alert("温馨提示：请您只输入数字！")
				    return false;
			    }
		    }
		    return true;
	    }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="div_body">
     <div id="leftBodyer">
            <div id="div_top">
                <a href="JournalList.aspx" target="_self" class="top_a">总结查看</a><br /><br />
                <a href="userManager.aspx" target="_self"  class="top_b">用户管理</a><br /><br />
                <a href="MissionList.aspx" target="_self"  class="top_a">任务管理</a><br /><br />
                 <a href="/filemanage/default.aspx" target="_self"  class="top_a">文档管理</a><br /><br />
                 <a href="systemManage.aspx" target="_self"  class="top_a">假日管理</a><br />
                <br />
                <a href="../loginout.aspx" target="_self"  class="top_a">退出管理</a>    
                <br />
                <br />
                <br />
            </div>
        </div>
        <div id="mainBodyer">
        <div id="div_add">
            <table cellpadding="0" cellspacing="0" border="0" align="center">
                <tr>
                    <td colspan="3" height="20px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 22px;">
                        所属分组：</td>
                    <td style="width: 100px; height: 22px;">
                        <asp:DropDownList ID="drop_groupList" runat="server">
                        </asp:DropDownList></td>
                    <td style="width: 100px; height: 22px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 24px;">
                        用 户 名：</td>
                    <td style="width: 100px; height: 24px;">
                        <asp:TextBox ID="txt_UserName" runat="server"></asp:TextBox></td>
                    <td style="width: 100px; height: 24px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        真实姓名：</td>
                    <td style="width: 100px">
                        <asp:TextBox ID="txt_RealName" runat="server"></asp:TextBox></td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 26px;">
                        密 &nbsp; &nbsp;&nbsp; 码：</td>
                    <td style="width: 100px; height: 26px;">
                        <asp:TextBox ID="txt_Pass" runat="server" TextMode="Password" Width="149px"></asp:TextBox></td>
                    <td style="width: 100px; height: 26px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        序 &nbsp; &nbsp;&nbsp; 号：</td>
                    <td style="width: 100px">
                        <asp:TextBox ID="txt_XH" runat="server" Width="149px">0</asp:TextBox></td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="20px">
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3" style="height: 24px">
                        <asp:Button ID="btn_AddUser" runat="server" Text="添加" OnClick="btn_AddUser_Click"
                            OnClientClick="return checkForm();" />
                        <input id="btn_cancel" type="button" value="重新来过" onclick="clearData();" /></td>
                </tr>
                <tr>
                    <td style="height: 26px;" align="center" colspan="3">
                        <asp:Literal ID="lit_Message" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                </tr>
            </table>
        </div>
        <div id="div_list">
            <div style="width:100%">
            <table cellpadding="0" cellspacing="0" border="0" align="center" id="tbl_Search">
                <tr>
                    <td height="20px">
                        <span>所属分组:</span>&nbsp;&nbsp;<asp:DropDownList runat="server" ID="drop_Group">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btn_Search" Text="按组搜索" OnClick="btn_Search_Click"></asp:Button></td>
                </tr>
            </table>
            </div>
            <div style="width:100%">
            <table cellpadding="0" cellspacing="0" align="center" border="1" id="tbl_list" runat="server" class="sortable">
                <tr>
                    <th>编    号</th>
                    <th>用 户 名</th>
                    <th>真实姓名</th>
                    <th>用户密码</th>
                    <th>修改时间</th>
                    <th>所属分组</th>
                    <th>序    号</th>
                    <th>操    作</th>
                </tr>                
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
        </div>
        </div>
        <div class="window " id="panelWindow">
	<div class="title">修改用户信息<span class="buttons"><span class="close"></span>&nbsp;&nbsp;</span></div>
	<div class="content" id="div_content">		   
	</div>
	<div class="status"><span class="resize"></span></div>
</div>
    </form>
</body>
</html>
