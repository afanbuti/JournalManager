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
    .hover{background-color:#E5EEF7;cursor:pointer}  /*#FFD900��������꾭��ʱ����ɫ*/       
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
        width:10%; /*�趨���*/
        height:100%;
        text-align:left; /*���������*/
        float:left; /*��������*/
        clear:left; /*�����������ڸ���*/
        /*overflow:hidden;������Ȳ�������*/
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
        float: left; /*��������*/
        clear: right; /*�������Ҳ���ڸ���*/
        /*overflow: hidden;������Ȳ�������*/
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
      
      //���ݱ����л�ɫ
    function rowColor()
    {
        $("#tbl_list tr").hover( 
        function(){ 
            $(this).addClass("hover");      //��꾭�����hover��ʽ 
        }, 
        function(){ 
            $(this).removeClass("hover");   //����뿪�Ƴ�hover��ʽ 
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
                obj2.value='ȡ��';
                document.title="���--["+drop2.options[index].text+"]--������";
                $("#txt_desc").focus();
                document.getElementById("div_MissPerson").innerHTML= drop2.options[index].text;
                drop1.disabled =true;
                
            }
    } 
      
      //��ʾ�������
      function ShowAddText()
      {
        var obj=document.getElementById("div_addMission");
        var obj2=document.getElementById("btn_add");
        var drop1=document.getElementById("drop_Status");
        var drop2=document.getElementById("drop_UserList");
        var   index   =   drop2.selectedIndex; 
//        if(drop2.options[index].value=="-1"  )
//        {
//             alert("��ܰ��ʾ���������ʱ������ѡ��ȷ����һ����Ա��");
//        }
//        else
//        {
            if(obj.style.display=='none')
            {
                obj.style.display='block';
                obj2.value='ȡ��';
                document.title="���--["+drop2.options[index].text+"]--������";
                $("#txt_desc").focus();
                document.getElementById("div_MissPerson").innerHTML= drop2.options[index].text;
                drop1.disabled =true;
                
            }
            else if(obj.style.display=='block')
            {
                obj.style.display='none';            
                obj2.value='���';  
                $("#txt_desc").val(''); 
                document.title="["+drop2.options[index].text+"]--�������б�";
                document.getElementById("div_MissPerson").innerHTML=""; 
                drop1.disabled =false;          
            }    
        //}
      }
      
      //��ѯȡ�����
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
            obj2.value='���';  
            $("#txt_desc").val('');             
            document.getElementById("div_MissPerson").innerHTML=""; 
            drop1.disabled =false;          
        }    
        document.title="["+drop2.options[index].text+"]--�������б�";   
      }
      //�˶��Ƿ���д����
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
        
        var type_value=drop_type.options[index_type].value;//���ͱ��
        var type_text=drop_type.options[index_type].text;//�����ı�
        
        if(trim(tempDesc).length<=0)
        {
            alert("��ܰ��ʾ��������Ҫ�����--["+tempPerson+"]--���������ݣ�");
            $("#txt_desc").focus();
            return false;
        }   
        if(trim(tempWorkHour).length<=0)
        {
            alert("��ܰ��ʾ��������Ҫ�����--["+tempPerson+"]--������ʱ��");
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
        //if(confirm("ȷ��Ҫ���--["+uName+"]--������ô��"))
        //{          
           $.ajax({
            type:"post",
            url:"/handler/doMission.ashx",
            data:"uid="+uid+"&desc="+escape(udesc)+"&workHour="+escape(uWorkHour)+"&tp=add&tpValue="+escape(typeValue)+"&path="+escape(txtpath),
            success:function(msg){
                if(msg=="1")
                {
                    //if(confirm("��ܰ��ʾ��--["+uName+"]--��������ӳɹ�!\n\n���ȷ�Ͻ��ر���ӶԻ���!"))
                    //{
                        ShowAddText();
                    //}
                    location="?opt=on&page=1&uid="+uid+"&to="+escape(typeValue)+"&bd="+beginDate+"&ed="+endDate+"&isa="+isa;
                }   
                else
                {
                    alert("��ܰ��ʾ��--["+uName+"]--���������ʧ��!");
                }      
            }
           });
       //}     
    }
    
    function deldata(mid,uid,path)
    {        
        if(confirm("��ܰ��ʾ��ȷ��Ҫɾ����������ô��"))
        {
            $.ajax({
                type:"post",
                url:"/handler/doMission.ashx",
                data:"mid="+mid+"&tp=del&uid="+uid+"&path="+escape(path),
                success:function(msg){
                    if(msg=="1") //��ʾɾ���ɹ�
                    {
                        //alert("��ܰ��ʾ���������Ѿ�ɾ���ɹ���\n\n�����޷���أ�ֻ����д��лл������"); 
                        var drop_type=document.getElementById("drop_type");       
                        var index_type=drop_type.selectedIndex;
                        var type_value=drop_type.options[index_type].value;//���ͱ��
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
                        alert("��ܰ��ʾ������ɾ��ʧ�ܣ�");
                    }
                }            
            });
        }
        else
        {
            return false;
        }
    }
    
    //ѡ������
    function selectDate()
    {
        var obj=document.getElementById("beginTime");        
        HS_setDate(obj);
    }
    //ѡ������
    function selectDate1()
    {
        var obj1=document.getElementById("endTime");        
        HS_setDate(obj1);
    }
    //�ı�ֵ
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
    
    //����div�� 
    function showDiv(mid)
    {
        var bDate=$("#beginTime").val();
        var eDate=$("#endTime").val();
        $("#panelWindow").css("width","500");
        document.getElementById("div_WinTitle").innerHTML="�û��б�";
        document.getElementById("div_content").innerHTML="<iframe src='userList.aspx?mid="+mid+"&bd="+bDate+"&ed="+eDate+"' frameborder='0' width='100%' height='280'/>";
        $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				   			
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
    }
    //��ʾ����
    function showMission(mid,cbh)
    {
        $("#panelWindow").css("width","500");
        document.getElementById("div_WinTitle").innerHTML="�޸�����";
        document.getElementById("div_content").innerHTML="<iframe src='showMission.aspx?mid="+mid+"&bh="+cbh+"' frameborder='0' width='100%' height='300'/>";
        $("#panelWindow").jWindowOpen({
				    modal:true,
				    center:true,				    			
				    close:"#panelWindow .close",
				    closeHoverClass:"hover"
			    });
    }
    
    //enter�ύ
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
            alert("��ܰ��ʾ�������������Ѿ���ɵ�����");
        }
        
        function setTxt(objValue) 
        { 
            var t=document.getElementById(objValue);         
            if(!window.clipboardData.setData('text',t.innerText))
            {
                alert("��ܰ��ʾ��ȷʵ������ҳ���ʼ��а�\n\nˢ��ҳ�棬�㸴��ʱ����");
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
                    <a href="JournalList.aspx" target="_self" class="top_a">�ܽ�鿴</a>&nbsp;&nbsp;
                <a href="userManager.aspx" target="_self"  class="top_a">�û�����</a>&nbsp;&nbsp;
                <a href="MissionList.aspx" target="_self"  class="top_b">�������</a>&nbsp;&nbsp;
                 <a href="/filemanage/default.aspx" target="_self"  class="top_a">�ĵ�����</a>&nbsp;&nbsp;
                 <a href="systemManage.aspx" target="_self"  class="top_a">���չ���</a>&nbsp;&nbsp;
                <a href="../loginout.aspx" target="_self"  class="top_a">�˳�����</a>    
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
                                    �����ˣ�<asp:DropDownList runat="server" ID="drop_UserList" onchange="chageValue();" Width="68px">
                                    </asp:DropDownList>����<asp:DropDownList runat="server" ID="drop_type">
                                    <asp:ListItem Value="-1">����</asp:ListItem>
                                        <asp:ListItem Value="0">ά��</asp:ListItem>
                                        <asp:ListItem Value="1">����</asp:ListItem>
                                        <asp:ListItem Value="2">�������</asp:ListItem>
                                         <asp:ListItem Value="3">ϵͳ���</asp:ListItem>
                                    </asp:DropDownList>�ѷ��䣺<asp:CheckBox runat="server" ID="chk_IsAsigin" />
                                    <span>��<input style="width: 68px; height: 17px" id="beginTime" readonly onfocus="HS_setDate(this);"
                                            type="text" name="beginTime" runat="server" /></span>��<input style="width: 66px; height: 17px" id="endTime" readonly onfocus="HS_setDate(this);"
                                            type="text" name="endTime" runat="server" />״̬��<asp:DropDownList runat="server" ID="drop_Status" Width="62px">
                                        <asp:ListItem Value="-1">-ȫ��-</asp:ListItem>
                                        <asp:ListItem Value="0">��ִ��</asp:ListItem>
                                        <asp:ListItem Value="1">ִ����</asp:ListItem>
                                        <asp:ListItem Value="2">�Ѿ����</asp:ListItem>
                                    </asp:DropDownList>���ݣ�<input type="text" id="txt_content" value="" style="width: 80px" runat="server" />
                                    <input runat="server" id="btn_Search" type="button" value="��ѯ" class="btn" onclick="checkAdd();" onserverclick="btn_Search_ServerClick" />&nbsp;<input runat="server" id="btn_add" type="button" value="���" class="btn"
                                        onclick="ShowAddText();" />&nbsp;<input runat="server" id="btn_Export" type="button" value="����" class="btn"
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
                                        �������&gt;&gt;&nbsp;�����ˣ�&nbsp;<span id="div_MissPerson" style="color:Red;"></span></div>
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
                                                            &nbsp; &nbsp;�������ͣ�<asp:DropDownList runat="server" ID="drop_typeList" Width="110px"><asp:ListItem Text="ά��" Value="0"></asp:ListItem><asp:ListItem Text="����" Value="1"></asp:ListItem>
                                                    <asp:ListItem Value="2">�������</asp:ListItem>
                                                    <asp:ListItem Value="3">ϵͳ���</asp:ListItem>
                                                </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 30px">
                                                           &nbsp; &nbsp;�ƻ���ʱ��<asp:TextBox  id="Text_workHour"  runat="server" style="width: 103px" ToolTip="ֻ�������������� 20"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 30px">
                                                            &nbsp; &nbsp;<input type="button" value="��Ӹ���" onclick="openUpload();" id="btn_addDoc" class="btn" style="width: 56px"/>
                                                            <input type="text" id="txt_Path" readonly runat="server" style="width: 110px" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 50px">
                                                            &nbsp; &nbsp;<input class="btn" runat="server"
                                                type="button" id="btn_addMission" value="�������" onclick="return checkForm();" style="width: 179px; height: 29px" /></td>
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
            <div style="width:100%;text-align:center;margin-bottom:5px;display:none;">ע�����&nbsp;<img src="../inc/images/red.gif" border='0'/>[��ִ��]&nbsp;�Ƶ�&nbsp;<img src="../inc/images/yellow.gif" border='0'/>[ִ����]&nbsp;�̵�&nbsp;<img src="../inc/images/green.gif" border='0'/>[�Ѿ����]</div>
         </div>   </div>
           
        <input id="hid_uId" runat="server" style="width: 68px" type="hidden" /><input id="hid_IsAsign" runat="server" style="width: 68px" type="hidden" />
         <div class="window " id="panelWindow"  onmouseover="">
            <div class="title">
                <span id="div_WinTitle">�û��б�</span><span class="buttons"><span class="close"></span>&nbsp;&nbsp;</span></div>
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
