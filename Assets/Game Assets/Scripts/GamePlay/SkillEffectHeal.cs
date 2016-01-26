using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    public class SkillEffectHeal : SkillEffectCalculator
    {
        public SkillEffectHeal(SkillEffect effect)
            : base(effect)
        {

        }

        public override float GetEffectValue(Character character)
        {
            return 0;
        }

        public override void Apply(Character character)
        {
            character.HitPoint = Mathf.Min(character.HitPoint + Calculate(character.MaxHitPoint), 
                                           character.MaxHitPoint);
        }

        public override void Unapply(Character character)
        {
            //Don't
        }

        public override float GetBeforeApplyEffectValue(Character character)
        {
            return 0;
        }

    }
}
