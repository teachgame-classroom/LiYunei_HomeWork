using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_03_26_Homework
{
    class Character
    {
        string name;
        int hp;
        int atk;
        Equipment equipment;

        public Character(string name, int hp, int atk)
        {
            this.name = name;
            this.hp = hp;
            this.atk = atk;

            ShowCharacterFile();
        }

        public void ShowCharacterFile()
        {
            Console.WriteLine("name-{0},hp-{1},atk-{2}",name,hp,atk);
        }

        public void Hurt(int amount)
        {
            hp -= amount;
        }

        public void Attack(Character target)
        {
            int amount = atk + GetDamage() - target.GetDefense();
        }

        public void Equip(Equipment equipment)
        {
            this.equipment = equipment;
        }

        public int GetDefense()
        {
            string equip = equipment.ToString();
            string[] strings = equip.Split(',');

            int def = int.Parse(strings[3]);
            return def;
        }

        public int GetDamage()
        {
            string equip = equipment.ToString();
            string[] strings = equip.Split(',');

            int atk = int.Parse(strings[2]);
            return atk;
        }

        public static Character CreateCharacterFromText(string info)
        {
            string[] strings = info.Split(',');

            string name = strings[0];
            int hp = int.Parse(strings[1]);
            int atk = int.Parse(strings[2]);

            Character character = new Character(name, hp, atk);
            return character;
        }
    }
}
