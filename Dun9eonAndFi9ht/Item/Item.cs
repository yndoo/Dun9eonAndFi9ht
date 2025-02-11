using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;

namespace Dun9eonAndFi9ht.Item
{
    public class Item
    {
        public string Name { get; }
        public EItemEquipType EquipSlot {  get; }
        public float MaxHp { get; }
        public float MaxMp { get; }
        public float Atk { get; }
        public float Def { get; }
        public float CriticalRate { get; }
        public float CriticalDamage {  get; }
        public float MissRate { get; }

        public Item(string name, EItemEquipType equipslot, float maxHp, float maxMp, float atk, float def, float criticalRate, float criticalDamage, float missRate)
        {
            Name = name;
            EquipSlot = equipslot;
            MaxHp = maxHp;
            MaxMp = maxMp;
            Atk = atk; 
            Def = def;
            CriticalRate = criticalRate;
            CriticalDamage = criticalDamage;
            MissRate = missRate;
        }
        
    }
}
