using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Skill Self")]
    public class SkillSelf : BaseSkill
    {
        public float MaxDuration;
        public float Duration;
        private bool Using;

        public SkillSelf()
        {
            this.SkillType = SkillType.Self;
        }

        protected override void OnStart()
        {

        }

        protected override void ProcessSkill(float daltaTime)
        {
            if (!Using)
                return;

            Duration = Mathf.Max(Duration - daltaTime, 0);

            if (Duration != 0)
                return;

            Unapply();

            Duration = MaxDuration;
            Using = false;
        }

        protected override void ProcessUseSkill(float daltaTime)
        {
            Apply();
            Duration = MaxDuration;
            Using = true;
        }

        private void Apply()
        {
            //Play Animation

            foreach (var effect in Effects)
            {
                effect.Apply(OwnerSkill);
            }
        }

        private void Unapply()
        {
            //Play Animation

            foreach (var effect in Effects)
            {
                effect.Unapply(OwnerSkill);
            }
        }
    }
}
