using UnityEngine;
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

        [Header("Effects")]
        public SkillEffect[] Effects;


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

        public void EndUseSkill()
        {
            ProcessEndUseSkill();
        }

        protected virtual void ProcessUseSkill(float daltaTime)
        {
            OwnerSkill.DisableMove();
            Animator.SetInteger("UseSkill", this.AnimationNumber);
        }

        protected virtual void UpdateSkill(float daltaTime) { }

        protected virtual void ProcessActionSkill()
        { }

        protected virtual void ProcessEndUseSkill()
        {
            OwnerSkill.EnableMove();
        }

        protected virtual void OnDrawActionGizmos()
        { }

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