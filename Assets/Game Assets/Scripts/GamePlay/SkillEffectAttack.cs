using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    public class SkillEffectAttack : SkillEffect
    {
        public override void Apply(Character character)
        {
            character.MaxAttack += Calculate(character.MaxAttack);
            character.MinAttack += Calculate(character.MinAttack);
        }

        public override float GetEffectValue(Character character)
        {
            return Calculate(character.MaxAttack);
        }

        public override float GetValueBeforeApply(Character character)
        {
            return character.MaxAttack;
        }

        public override void Unapply(Character character)
        {
            character.MaxAttack -= Calculate(character.MaxAttack);
            character.MinAttack -= Calculate(character.MinAttack);
        }
    }
}
