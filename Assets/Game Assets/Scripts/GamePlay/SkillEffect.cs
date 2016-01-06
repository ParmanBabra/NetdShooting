using UnityEngine;
using System.Collections;
namespace NetdShooting.GamePlay
{
    [System.Serializable]
    public class SkillEffect
    {
        public bool IsMinus;
        public bool IsStack;
        public float Value;
        public EffectUnit Unit;
        public SkillEffectType EffectType;

        private SkillEffectCalculator _calculater;
        public SkillEffectCalculator Calculater
        {
            get
            {
                if (_calculater != null)
                    return _calculater;

                switch (EffectType)
                {
                    case SkillEffectType.MaxMinAttack:
                        return _calculater = new SkillEffectMaxMinAttack(this);
                }

                return _calculater = new SkillEffectMaxMinAttack(this);
            }
        }

        public float GetEffectValue(Character character)
        {
            return Calculater.GetEffectValue(character);
        }

        public float GetEffectValue(float effectValue)
        {
            return Calculater.Calculate(effectValue);
        }

        public float GetBeforeApplyEffectValue(Character character)
        {
            return Calculater.GetBeforeApplyEffectValue(character);
        }

        public void Apply(Character character)
        {
            Calculater.Apply(character);
        }
        public void Unapply(Character character)
        {
            Calculater.Unapply(character);
        }
    }

    public abstract class SkillEffectCalculator
    {
        protected SkillEffect _effect { get; private set; }

        public SkillEffectCalculator(SkillEffect effect)
        {
            _effect = effect;
        }

        public float Calculate(float attribute)
        {
            float v = 1.0f;
            if (_effect.IsMinus)
                v = -1.0f;

            switch (_effect.Unit)
            {
                case EffectUnit.Amount:
                    return v * _effect.Value;
                case EffectUnit.Percent:
                    return v * (attribute * _effect.Value);
                default:
                    return v * _effect.Value;
            }
        }

        protected int Calculate(int attribute)
        {
            return Mathf.RoundToInt(Calculate((float)attribute));
        }

        public abstract float GetEffectValue(Character character);

        public abstract float GetBeforeApplyEffectValue(Character character);

        public abstract void Apply(Character character);
        public abstract void Unapply(Character character);
    }
}
