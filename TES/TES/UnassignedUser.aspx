<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnassignedUser.aspx.cs" Inherits="TES.Test1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script type="text/javascript">


</script>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>
    <div style="text-align:center;">
    <h1 runat="server" class="text" id="UserName"></h1>
        <form runat="server">
            <asp:Button runat="server" ID="logoutButton" Text="Logout" OnClick="LogoutButton_Click" />
            <div id="content" runat="server">
                <h2>Apply for a Role:</h2>
                <input type="radio" name="role" value="Student" />Student<br />
                <input type="radio" name="role" value="Teacher" />Teacher<br />
                <input type="radio" name="role" value="Admin" />Admin<br />
                <asp:Button runat="server" ID="submitButton" Text="Submit" OnClick="Submit_ServerClick" />
            </div>
            <asp:Button runat="server" ID="PieButton" Text="Preview Pie Chart" OnClick="PieButton_Click" />
        </form>
    </div>
</body>
</html>
