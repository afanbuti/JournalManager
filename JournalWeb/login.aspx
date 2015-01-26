<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="JournalWeb.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>后台登陆地址</title>
    <script src="/inc/js/jquery.js" type="text/javascript"></script>
    <style type="text/css">
   body { font-size: 12px;background-color:#E1E1E1 }
    td { font-size: 12px }
	th { font-size: 12px }
	
    </style>
    <script type="text/javascript">
    $(document).ready(
        function()
        {
            $("#txtUserName").focus();
        }
    );
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <TABLE id="Table1" style="WIDTH: 600px; HEIGHT: 116px" cellSpacing="0" cellPadding="0"
					width="272" align="center" border="0">
					<TR>
						<TD style="WIDTH: 252px" align="center" colSpan="2"><IMG height="126" src="inc/images/Admin_Login1.gif" width="600"></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 535px" align="center" background="inc/images/Admin_Login2.gif">
							<TABLE height="95" cellSpacing="0" cellPadding="0" width="497" border="0">
								<TR>
									<td style="WIDTH: 80px" colSpan="4"></td>
								</TR>
								<tr>
									<td style="WIDTH: 80px; HEIGHT: 52px" width="83" rowSpan="2">&nbsp;</td>
									<td style="WIDTH: 181px" width="181">&nbsp;</td>
									<td style="WIDTH: 41px; HEIGHT: 52px" width="41" rowSpan="2">&nbsp;</td>
									<td width="192">&nbsp;</td>
								</tr>
								<tr>
									<td class="" style="FONT-SIZE: 12px">用户名:<span style="WIDTH: 116px; HEIGHT: 23px">
											<asp:textbox id="txtUserName" runat="server" Width="112px"  onpaste="return false;"></asp:textbox></span><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtUserName"></asp:requiredfieldvalidator></td>
									<TD style="FONT-SIZE: 12px">密码:<span style="WIDTH: 133px; HEIGHT: 26px">
											<asp:textbox id="txtPwd" runat="server" Width="128px" TextMode="Password" 
												onpaste="return false;"></asp:textbox></span><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtPwd"></asp:requiredfieldvalidator></TD>
								</tr>
								<TR>
									<TD colSpan="4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="errMessage" runat="server" Font-Size="16px"></asp:label></TD>
								</TR>
							</TABLE>
						</TD>
						<TD width="65"><asp:imagebutton id="imgBtn_Login" runat="server" ImageUrl="inc/images/Admin_Login3.gif" OnClick="imgBtn_Login_Click"></asp:imagebutton></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 252px" align="center" colSpan="2">
							<P>&nbsp;</P>
						</TD>
					</TR>
				</TABLE>
    </div>
    </form>
</body>
</html>
