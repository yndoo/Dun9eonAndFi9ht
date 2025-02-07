using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Characters
{
    enum EJobType
    {
        None,
    }
    internal class Player : Character
    {
        private int gold;
        private EJobType job;
        private int maxExp;
        private int curExp;

        public int Gold { get; set; }
        public EJobType Job { get; set; }
        public int MaxExp { get; set; }
        public int CurExp { get; set; }

        public void DisplayStatus()
        {

        }
    }
}
