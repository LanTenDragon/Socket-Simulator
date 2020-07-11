using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using Stateless;
using Stateless.Graph;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SocketPiece
{
    public partial class Piece : UserControl
    {
        Random rng = new Random();
        StateMachine<State, Trigger> machine;
        MqttClientTlsOptions tlsOptions;
        MqttClientOptions PublisherOptions;
        MqttClientOptions SubscriberOptions;
        MqttTopicFilter topicFilter;
        string userid;
        string socketid;

        private State CurrentState = State.PhyOff;
        private IManagedMqttClient managedMqttClientPublisher;
        private IManagedMqttClient managedMqttClientSubscriber;

        public Piece()
        {
            InitializeComponent();
        }

        public Piece(string Name, string uid, string socketid, string mqtt_url) : this()
        {
            SocketName.Text = Name;
            userid = uid;
            this.socketid = socketid;

            #region Initialising Clients

            tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            PublisherOptions = new MqttClientOptions
            {
                ClientId = SocketName.Text + "Publisher",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = mqtt_url,
                    Port = 1883,
                    TlsOptions = tlsOptions
                },
                Credentials = new MqttClientCredentials
                {
                    Username = SocketName.Text + "Publisher",
                    Password = Encoding.UTF8.GetBytes("test")
                },
                CleanSession = true,
                KeepAlivePeriod = TimeSpan.FromSeconds(5)
            };

            SubscriberOptions = new MqttClientOptions
            {
                ClientId = SocketName.Text + "Subscriber",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = mqtt_url,
                    Port = 1883,
                    TlsOptions = tlsOptions
                },
                Credentials = new MqttClientCredentials
                {
                    Username = SocketName.Text + "Subscriber",
                    Password = Encoding.UTF8.GetBytes("test")
                },
                CleanSession = true,
                KeepAlivePeriod = TimeSpan.FromSeconds(5)
            };

            MqttFactory mqttFactory = new MqttFactory();

            managedMqttClientPublisher = mqttFactory.CreateManagedMqttClient();
            managedMqttClientPublisher.UseApplicationMessageReceivedHandler(HandleReceivedApplicationMessage);
            managedMqttClientPublisher.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnPublisherConnected);
            managedMqttClientPublisher.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnPublisherDisconnected);

            managedMqttClientSubscriber = mqttFactory.CreateManagedMqttClient();
            managedMqttClientSubscriber.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnSubscriberConnected);
            managedMqttClientSubscriber.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnSubscriberDisconnected);
            managedMqttClientSubscriber.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnSubscriberMessageReceived);

            topicFilter = new MqttTopicFilter { Topic = "socket/" + userid + "/" + socketid + "/state" };

            #endregion

            #region State Machine Initialisation

            machine = new StateMachine<State, Trigger>(() => CurrentState, s => CurrentState = s);

            machine.Configure(State.PhyOn)
                .Permit(Trigger.PhysicalSwitch, State.PhyOff)
                .Permit(Trigger.EnterLogicalState, State.LogicalOff)
                .Ignore(Trigger.LogicalSwitch)
                .OnEntry(() =>
                {
                    Debug.WriteLine(Name + " entering phy on");
                    PhyBulb.On = true;
                    button1.Invoke( new MethodInvoker( delegate { button1.Enabled = true; }));
                });

            machine.Configure(State.PhyOff)
                .Permit(Trigger.PhysicalSwitch, State.PhyOn)
                .OnActivate(() =>
                {
                    Debug.WriteLine(Name + " Activating");
                    PhyBulb.On = false;
                    //Invoke(new MethodInvoker(delegate { timer1.Stop(); }));
                    timer1.Stop();
                    PowerDisplay.Text = "0.0";
                    LogicBulb.On = false;
                })
                .OnEntry(() =>
                {
                    Debug.WriteLine(Name + " entering phy off");
                    PhyBulb.On = false;
                    Invoke(new MethodInvoker(delegate { timer1.Stop(); PowerDisplay.Text = "0.0"; }));
                    LogicBulb.On = false;
                });

            machine.Configure(State.LogicalOn)
                .SubstateOf(State.PhyOn)
                .Permit(Trigger.LogicalSwitch, State.LogicalOff)
                .Permit(Trigger.PhysicalSwitch, State.PhyOff)
                .OnEntry(() =>
                {
                    Debug.WriteLine(Name + " entering logical on");
                    LogicBulb.On = true;
                    Invoke(new MethodInvoker(delegate { timer1.Start(); }));
                });

            machine.Configure(State.LogicalOff)
                .SubstateOf(State.PhyOn)
                .Permit(Trigger.LogicalSwitch, State.LogicalOn)
                .Permit(Trigger.PhysicalSwitch, State.PhyOff)
                .OnEntry(() =>
                {
                    Debug.WriteLine(Name + " entering logical off");
                    LogicBulb.On = false;
                    Invoke(new MethodInvoker(delegate { timer1.Stop(); PowerDisplay.Text = "0.0"; }));
                });

            machine.Activate();
            //textBox1.Text = UmlDotGraph.Format(machine.GetInfo());
            #endregion
        }

        private void OnPublisherConnected(MqttClientConnectedEventArgs x)
        {
            machine.Fire(Trigger.PhysicalSwitch);
            machine.Fire(Trigger.EnterLogicalState);
        }

        private void OnPublisherDisconnected(MqttClientDisconnectedEventArgs x)
        {
            machine.Fire(Trigger.PhysicalSwitch);
        }

        private void OnSubscriberConnected(MqttClientConnectedEventArgs x)
        {

        }

        private void OnSubscriberDisconnected(MqttClientDisconnectedEventArgs x)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(managedMqttClientPublisher.IsStarted.ToString());
            if (!managedMqttClientPublisher.IsStarted || !managedMqttClientSubscriber.IsStarted)
            {
                await managedMqttClientPublisher.StartAsync(new ManagedMqttClientOptions { ClientOptions = PublisherOptions });
                await managedMqttClientSubscriber.StartAsync(new ManagedMqttClientOptions { ClientOptions = SubscriberOptions });
                await this.managedMqttClientSubscriber.SubscribeAsync(topicFilter);
            }
            else
            {
                await managedMqttClientPublisher.StopAsync();
                await managedMqttClientSubscriber.StopAsync();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string power = rng.Next(1, 100).ToString();
            PowerDisplay.Text = power;
            PublishPower(power);
        }

        public async void PublishPower(string power)
        {
            try
            {
                var payload = Encoding.UTF8.GetBytes(power.ToString());
                var message = new MqttApplicationMessageBuilder().WithTopic("socket/" + userid + "/" + socketid + "/power").WithPayload(payload).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).WithRetainFlag().Build();

                if (this.managedMqttClientPublisher != null)
                {
                    await this.managedMqttClientPublisher.PublishAsync(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async void startSocket()
        {
            Debug.WriteLine("startSocket");
            await managedMqttClientPublisher.StartAsync(new ManagedMqttClientOptions { ClientOptions = PublisherOptions });
            await managedMqttClientSubscriber.StartAsync(new ManagedMqttClientOptions { ClientOptions = SubscriberOptions });
            await managedMqttClientSubscriber.SubscribeAsync(topicFilter);
        }

        private void HandleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs x)
        {
            
        }

        private void OnSubscriberMessageReceived(MqttApplicationMessageReceivedEventArgs x)
        {
            string topic = x.ApplicationMessage.Topic;
            string payload = x.ApplicationMessage.ConvertPayloadToString();

            if (payload == "on")
            {
                if (!machine.IsInState(State.LogicalOn))
                {
                    machine.Fire(Trigger.LogicalSwitch);
                }
            }
            else if (payload == "off")
            {
                if (!machine.IsInState(State.LogicalOff))
                    machine.Fire(Trigger.LogicalSwitch);
            }
        }
    }

    enum State { PhyOn, PhyOff, LogicalOn, LogicalOff, NoLogicalState }
    enum Trigger { PhysicalSwitch, LogicalSwitch, EnterLogicalState }
}
