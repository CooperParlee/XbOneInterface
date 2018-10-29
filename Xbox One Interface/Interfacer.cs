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

        private List<KeyValuePair<string, bool>> statearray;

        private readonly string[] buttons = {"BumperLeft", "BumperRight", "JoystickLeft", "JoystickRight", "DPadDown", "DPadUp", "DPadLeft", "DPadRight", "X", "Y", "A", "B", "Xbox", "View", "Menu"};
        public enum triggers {
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


            statearray = new List<KeyValuePair<string, bool>>();

            foreach(string str in buttons)
            {
                statearray.Add(new KeyValuePair<string, bool>(str, false));
            }
        }
        public void Update()
        {
            connected = controller.IsConnected;
            if (!connected)
                
                return;

            gamepad = controller.GetState().Gamepad;
            Console.WriteLine(GetTrigger(triggers.TriggerLeft));

            leftTrigger = GetTrigger(triggers.TriggerLeft);
            rightTrigger = GetTrigger(triggers.TriggerRight);
            

        }

        private void UpdateGetState()
        {

        }

        public void GetState(string bind)
        {

        }

        public float GetTrigger(triggers Trig)
        {
            int m_TrigVal;
            switch (Trig) {
                case triggers.TriggerLeft:
                    m_TrigVal = gamepad.LeftTrigger;
                break;
                case triggers.TriggerRight:
                    m_TrigVal = gamepad.RightTrigger;
                break;
                default:
                    throw new System.ArgumentException("Passed value is not an acceptable parameter; please use 'leftTrigger' or 'rightTrigger'", "original");
            }
            return m_TrigVal / 255.0f; // Ensures this function returns a value between 0 and 1
        }

    }
}
