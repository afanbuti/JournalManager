<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userModify.aspx.cs" Inherits="JournalWeb.manage.userModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script src="../inc/js/jquery.js" type="text/javascript"></script>
    <style type="text/css">
    body
    {
        margin:0px;
        padding:0px;
        font-size:14px;
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
    </style>
    <script type="text/javascript">
    
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
	    
    //核对表单输入
    function checkForm()
    {
        var user=$("#txt_UserName").val();
        var realName=$("#txt_RealName").val();
        var newpass=$("#txt_newPass").val(); 
        var checkpass=$("#txt_checkPass").val();
        var uid=$("#hid_userid").val();
        var txtXh=$("#txt_XH").val();
        //var gid=$("#hidgid").val();        
        var   ddl   =   document.getElementById("drop_groupList")   
        var   index   =   ddl.selectedIndex;    
        var   gid   =   ddl.options[index].value;
                
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
        if(trim(newpass).length<=0)
        {
            alert("温馨提示：请输入新密码!");
            $("#txt_newPass").focus();
            $("#txt_newPass").val(""); 
            return false;
        }     
        if(trim(checkpass).length<=0)
        {
            alert("温馨提示：请输入确认密码!");
            $("#txt_checkPass").focus();
            $("#txt_checkPass").val(""); 
            return false;
        }
        if(newpass!=checkpass)
        {
            alert("温馨提示：两次密码输入不一致!");
            $("#txt_newPass").focus();
            $("#txt_newPass").val(""); 
            $("#txt_checkPass").val("");
            return false;            
        }
        if(trim(txtXh).lenght<=0||!isNum(txtXh))  
        {
            alert("温馨提示：序号必须为数字");
            $("#txt_XH").focus();
            $("#txt_XH").val("0"); 
            return false;
        } 
        $.ajax({
            type:"post",
            url:"/handler/dosome.ashx",
            data:"uid="+uid+"&uname="+escape(user)+"&pwd="+newpass+"&rname="+escape(realName)+"&gid="+gid+"&tp=uMod&bh="+txtXh,
            success:function(msg){
                if(msg=="1")
                {
                    alert("温馨提示：修改数据成功!");
                    top.location="/manage/userManager.aspx?page=1&gid="+gid;
                }   
                else
                {
                    alert("温馨提示：修改数据失败!");
                }      
            }
           });
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
    //删除左右两端的空格
    function trim(str){  
        return str.replace(/(^\s*)|(\s*$)/g, "");
    }   
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table cellpadding="0" cellspacing="0" border="0" align="center">                
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
                    <td style="width: 100px">
                        新&nbsp;
                        密&nbsp; 码：</td>
                    <td style="width: 100px">
                        <asp:TextBox ID="txt_newPass" runat="server" TextMode="Password" Width="149px"></asp:TextBox></td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        确认密码：</td>
                    <td style="width: 100px">
                        <asp:TextBox ID="txt_checkPass" runat="server" TextMode="Password" Width="149px"></asp:TextBox></td>
                    <td style="width: 100px">
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
                        <input id="hid_userid" type="hidden" runat="server" style="width: 81px" />
                        <input id="hidgid" type="hidden" runat="server" style="width: 88px" /></td>
                </tr>
                <tr>
                    <td align="center" colspan="3" style="height: 24px">
                    <input class="btn" value="修 改" id=btn_UpdateData type="button" onclick="return checkForm();" style="width: 36px" />                        
                        <input class="btn"  id="btn_cancel" type="button" value="不改了,就这！" onclick="closeWin();" /></td>
                </tr>                
            </table>
    </div>
    </form>
</body>
</html>
