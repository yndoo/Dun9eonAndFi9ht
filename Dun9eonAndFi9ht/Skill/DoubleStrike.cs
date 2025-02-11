using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Skill;
using System;

public class DoubleStrike : Skill
{
	public DoubleStrike() : base("더블 스트라이크", 15, "공격력 * 1.5 로 2명의 적을 랜덤으로 공격합니다.")
    {
	}

    /// <summary>
    /// 스킬 사용 "더블 스트라이크"
    /// 현재 모든 적을 받아와서 살아있는 적들만 선택
    /// </summary>
    /// <param name="user">스킬을 사용하는 유저</param>
    /// <param name="targets">모든 적 리스트</param>
    public override void UseSkill(Character user, List<Character> targets)
    {
        List<Character> aliveTargets = new List<Character>();
        foreach (Character target in targets)
        {
            if (!target.IsDead)
            {
                aliveTargets.Add(target);
            }
        }

        Random random = new Random();
        // 랜덤하게 2명 선택
        List<Character> selectedTargets = aliveTargets.OrderBy(x => random.Next()).Take(2).ToList();

        // 선택된 대상 공격
        foreach (Character target in selectedTargets)
        {
            float damage = user.Atk * 1.5f;
            target.Damaged(damage);
        }
    }

}
