using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _19_03_26_Homework
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] profileEquipmentString = File.ReadAllLines("./Equipment.csv");

            List<Equipment> equipments = new List<Equipment>();

            for (int i = 1; i < profileEquipmentString.Length; i++)
            {
                Equipment saveEquipment = Equipment.CreateEquipmentFromText(profileEquipmentString[i]);
                equipments.Add(saveEquipment);
            }

            Console.ReadLine();

            string[] profileCharacterString = File.ReadAllLines("./Character.csv");

            List<Character> characters = new List<Character>();

            for (int i = 1; i < profileCharacterString.Length; i++)
            {
                Character saveCharacter = Character.CreateCharacterFromText(profileCharacterString[i]);
                characters.Add(saveCharacter);
            }

            Console.ReadLine();


            characters[0].Equip(equipments[0],false);

            characters[1].Equip(equipments[3],false);

            characters[0].Attack(characters[1]);
            
            characters[1].ShowCharacterFile();

            Console.Read();
        }
    }
}
