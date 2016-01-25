using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetdShooting.GamePlay
{
    public class SkillEffectHeal : SkillEffectCalculator
    {
        public SkillEffectHeal(SkillEffect effect)
            : base(effect)
        {

        }

        public override void Apply(Character character)
        {
            throw new NotImplementedException();
        }

        public override float GetBeforeApplyEffectValue(Character character)
        {
            throw new NotImplementedException();
        }

        public override float GetEffectValue(Character character)
        {
            throw new NotImplementedException();
        }

        public override void Unapply(Character character)
        {
            throw new NotImplementedException();
        }
    }
}
