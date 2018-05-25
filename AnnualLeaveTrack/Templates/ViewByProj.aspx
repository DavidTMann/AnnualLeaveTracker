<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewByProj.aspx.cs" Inherits="AnnualLeaveTrack.Templates.ViewByProj" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Leave by Project</title>
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="~/assets/css/bootstrap.min.css" runat="server"/>
    <script src="~/assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="~/assets/js/bootstrap.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <nav class="navbar navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>                        
                    </button>
                    <a class="navbar-brand" href="HomePage.aspx">Annual Leave Tracker</a>
                </div>
                <div class="collapse navbar-collapse" id="myNavbar">
                    <ul class="nav navbar-nav">
                        <li>
                            <a href="HomePage.aspx">Home</a>
                        </li>
                        <li id="ViewProj" runat="server" Visible="True" class="active">
                                <a href="ViewByProj.aspx">View by Project<span></span></a>
                        </li>
                        <li id="ViewMemb" runat="server" Visible="True">
                                <a href="ViewByMemb.aspx">View by Member<span></span></a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
        <div class="input-group" style="padding-left:50px;">
            <asp:DropDownList ID="buDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="buDropDown_SelectedIndexChanged">
            </asp:DropDownList>
            <div style="text-align:right; padding-top:20px;">
                <asp:Button ID="selectBuBtn" OnClick="selectBuBtn_Click" CssClass="btn btn-primary" runat="server" Text="Select Business Unit" ToolTip="Select a specific business unit to show associated projects." />
            </div>
        </div>
        <div class="input-group" style="padding-left:50px;padding-top:20px;">
            <asp:DropDownList ID="projDropDown" runat="server" OnSelectedIndexChanged="projDropDown_SelectedIndexChanged">
            </asp:DropDownList>
            <div style="text-align:right; padding-top:20px;">
                <asp:Button ID="selectProjBtn" OnClick="selectProjBtn_Click" CssClass="btn btn-primary" runat="server" Text="Select Project" ToolTip="Select a project from a specific business unit." />
            </div>
        </div>

        <div>
            <div style="padding-top:20px; padding-left:20px;">
                <asp:Calendar ID="projCalendar" OnDayRender="projCalendar_DayRender" OnVisibleMonthChanged="projCalendar_VisibleMonthChanged" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" NextPrevFormat="FullMonth" SelectionMode="DayWeekMonth" Height="500px" Width="1200px">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
            </div>
            
            <div style="padding-top:20px;padding-left:20px;width:800px">
                <asp:GridView ID="projGrid" RowStyle-Wrap="true" AutoGenerateColumns="false" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" >
                    <Columns>
                        <asp:BoundField HeaderText="Full name"  DataField="Full name" />
                        <asp:BoundField HeaderText="Leave dates"  DataField="Leave dates" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Days booked"  DataField="Days booked" />
                        <asp:BoundField HeaderText="Days left"  DataField="Days left" />
                    </Columns>
                    <AlternatingRowStyle BackColor="White" />
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
            </div>
        </div>
        <div style="float:right;padding-top:20px;">
            <div style="position:relative;bottom:150px;padding-right:100px;">
                <asp:Button ID="getConflictBtn" OnClick="getConflictBtn_Click" Visible="false" CssClass="btn btn-danger" runat="server" Text="Identify Date Conflicts" ToolTip="Click this button to see if there are any conflicting leave dates for members of this project." />
            </div>
            <div style="position:relative;float:right;bottom:100px;padding-right:100px;">
                <asp:GridView ID="conflictGrid" RowStyle-Wrap="true" AutoGenerateColumns="False" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" >
                    <Columns>
                        <asp:BoundField HeaderText="Members"  DataField="Members" />
                        <asp:BoundField HeaderText="Conflicting Dates"  DataField="Conflicting Dates" HtmlEncode="false" />
                    </Columns>
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
