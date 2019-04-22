using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_04_22_Homework
{
    class CarEngine
    {
        protected bool isOn;

        public CarEngine()
        {
            isOn = true;
            Console.WriteLine("Creat engine");
        }

        public virtual void TurnOn()
        {
            isOn = true;
            Console.WriteLine("Run engine");
        }

        public virtual void TurnOff()
        {
            isOn = false;
            Console.WriteLine("Close engine");
        }

        public bool IsOn()
        {
            return isOn;
        }
    }
}
