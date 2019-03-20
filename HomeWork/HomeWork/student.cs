using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork
{
    class Student
    {
        public string name;
        public string sex;
        public int age;
        public int chinese;
        public int math;
        public int english;

        private int average;
        private int total;
        private int grade;

        private const int FAIL = 0;
        private const int MEDIUM = 1;
        private const int GOOD = 2;
        private const int VERYGOOD = 3;

        public Student(string name, string sex, int age, int chinese, int math, int english)
        {
            this.name = name;
            this.sex = sex;
            this.age = age;
            this.chinese = chinese;
            this.math = math;
            this.english = english;

            //Total(this.chinese, this.math, this.english);
            //Average(total);
            //Grade(average);
        }

        public void Sayhello()
        {
            Console.WriteLine("I'm:" + name + ",I'm:" + age + " years old ," + "I'm:" + sex);
        }

        public int Total()
        {
            return this.chinese + this.math + this.english;
        }

        public int Average()
        {
            return Total() / 3;
        }

        public int contrast()
        {
            int average = Average();

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

        public string Grade()
        {
            int grade = contrast();

            string ret = "VeryGood";

            switch (grade)
            {
                case FAIL:
                    ret = "Fail";
                    break;

                case MEDIUM:
                    ret = "Medium";
                    break;

                case GOOD:
                    ret = "Good";
                    break;

                case VERYGOOD:
                    ret = "VeryGood";
                    break;
            }

            return ret;
        }

        public void PrintScore()
        {
            Console.WriteLine("I'm:" + name + ", Total:" + Total() + ", Average:" + Average() + ", Constrast:"+ Grade());
        }
    }
}
