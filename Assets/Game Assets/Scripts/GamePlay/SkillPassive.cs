using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Skill Passive")]
    public class SkillPassive : BaseSkill
    {
        public SkillPassive()
        {
            this.SkillType = SkillType.Passive;
        }

        protected override void OnStart()
        {
            Apply();
        }

        private void Apply()
        {
            //Play Animation

            foreach (var effect in Effects)
            {
                effect.Apply(OwnerSkill);
            }
        }
    }
}
