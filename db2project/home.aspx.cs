using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using System.Data;



namespace db2project
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {







        }

        public class City
        {
            public string _id { get; set; }
            public string name { get; set; }
        }

        protected void InsertButton_Click(object sender, EventArgs e)
        {
            //var connectionString = "mongodb://localhost:27017/";
            //var client = new MongoClient(connectionString);
            //var server = client.GetServer();
            //MongoDatabase db = server.GetDatabase("test_bhavishya");
            //MongoCollection collection = db.GetCollection<BsonDocument>("test_collection_bhavishya");
            //BsonDocument city = new BsonDocument {
            //    { "_id", InsertIDTextBox.Text.ToString() },
            //    { "name", InsertNameTextBox.Text.ToString() }
            //    };
            //Response.Write("Bson created" + city.ToJson());
            //collection.Insert(city);
            //City city1 = new City { _id = "12", name = "vij" };
            //Response.Write("insert succesful");








        }

        protected void TopRatedMovies_Click(object sender, EventArgs e)
        {

            var connectionString = "mongodb://35.16.0.75:27017/";

            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("ratings");
            //var query = Query.EQ("_id", findIDTextBoxText);

            //MongoCursor<BsonDocument> cursor = collection.Find(query);

            var sort1 = SortBy.Descending("votes").Descending("rank");

            MongoCursor<BsonDocument> cursor1 = collection.FindAll().SetSortOrder(sort1).SetLimit(50);

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
            var connectionString = "mongodb://192.168.1.134:27017/";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();


            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies");
            var query = Query.And(Query.EQ("year", 2013), Query.EQ("type", "(MOVIE)"));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            cursor.SetFields(Fields.Include("title"));

            DataTable dt = new DataTable();
            dt.Columns.Add("title");

            foreach (var i in cursor)
            {
                dt.Rows.Add(i["title"]);
            }

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();
        }

        protected void ResultTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        protected void MoviesInRange_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://192.168.1.134:27017/";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();


            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies");
            var query = Query.And(Query.EQ("type", "(MOVIE)"), Query.LTE("year", 2015), Query.GTE("year", 2013));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            cursor.SetFields(Fields.Include("title"));

            DataTable dt = new DataTable();
            dt.Columns.Add("title");

            foreach (var i in cursor)
            {
                dt.Rows.Add(i["title"]);
            }

            ResultGridView.DataSource = dt;
            ResultGridView.DataBind();


        }


    }
}