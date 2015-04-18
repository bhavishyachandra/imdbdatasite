<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="db2project.home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Latest compiled and minified CSS -->
<link rel="stylesheet" href="Content/bootstrap.min.css"/>
    <!-- Optional theme -->
<link rel="stylesheet" href="Content/lavish-bootstrap.min.css" type="text/css"/>

<!-- jQuery library -->
<script src="Scripts/jquery.min.js"></script>

<!-- Latest compiled JavaScript -->
<script src="~/Scripts/bootstrap.min.js"></script>
    <title></title>

</head>
<body>

    <form id="form1" runat="server">

    <div>
        <div class="container">
	<div class="row clearfix">
		<div class="col-md-12 column">
			&nbsp;<br />
            <br />
            <br />
            <br />
			<img src="Images/imdb_thumbnail.jpg" class="img-circle" /><div class="page-header">
				<h1>
					IMDB Using MongoDB
				</h1>
			</div>
			<nav class="navbar navbar-default navbar-fixed-top" role="navigation">
				<div class="navbar-header">
					 <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1"> <span class="sr-only">Toggle navigation</span><span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></button> 
				</div>
				
				<div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
					<ul class="nav navbar-nav">
						<li class="active">
							<a href="http://www.imdb.com/">IMDB</a>
						</li>
						<li>
							<a href="http://www.mongodb.org/">MongoDB</a>
						</li>
						<li class="dropdown">
							 <a href="#" class="dropdown-toggle" data-toggle="dropdown">Dropdown<strong class="caret"></strong></a>
							<ul class="dropdown-menu">
								<li>
									<a href="#">Action</a>
								</li>
								<li>
									<a href="#">Another action</a>
								</li>
								<li>
									<a href="#">Something else here</a>
								</li>
								<li class="divider">
								</li>
								<li>
									<a href="#">Separated link</a>
								</li>
								<li class="divider">
								</li>
								<li>
									<a href="#">One more separated link</a>
								</li>
							</ul>
						</li>
					</ul>

					<ul class="nav navbar-nav navbar-right">
						<li>
							<a href="#">Link</a>
						</li>
						<li class="dropdown">
							 <a href="#" class="dropdown-toggle" data-toggle="dropdown">Dropdown<strong class="caret"></strong></a>
							<ul class="dropdown-menu">
								<li>
									<a href="#">Action</a>
								</li>
								<li>
									<a href="#">Another action</a>
								</li>
								<li>
									<a href="#">Something else here</a>
								</li>
								<li class="divider">
								</li>
								<li>
									<a href="#">Separated link</a>
								</li>
							</ul>
						</li>
					</ul>
				</div>
				
			</nav> <asp:Button ID="MoviesInRange" runat="server" OnClick="MoviesInRange_Click" Text="Movies From 2013 to 2015" class="btn btn-info"/> <asp:Button ID="TopRatedMovies" runat="server" Text="Top Rated Movies" OnClick="TopRatedMovies_Click" class="btn btn-info"/> <asp:Button ID="MoviesOf2013" runat="server" OnClick="MoviesOf2013_Click" Text="Movies of 2013" class="btn btn-info"/>
			<asp:GridView ID="ResultGridView" runat="server" class="table table-hover table-bordered">
				
			</asp:GridView>
		</div>
	</div>
</div>
    
    </div>
    </form>

</body>
</html>
