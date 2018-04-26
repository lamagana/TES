<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentHomepage.aspx.cs" Inherits="TES.StudentHomepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TES</title>
</head>
<body>
    <form id="form1" runat="server">

        <div style="text-align: center;">
            <h1 runat="server" class="text" id="UserName"></h1>
            <asp:Button runat="server" ID="logoutButton" Text="Logout" OnClick="LogoutButton_Click" />
        </div>

        <br />

        <asp:ScriptManager runat="server" ID="sm">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div style="width: 800px; margin: 0 auto;">

                    <!-- Time Entry -->
                    <div style="width: 400px; height: 600px; float: left;">
                        <h3>Time Entry</h3>
                        <asp:Label runat="server" Text="Select a project: " /><asp:DropDownList ID="ProjectListDropDown" runat="server" OnSelectedIndexChanged="ProjectListDropDown_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="- - - - - - - -" Value="-1" />
                        </asp:DropDownList>

                        <br />
                        <br />

                        <span style="display: inline-block">
                            <asp:Button ID="PunchIn" runat="server" Text="Punch In" OnClick="PunchIn_Click" Height="30px" Width="80px" />
                            <asp:Button ID="PunchOut" runat="server" Text="PunchOut" OnClick="PunchOut_Click" Height="30px" Width="80px" />
                            <br />
                            <br />
                        </span>

                        <div style="max-height: 300px; width: 400px; overflow-y: scroll; scrollbar-face-color: darkgray; border: 2px; border-color: black;">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="TimeSheetId" DataSourceID="SqlDataSource1">
                                <Columns>
                                    <asp:BoundField DataField="TimeSheetId" HeaderText="TimeSheetId" InsertVisible="False" ReadOnly="True" SortExpression="TimeSheetId" Visible="false" />
                                    <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="UserId" Visible="false" />
                                    <asp:BoundField DataField="ProjectId" HeaderText="ProjectId" SortExpression="ProjectId" Visible="false" />
                                    <asp:BoundField DataField="GroupId" HeaderText="GroupId" SortExpression="GroupId" Visible="false" />
                                    <asp:TemplateField HeaderText="Start Time">
                                        <ItemTemplate>
                                            <asp:TextBox ID="StartTime" runat="server" Text='<%# Bind("StartTime") %>' OnTextChanged="txtStartTime_TextChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stop Time">
                                        <ItemTemplate>
                                            <asp:TextBox ID="StopTime" runat="server" Text='<%# Bind("StopTime") %>' OnTextChanged="txtStopTime_TextChanged" AutoPostBack="true" />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:Button ID="btnAdd" runat="server" Text="Add Row" OnClick="btnAddRow_Click" />
                        <div id="TimeAlert" runat="server"></div>

                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:tesProjectDatabaseConnectionString %>" SelectCommand="" InsertCommand="" UpdateCommand="UPDATE [TimeSheet] SET [StartTime] = @StartTime, [StopTime] = @StopTime WHERE [TimeSheetId] = @TimeSheetId">

                            <InsertParameters>
                                <asp:Parameter Name="UserId" Type="Int32" />
                                <asp:Parameter Name="ProjectId" Type="Int32" />
                                <asp:Parameter Name="GroupId" Type="Int32" />
                                <asp:Parameter Name="StartTime" Type="DateTime" />
                                <asp:Parameter Name="StopTime" Type="DateTime" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="StartTime" Type="DateTime" />
                                <asp:Parameter Name="StopTime" Type="DateTime" />
                                <asp:Parameter Name="TimeSheetId" Type="Int32" />
                            </UpdateParameters>
                        </asp:SqlDataSource>
                    </div>

                    <!-- Project Hyperlinks -->
                    <div style="width: 400px; height: 600px; float: right;">

                        <h3>Current Projects</h3>

                        <asp:Table ID="ProjectHyperlinks" runat="server" GridLines="Both" CellPadding="3" BorderColor="Black" BorderWidth="2" HorizontalAlign="Center" Width="175" />

                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>




    </form>
</body>
</html>
