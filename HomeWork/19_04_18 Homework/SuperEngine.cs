using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_04_18_Homework
{
    class SuperEngine : CarEngine
    {
        public void Turbo()
        {
            Console.WriteLine("Turbocharged super engine");
        }

        public override void TurnOn()
        {
            Console.WriteLine("Run super engine");
        }

        public override void TurnOff()
        {
            Console.WriteLine("Colse super engine");
        }
    }
}
