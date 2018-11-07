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
            JoystickRight,
            X,
            Y,
            A,
            B,
            View,
            Menu
        }

        public enum Thumbs
        {
            ThumbLeft,
            ThumbRight
        }

        public enum Triggers
        {
            TriggerLeft,
            TriggerRight
        }

        private Controller controller;
        private Gamepad gamepad;


        //private List<KeyValuePair<Button, >>
        private List<KeyValuePair<Button, bool>> buttonstatearray;
        private List<KeyValuePair<Button, GamepadButtonFlags>> buttoncongruency;
        private float xrange = 32768.0f;
        private float yrange = 32768.0f;
        private float deadzone = 0.014f;
        private bool connected = false;

        public Interfacer(UserIndex id)
        {
            controller = new Controller(id);
            gamepad = new Gamepad();
            connected = controller.IsConnected;

            buttonstatearray = new List<KeyValuePair<Button, bool>>();

            buttoncongruency = new List<KeyValuePair<Button, GamepadButtonFlags>>
            {
                new KeyValuePair<Button, GamepadButtonFlags>(Button.A, GamepadButtonFlags.A),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.BumperLeft, GamepadButtonFlags.LeftShoulder),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.BumperRight, GamepadButtonFlags.RightShoulder),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.JoystickLeft, GamepadButtonFlags.LeftThumb),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.JoystickRight, GamepadButtonFlags.RightThumb),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.X, GamepadButtonFlags.X),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.Y, GamepadButtonFlags.Y),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.A, GamepadButtonFlags.A),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.B, GamepadButtonFlags.B),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.View, GamepadButtonFlags.Back),
                new KeyValuePair<Button, GamepadButtonFlags>(Button.Menu, GamepadButtonFlags.Start)
            };

            foreach (Button button in Enum.GetValues(typeof(Button)))
            {
                buttonstatearray.Add(new KeyValuePair<Button, bool>(button, false));
            }
            
        }
        public void Update()
        {
            connected = controller.IsConnected;
            if (!connected)
                
                return;
            
 
        }

        public void RedefineDeadzone(float deadzone)
        {
            this.deadzone = deadzone;
        }
        public void RedefineXRange(int NewMax)
        {
            xrange = NewMax;
        }
        public void RedefineYRange(int NewMax)
        {
            yrange = NewMax;
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

        public PointF GetAxis(Thumbs thumb)
        {
            switch (thumb)
            {
                case Thumbs.ThumbLeft:
                    float xL = controller.GetState().Gamepad.LeftThumbX / xrange;
                    float yL = controller.GetState().Gamepad.LeftThumbY / yrange;

                    if (Math.Abs(xL) < deadzone)
                    {
                        xL = 0;
                    }
                    if (Math.Abs(yL) < deadzone)
                    {
                        yL = 0;
                    }
                    return new PointF(xL, yL);
                break;
                case Thumbs.ThumbRight:
                    float xR = controller.GetState().Gamepad.RightThumbX / xrange;
                    float yR = controller.GetState().Gamepad.RightThumbY / yrange;
                    
                    if(Math.Abs(xR) < deadzone)
                    {
                        xR = 0;
                    }
                    if (Math.Abs(yR) < deadzone)
                    {
                        yR = 0;
                    }
                    return new PointF(xR, yR);
                break;
            }
            throw new System.ArgumentException("Passed value was not of either appropriate thumb type, could you perhaps have a special controller? Please file a issue report on https://github.com/CooperParlee/XbOneInterface.", "original");
        }
        public void SetVibration(float LeftMotor, float RightMotor)
        {
            LeftMotor = Math.Max(-1, Math.Min(1, LeftMotor));
            RightMotor = Math.Max(-1, Math.Min(1, RightMotor));
            Vibration vibrationcharacteristics = new Vibration();
            vibrationcharacteristics.LeftMotorSpeed = (ushort)(LeftMotor * 65535);
            vibrationcharacteristics.RightMotorSpeed = (ushort)(RightMotor * 65535);
            controller.SetVibration(vibrationcharacteristics);
        }

        public bool IsButtonPressed(Button button)
        {
            var pair = buttoncongruency.Find(x => x.Key == button);

            if (controller.GetState().Gamepad.Buttons.HasFlag(pair.Value) == true){
                return true;
            }
            return false;
        }

        

    }
}
