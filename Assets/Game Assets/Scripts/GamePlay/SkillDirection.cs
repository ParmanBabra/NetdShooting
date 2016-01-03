using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Skill Direction")]
    public class SkillDirection : BaseSkill
    {
        public bool AreaDamage;
        public float Radius;
        public int Damage = 1;
        public DamageType DamageType = DamageType.Physic;

        public SkillDirection()
        {
            this.SkillType = SkillType.Direction;
        }

        protected override void OnStart()
        {

        }

        protected override void ProcessUseSkill(float daltaTime)
        {

        }
    }
}
