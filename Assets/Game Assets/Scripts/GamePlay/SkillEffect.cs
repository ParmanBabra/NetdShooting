using UnityEngine;
using System.Collections;
namespace NetdShooting.GamePlay
{
    public abstract class SkillEffect
    {
        public bool IsMinus;
        public float Value;
        public EffectUnit Unit;

        protected float Calculate(float attribute)
        {
            float v = 1.0f;
            if (IsMinus)
                v = -1.0f;

            switch (Unit)
            {
                case EffectUnit.Amount:
                    return v * (attribute);
                case EffectUnit.Percent:
                    return v * (attribute * Value);
                default:
                    return v * (attribute);
            }
        }

        protected int Calculate(int attribute)
        {
            return Mathf.RoundToInt(Calculate(attribute));
        }

        public float GetEffectValue(float attribute)
        {
            return Calculate(attribute);
        }

        public int GetEffectValue(int attribute)
        {
            return Calculate(attribute);
        }

        public abstract float GetEffectValue(Character character);
        public abstract float GetValueBeforeApply(Character character);
        public abstract void Apply(Character character);
        public abstract void Unapply(Character character);
    }
}
