<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showMission.aspx.cs" Inherits="JournalWeb.manage.showMission" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script src="../inc/js/jquery.js" type="text/javascript"></script>
    <style type="text/css">
    body
    {
        font-size:14px;
        margin:0px;
        padding:0px;
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
    
     //删除左右两端的空格
    function trim(str){  
        return str.replace(/(^\s*)|(\s*$)/g, "");
    }
    
    function checkForm()
    {
     //   var tempContent=document.getElementById('<%=txt_EvalDesc.ClientID%>').value;//$("#txt_Desc").val();
      var tempContent=document.getElementById("txt_EvalDesc").value;
        var tempBH=document.getElementById("hid_BH").value;
        var mid=document.getElementById("hid_mid").value;  
        var txt_workHour=document.getElementById("TextBox_workHour").value;
        var txt_realProcess=document.getElementById("TextBox_realProcess").value;
        var drop_type=document.getElementById("drop_type");  
            
        var index_type=drop_type.selectedIndex;
        var type_value=drop_type.options[index_type].value;//类型编号
        var type_text=drop_type.options[index_type].text;//类型名称
                        
        tempContent=trim(tempContent);
        
        $.ajax({
            type:"post",
            url:"/handler/doMission.ashx",
            data:"mid="+mid+"&tp=edit&content="+escape(tempContent)+"&stype="+escape(type_value)+"&sworkHour="+escape(txt_workHour)+"&sRealProcess="+escape(txt_realProcess),
            success:function(msg){
                if(msg!="0") //表示修改成功
                {            
//                    var temp="div_mission_"+tempBH;  
//                    var temp2="div_type"+tempBH;                                  
//                    top.document.getElementById(temp).innerHTML=repstr(tempContent);
//                    top.document.getElementById(temp2).innerHTML=type_text;

                    closeWin();
                    parent.location.reload();
                }
            }            
        });
         
    }
    
      function repstr(str)    
  {    
    return str.replace(/\r\n/ig,"<br/>")    
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div>       
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="3" style="width: 207px">
            <textarea id="txt_EvalDesc" style="width: 473px; height: 239px" runat="server"></textarea></td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="padding-top:5px;">   
               类型
                    <asp:DropDownList ID="drop_type" runat="server" Width="65px">                        
                        <asp:ListItem Value="0">维护</asp:ListItem>
                        <asp:ListItem Value="1">开发</asp:ListItem>
                         <asp:ListItem Value="2">需求分析</asp:ListItem>
                        <asp:ListItem Value="3">系统设计</asp:ListItem>
                    </asp:DropDownList> &nbsp;
                    工时<asp:TextBox ID="TextBox_workHour" runat="server" ToolTip="必须输入整型数字！" Width="50px"></asp:TextBox>
                   &nbsp;实际进度<asp:TextBox ID="TextBox_realProcess" runat="server" ToolTip="必须输入整型数字！" Width="50px"></asp:TextBox>
        &nbsp;<input id="btn_insert" type="button" class="btn" value="修改" onclick="return checkForm();" />&nbsp;<input class="btn" id="tbn_Cancel" type="button" value="取消" onclick="closeWin();" />
                    <input id="hid_BH" style="width: 20px" type="hidden" runat="server" />
                    <input id="hid_mid" style="width: 20px" type="hidden" runat="server" /></td>
            </tr>           
        </table>
    </div>
    </div>
    </form>
</body>
</html>
