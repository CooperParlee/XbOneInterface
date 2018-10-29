using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;
using System.Drawing;



namespace XboneInterface
{
    public class Interfacer
    {
        private Controller controller;
        private Gamepad gamepad;

        private List<KeyValuePair<Button, bool>> statearray;
        public enum Button
        {
            BumperLeft = 3,
            BumperRight = 4,
            JoystickLeft = 5, 
            DPadUp = 6,
            DPadDown = 7,
            DPadLeft = 8,
            DPadRight = 9,
            X = 10,
            Y = 11,
            A = 12,
            B = 13,
            Xbox = 14,
            View = 15,
            Menu = 16
        }
        public enum Triggers {
            TriggerLeft = 1,
            TriggerRight = 2
            
        }
        private bool connected = false;
        private readonly int deadzone = 2500;

        private Point leftThumb, rightThumb = new Point(0, 0);
        private float leftTrigger, rightTrigger;

        public Interfacer(UserIndex id)
        {
            controller = new Controller(id);
            connected = controller.IsConnected;


            statearray = new List<KeyValuePair<Button, bool>>();
            /*
            foreach(Button str in Button)
            {
                statearray.Add(new KeyValuePair<Button, bool>(str, false));
            }
            */
        }
        public void Update()
        {
            connected = controller.IsConnected;
            if (!connected)
                
                return;

            gamepad = controller.GetState().Gamepad;
            Console.WriteLine(GetTrigger(Triggers.TriggerLeft));

            leftTrigger = GetTrigger(Triggers.TriggerLeft);
            rightTrigger = GetTrigger(Triggers.TriggerRight);
            

        }

        private void UpdateGetState()
        {

        }

        public void GetState(string bind)
        {

        }

        public float GetTrigger(Triggers Trig)
        {
            int m_TrigVal;
            switch (Trig) {
                case Triggers.TriggerLeft:
                    m_TrigVal = gamepad.LeftTrigger;
                break;
                case Triggers.TriggerRight:
                    m_TrigVal = gamepad.RightTrigger;
                break;
                default:
                    throw new System.ArgumentException("Passed value is not an acceptable parameter; please use 'leftTrigger' or 'rightTrigger'", "original");
            }
            return m_TrigVal / 255.0f; // Ensures this function returns a value between 0 and 1
        }

    }
}
