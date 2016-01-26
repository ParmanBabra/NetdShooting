using UnityEngine;
using System.Collections;
using NetdShooting.Core;
using System.Collections.Generic;
using System;

namespace NetdShooting.GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
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
        public int ManaPoint;
        public int MaxManaPoint;
        public int Defend;
        public int MagicDefend;
        public int MaxAttack;
        public int MinAttack;
        public float AttackSpeed; //Per Sec
        public float Speed;
        public float AttackDistance;

        [Header("Range Attacking")]
        public GameObject BulletPrefab;
        private GameObject _muzzle;

        [Header("Malee Attacking")]
        public float FOV;

        IAttack _attack;
        IDeathable _deathable;

        Dictionary<Type, ApplyEffect> _appledEffects;
        List<ApplyEffect> _stackEffect;

        CharacterManager _characterManager;

        class ApplyEffect
        {
            public float EffectValue { get; set; }
            public float BeforeApplyEffectValue { get; set; }
            public SkillEffect Effect { get; set; }
            public float During { get; set; }
        }

        // Use this for initialization
        public void Start()
        {
            var anime = GetComponent<Animator>();

            switch (AttackType)
            {
                case AttackType.Malee:
                    _attack = new MaleeAttack(this, anime);
                    break;
                case AttackType.Range:
                    _attack = new RangeAttack(this, anime);
                    break;
                default:
                    _attack = new NoneAttack(this);
                    break;
            }

            _deathable = new PlayerDeathable(this, anime);

            _appledEffects = new Dictionary<Type, ApplyEffect>();
            _stackEffect = new List<ApplyEffect>();

            ManaPoint = MaxManaPoint;
            HitPoint = MaxHitPoint;
            _characterManager = GameHelper.GetCharacterManager();
        }

        public void Update()
        {
            float daltaTime = Time.deltaTime;

            ApplyEffect[] appledEffects = new ApplyEffect[_appledEffects.Values.Count];
            _appledEffects.Values.CopyTo(appledEffects, 0);

            foreach (ApplyEffect applyEffect in appledEffects)
            {
                applyEffect.During -= daltaTime;

                if (applyEffect.During <= 0)
                {
                    applyEffect.Effect.Unapply(this);
                    _appledEffects.Remove(applyEffect.Effect.GetType());
                }
            }

            foreach (ApplyEffect applyEffect in _stackEffect.ToArray())
            {
                applyEffect.During -= daltaTime;

                if (applyEffect.During <= 0)
                {
                    applyEffect.Effect.Unapply(this);
                    _stackEffect.Remove(applyEffect);
                }
            }
        }

        public void DealDamage(Damage damage)
        {
            //apply basic damage
            HitPoint -= damage.HitDamage;

            foreach (var effect in damage.Effects)
            {
                if (!effect.IsStack)
                {
                    ApplyEffect applyEffect = new ApplyEffect();
                    applyEffect.During = damage.During;
                    applyEffect.Effect = effect;
                    applyEffect.EffectValue = effect.GetEffectValue(this);
                    applyEffect.BeforeApplyEffectValue = effect.GetBeforeApplyEffectValue(this);
                    _stackEffect.Add(applyEffect);

                    effect.Apply(this);
                }
                else
                {
                    if (_appledEffects.ContainsKey(effect.GetType()))
                    {
                        ApplyEffect applyEffect = (ApplyEffect)_appledEffects[effect.GetType()];
                        var oldEffectValue = applyEffect.EffectValue;
                        var newEffectValue = effect.GetEffectValue(applyEffect.BeforeApplyEffectValue);

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
                        applyEffect.BeforeApplyEffectValue = effect.GetBeforeApplyEffectValue(this);
                        _appledEffects.Add(effect.GetType(), applyEffect);

                        effect.Apply(this);
                    }
                }
            }

            if (HitPoint <= 0)
            {
                SendMessage("Death");
                _deathable.Death();
            }

            Debug.Log(string.Format("{0} take damage {1}", this.name, damage.HitDamage));
        }

        public void OnHit(int combo)
        {
            _attack.OnHit(combo);
        }

        public void Die()
        {
            _characterManager.DestoryCharacter(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            MyGizmos.DrawFOV(transform.position + new Vector3(0, 0.5f, 0), transform.forward, FOV, AttackDistance);
        }

        public void PassAttack()
        {
            _attack.PassAttacking(Time.deltaTime);
        }

        public void ReleaseAttack()
        {
            _attack.ReleaseAttack(Time.deltaTime);
        }

        public void DisableMove()
        {
            this.SendMessage("DisableMovement");
        }

        public void EnableMove()
        {
            this.SendMessage("EnableMovement");
        }
    }
}
