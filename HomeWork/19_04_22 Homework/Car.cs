using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_04_22_Homework
{
    class Car
    {
        protected CarEngine engine;

        public Car()
        {
            engine = new CarEngine();
            
            Drive();

            Start();

            Drive();

            Stop();

            Drive();
        }

        void Start()
        {
            engine.TurnOn();
        }

        void Stop()
        {
            engine.TurnOff();
        }

        protected virtual void Drive()
        {
            if (engine.IsOn())
            {
                Console.WriteLine("Dirve car");
            }
            else
            {
                Console.WriteLine("Can not dirve");
            }
        }
    }
}
