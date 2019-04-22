using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_04_22_Homework
{
    class SuperEngine : CarEngine
    {
        public void Turbo()
        {
            Console.WriteLine("Turbocharged super engine");
        }

        public override void TurnOn()
        {
            base.TurnOn();
            Turbo();
            Console.WriteLine("Run super engine");
        }

        public override void TurnOff()
        {
            base.TurnOff();
            Turbo();
            Console.WriteLine("Colse super engine");
        }
    }
}
