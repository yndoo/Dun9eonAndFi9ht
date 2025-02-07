using Dun9eonAndFi9ht.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class Dungeon : Scene
    {
        private Player player;
        private List<Monster> monsterList;
        private bool isPlayerWin;
        public Player Player { get; set; }
        public List<Monster> MonsterList { get; set; }
        public bool IsPlayerWin { get; set; }
        private void EnterDungeon()
        {

        }
    }
}
