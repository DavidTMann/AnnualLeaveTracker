<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="AnnualLeaveTrack.Templates.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="~/assets/css/bootstrap.min.css" runat="server"/>
    <script src="~/assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="~/assets/js/bootstrap.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">

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
                    <li class="active">
                        <a href="HomePage.aspx">Home</a>
                    </li>
                    <li id="ViewProj" runat="server" Visible="True">
                            <a href="ViewByProj.aspx">View by Project<span></span></a>
                    </li>
                    <li id="ViewMemb" runat="server" Visible="True">
                            <a href="ViewByMemb.aspx">View by Member<span></span></a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

        <h3 style="top:20%;left:30%;position:fixed;color:#8B0000;">Welcome to CGI's Annual Leave Tracker</h3>
  </form>
</body>
</html>
