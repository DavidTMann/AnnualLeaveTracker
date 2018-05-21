<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewByMemb.aspx.cs" Inherits="AnnualLeaveTrack.Templates.ViewByMemb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Leave by Member</title>
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="~/assets/css/bootstrap.min.css" runat="server"/>
    <script src="~/assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="~/assets/js/bootstrap.min.js" type="text/javascript"></script>
    <style>
        .halfday {
            height: 100%;
            background: linear-gradient(90deg, #2fa912 50%, #ebe7e7 50%);
        }
    </style>
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
                        <li id="ViewProj" runat="server" Visible="True">
                                <a href="ViewByProj.aspx">View by Project<span></span></a>
                        </li>
                        <li id="ViewMemb" runat="server" Visible="True" class="active">
                                <a href="ViewByMemb.aspx">View by Member<span></span></a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
        <div>
            <div class="input-group" style="left:50px;">
                <asp:Label ID="enterUsrLbl" runat="server" Text="Enter user ID "></asp:Label>
                <asp:TextBox ID="userSearchTxtBox" runat="server"></asp:TextBox>
            
                <div style="text-align:right; padding-top:20px;">
                    <asp:Button CssClass="btn btn-primary" runat="server" Text="Search" ID="userSearchBtn" OnClick="userSearchBtn_Click"/>
                </div>
            </div>

            <div style="padding-top:20px; padding-left:20px;">
                <asp:Calendar ID="userCalendar" OnDayRender="userCalendar_DayRender" OnVisibleMonthChanged="userCalendar_VisibleMonthChanged" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" NextPrevFormat="FullMonth" SelectionMode="DayWeekMonth" Height="395px" Width="1017px">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
            </div>
            
            <div style="padding-top:20px;padding-left:20px;width:800px">
                <asp:GridView ID="userGrid" RowStyle-Wrap="true" AutoGenerateColumns="false" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" >
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

    </form>
</body>
</html>
