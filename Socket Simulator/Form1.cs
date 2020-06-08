using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketPiece;

namespace Socket_Simulator
{
    public partial class Form1 : Form
    {
        Piece sp1, sp2, sp3, sp4;
        private Boolean buttonEnabled = true; 


        private async void button1_Click(object sender, EventArgs e)
        {
            //if (buttonEnabled)
            //{
            //    textBox1.Enabled = false;
            //    textBox2.Enabled = false;
            //    button1.Text = "Disconnect";

            //    await sp1.setServer("http://localhost:1883");
            //}
            //else
            //{
            //    textBox1.Enabled = true;
            //    textBox2.Enabled = true;
            //    button1.Text = "Connect";

            //    sp1.ssm.Disconnect();
            //    sp2.ssm.Disconnect();
            //    sp3.ssm.Disconnect();
            //    sp4.ssm.Disconnect();
            //}

            textBox1.Enabled = false;
            textBox2.Enabled = false;

            await sp1.setServer("http://localhost:1883");

            buttonEnabled = !buttonEnabled;
        }

        public Form1()
        {
            this.InitializeComponent();

            sp1 = new Piece("Test4", "test4");
            sp2 = new Piece("Test5", "test5");
            sp3 = new Piece("Test6", "test6");
            sp4 = new Piece("Test7", "test7");
            this.flowLayoutPanel1.Controls.Add(this.sp1);
            this.flowLayoutPanel1.Controls.Add(this.sp2);
            this.flowLayoutPanel1.Controls.Add(this.sp3);
            this.flowLayoutPanel1.Controls.Add(this.sp4);

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
