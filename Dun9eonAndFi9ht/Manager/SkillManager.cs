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
            LoadPlayerSkills();
            LoadMonsterSkills();
        }

        private static Dictionary<EJobType, List<SkillBase>> playerSkills = new Dictionary<EJobType, List<SkillBase>>();


        /// <summary>
        /// 플레이어 스킬을 json에서 로드
        /// </summary>
        private void LoadPlayerSkills()
        {
            DataTableManager dtManager = DataTableManager.Instance;

            // Enum의 각 값을 순회하면서 초기화
            foreach (EJobType job in Enum.GetValues(typeof(EJobType)))
            {
                playerSkills[job] = new List<SkillBase>();
            }

            for (int i = 0; i < 6; i++)
            {
                Dictionary<string, object>? skillData = dtManager.GetDBData("playerSkill", i);
                if (skillData == null)
                {
                    Console.WriteLine($"PlayerSkill 데이터를 찾을 수 없습니다. ID: {i}");
                    continue;
                }

                string skillName = skillData["Name"].ToString();
                int mpCost = Convert.ToInt32(skillData["MpCost"].ToString());
                float value = float.Parse(skillData["Value"].ToString());
                string description = skillData["Description"].ToString();

                if (!Enum.TryParse(skillData["TargetType"].ToString(), out ESkillTargetType targetType))
                {
                    Console.WriteLine($"잘못된 스킬 대상 타입: {skillData["TargetType"]}");
                    continue;
                }

                if (!Enum.TryParse(skillData["Job"].ToString(), out EJobType jobType))
                {
                    Console.WriteLine($"잘못된 직업 타입: {skillData["Job"]}");
                    continue;
                }

                SkillBase newSkill = CreateSkill(i, skillName, mpCost, description, targetType, value);

                // 해당 직업에 스킬 추가
                if (newSkill != null)
                {
                    playerSkills[jobType].Add(newSkill);
                }
            }
        }

        /// <summary>
        /// json의 id값에 따라 해당 스킬을 생성시킴.
        /// </summary>
        /// <param name="id">json의 id값</param>
        /// <param name="name">이름</param>
        /// <param name="mpCost">소모mp</param>
        /// <param name="desc">설명</param>
        /// <param name="targetType">EskillTargetType 유형</param>
        /// <param name="value">고유 값(데미지 등)</param>
        /// <returns>해당 스킬 반환</returns>
        SkillBase CreateSkill(int id, string name, int mpCost, string desc, ESkillTargetType targetType, float value)
        {
            switch (id)
            {
                case 0: return new AlphaStrike(name, mpCost, desc, targetType, value);

                case 1: return new DoubleStrike(name, mpCost, desc, targetType, value);

                case 2: return new Fireball(name, mpCost, desc, targetType, value);

                case 3: return new IceSpear(name, mpCost, desc, targetType, value);

                case 4: return new VitalStrike(name, mpCost, desc, targetType, value);

                case 5: return new SlashFrenzy(name, mpCost, desc, targetType, value);

                default: break;
            }
            return null;
        }


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
                float Value = float.Parse(skillData["Value"].ToString());
                int mpCost = Convert.ToInt32(skillData["MpCost"].ToString());

                MonsterSkill newSkill = new MonsterSkill(skillName, mpCost, "", ESkillTargetType.Single, Value);

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