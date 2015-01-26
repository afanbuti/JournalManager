<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="JournalWeb.filemanage._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>文件管理页面</title>
    <script src="../inc/js/datePicker.js" type="text/javascript"></script>
    <script src="../inc/js/jquery.js" type="text/javascript"></script>
    <script src="../inc/js/jquery.interface.js" type="text/javascript"></script>
    <script src="../inc/js/jquery.jwindow.js" type="text/javascript"></script>
    
    <link type="text/css" href="../inc/css/jwindowFile.css" rel="Stylesheet" />
    
    <style type="text/css">
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
     #div_top{width:100%;height:100%;text-align:center;padding-top:20px}
    body{font-size:14px;margin:0px;padding:0px;background-color:#EEEEFF;}     
      /*#div_top{width:100%;background-color:#E5EEF7;height:100%;text-align:center;padding-top:20px}background-color:#EEEEFF;*/
      #tbl_list{width:870px;border-collapse:collapse;border:1px solid #EEE;font-size:14px;text-align:center}
      #tbl_list th{background:#EEE;border:1px solid #CCC;padding:4px;cursor:pointer;}
      #tbl_list td{border:1px solid #EEE;padding:4px;cursor:pointer;}
      
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
    //显示文档添加页面
    function showDocPage()
    {           
        document.getElementById("div_content").innerHTML="<iframe src='AddDoc.aspx' frameborder='0' width='100%' height='420'/>";
        $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				    			
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
    }
    
    //删除数据
    function del(id,uid,path)
    {   
        if(confirm("确认要删除这个文件信息么？"))
        {          
           $.ajax({
            type:"post",
            url:"/handler/delpage.ashx",
            data:"id="+id+"&tp=delfile&uid="+uid+"&path="+path,
            success:function(msg){
                if(msg=="1")
                {
                    alert("温馨提示：删除数据成功!");
                    location.href=location;                    
                }   
                else
                {
                    alert("温馨提示：删除数据失败!");
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
                <span runat="server" id="span_Manager"></span>  
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
                            <td colspan="5">
                                <span>文档类别：<asp:DropDownList runat="server" ID="drop_DocType">
                                </asp:DropDownList></span>
                                <span>从
                                    <input style="width: 80px; height: 17px" id="beginTime" readonly onfocus="HS_setDate(this);"
                                        type="text" name="beginTime" runat="server" />
                                </span>&nbsp;
                                <span>到
                                    <input style="width: 80px; height: 17px" id="endTime" readonly onfocus="HS_setDate(this);"
                                        type="text" name="endTime" runat="server" />
                                </span>&nbsp;
                                <span>标题&nbsp;<input runat="server" id="txt_KeyWord"  /></span>
                                <asp:Button ID="btn_Search" runat="server" Text="搜 索" CssClass="btn" OnClick="btn_Search_Click" />
                                &nbsp;<input type="button" value="发文档" id="btn_ReleaseDoc" class="btn" onclick="showDocPage();"/></td>
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
                    <tr>
                        <th width="100">操作</th>
                        <th width="490">标题</th>
                        <th width="80">类别</th>
                        <th width="100">提交人</th>
                        <th width="110">提交日期</th>
                    </tr>
                    <tr><td colspan="5"><div runat="server" id="div_noInfo">温馨提示：没有文档信息</div></td></tr>
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
                        <td id="tbl_td_page" runat="server">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="window " id="panelWindow">
            <div class="title">
                <span id="div_WinTitle">文档上传</span><span class="buttons"><span class="close"></span>&nbsp;&nbsp;</span></div>
            <div class="content" id="div_content">
            </div>
            <div class="status">
                <span class="resize"></span>
            </div>
        </div>
        </div>
    </div>
    </form>
</body>
</html>
