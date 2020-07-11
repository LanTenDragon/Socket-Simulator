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

        private State CurrentState = State.PhyOff;

        public SocketStateMachine(Bulb PhysicalBulb, Bulb LogicalBulb, Timer time, Button PhyButton)
        {
            PhyBulb = PhysicalBulb;
            LogicBulb = LogicalBulb;
            timer = time;
            button = PhyButton;

            machine = new StateMachine<State, Trigger>(() => CurrentState, s => CurrentState = s);

            machine.Configure(State.PhyOn)
                .PermitIf(Trigger.PhysicalSwitch, State.PhyOff)
                .OnEntry(() =>
                {
                    PhyBulb.On = true;
                    timer.Start();
                    button.Enabled = true;
                });

            machine.Configure(State.PhyOff)
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
            
        }
        public void Disconnect() { }

        public void Success() { button.Text = "Success"; }

        public void ToggleLogicalSwitch()
        {
            machine.Fire(Trigger.PhysicalSwitch); 
        }
    }

    //public enum State { PhyOn, PhyOff, NoServer }
    //enum Trigger { PhysicalSwitch, LogicalSwitch, ConnectToServer, DisconnectFromServer }
}
