using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    public class SkillEffectMaxMinAttack : SkillEffectCalculator
    {
        private int _max;
        private int _min;

        public SkillEffectMaxMinAttack(SkillEffect effect)
            : base(effect)
        {

        }

        public override float GetEffectValue(Character character)
        {
            return Calculate(character.MaxAttack);
        }

        public override void Apply(Character character)
        {
            _max = Calculate(character.MaxAttack);
            _min = Calculate(character.MinAttack);
            character.MaxAttack += _max;
            character.MinAttack += _min;
        }

        public override void Unapply(Character character)
        {
            character.MaxAttack -= _max;
            character.MinAttack -= _min;
        }

        public override float GetBeforeApplyEffectValue(Character character)
        {
            return character.MaxAttack;
        }
    }
}
