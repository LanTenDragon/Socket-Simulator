using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace SocketPiece
{
    class MQTT
    {
        private IManagedMqttClient managedMqttClientPublisher;
        private IManagedMqttClient managedMqttClientSubscriber;
        private string Topic;
        private string serverString;
        private SocketStateMachine StateMachine;

        public MQTT(string topic, SocketStateMachine ssm)
        { 
            Topic = topic;
            StateMachine = ssm;
        }

        public async Task InitialiseManagedClient()
        {
            var mqttFactory = new MqttFactory();

            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            var options = new MqttClientOptions
            {
                ClientId = "ClientPublisher",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = serverString,
                    TlsOptions = tlsOptions
                }
            };

            if (options.ChannelOptions == null)
            {
                throw new InvalidOperationException();
            }

            options.Credentials = new MqttClientCredentials
            {
                Username = "username",
                Password = Encoding.UTF8.GetBytes("password")
            };

            options.CleanSession = true;
            options.KeepAlivePeriod = TimeSpan.FromSeconds(5);

            this.managedMqttClientPublisher = mqttFactory.CreateManagedMqttClient();
            //this.managedMqttClientPublisher.UseApplicationMessageReceivedHandler(HandleReceivedApplicationMessage);
            //this.managedMqttClientPublisher.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnPublisherConnected);

            this.managedMqttClientSubscriber= mqttFactory.CreateManagedMqttClient();
            this.managedMqttClientSubscriber.UseApplicationMessageReceivedHandler(this.HandleReceivedApplicationMessage);
            //this.managedMqttClientSubscriber.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnSubscriberDisconnected);
            //this.managedMqttClientSubscriber.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(this.OnSubscriberMessageReceived);

            await managedMqttClientPublisher.StartAsync(
                new ManagedMqttClientOptions 
                { 
                    ClientOptions = options 
                }
            );
            StateMachine.Success();

            await managedMqttClientSubscriber.StartAsync(
                new ManagedMqttClientOptions { ClientOptions = options }
            );
        }

        public void setServer(string server) { serverString = server; }

        public async void PublishPower(string power)
        {
            try
            {
                var payload = Encoding.UTF8.GetBytes(power.ToString());
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("socket/" + Topic + "/pwr")
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .WithRetainFlag()
                    .Build();

                if (managedMqttClientPublisher != null)
                {
                    await managedMqttClientPublisher.PublishAsync(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Occurs", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs x)
        {
            //var item = $"Timestamp: {DateTime.Now:O} | Topic: {x.ApplicationMessage.Topic} | Payload: {x.ApplicationMessage.ConvertPayloadToString()} | QoS: {x.ApplicationMessage.QualityOfServiceLevel}";
            //this.BeginInvoke((MethodInvoker)delegate { this.TextBoxSubscriber.Text = item + Environment.NewLine + this.TextBoxSubscriber.Text; });
            StateMachine.ToggleLogicalSwitch();
        }
    }
}
