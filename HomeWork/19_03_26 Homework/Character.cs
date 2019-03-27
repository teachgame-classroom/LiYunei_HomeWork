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
            target.Hurt(amount);
        }

        public void Equip(Equipment equipment)
        {
            this.equipment = equipment;
        }

        public int GetDefense()
        {
            return equipment.def;
        }

        public int GetDamage()
        {
            return equipment.atk;
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
