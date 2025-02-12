using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Manager;

namespace Dun9eonAndFi9ht.Items
{
    public class Item
    {
        public string Name { get; }
        public EItemEquipType EquipType {  get; }
        public float MaxHp { get; }
        public float MaxMp { get; }
        public float Atk { get; }
        public float Def { get; }
        public float CriticalRate { get; }
        public float CriticalDamage {  get; }
        public float MissRate { get; }
        public bool IsEquipped { get; set; }
        public int Id { get; set; }

        public Item(int id, string name, EItemEquipType equipslot, float maxHp, float maxMp, float atk, float def, float criticalRate, float criticalDamage, float missRate/*, bool isEquipped*/)
        {
            Name = name;
            EquipType = equipslot;
            MaxHp = maxHp;
            MaxMp = maxMp;
            Atk = atk; 
            Def = def;
            CriticalRate = criticalRate;
            CriticalDamage = criticalDamage;
            MissRate = missRate;
            IsEquipped = false;
            Id = id;
        }
        
        public string ItemDisplay()
        {
            string desc1 = IsEquipped ? "[E] " : "";
            desc1 += $"{Name}";
            desc1 = desc1.PadRight(20);
            desc1 += "\t";
            string desc2 = "";
            desc2 += MaxHp   != 0 ? $" | 최대 체력 +{MaxHp}" : "";
            desc2 += MaxMp   != 0 ? $" | 최대 마나 +{MaxMp}" : "";
            desc2 += Atk     != 0 ? $" | 공격력 +{Atk}" : "";
            desc2 += Def     != 0 ? $" | 방어력 +{Def}" : "";
            desc2 += CriticalRate != 0 ? $" | 치명타 확률 +{CriticalRate}%" : "";
            desc2 += CriticalDamage != 0 ? $" | 치명타 데미지 +{CriticalDamage}%" : "";
            desc2 += MissRate != 0 ? $" | 회피 확률 +{MissRate}%" : "";
            return desc1 + desc2;
        }
    }
}
