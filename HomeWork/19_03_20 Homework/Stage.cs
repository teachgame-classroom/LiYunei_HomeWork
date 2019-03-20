using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_03_20_Homework
{
    class Stage
    {

        int row = 4;        //向下行
        int col = 8;        //向右列

        int integer;

        char[] intArray;

        public void CreatIntArray()
        {
            //数组的定义
            int[] aintArrayrr;

            //数组的创建
            aintArrayrr  = new int[64];
  
            for (int i = 0; i < 64; i++)
            {
                //数组的赋值
                aintArrayrr[i] = i;

                Console.WriteLine(aintArrayrr[i]);
            }

        }

        public void CreatIntArray2()
        {
            //定义并且创建一个数组
            int[] arrint = new int[64];

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    int index = x * 16 + y;
                    arrint[index] = GetIndexOfCell(x, y);
                    string strtemp = "x=" + x + ",y=" + y + ",index=" + index;

                    Console.WriteLine(strtemp);
                }
            }

        }

        public void CreatStrArray()
        {
            string[] arrstr = new string[64];
            for (int i = 0; i < 64; i++)
            {
                arrstr[i] = "abc_" + i;
                Console.WriteLine(arrstr[i]);
            }

        }

        private static int GetIndexOfCell(int x, int y)
        {            
            int i = y * 16 + x;
            return i;
        }

        public void contrast(int var_x, int var_y)
        {
             CreatIntArray();               

            if (integer == GetIndexOfCell(var_x, var_y))
            {
                Console.WriteLine("Right");
            }
            else
            {
                Console.WriteLine("Erro");
            }
            
        }

        public void Cube()
        {
            for(int i =0; i < 15; i ++)
            {
                Console.Write('*');

                int mod = i % 5;
                if(mod == 4)
                {
                    Console.Write('\n');
                }
            }
        }


        public bool IsWall(int i,int t)
        {
            bool isWall = false;

            bool isFirstRow = false;
            bool isLastRow = false;
            bool isFirstCol = false;
            bool isLastCol = false;

            isFirstRow = (i == 0);
            isLastRow = (i == row - 1);       //向下行
            isFirstCol = (t == 0);
            isLastCol = (t == col - 1);       //向右列

            isWall = isFirstRow || isLastRow || isFirstCol || isLastCol;
            return isWall;
        }

        public void ForBox(int row, int col)
        {
            for (int i = 0; i < row; i++)
            {
                for (int t = 0; t < col; t++)  //在向下第i行循环t次
                {

                    bool isWall = IsWall(i, t);
                    if (isWall)
                    {
                        Console.Write('*');
                    }
                    else
                    {
                        Console.Write(' ');
                    }

                    Console.Write(' ');

                    int mod = (i              //向下第i行
                        * col +               //向右总列数
                        t)                    //向右第t列
                        % col;                //第几列换行


                    if (mod == col-1)           //从0开始到最后一列
                    {
                        Console.Write('\n');

                    }

                }
            }
        }

        
    
    }
}
