using UnityEngine;
using System.Collections;
using System;
using NetdShooting.Core;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Skill Direction")]
    public class SkillBasicDirection : BaseSkill
    {
        [Header("Skill Infomation")]
        public bool AreaDamage;
        public float Radius;
        public int Damage = 1;
        public DamageType DamageType = DamageType.Physic;
        public float During = 0;

        [Header("Skill Prefab")]
        public GameObject BulletPrefab;
        public string MuzzleSlotName;
        public float MovingSpeed;

        private GameObject _muzzle;
        private CharacterManager _characterManager;

        public SkillBasicDirection()
            : base()
        {
            this.SkillType = SkillType.Direction;
        }

        protected override void OnStart()
        {
            if (BulletPrefab == null)
                throw new System.Exception("Bullet Prefab is empty.");

            _muzzle = OwnerSkill.gameObject.FindMuzzle(MuzzleSlotName);

            if (_muzzle == null)
                throw new System.Exception("Can't find muzzle.");
        }

        protected override void ProcessActionSkill()
        {
            base.ProcessActionSkill();

            var bulletGO = (GameObject)UnityEngine.Object.Instantiate(BulletPrefab, _muzzle.transform.position, _muzzle.transform.rotation);
            IProjection projection;

            if (!bulletGO.TryGetComponent<IProjection>(out projection))
                Debug.LogWarning("Can't get projection for bullet prefab");

            var damage = CreateDmage(DamageType, Damage, During);
            projection.SetDamage(damage, AreaDamage, Radius);
            projection.SetOwner(OwnerSkill);
            projection.MovingSpeed = MovingSpeed;
            projection.SetEffect(CreateEffect());
        }


    }
}
