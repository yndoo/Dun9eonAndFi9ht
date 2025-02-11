using DataDefinition;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.Scenes;
using Dun9eonAndFi9ht.Skill;
using System;

namespace Dun9eonAndFi9ht.Skill
{
     class SkillManager
    {

        private static SkillManager? instance;
        public static SkillManager Instance => instance ??= new SkillManager();


        private SkillManager()
        {
            LoadMonsterSkills();
        }

        private static Dictionary<EJobType, List<SkillBase>> playerSkills = new Dictionary<EJobType, List<SkillBase>>
        {
            { EJobType.Warrior, new List<SkillBase> { new AlphaStrike(), new DoubleStrike() } },
            { EJobType.Mage, new List<SkillBase> { new Fireball(), new IceSpear() } },
            { EJobType.Rogue, new List<SkillBase> { new VitalStrike(), new SlashFrenzy() } }
        };

        private static Dictionary<EMonsterType, List<SkillBase>> monsterSkills = new Dictionary<EMonsterType, List<SkillBase>>();

        /// <summary>
        /// 몬스터 스킬을 json에서 로드
        /// </summary>
        private void LoadMonsterSkills()
        {
            DataTableManager dtManager = DataTableManager.Instance;

            // Enum의 각 값을 순회하면서 초기화
            foreach (EMonsterType type in Enum.GetValues(typeof(EMonsterType)))
            {
                monsterSkills[type] = new List<SkillBase>();
            }

            // json스킬 개수만큼 반복
            for (int i = 0; i < 6; i++)
            {
                Dictionary<string, object>? skillData = dtManager.GetDBData("monsterSkill", i);
                if (skillData == null)
                {
                    Console.WriteLine($"MonsterSkill 데이터를 찾을 수 없습니다. ID: {i}");
                    continue;
                }

                string skillName = skillData["Name"].ToString();
                float damageMultiplier = float.Parse(skillData["DamageMultiplier"].ToString());
                int mpCost = Convert.ToInt32(skillData["MpCost"].ToString());

                MonsterSkill newSkill = new MonsterSkill(skillName, mpCost, "", damageMultiplier);


                // 몬스터 타입 가져오기
                if (!Enum.TryParse(skillData["MonsterType"].ToString(), out EMonsterType monsterType))
                {
                    Console.WriteLine($"잘못된 몬스터 타입: {skillData["MonsterType"]}");
                    continue;
                }

                if (monsterSkills.ContainsKey(monsterType))
                {
                    monsterSkills[monsterType].Add(newSkill);
                }
                else
                {
                    Console.WriteLine($"존재하지 않는 몬스터 타입: {monsterType}");
                }
            }
        }

        /// <summary>
        /// 플레이어 및 몬스터의 직업 또는 유형별로 스킬 전달
        /// </summary>
        /// <param name="job">직업 유형</param>
        /// <param name="monsterType">몬스터 유형</param>
        /// <returns>선별된 스킬 부여</returns>
        public List<SkillBase> GetPlayerSkills(EJobType job)
        {
            return playerSkills.ContainsKey(job) ? playerSkills[job] : new List<SkillBase>();
        }
        public List<SkillBase> GetMonsterSkills(EMonsterType monsterType)
        {
            return monsterSkills.ContainsKey(monsterType) ? monsterSkills[monsterType] : new List<SkillBase>();
        }
    }
}