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
using System.Reflection;
using System.Text.RegularExpressions;

namespace db2project
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Chart1.Visible = false;
            Chart2.Visible = false;

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
            Label_time.Text = "Query completed in " + (watch.ElapsedMilliseconds / 1000) + " seconds";

            cursor1.SetFields(Fields.Include("movie_name", "rank", "votes"));
            //ResultTextBox.Text += cursor1.ToJson();
            DataTable dt = new DataTable();
            dt.Columns.Add("movie_name");
            dt.Columns.Add("rank");
            dt.Columns.Add("votes");
            foreach (var i in cursor1)
            {
                dt.Rows.Add(i["movie_name"], i["rank"], i["votes"]);
            }


            dt.DefaultView.Sort = "rank DESC";

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
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies");
            var query = Query.And(Query.LTE("year_released", year2), Query.GTE("year_released", year1));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            cursor.SetFields(Fields.Include("movie_name"));

            Double ResultCount1 = collection.Find(query).Count();
            Label_Result.Text = "The number of records are " + Convert.ToInt32(ResultCount1);
            watch.Stop();
            Label_time.Text = "Query completed in " + (watch.ElapsedMilliseconds / 1000) + " seconds";

            DataTable dt = new DataTable();
            dt.Columns.Add("movie_name");

            foreach (var i in cursor)
            {
                dt.Rows.Add(i["movie_name"]);
            }

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();
            TextBox_From.Text = "";
            TextBox_To.Text = "";

        }

        protected void Button_MoviesPerYear_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Label_time.Text = "Map Reduce Started....";

            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
             
            var server = client.GetServer();

            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies");
            
            String string1 = @"function() { emit(this.year_released, 1); }";
            String string2 = @"function(key, values) { var total = 0; for(var i = 0; i < values.length; i++) { total += values[i]; } return total;}";
            BsonJavaScript map = new BsonJavaScript(string1);
            BsonJavaScript reduce = new BsonJavaScript(string2);


            MapReduceArgs args = new MapReduceArgs();
            args.MapFunction = map;
            args.ReduceFunction = reduce;
            args.OutputMode = MapReduceOutputMode.Replace;
            args.OutputCollectionName = "movies_count";
            args.OutputDatabaseName = "imdb";
            
            
            MapReduceResult result = collection.MapReduce(args);
            MongoCollection<BsonDocument> output_collection = db.GetCollection<BsonDocument>("movies_count");
            MongoCursor<BsonDocument> cursor = output_collection.FindAll().SetSortOrder(SortBy.Descending("value"));

            watch.Stop();
            Label_time.Text = "Mapreduce completed in " + (watch.ElapsedMilliseconds / 1000)+" seconds";

            cursor.SetFields(Fields.Include("_id", "value"));



            DataTable dt = new DataTable();
            dt.Columns.Add("Year");
            dt.Columns.Add("Movies");


            

            foreach (var i in cursor)
            {
                dt.Rows.Add(i["_id"], i["value"]);
                
            }


            Chart2.DataSource = dt;
            Chart2.Series["Series2"].ChartType = SeriesChartType.Line;
            Chart2.Series["Series2"].XValueMember = "Year";
            Chart2.Series["Series2"].YValueMembers = "Movies";

            Chart2.DataBind();

            //Chart2.Series["Series2"].Sort(PointSortOrder.Ascending, "X");
            Chart2.ChartAreas["ChartArea2"].AxisX.LabelStyle.Interval = 10;
            Chart2.Visible = true;


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
            int user_input = Convert.ToInt32(TextBox_genre.Text.ToString());
            var query_year = Query.EQ("year_released", user_input);

            String string1_genre = @"function() { emit(this.genre,1); };";
            String string2_genre = @"function(key, values) { var total = 0; for(var i = 0; i < values.length; i++) { total += values[i]; } return total; };";
            BsonJavaScript map_genre = new BsonJavaScript(string1_genre);
            BsonJavaScript reduce_genre = new BsonJavaScript(string2_genre);


            MapReduceArgs args1 = new MapReduceArgs();
            args1.MapFunction = map_genre;
            args1.ReduceFunction = reduce_genre;
            args1.Query = query_year;
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

            TextBox_genre.Text = "";
        }

        protected void TextBox_genre_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Chart1_Load(object sender, EventArgs e)
        {

        }

        protected void ResultGridView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button_genre_selection_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Label_time.Text = "Map Reduce Started....";

            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);

            var server = client.GetServer();

            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection_genre = db.GetCollection<BsonDocument>("genres");
            String user_input1 = autocomplete1.Text.ToString();
            var query_genre = Query.And(Query.EQ("genre", user_input1), Query.GTE("year_released",1984), Query.LTE("year_released", 2014));

            String string1_genre = @"function() { emit(this.year_released,1); };";
            String string2_genre = @"function(key, values) { var total = 0; for(var i = 0; i < values.length; i++) { total += values[i]; } return total; };";
            BsonJavaScript map_genre = new BsonJavaScript(string1_genre);
            BsonJavaScript reduce_genre = new BsonJavaScript(string2_genre);


            MapReduceArgs args_genre1 = new MapReduceArgs();
            args_genre1.MapFunction = map_genre;
            args_genre1.ReduceFunction = reduce_genre;
            args_genre1.Query = query_genre;
            args_genre1.OutputDatabaseName = "imdb";
            args_genre1.OutputCollectionName = "genre_year_count";
            args_genre1.OutputMode = MapReduceOutputMode.Replace;


            MapReduceResult result_genre = collection_genre.MapReduce(args_genre1);
            MongoCollection<BsonDocument> output_collection_genre_year = db.GetCollection<BsonDocument>("genre_year_count");
            MongoCursor<BsonDocument> cursor_genre_year = output_collection_genre_year.FindAll();

            watch.Stop();
            Label_time.Text = "Mapreduce completed in " + (watch.ElapsedMilliseconds / 1000) + " seconds";

            cursor_genre_year.SetFields(Fields.Include("_id", "value"));



            DataTable dt = new DataTable();
            dt.Columns.Add("_id");
            dt.Columns.Add("value");

            foreach (var i in cursor_genre_year)
            {
                dt.Rows.Add(i["_id"], i["value"]);

            }

            Chart2.DataSource = dt;
            Chart2.Series["Series2"].ChartType = SeriesChartType.Line;
            Chart2.Series["Series2"].XValueMember = "_id";
            Chart2.Series["Series2"].YValueMembers = "value";

            Chart2.DataBind();

            //Chart2.Series["Series2"].Sort(PointSortOrder.Ascending, "X");
            Chart2.ChartAreas["ChartArea2"].AxisX.LabelStyle.Interval = 1;
            Chart2.Visible = true;



            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();
            autocomplete1.Text = "";
        }

        protected void Button_actor_search_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Label_time.Text = "Query Started....";

            String actor_name = TextBox_actor_search.Text.ToString();
            Regex regex = new Regex(actor_name, RegexOptions.IgnoreCase);
            BsonRegularExpression bsonRegex1 = new BsonRegularExpression(actor_name, "i");
            BsonRegularExpression bsonRegex = new BsonRegularExpression(regex);
            

            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            MongoServer server = client.GetServer();

            
            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("actors");
            var query = Query.Matches("name", bsonRegex);
            DistinctArgs args1 = new DistinctArgs();
            args1.Key = "name";
            args1.Query = query;

            IEnumerable<String> cursor1 = collection.Distinct<String>(args1);

            watch.Stop();
            Label_time.Text = "Query completed in " + (watch.ElapsedMilliseconds / 1000) + " seconds";
            int count_distinct = cursor1.Count<String>();
            
            Label_Result.Text = count_distinct.ToString()+" distinct records found";
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            int j = 0;
            
            while(j<50 && j<count_distinct){
               
                
                    foreach (var i in cursor1)
                    {
                        dt.Rows.Add(i);
                        j++;
                    }
                
            }

           ResultGridView.DataSource = dt;
            ResultGridView.DataBind();

            TextBox_actor_search.Text = "";
        }

        protected void Button_movies_search_Click(object sender, EventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Label_time.Text = "Query Started....";

            String movie_name = TextBox_movies_search.Text.ToString();
            Regex regex = new Regex(movie_name, RegexOptions.IgnoreCase);
            BsonRegularExpression bsonRegex = new BsonRegularExpression(regex);

            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            MongoServer server = client.GetServer();

            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies");
            var query = Query.Matches("movie_name", bsonRegex);
            DistinctArgs args1 = new DistinctArgs();
            args1.Key = "movie_name";
            args1.Query = query;

            IEnumerable<String> cursor1 = collection.Distinct<String>(args1);

            watch.Stop();
            Label_time.Text = "Query completed in " + (watch.ElapsedMilliseconds / 1000) + " seconds";
            int count_distinct = cursor1.Count<String>();

            Label_Result.Text = count_distinct.ToString() + " distinct records found";
            DataTable dt = new DataTable();
            dt.Columns.Add("Movie_name");
            int j = 0;

            while (j < 50 && j < count_distinct)
            {


                foreach (var i in cursor1)
                {
                    dt.Rows.Add(i);
                    j++;
                }

            }

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();

            Button_movies_search.Text = "";

        }


    }
}