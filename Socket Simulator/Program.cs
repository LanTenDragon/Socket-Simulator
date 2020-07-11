using System;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Socket_Simulator
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            User user = new User();
            Mongo mongo = new Mongo();
            UserId userid = new UserId();

            DialogResult dialogResult;

            using (var loginForm = new LoginForm(user, mongo, userid))
                dialogResult = loginForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                Application.Run(new Form1(mongo, userid));
            }
        }
    }

    public class Mongo
    {
       public MongoClient dbClient = new MongoClient("mongodb://lanten2:lolpasswordlol@lantendragon.southeastasia.cloudapp.azure.com/IoT?authSource=admin");
    }

    public class User
    {
        public ObjectId _id { get; set; }
        public string username { get; set; }
        public string hash { get; set; }
        public int __v { get; set; }
    }

    public class UserId
    {
        public string userid { get; set; }
    }

    public class Socket
    {
        public ObjectId _id { get; set; }
        public bool status { get; set; }
        public string image { get; set; }
        public ObjectId[] groups { get; set; }
        public string name { get; set; }
        public ObjectId belongsTo { get; set; }
        public string colour { get; set; }
        public int __v { get; set; }
    }
}
