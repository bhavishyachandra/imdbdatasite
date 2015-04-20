﻿using System;
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
using System.Configuration;


namespace db2project
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {







        }



        protected void TopRatedMovies_Click(object sender, EventArgs e)
        {

            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            var server = client.GetServer();

            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("ratings");
            //var query = Query.EQ("_id", findIDTextBoxText);

            //MongoCursor<BsonDocument> cursor = collection.Find(query);

            var sort1 = SortBy.Descending("votes").Descending("rank");

            MongoCursor<BsonDocument> cursor1 = collection.FindAll().SetSortOrder(sort1).SetLimit(50);

            Label_Result.Text = "";

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

            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            var server = client.GetServer();


            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies1");
            var query = Query.And(Query.EQ("year", 2013), Query.EQ("type", "(MOVIE)"));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            Double ResultCount = collection.Find(query).Count();
            cursor.SetFields(Fields.Include("title"));

            Label_Result.Text = "The number of records are " + Convert.ToInt32(ResultCount);

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
            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            var server = client.GetServer();


            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies1");
            var query = Query.And(Query.EQ("type", "(MOVIE)"), Query.LTE("year", 2015), Query.GTE("year", 2013));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            cursor.SetFields(Fields.Include("title"));

            Double ResultCount1 = collection.Find(query).Count();
            Label_Result.Text = "The number of records are " + Convert.ToInt32(ResultCount1);

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
            int year1 = Convert.ToInt32(TextBox_From.Text.ToString());
            int year2 = Convert.ToInt32(TextBox_To.Text.ToString());

            var client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            var server = client.GetServer();


            MongoDatabase db = server.GetDatabase("imdb");
            MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("movies1");
            var query = Query.And(Query.EQ("type", "(MOVIE)"), Query.LTE("year", year2), Query.GTE("year", year1));
            MongoCursor<BsonDocument> cursor = collection.Find(query).SetLimit(50);
            cursor.SetFields(Fields.Include("title"));

            Double ResultCount1 = collection.Find(query).Count();
            Label_Result.Text = "The number of records are " + Convert.ToInt32(ResultCount1);

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