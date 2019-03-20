using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_03_20_Homework
{
    class Program
    {

        static void Main(string[] args)
        {
            Stage stage = new Stage();

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("1-question1 2-question2 3-question6");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    stage.Cube();
                }
                if (input == "2")
                {
                    stage.ForBox(4, 8);                    
                }
                if (input == "3")
                {
                    stage.CreatIntArray2();

                    //Console.WriteLine("Please Entry x");
                    //int.TryParse(Console.ReadLine(), out int x);

                    //Console.WriteLine("Please Entry y");
                    //int.TryParse(Console.ReadLine(), out int y);

                    //stage.contrast(x, y);
                }

                Console.WriteLine("1-Continue  2-Exit");
                if(Console.ReadLine() == "2")
                {
                    isRunning = false;
                }

            }
        }
    }
}
