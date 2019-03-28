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
            int finalequipment = 0;

            if (HasEquiment())
            {
                finalequipment = amount - equipment.def;
            }

            finalequipment = Math.Max(finalequipment, 1);

            hp -= amount;
            Console.WriteLine("{0} suffer {1} damage, hp :{2}", name, amount, hp);
        }

        public void Attack(Character target)
        {
            int amount = atk + GetDamage();

            target.Hurt(amount);
        }

        public void Equip(Equipment equipment, bool grabByForce)
        {
            if(equipment.owner == null)
            {
                if (HasEquiment())
                {
                    this.equipment.owner = null;
                }

                this.equipment = equipment;
                equipment.owner = this;

                if (HasEquiment())
                {
                    Console.WriteLine("{0} Equip {1}", name, equipment.equipmentName);
                }
                else
                {
                    Console.WriteLine("Can not Equip!");
                }
            }
            else
            {
                if(grabByForce == false)
                {
                    Console.WriteLine("{0} has been equip by {1}, can not equip by{2}", equipment.equipmentName, equipment.owner.name, this.name);
                }
                else
                {
                    equipment.owner.UnEquip();
                    this.equipment = equipment;
                    equipment.owner = this;

                    if (HasEquiment())
                    {
                        Console.WriteLine("{0} Equip {1}", name, equipment.equipmentName);
                    }
                    else
                    {
                        Console.WriteLine("Can not Equip !");
                    }
                }
            }

        }

        public void UnEquip()
        {
            if (HasEquiment())
            {
                string equipmentName = equipment.equipmentName;
                equipment.owner = null;
                equipment = null;
                Console.WriteLine("{0} unequip {1}", this.name,equipmentName);
            }
            else
            {
                Console.WriteLine("{0}haven't equipment", this.name);
            }

        }

        public int GetDefense()
        {
            if (HasEquiment())
            {
                return equipment.def;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 判断是否有装备
        /// </summary>
        /// <returns></returns>
        private bool HasEquiment()
        {
            return equipment != null;
        }

        public int GetDamage()
        {
            if(equipment != null)
            {
                return equipment.atk;
            }
            else
            {
                return 0;
            }
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
