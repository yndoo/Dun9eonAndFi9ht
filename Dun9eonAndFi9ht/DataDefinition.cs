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
        public int dropItemID;

        public Reward(int exp, int gold, int dropItemID)
        {
            this.exp = exp;
            this.gold = gold;
            this.dropItemID = dropItemID;
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
        MoveStage,
        InventoryScene,
    }
    public enum EItemEquipType
    {
        Armor,
        Weapon,
    }
    public enum EDungeonResultType
    {
        Victory,
        Lose,
        Escaped,
    }
    public enum ESkillTargetType
    {
        Single,
        Multi,
    }
    public enum EMonsterType
    {
        Minion,          
        SmallJungle,     
        MediumJungle,    
        LargeJungle,     
        NeutralObjective,
        BossMonster      
    }
    #endregion

    #region 상수
    static class Constants
    {
        public const int ERROR_RATE = 10;
    }
    #endregion
}
