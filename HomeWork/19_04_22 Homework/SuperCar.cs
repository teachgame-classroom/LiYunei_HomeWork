using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_04_22_Homework
{
    class SuperCar:Car
    {
        protected override void Drive()
        {
            if (engine.IsOn())
            {
                Console.WriteLine("Run supercar");
            }
            else
            {
                Console.WriteLine("Can not run supercar");
            }
        }
    }
}
