using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;


using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI.DataVisualization.Charting;

namespace db2project
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Chart1.Visible = false;






        }



        protected void TopRatedMovies_Click(object sender, EventArgs e)
        {

            var watch = Stopwatch.StartNew();
            Label_time.Text = "Query Started....";

            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            var server = client.GetServer();

            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("ratings");
            //var query = Query.EQ("_id", findIDTextBoxText);

            //MongoCursor<BsonDocument> cursor = collection.Find(query);

            var sort1 = SortBy.Descending("votes").Descending("rank");

            MongoCursor<BsonDocument> cursor1 = collection.FindAll().SetSortOrder(sort1).SetLimit(50);

            Label_Result.Text = "";
            watch.Stop();
            Label_time.Text = "Query completed in " + (watch.ElapsedMilliseconds / 1000);

            cursor1.SetFields(Fields.Include("title", "rank", "votes"));
            //ResultTextBox.Text += cursor1.ToJson();
            DataTable dt = new DataTable();
            dt.Columns.Add("title");
            dt.Columns.Add("rank");
            dt.Columns.Add("votes");
            foreach (var i in cursor1)
            {
                dt.Rows.Add(i["title"], i["rank"], i["votes"]);
            }


            dt.DefaultView.Sort = "rank DESC";

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();

        }

        protected void MoviesOf2013_Click(object sender, EventArgs e)
        {

            var watch = Stopwatch.StartNew();
            Label_time.Text = "Query Started....";
            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            var server = client.GetServer();


            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies1");
            var query = Query.And(Query.EQ("year", 2013), Query.EQ("type", "(MOVIE)"));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            Double ResultCount = collection.Find(query).Count();
            cursor.SetFields(Fields.Include("title"));

            Label_Result.Text = "The number of records are " + Convert.ToInt32(ResultCount);
            watch.Stop();
            Label_time.Text = "Query completed in" + (watch.ElapsedMilliseconds / 1000);

            DataTable dt = new DataTable();
            dt.Columns.Add("title");

            foreach (var i in cursor)
            {
                dt.Rows.Add(i["title"]);
            }

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();
        }


        protected void MoviesInRange20132015_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Label_time.Text = "Query Started....";
            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            var server = client.GetServer();


            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies1");
            var query = Query.And(Query.EQ("type", "(MOVIE)"), Query.LTE("year", 2015), Query.GTE("year", 2013));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            cursor.SetFields(Fields.Include("title"));

            Double ResultCount1 = collection.Find(query).Count();
            Label_Result.Text = "The number of records are " + Convert.ToInt32(ResultCount1);
            watch.Stop();
            Label_time.Text = "query completed in" + (watch.ElapsedMilliseconds / 1000);

            DataTable dt = new DataTable();
            dt.Columns.Add("title");

            foreach (var i in cursor)
            {
                dt.Rows.Add(i["title"]);
            }

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();


        }

        protected void Button_MoviesInRange_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Label_time.Text = "Query Started....";
            int year1 = Convert.ToInt32(TextBox_From.Text.ToString());
            int year2 = Convert.ToInt32(TextBox_To.Text.ToString());

            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            MongoServer server = client.GetServer();


            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies1");
            var query = Query.And(Query.EQ("type", "(MOVIE)"), Query.LTE("year", year2), Query.GTE("year", year1));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            cursor.SetFields(Fields.Include("title"));

            Double ResultCount1 = collection.Find(query).Count();
            Label_Result.Text = "The number of records are " + Convert.ToInt32(ResultCount1);
            watch.Stop();
            Label_time.Text = "Query completed in" + (watch.ElapsedMilliseconds / 1000);

            DataTable dt = new DataTable();
            dt.Columns.Add("title");

            foreach (var i in cursor)
            {
                dt.Rows.Add(i["title"]);
            }

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();

        }

        protected void Button_MoviesPerYear_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Label_time.Text = "Map Reduce Started....";

            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
             
            var server = client.GetServer();

            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies1");
            var query = Query.EQ("type", "(MOVIE)");
            //String string1 = @"function() { emit(this.year, 1); }";
            //String string2 = @"function(key, values) { var total = 0; for(var i = 0; i < values.length; i++) { total += values[i]; } return total;}";
            //BsonJavaScript map = new BsonJavaScript(string1);
            //BsonJavaScript reduce = new BsonJavaScript(string2);


            //MapReduceArgs args = new MapReduceArgs();
            //args.MapFunction = map;
            //args.ReduceFunction = reduce;
            //args.Query = query;
            ////args.OutputMode = MapReduceOutputMode.Inline;
            
            //args.OutputCollectionName = "movies_count";
            //args.OutputDatabaseName = "imdb";

            
            
            
            
            //MapReduceResult result = collection.MapReduce(args);
            MongoCollection<BsonDocument> output_collection = db.GetCollection<BsonDocument>("movies_count");
            MongoCursor<BsonDocument> cursor = output_collection.FindAll().SetSortOrder(SortBy.Descending("value"));

            watch.Stop();
            Label_time.Text = "Mapreduce completed in" + (watch.ElapsedMilliseconds / 1000);

            cursor.SetFields(Fields.Include("_id", "value"));



            DataTable dt = new DataTable();
            dt.Columns.Add("Year");
            dt.Columns.Add("Movies");

            foreach (var i in cursor)
            {
                dt.Rows.Add(i["_id"], i["value"]);
                
            }



            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();

            
            


        }

        protected void Button_genre_mapreduce_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Label_time.Text = "Map Reduce Started....";

            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);

            var server = client.GetServer();

            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection_genre = db.GetCollection<BsonDocument>("genres");
            String user_input = TextBox_genre.Text.ToString();
            //var query_year = Query.EQ("year", user_input);

            String string1_genre = @"function() {if ( this.year == '" + user_input + "' ) { emit(this.genre,1); }};";
            String string2_genre = @"function(key, values) { var total = 0; for(var i = 0; i < values.length; i++) { total += values[i]; } return total; };";
            BsonJavaScript map_genre = new BsonJavaScript(string1_genre);
            BsonJavaScript reduce_genre = new BsonJavaScript(string2_genre);


            MapReduceArgs args1 = new MapReduceArgs();
            args1.MapFunction = map_genre;
            args1.ReduceFunction = reduce_genre;

            args1.OutputDatabaseName = "imdb";
            args1.OutputCollectionName = "genre_count";
            args1.OutputMode = MapReduceOutputMode.Replace;


            MapReduceResult result_genre = collection_genre.MapReduce(args1);
            MongoCollection<BsonDocument> output_collection_genre = db.GetCollection<BsonDocument>("genre_count");
            MongoCursor<BsonDocument> cursor_genre = output_collection_genre.FindAll();

            watch.Stop();
            Label_time.Text = "Mapreduce completed in " + (watch.ElapsedMilliseconds / 1000) +" seconds";

            cursor_genre.SetFields(Fields.Include("_id", "value"));



            DataTable dt = new DataTable();
            dt.Columns.Add("_id");
            dt.Columns.Add("value");

            foreach (var i in cursor_genre)
            {
                dt.Rows.Add(i["_id"], i["value"]);

            }

            Chart1.DataSource = dt;
            Chart1.Series["Series1"].ChartType = SeriesChartType.Bar;
            Chart1.Series["Series1"].XValueMember = "_id";
            Chart1.Series["Series1"].YValueMembers = "value";
            
            Chart1.DataBind();

            Chart1.Series["Series1"].Sort(PointSortOrder.Ascending, "Y");
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 1;
            Chart1.Visible = true;

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();

        }

        protected void TextBox_genre_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Chart1_Load(object sender, EventArgs e)
        {

        }


    }
}