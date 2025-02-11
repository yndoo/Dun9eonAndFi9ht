using DataDefinition;
using Dun9eonAndFi9ht.Skill;
using System;

namespace Dun9eonAndFi9ht.Skill
{
    public static class SkillManager
    {
        private static Dictionary<EJobType, List<SkillBase>> playerSkills = new Dictionary<EJobType, List<SkillBase>>
        {
            { EJobType.Warrior, new List<SkillBase> { new AlphaStrike(), new DoubleStrike() } },
            { EJobType.Mage, new List<SkillBase> { new Fireball(), new IceSpear() } },
            { EJobType.Rogue, new List<SkillBase> { new VitalStrike(), new SlashFrenzy() } }
        };

        // To Do : 몬스터 스킬 관련 구현되면 사용하기
        // private static Dictionary<EMonsterType, List<SkillBase>> monsterSkills = new Dictionary<EMonsterType, List<SkillBase>>
        // {
        //     { EMonsterType.Goblin, new List<SkillBase> { new ClawSlash(), new PoisonBite() } },
        //     { EMonsterType.Dragon, new List<SkillBase> { new FireBreath(), new TailWhip() } }
        // };
       
        public static List<SkillBase> GetPlayerSkills(EJobType job)
        {
            return playerSkills.ContainsKey(job) ? playerSkills[job] : new List<SkillBase>();
        }

        // To Do : 몬스터 스킬 관련 구현되면 사용하기
        // public static List<SkillBase> GetMonsterSkills(EMonsterType monsterType)
        // {
        //     return monsterSkills.ContainsKey(monsterType) ? monsterSkills[monsterType] : new List<SkillBase>();
        // }
    }
}