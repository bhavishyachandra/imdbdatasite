<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="db2project.home" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="Content/bootstrap.min.css" />

    <!-- Optional theme -->
    <%--<link rel="stylesheet" href="~/Content/bootstrap-theme.min.css" />--%>

    <!-- jQuery library -->
    <script src="Scripts/jquery-2.1.3.min.js" type="text/javascript"></script>

    <!-- Latest compiled JavaScript -->
    <script src="Scripts/bootstrap.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="Content/themes/base/all.css" />
    <script src="Scripts/jquery-ui-1.11.4.min.js" type="text/javascript"></script>
    	


         <script type="text/javascript">
             $(function () {
                 var availableTags = [
                 "Documentary",
                 "Horror",
                 "Reality-TV",
                 "Short",
                 "Comedy",
                 "Drama",
                 "Talk-Show",
                 "Mystery",
                 "Sport",
                 "News",
                 "Sci-Fi",
                 "Romance",
                 "Family",
                 "Biography",
                 "Music",
                 "Game-Show",
                 "Adventure",
                 "Crime",
                 "War",
                 "Musical",
                 "Animation",
                 "Fantasy",
                 "Thriller",
                 "Action",
                 "History",
                 "Western",
                 "Adult",
                 "Lifestyle",
                 "Film-Noir",
                 "Experimental",
                 "Commercial",
                 "Erotica"
                 ];

                 $("#<%= autocomplete1.ClientID %>").autocomplete({
                     source: availableTags
                 });
             });
  </script>  




    <title></title>

