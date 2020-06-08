using Stateless;
using LedBulb;
using System.Windows.Forms;

namespace SocketPiece
{
    public class SocketStateMachine
    {
        StateMachine<State, Trigger> machine;
        Bulb PhyBulb, LogicBulb;
        Timer timer;
        Button button;

        private State CurrentState = State.NoServer;

        public SocketStateMachine(Bulb PhysicalBulb, Bulb LogicalBulb, Timer time, Button PhyButton)
        {
            PhyBulb = PhysicalBulb;
            LogicBulb = LogicalBulb;
            timer = time;
            button = PhyButton;

            machine = new StateMachine<State, Trigger>(() => CurrentState, s => CurrentState = s);

            machine.Configure(State.NoServer)
                .Permit(Trigger.ConnectToServer, State.PhyOn)
                .OnActivate(() =>
                {
                    PhyBulb.On = false;
                    timer.Stop();
                    LogicBulb.On = false;
                    button.Enabled = false;
                })
                .OnEntry(() =>
                {
                    PhyBulb.On = false;
                    timer.Stop();
                    LogicBulb.On = false;
                    button.Enabled = false;
                });

            machine.Configure(State.PhyOn)
                .Permit(Trigger.DisconnectFromServer, State.NoServer)
                .PermitIf(Trigger.PhysicalSwitch, State.PhyOff)
                .OnEntry(() =>
                {
                    PhyBulb.On = true;
                    timer.Start();
                    button.Enabled = true;
                });

            machine.Configure(State.PhyOff)
                .Permit(Trigger.DisconnectFromServer, State.NoServer)
                .Permit(Trigger.PhysicalSwitch, State.PhyOn)
                .OnEntry(() =>
                {
                    PhyBulb.On = false;
                    timer.Stop();
                    LogicBulb.On = false;
                });

            machine.Activate();
        }

        public void TogglePhysicalSwitch() { machine.Fire(Trigger.PhysicalSwitch); }
        public void Connect() 
        {
            machine.Fire(Trigger.ConnectToServer);
        }
        public void Disconnect() { machine.Fire(Trigger.DisconnectFromServer); }

        public void Success() { button.Text = "Success"; }

        public void ToggleLogicalSwitch()
        {
            machine.Fire(Trigger.PhysicalSwitch); 
        }
    }

    public enum State { PhyOn, PhyOff, NoServer }
    enum Trigger { PhysicalSwitch, LogicalSwitch, ConnectToServer, DisconnectFromServer }
}
