using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_03_25_Homework
{
    class Program
    {
        static void Main(string[] args)
        {
           // Question1();

            //Question2();

            //Question3();

            Qunstion4();

            Console.ReadLine();
        }

        static void Question1()
        {
            Console.WriteLine("Question1:编写一个方法，输入3个整型参数a，b，c，按从大到小的顺序将它们打印出来");
            Console.WriteLine("Please entry three integer");

            Console.Write("a: ");
            int.TryParse(Console.ReadLine(), out int a);
            Console.Write("b: ");
            int.TryParse(Console.ReadLine(), out int b);
            Console.Write("c: ");
            int.TryParse(Console.ReadLine(), out int c);

            //b=2;c=3;
            if (c > b)
            {
                int t = b; b = c; c = t;
            }
            if(b > a)
            {
                int t = a; a = b; b = t;
            }
            Console.WriteLine("a:" + a + " b:" + b + " c:" + c);

            /*if (a > b && b > c)
            {
                Console.WriteLine("a:" + a + " b:" + b + " c:" + c);
            }
            else if (a > b && c > b)
            {
                Console.WriteLine("a:" + a + " c:" + c + " b:" + b);
            }
            else if (c > b && b > a)
            {
                Console.WriteLine("c:" + c + " b:" + b + " a:" + a);
            }
            else if (c > b && a > b)
            {
                Console.WriteLine("c:" + c + " a:" + a + " b:" + b);
            }
            else if (b > a && a > c)
            {
                Console.WriteLine("b:" + b + " a:" + a + " c:" + c);
            }
            else if (b > c && c > a)
            {
                Console.WriteLine("b:" + b + " c:" + c + " a:" + a);
            }*/

        }

        static void Question2()
        {
            Console.ReadLine();
            Console.WriteLine("Question2:求 1 + 2 + 3 + …… + 100 的和");
            Console.ReadLine();

            int[] number = new int[100];
            int total = 0;

            for (int i = 0; i < number.Length; i++)
            {
                number[i] = i + 1;
                total += number[i];
            }

            Console.WriteLine("Total:" + total);
        }

        static void Question3()
        {

            Console.ReadLine();
            Console.WriteLine("Question3:判断一个数 n 能否同时被 3 和 5 整除");
            Console.WriteLine("Please Entry integer");

            int.TryParse(Console.ReadLine(), out int num);

            int div3 = num % 3;
            int div5 = num % 5;
            if (div3 == 0 && div5 == 0)
            {
                Console.WriteLine("{0} can exact division, %3: {1},%5: {2}", num, div3, div5);
            }
            else
            {
                Console.WriteLine("{0} Can not exact divison, %3: {1},%5: {2}", num, div3, div5);
            }

        }

        static bool PrimeNumber(int d)
        {
            for(int i = 2; i < d; i++)
            {
                if (d % i == 0) return false;
            }
            return true;
        }

        static void Qunstion4()
        {
            Console.ReadLine();
            Console.WriteLine("Qunstion4:将 100 以内的素数打印出来");
            Console.ReadLine();

            int num = 100;

            for (int i = 1; i <= num; i++)
            {
                if (PrimeNumber(i))
                {
                   Console.Write(i + ", ");
                }
            }
        }

        static bool St(int n)
        {
            int m = (int)Math.Sqrt(n);
            for (int i = 2; i <= m; i++)
            {
                if (n % i == 0 && i != n)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

