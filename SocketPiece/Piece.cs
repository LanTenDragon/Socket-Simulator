using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketPiece
{
    public partial class Piece : UserControl
    {
        Random rng = new Random();
        public SocketStateMachine ssm;
        MQTT mqtt;

        public Piece()
        {
            InitializeComponent();
        }

        public Piece(string Name, string Topic) : this()
        {
            SocketName.Text = Name;
            ssm = new SocketStateMachine(PhyBulb, LogicBulb, timer1, PhysicalSwitchButton);
            mqtt = new MQTT (Topic, ssm);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ssm.TogglePhysicalSwitch();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string power = rng.Next(1, 100).ToString();
            PowerDisplay.Text = power;
            mqtt.PublishPower(power);
        }

        public async Task setServer(string server)
        {
            mqtt.setServer(server);
            await mqtt.InitialiseManagedClient();
        }
    }
}
