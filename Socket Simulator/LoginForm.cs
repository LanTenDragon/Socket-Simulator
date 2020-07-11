using System;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Socket_Simulator
{
    public partial class LoginForm : Form
    {
        User loginUserObject;
        Mongo mongo;
        UserId userId;

        public LoginForm(User usr, Mongo mong, UserId uid)
        { 
            InitializeComponent();
            loginUserObject = usr;
            mongo = mong;
            userId = uid;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            label4.Visible = false;

            var database = mongo.dbClient.GetDatabase("IoT");
            var collection = database.GetCollection<BsonDocument>("users");


            FilterDefinition<BsonDocument> UsernameFilter = Builders<BsonDocument>.Filter.Eq("username", textBox1.Text);
            BsonDocument document = collection.Find(UsernameFilter).FirstOrDefault();

            if (document == null)
            {
                label3.Text = "User does not exist";
                label3.Visible = true;
                return;
            }

            loginUserObject = BsonSerializer.Deserialize<User>(document);
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(textBox2.Text, loginUserObject.hash);
            userId.userid = loginUserObject._id.ToString();

            if (!passwordMatch)
            {
                label4.Text = "Icorrect Password";
                label4.Visible = true;
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }

}
