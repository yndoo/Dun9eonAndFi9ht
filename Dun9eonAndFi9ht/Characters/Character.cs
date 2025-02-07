using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Characters
{
    internal class Character
    {
        private string name;
        private int maxHp;
        private int atk;
        private int def;
        private int level;
        private int currentHp;

        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Level { get; set; }
        public int CurrentHp { get; set; }

        public virtual void Attack(Character target)
        {

        }

        public virtual void Damaged(int damage)
        {
            
        }

        public virtual void Dead()
        {

        }
    }
}
