using UnityEngine;
using System.Collections;

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
        public SkillType SkillType { get; protected set; }

        [Header("Effects")]
        public SkillEffect[] Effects;

        [Header("Debug")]
        public bool DrawGizmos;
        public bool DrawGizmosOnSelect;

        protected Character OwnerSkill { get; set; }

        public void Start()
        {
            OwnerSkill = this.transform.parent.gameObject.GetComponent<Character>();

            if (OwnerSkill == null)
                throw new System.Exception("Can't Find Owner Skill");

            Effects = new SkillEffect[0];

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
            CoolDown = Mathf.Max(CoolDown - Time.deltaTime, 0);
        }

        public void Use()
        {
            if (!canUse())
                return;

            ProcessUseSkill(Time.deltaTime);

            CoolDown = MaxCoolDown;
        }

        protected abstract void ProcessUseSkill(float daltaTime);

        protected virtual void OnDrawActionGizmos()
        {

        }

        private bool canUse()
        {
            return CoolDown <= 0;
        }
    }
}