using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Skill Trigger")]
    public class SkillTrigger : BaseSkill
    {
        float _consumeMaxDuration = 1.0f;
        float _consumeDuration;

        public SkillTrigger()
            : base()
        {
            this.SkillType = SkillType.Trigger;
        }

        protected override void OnStart()
        {

        }

        protected override void UpdateSkill(float daltaTime)
        {
            if (!IsOn)
                return;

            _consumeDuration = Mathf.Max(_consumeDuration - daltaTime, 0);

            if (_consumeDuration != 0)
                return;

            if (OwnerSkill.ManaPoint == 0)
            {
                Off();
                IsOn = false;
            }

            OwnerSkill.ManaPoint = Mathf.Max(OwnerSkill.ManaPoint - MPCost, 0);
            _consumeDuration = _consumeMaxDuration;
        }

        protected override void ProcessUseSkill(float daltaTime)
        {
            base.ProcessUseSkill(daltaTime);
            _consumeDuration = _consumeMaxDuration;
        }

        protected override void ProcessActionSkill()
        {
            base.ProcessActionSkill();
            IsOn = !IsOn;

            if (IsOn)
            {
                On();
                _consumeDuration = _consumeMaxDuration;
            }
            else
                Off();
        }

        private void On()
        {
            //Play Animation

            foreach (var effect in Effects)
            {
                effect.Apply(OwnerSkill);
            }
        }

        private void Off()
        {
            //Play Animation

            foreach (var effect in Effects)
            {
                effect.Unapply(OwnerSkill);
            }
        }

        public bool IsOn { get; private set; }
    }
}