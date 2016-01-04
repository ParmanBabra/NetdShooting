using UnityEngine;
using System.Collections;
using NetdShooting.Core;

namespace NetdShooting.GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("Game Play/Character")]
    public class Character : MonoBehaviour
    {
        [Header("Information")]

        public AttackType AttackType;
        public string ClassName;
        public int Level;
        public int Team;

        [Header("Status")]
        public int HitPoint;
        public int MaxHitPoint;
        public int Defend;
        public int MagicDefend;
        public int MaxAttack;
        public int MinAttack;
        public float AttackSpeed; //Per Sec

        [Header("Range Attacking")]
        public GameObject BulletPrefab;
        private GameObject _muzzle;

        IAttack _attack;

        Hashtable _appledEffects;

        class ApplyEffect
        {
            public float ValueBeforeApply { get; set; }
            public float EffectValue { get; set; }
            public SkillEffect Effect { get; set; }
            public float During { get; set; }
        }

        // Use this for initialization
        public void Start()
        {
            switch (AttackType)
            {
                case AttackType.Malee:
                    _attack = new MaleeAttack(this);
                    break;
                case AttackType.Range:
                    _attack = new RangeAttack(this);
                    break;
                default:
                    _attack = new NoneAttack(this);
                    break;
            }

            _appledEffects = new Hashtable();
        }

        public void Update()
        {
            float daltaTime = Time.deltaTime;
            foreach (ApplyEffect applyEffect in _appledEffects)
            {
                applyEffect.During -= daltaTime;

                if (applyEffect.During <= 0)
                    _appledEffects.Remove(applyEffect.Effect.GetType());
            }

        }

        public void DealDamage(Damage damage)
        {
            //apply basic damage
            HitPoint -= damage.HitDamage;

            foreach (var effect in damage.Effects)
            {
                if (_appledEffects.ContainsKey(effect.GetType()))
                {
                    ApplyEffect applyEffect = (ApplyEffect)_appledEffects[effect.GetType()];
                    var oldEffectValue = applyEffect.EffectValue;
                    var newEffectValue = effect.GetEffectValue(applyEffect.ValueBeforeApply);

                    if (newEffectValue > oldEffectValue)
                    {
                        applyEffect.Effect.Unapply(this);
                        effect.Apply(this);
                        applyEffect.EffectValue = newEffectValue;
                        applyEffect.During = damage.During;
                    }
                    else
                        applyEffect.During = damage.During;
                }
                else
                {
                    ApplyEffect applyEffect = new ApplyEffect();
                    applyEffect.During = damage.During;
                    applyEffect.Effect = effect;
                    applyEffect.EffectValue = effect.GetEffectValue(this);
                    applyEffect.ValueBeforeApply = effect.GetValueBeforeApply(this);
                    _appledEffects.Add(effect.GetType(), applyEffect);

                    effect.Apply(this);
                }
            }
        }

        public bool Attack()
        {
            return _attack.Attacking(Time.deltaTime);
        }
    }

}
