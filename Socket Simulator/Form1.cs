using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SocketPiece;

namespace Socket_Simulator
{
    public partial class Form1 : Form
    {
        //string mqtt_url = "localhost:1883"; //dev
        string mqtt_url = "lantendragon.southeastasia.cloudapp.azure.com"; //prod

        UserId userId;
        Mongo mongo;
        List<Piece> socketlist = new List<Piece>();

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled = false;

            foreach (Piece piece in socketlist)
            {
                piece.startSocket();
            }
        }

        public Form1(Mongo mong, UserId uid)
        {
            userId = uid;
            mongo = mong;

            this.InitializeComponent();

            var database = mongo.dbClient.GetDatabase("IoT");
            var socketCollection = database.GetCollection<BsonDocument>("sockets");
            var userCollection = database.GetCollection<BsonDocument>("users");

            userId = uid;
            FilterDefinition<BsonDocument> EmptyFilter = Builders<BsonDocument>.Filter.Empty;
            FilterDefinition<BsonDocument> SocketIdFilter = Builders<BsonDocument>.Filter.Eq("belongsTo", ObjectId.Parse(userId.userid));
            var documents = socketCollection.Find(SocketIdFilter).ToList();

            bool happened = false;

            foreach (BsonDocument doc in documents)
            {
                Socket socket = BsonSerializer.Deserialize<Socket>(doc);
                Piece piece = new Piece(socket.name, userId.userid, socket._id.ToString(), mqtt_url);
                socketlist.Add(piece);
                flowLayoutPanel1.Controls.Add(piece);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
