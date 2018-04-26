<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminHomepage.aspx.cs" Inherits="TES.AdminHomepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <div style="text-align: center;">
            <h1 runat="server" class="text" id="UserName"></h1>
            <asp:Button runat="server" ID="logoutButton" Text="Logout" OnClick="LogoutButton_Click" />
        </div>

        <div runat="server" id="requests"></div>

        <asp:Table ID="Table1" runat="server" GridLines="Both" CellPadding="3" BorderColor="Black" BorderWidth="2" HorizontalAlign="Center" />

    </form>
</body>
</html>

