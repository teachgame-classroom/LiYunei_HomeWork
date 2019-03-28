using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _19_03_26_Homework
{
    class Equipment
    {
        public int equipmentNum;
        public string equipmentName;
        public int atk;
        public int def;
        public int price;
        public string description;
        public Character owner;

        public Equipment(int equipmentNum,string equipmentName, int atk, int def,int price, string description)
        {
            this.equipmentNum = equipmentNum;
            this.equipmentName = equipmentName;
            this.atk = atk;
            this.def = def;
            this.price = price;
            this.description = description;

            ShowEquipmentFile();
        }

        public void ShowEquipmentFile()
        {
            Console.WriteLine("equipmentNum-{0},equipmentName-{1},atk{2},def-{3},price-{4},descpription-{5}",equipmentNum,equipmentName,atk,def,price,description);
        }

        public static Equipment CreateEquipmentFromText(string info)
        {
            string[] strings = info.Split(',');
            
            for(int i = 0; i < strings.Length; i++)
            {
                //Console.WriteLine(':'+strings[i]);
            }



            int equipmentNum = int.Parse(strings[0]);
            string equipmentName = strings[1];
            int atk = int.Parse(strings[2]);
            int def = int.Parse(strings[3]);
            int price = int.Parse(strings[4]);
            string description = strings[5];

            Equipment equipment = new Equipment(equipmentNum, equipmentName, atk, def, price, description);
            return equipment;
        }
    }
}
