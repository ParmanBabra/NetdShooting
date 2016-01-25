using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Skill Self")]
    public class SkillSelf : BaseSkill
    {
        [Header("Skill Info")]
        public float MaxDuration;
        public float Duration;

        [Header("Repeat")]
        public bool RepeatApply;
        public float RepeatMaxDuration;
        public float RepeatDuration;


        private bool Using;

        public SkillSelf()
        {
            this.SkillType = SkillType.Self;
        }

        protected override void OnStart()
        {

        }

        protected override void UpdateSkill(float daltaTime)
        {
            if (!Using)
                return;

            if (RepeatApply)
            {

            }

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
