using UnityEngine;
using NetdShooting.Core;
using System.Collections;
using System.Collections.Generic;

namespace NetdShooting.GamePlay
{
    public abstract class BaseSkill : MonoBehaviour
    {
        [Header("Common Infomation")]
        public string SkillName;
        public float CoolDown;
        public float MaxCoolDown;
        public int Level;
        public int MPCost;
        public int AnimationNumber;
        public SkillType SkillType { get; protected set; }
        public SkillEffect[] Effects;

        [Header("Effect")]
        public GameObject FXEffect;
        public string EffectSocketName = "Footer";
        private GameObject _effectObject;


        [Header("Debug")]
        public bool DrawGizmos;
        public bool DrawGizmosOnSelect;

        protected Character OwnerSkill { get; set; }
        protected Animator Animator { get; private set; }
        public bool CanUse { get; protected set; }

        public BaseSkill()
        {
            CanUse = true;
        }

        public void Start()
        {
            OwnerSkill = this.transform.parent.gameObject.GetComponent<Character>();
            Animator = this.OwnerSkill.GetComponent<Animator>();

            if (OwnerSkill == null)
                throw new System.Exception("Can't Find Owner Skill");

            OnStart();
        }


        public void OnDrawGizmos()
        {
            if (DrawGizmos)
                OnDrawActionGizmos();
        }

        public void OnDrawGizmosSelected()
        {
            if (DrawGizmosOnSelect)
                OnDrawActionGizmos();
        }

        //For get skill infomation from data souce
        protected abstract void OnStart();

        public void Update()
        {
            var deltaTime = Time.deltaTime;
            CoolDown = Mathf.Max(CoolDown - deltaTime, 0);

            UpdateSkill(deltaTime);
        }

        public bool Use()
        {
            if (!CoolDowning())
                return false;

            if (!HaveMP())
                return false;

            ProcessUseSkill(Time.deltaTime);

            CoolDown = MaxCoolDown;

            return true;
        }

        public void ActionSkill()
        {
            ProcessActionSkill();
            Animator.SetInteger("UseSkill", 0);
        }

        private UnityEngine.GameObject CreateEffect(Transform location)
        {
            return (GameObject)UnityEngine.Object.Instantiate(FXEffect, location.position, OwnerSkill.transform.rotation);
        }

        public void EndUseSkill()
        {
            ProcessEndUseSkill();
        }

        protected virtual void ProcessUseSkill(float daltaTime)
        {
            OwnerSkill.ManaPoint -= MPCost;
            OwnerSkill.DisableMove();
            Animator.SetInteger("UseSkill", this.AnimationNumber);
        }

        protected virtual void UpdateSkill(float daltaTime) { }

        protected virtual void ProcessActionSkill() { }

        protected virtual void ProcessEndUseSkill()
        {
            OwnerSkill.EnableMove();
        }

        protected virtual void OnStartEffect()
        {
            var socket = OwnerSkill.gameObject.FindObjectWithName(EffectSocketName);

            if (socket == null)
                return;

            if (FXEffect == null)
                return;

            _effectObject = CreateEffect(socket.transform);

            _effectObject.transform.SetParent(OwnerSkill.transform);
            _effectObject.transform.Rotate(Vector3.up, 180.0f);
        }

        protected virtual GameObject CreateEffect()
        {
            var socket = OwnerSkill.gameObject.FindObjectWithName(EffectSocketName);

            if (socket == null)
                return null;

            if (FXEffect == null)
                return null;

            var effect = CreateEffect(socket.transform);
            effect.transform.Rotate(Vector3.up, 180.0f);
            return effect;
        }

        protected virtual void OnEndEffect()
        {
            GameObject.Destroy(_effectObject, 0.01f);
        }

        protected virtual void OnDrawActionGizmos() { }

        protected Damage CreateDmage(DamageType type, int hitDamage, float during)
        {
            Damage damage = new GamePlay.Damage();
            damage.DamageType = type;
            damage.Effects = Effects;
            damage.HitDamage = hitDamage;
            damage.Effects = Effects;
            damage.During = during;
            return damage;
        }

        public bool CoolDowning()
        {
            return CoolDown <= 0;
        }

        public bool HaveMP()
        {
            return OwnerSkill.ManaPoint >= MPCost;
        }
    }
}