<%--    <style type="text/css">
   body { background: #DFEFF7; }
</style>--%>

</head>
<body>

    <form id="form1" runat="server">

        <div>
            <div class="container">
                <%--                <div class="row clearfix">
                    <div class="col-md-12 column">--%>
                        <ol style="visibility:hidden;"><li>&nbsp</li><li>&nbsp</li><li>&nbsp</li></ol>
                        <img src="Images/imdb_thumbnail.jpg" class="img-circle" />
                        <div class="page-header">
                            
                                <div class="container">
                                <h1>IMDB Using MongoDB
                                </h1>
                                </div>
                            </div>
                        
                        <nav class="navbar navbar-default navbar-fixed-top" role="navigation">
                            <div class="navbar-header">
                                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1"><span class="sr-only">Toggle navigation</span><span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></button>
                            </div>

                            
                            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                                <ul class="nav navbar-nav">
                                    <li class="active">
                                        <a href="http://www.imdb.com/">IMDB</a>
                                    </li>
                                    <li>
                                        <a href="http://www.mongodb.org/">MongoDB</a>
                                    </li>

                                </ul>

                                
                            </div>

                        </nav>
                        <asp:Button ID="TopRatedMovies" runat="server" Text="Top Rated Movies" OnClick="TopRatedMovies_Click" class="btn btn-info" />
                        <asp:Button ID="Button_MoviesPerYear" runat="server" Text="Movies Per Year" CssClass="btn btn-info" OnClick="Button_MoviesPerYear_Click" />
                        <br />
                        <br />
                        <p class="text-primary">
                            Movies between
            <asp:TextBox ID="TextBox_From" ValidationGroup="ValidateMe" runat="server"></asp:TextBox>
                            &nbsp;and
            <asp:TextBox ID="TextBox_To" ValidationGroup="ValidateMe" runat="server"></asp:TextBox>

                            &nbsp;<asp:Button ID="Button_MoviesInRange" class="btn-info" ValidationGroup="ValidateMe" runat="server" Text="Find" OnClick="Button_MoviesInRange_Click" />
                        </p>
                <p class="text-primary">
                            Movies in year
                            <asp:TextBox ID="TextBox_genre" runat="server" OnTextChanged="TextBox_genre_TextChanged"></asp:TextBox>
&nbsp;in each genre
                            <asp:Button ID="Button_genre_mapreduce" class="btn-info" runat="server" Text="Find" ValidationGroup="ValidateMe2" OnClick="Button_genre_mapreduce_Click" />
                        </p>
                <p class="text-primary">Select a genre to see its trend in last 30 years <asp:TextBox id="autocomplete1" runat="server" placeholder="Genre name"/>&nbsp;
                    <asp:Button ID="Button_genre_selection" runat="server" class="btn-info" OnClick="Button_genre_selection_Click" Text="Find" />
                </p>
                <p class="text-primary">Enter actor name
                    <asp:TextBox ID="TextBox_actor_search" runat="server"></asp:TextBox>
&nbsp;<asp:Button ID="Button_actor_search" runat="server" Text="Search" class="btn-info" OnClick="Button_actor_search_Click" />
                </p>
                <p class="text-primary">Enter movie name
                    <asp:TextBox ID="TextBox_movies_search" runat="server"></asp:TextBox>
&nbsp;<asp:Button ID="Button_movies_search" runat="server" Text="Find" CssClass="btn-info" OnClick="Button_movies_search_Click" />
                </p>
                    
		
	                

                        




                        <asp:Label ID="Label_Result" runat="server" Text=""></asp:Label>
                        <br />
                <asp:Label ID="Label_time" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:CustomValidator ID="CustomValidator1" Display="Dynamic" ValidationGroup="ValidateMe" runat="server" ControlToValidate="TextBox_From" ValidationExpression="[0-9]{4}" ErrorMessage="Enter years properly" ForeColor="Red"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator2" Display="Dynamic" ValidationGroup="ValidateMe" runat="server" ControlToValidate="TextBox_To" ValidationExpression="[0-9]{4}" ErrorMessage="Enter years properly" ForeColor="Red"></asp:CustomValidator>
                        <asp:CompareValidator ID="CompareValidator1" Display="Dynamic" runat="server" ControlToCompare="TextBox_To" ControlToValidate="TextBox_From" ErrorMessage="Enter years properly" Operator="LessThanEqual" Type="Integer" ValidationGroup="ValidateMe" ForeColor="Red"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Enter years properly" ForeColor="Red" ValidationGroup="ValidateMe" ControlToValidate="TextBox_From"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="Enter years properly" ForeColor="Red" ValidationGroup="ValidateMe" ControlToValidate="TextBox_To"></asp:RequiredFieldValidator>
                        
                        <asp:CustomValidator ID="CustomValidator3" Display="Dynamic" ValidationGroup="ValidateMe2" runat="server" ControlToValidate="TextBox_genre" ValidationExpression="[0-9]{4}" ErrorMessage="Enter years properly" ForeColor="Red"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="Enter years properly" ForeColor="Red" ValidationGroup="ValidateMe2" ControlToValidate="TextBox_genre"></asp:RequiredFieldValidator>
                        
                        <br />
                <asp:Chart ID="Chart2" runat="server" Height="600px" Width="1000px">
                    <Series>
                        <asp:Series ChartType="Line" Name="Series2" YValuesPerPoint="2">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea2">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                        
                        <br />
                <asp:Chart ID="Chart1"  runat="server" Height="600px" OnLoad="Chart1_Load" Width="600px">
                    <Series>
                        <asp:Series Name="Series1"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
                        &nbsp;<asp:GridView ID="ResultGridView" runat="server" class="table table-hover table-bordered table-striped" OnSelectedIndexChanged="ResultGridView_SelectedIndexChanged">
                        </asp:GridView>
                    </div>
                </div>
        <%--</div>

        </div>--%>
        <div>
            <footer class="footer">

                <div class="navbar-fixed-bottom">
                    <a class="navbar-brand navbar-right" rel="home" href="http://www.mongodb.org" title="MongoDB" target="_blank">
                        <img style="max-width: 100px; margin-top: -7px;" src="Images/poweredbymongodb.png" />
                    </a>
                </div>



            </footer>
        </div>



    </form>

</body>
</html>
