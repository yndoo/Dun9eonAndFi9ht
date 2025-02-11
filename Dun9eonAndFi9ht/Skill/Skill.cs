using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Skill
{
    internal class Skill
    {
        private string name;
        private int reducedMana;
        private string desc;

        public Skill(string name, int reducedMana, string desc)
        {
            this.name = name;
            this.reducedMana = reducedMana;
            this.desc = desc;
        }
    }
}
