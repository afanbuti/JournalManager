<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userList.aspx.cs" Inherits="JournalWeb.manage.userList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script src="../inc/js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
    function updateState()
    {
        var objValue=document.getElementById("hidMissionID").value;
        
         var beginDate=document.getElementById("hid_bDate").value;         
        var endDate=document.getElementById("hid_eDate").value;
        
        var objSelectValue=document.getElementById("radio_UserList");
        var rbs= objSelectValue.getElementsByTagName("INPUT"); 
        var selectUser=document.getElementById("hidSelectUser").value;
        var tempValue="";       
        for(var i = 0;i<rbs.length;i++)
        { 
            if(rbs[i].checked)
            {                
               tempValue=rbs[i].value;               
            }
        }
        
        if(objValue!="")
        {
            if(tempValue==selectUser)
            {
                alert('温馨提示：您没有做出更改！请选择！');
                return false;
            }
            else
            {
                $.ajax({
                    type:"post",
                    url:"/handler/doMission.ashx",
                    data:"mid="+objValue+"&tp=update&uid="+tempValue,
                    success:function(msg){
                        if(msg=="1") //表示修改成功
                        {                       
                            top.location="/manage/MissionList.aspx?opt=off&page=1&isa=0&uid=-1&bd="+beginDate+"&ed="+endDate;
                        }
                        else
                        {
                            alert("温馨提示：操作失败！");
                        }
                    }            
                });
            }
        }
    }
    </script>
    <style type="text/css">
    body{
        margin:0px;
        padding:0px;
        font-size:14px;
    }
    #div_top
    {       
        text-align:left;
        margin-left:10px;
        margin-right:20px;  
        margin-top:5px;
                
    }
    #div_bottom
    {
        text-align:center;
    }
    #div_desc
    {
        text-align:left;
        margin-left:10px;
        margin-right:20px;
        margin-top:5px;
        height:auto;   
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
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-left:10px;"><span style="font-weight:bold;">任务详细：</span></div>
    <div id="div_desc" runat="server"></div>
    <div id="div_top">
        <p><span style="font-weight:bold;">任务人：</span><br /><asp:RadioButtonList ID="radio_UserList" runat="server" RepeatColumns="6" RepeatDirection="Horizontal" RepeatLayout="Flow">
        </asp:RadioButtonList></p>
        </div>
        
        <div id="div_bottom">
            <input id="hidMissionID" type="hidden" runat="server" />&nbsp;<input class="btn" runat="server" type="button" id="btn_Set" value="设置用户" onclick="updateState();" />
            <input id="hid_bDate" style="width: 47px" type="hidden" runat="server" />
            <input id="hid_eDate" style="width: 47px" type="hidden" runat="server" />
            <input id="hidSelectUser" style="width: 57px" type="hidden" runat="server" /></div>
    </form>
</body>
</html>
