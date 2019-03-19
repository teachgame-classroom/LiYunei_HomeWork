using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork
{
    class Program
    {
        static void Main(string[] args)
        {

            Game game = new Game("Li", "Male", 30, 70, 80, 90);

            while (game.isRunning)
            {
                game.Loop();
            }
        }
    }
}
