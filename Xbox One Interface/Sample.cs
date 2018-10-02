using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using XboneInterface;



namespace Xbox_One_Interface
{
    public class Sample
    {
        static Interfacer xbox;

        private static Timer timer;
        private static readonly double HERTZ = 50;
        /*
        private static void Update(object source, ElapsedEventArgs args)
        {
            
        }
        */
        static void Main()
        {
            Console.WriteLine("Run speed: " + HERTZ);
            timer = new Timer(1 / HERTZ * 1000);
            timer.Elapsed += Update;
            timer.AutoReset = true;
            timer.Enabled = true;
            xbox = new Interfacer(SharpDX.XInput.UserIndex.One);
            Console.ReadKey();
        }
        
    }
}
