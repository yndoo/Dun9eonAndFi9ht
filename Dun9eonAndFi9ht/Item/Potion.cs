using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Item
{
    public class Potion
    {
        public string name;
        public bool isPercent;
        public float changeHp;
        public float changeMp;
        public float changeAtk;
        public float changeDef;
        public float changeCrt;
        public float changeMiss;
        int duration;

        public Potion(string name, bool isPercent, float hp, float mp, float atk, float def, float crt, float miss, int duration)
        {
            this.name = name;
            this.isPercent = isPercent;
            this.changeHp = hp;
            this.changeMp = mp;
            this.changeAtk = atk;
            this.changeDef = def;
            this.changeCrt = crt;
            this.changeMiss = miss;
            this.duration = duration;
        }

        public void UsePotion(Character character)
        {
            if (isPercent)
            {
                if(duration == 0)
                {
                    character.CurrentHp += changeHp;
                    character.CurrentMp += changeMp;
                    character.Atk += changeAtk;
                    character.Def += changeDef;
                    character.Crt += changeCrt;
                    character.Miss += changeMiss;
                }
                else
                {

                }
            }
            else 
            {
                if(duration == 0)
                {

                }
                else
                {

                }
            }
        }

        private float CalResult(float chStat, float percent)
        {
            if (chStat == 0 || percent == 0) return 0;
            return chStat * percent;
        }

        private void 
    }
}
