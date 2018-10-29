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

        public enum DPad
        {
            DPadUp,
            DPadDown,
            DPadLeft,
            DPadRight,
        }
        public enum Button
        {
            BumperLeft,
            BumperRight,
            JoystickLeft, 
            X,
            Y,
            A,
            B,
            Xbox,
            View,
            Menu
        }

        public enum Thumbs
        {
            ThumbLeft,
            ThumbRight
        }

        public enum Triggers {
            TriggerLeft,
            TriggerRight 
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
            
            foreach(Button button in Enum.GetValues(typeof(Button)))
            {
                statearray.Add(new KeyValuePair<Button, bool>(button, false));
            }
            
        }
        public void Update()
        {
            connected = controller.IsConnected;
            if (!connected)
                
                return;

            gamepad = controller.GetState().Gamepad;
            //Console.WriteLine(GetTrigger(Triggers.TriggerLeft));
            Console.WriteLine(GetDPad(DPad.DPadLeft));
            leftTrigger = GetTrigger(Triggers.TriggerLeft);
            rightTrigger = GetTrigger(Triggers.TriggerRight);
            

        }

        public bool GetDPad(DPad dir)
        {
            string Data = controller.GetState().Gamepad.Buttons.ToString();

            Console.WriteLine(dir.ToString());
            return (Data == dir.ToString());
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

        public Point GetAxis(Thumbs thumb)
        {
            switch (thumb)
            {
                case Thumbs.ThumbLeft:
                    return;
                break;
            }

        }

    }
}
