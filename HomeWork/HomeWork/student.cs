using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork
{
    class student
    {
        public string name;
        public string sex;
        public int age;
        public int Chinese;
        public int math;
        public int English;

        private int average;
        private int total;
        private int grade;

        private const int FAIL = 0;
        private const int MEDIUM = 1;
        private const int GOOD = 2;
        private const int VERYGOOD = 3;

        public student(string name, string sex, int age, int Chinese, int math, int English)
        {
            this.name = name;
            this.sex = sex;
            this.age = age;
            this.Chinese = Chinese;
            this.math = math;
            this.English = English;

            Total(this.Chinese, this.math, this.English);
            Average(total);
            Grade(average);
        }

        public void Sayhello()
        {
            Console.WriteLine("I'm:" + name + ",I'm:" + age + " years old ," + "I'm:" + sex);
        }

        public void Total(int Chinese, int math, int English)
        {
            total = Chinese + math + English;
        }

        public void Average(int total)
        {
            int amount = 3;
            this.total = total;
             average = total / amount;
        }

        public int contrast(int average)
        {
            grade = VERYGOOD;

            if (average <= 100)
            {
                grade = VERYGOOD;

                if (average < 90)
                {
                    grade = GOOD;

                    if (average < 80)
                    {
                        grade = MEDIUM;

                        if (average < 60)
                        {
                            grade = FAIL;
                        }
                    }
                }
            }
            return grade;
        }

        public void Grade(int average)
        {
            grade = contrast(average);

            switch (grade)
            {
                case FAIL:                    
                    break;

                case MEDIUM:
                    break;

                case GOOD:
                    break;

                case VERYGOOD:
                    break;
            }
        }

        public void PrintScore()
        {
            string Grade = "test";
            if(grade == 0)
            {
                Grade = "VeryGood";
            }
            if (grade == 1)
            {
                Grade = "Good";
            }
            if (grade == 2)
            {
                Grade = "Medium";
            }
            if (grade == 3)
            {
                Grade = "Fail";
            }

            Console.WriteLine("I'm:" + name + ", Toatl:" + total + ", Average:" + average + ", Constrast:"+ Grade);
        }
    }
}
