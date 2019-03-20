using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork
{
    class Game
    {
        Student student;

        public bool isRunning;

        string input;

        public Game(string name, string sex, int age, int Chinese, int math, int English)
        {
            isRunning = true;
            student = new Student( name,  sex,  age,  Chinese,  math,  English);
        }

        public void Loop()
        {
            Input();
            Update();
            Render();
        }

        public void Input()
        {
            Console.WriteLine("1-SayHello, 2-PrintScore");
            input = Console.ReadLine();
        }

        public void Update()
        {

        }

        public void Render()
        {
            if(input == "1")
            {
                student.Sayhello();

            }
            if(input == "2")
            {
                student.PrintScore();
            }            

        }
    }
}
