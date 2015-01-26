<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showEval.aspx.cs" Inherits="JournalWeb.manage.showEval" %>

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
        var tempContent=document.getElementById("txt_EvalDesc").value;//$("#txt_Desc").val();
        var tempBH=document.getElementById("hid_BH").value;
        var jid=document.getElementById("hid_jid").value;  
        tempContent=trim(tempContent);      
//        if(trim(tempContent).length<=0)
//        {
//            alert("温馨提示：请输入总结评价内容！");
//            $("#txt_EvalDesc").focus();
//            return false;
//        }  
        
        $.ajax({
            type:"post",
            url:"/handler/dosome.ashx",
            data:"jid="+jid+"&tp=editpj&content="+escape(tempContent),
            success:function(msg){
                if(msg!="0") //表示修改成功
                {            
                    var temp1="sp_tempPj"+tempBH;
                    var temp2="sp_pjtitle"+tempBH;
                    top.document.getElementById(temp1).innerHTML=repstr(msg);
                    top.document.getElementById(temp2).style.display="";
                    if(tempContent=="")
                    {
                        top.document.getElementById(temp2).style.display="none";
                    }
                    closeWin();
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
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="3" style="width: 207px">
            <textarea id="txt_EvalDesc" style="width: 360px; height: 153px" runat="server"></textarea></td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="padding-top:5px;">        
        <input id="btn_insert" type="button" class="btn" value="评价总结" onclick="return checkForm();" />&nbsp;&nbsp;<input class="btn" id="tbn_Cancel" type="button" value="算了，不评了！" onclick="closeWin();" />
                    <input id="hid_BH" style="width: 61px" type="hidden" runat="server" />
                    <input id="hid_jid" style="width: 61px" type="hidden" runat="server" /></td>
            </tr>           
        </table>
    </div>
    </form>
</body>
</html>
