using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinition
{
    #region 구조체
    public struct Reward
    {
        public int exp;
        public int gold;

        public Reward(int exp, int gold)
        {
            this.exp = exp;
            this.gold = gold;
        }
    }
    #endregion

    #region 열거형
    public enum EJobType
    {
        None = -1,
        Warrior,
        Mage,
        Rogue,
    }
    public enum ESceneType
    {
        StartScene,
        Dungeon,
        PlayerStat,
        MoveStage
    }
    public enum EDungeonResultType
    {
        Victory,
        Lose,
        Escaped,
    }
    #endregion

    #region 상수
    static class Constants
    {
        public const int ERROR_RATE = 10;
        public const float CRITICAL_DAMAGE_RATE = 1.6f;
        public const int CRITICAL_RATE = 15;
        public const int MISS_RATE = 10;
    }
    #endregion
}
