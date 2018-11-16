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

        //Enumerable types; use when calling buttons etc.
        public enum DPad
        {
            DPadUp,
            DPadDown,
            DPadLeft,
            DPadRight,
        }
        public enum BatteryClass
        {
            Alkaline,
            LiIon,
            Nimh
        }
        public enum BatteryLevelArb
        {
            Low,
            Med,
            High,
            Empty
        }
        public enum Button //Bipolar buttons -- the ones that go on and off :/
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

        public enum Thumbs //Joysticks
        {
            ThumbLeft,
            ThumbRight
        }

        public enum Triggers //The left and right triggers found on the front of the controller.
        {
            TriggerLeft,
            TriggerRight
        }
        private List<KeyValuePair<Button, bool>> buttonstatearray;
        private List<KeyValuePair<Button, GamepadButtonFlags>> buttoncongruency;
        private float xrange = 32768.0f;
        private float yrange = 32768.0f;
        private float deadzone = 0.065f;

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
            Console.WriteLine(GetBattery());
        }

        //The deadzoning and ranging stuff
        public void RedefineJoystickDeadzone(float deadzone)
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
        public bool GetDPad(DPad dir) //Returns if the specified DPad direction is being pushed.
        {
            string Data = controller.GetState().Gamepad.Buttons.ToString();

            Console.WriteLine(dir.ToString());
            return (Data == dir.ToString());
        }

        public float GetTrigger(Triggers Trig) //Returns a floating-point number between 0.0 and 1.0.
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

        public PointF GetJoystick(Thumbs thumb) //Returns a point (x and a y) that the specified joystick is located at.
        {
            PointF buffer;
            buffer = GetJoystickRaw(thumb);

            buffer.X = buffer.X / xrange;
            buffer.Y = buffer.Y / yrange;

            if (Math.Abs(buffer.X) < deadzone)
            {
                buffer.X = 0;
            }
            Console.WriteLine(buffer.Y);
            if (Math.Abs(buffer.Y) < deadzone)
            {
                buffer.Y = 0;
            }

            

            return buffer;
        }
        public PointF GetJoystickRaw(Thumbs thumb)
        {
            switch (thumb)
            {
                case Thumbs.ThumbLeft:
                    float xL = controller.GetState().Gamepad.LeftThumbX;
                    float yL = controller.GetState().Gamepad.LeftThumbY;

                    return new PointF(xL, yL);
                    break;

                case Thumbs.ThumbRight:
                    float xR = controller.GetState().Gamepad.RightThumbX;
                    float yR = controller.GetState().Gamepad.RightThumbY;
                    return new PointF(xR, yR);
                    break;
                default:
                    throw new System.ArgumentException("Passed value was not of either appropriate thumb type, could you perhaps have a special controller? Please file a issue report on https://github.com/CooperParlee/XbOneInterface.", "original");
            }
        }
        public void SetVibration(float LeftMotor, float RightMotor) //Sets the controller vibration motor speeds
        {
            LeftMotor = Math.Max(-1, Math.Min(1, LeftMotor));
            RightMotor = Math.Max(-1, Math.Min(1, RightMotor));
            Vibration vibrationcharacteristics = new Vibration
            {
                LeftMotorSpeed = (ushort)(LeftMotor * 65535),
                RightMotorSpeed = (ushort)(RightMotor * 65535)
            };
            controller.SetVibration(vibrationcharacteristics);
        }
        public bool IsButtonPressed(Button button) //Returns if sent button is pressed.
        {
            var pair = buttoncongruency.Find(x => x.Key == button);

            if (controller.GetState().Gamepad.Buttons.HasFlag(pair.Value) == true){
                return true;
            }
            return false;
        }
        public bool HasBattery()
        {
            Console.WriteLine(controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryType);
            switch (controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryType) {

                case BatteryType.Wired:
                    return false;
                    break;
                case BatteryType.Disconnected:
                    return false;
                    break;
                case BatteryType.Nimh:
                    return true;
                    break;
                case BatteryType.Alkaline:
                    return true;
                    break;        
            }
            throw new System.ArgumentOutOfRangeException("Function was not able to locate a battery of an appropriate type. Please file a issue report on https://github.com/CooperParlee/XbOneInterface.", "original");

        }
        public BatteryLevelArb GetBattery()
        {
            switch (controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel)
            {
                case BatteryLevel.Full:
                    return BatteryLevelArb.High;
                case BatteryLevel.Medium:
                    return BatteryLevelArb.Med;
                case BatteryLevel.Low:
                    return BatteryLevelArb.Low;
                case BatteryLevel.Empty:
                    return BatteryLevelArb.Empty;
            }

            return 0.0f;
        }
    }
}
