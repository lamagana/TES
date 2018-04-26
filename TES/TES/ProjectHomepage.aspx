<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectHomepage.aspx.cs" Inherits="TES.ProjectHomepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TES</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="text-align: center;">
                <h1 runat="server" class="text" id="UserName"></h1>
                <asp:Button runat="server" ID="logoutButton" Text="Logout" OnClick="LogoutButton_Click" />
            </div>

            <div>
                <h1 id="ProjectLabel" runat="server"></h1>


            </div>

        </div>
    </form>
</body>
</html>
