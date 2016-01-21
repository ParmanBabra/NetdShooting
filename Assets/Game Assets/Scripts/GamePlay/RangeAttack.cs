using UnityEngine;
using System.Collections;
using NetdShooting.Core;
using System;

namespace NetdShooting.GamePlay
{
    public class RangeAttack : IAttack
    {
        private Character _character;
        private GameObject _muzzle;
        Animator _anime;

        private float _coolDown;
        private float _maxCoolDown;


        public RangeAttack(Character character, Animator anime)
        {
            _character = character;
            _muzzle = character.gameObject.FindMuzzle("Spawn Bullet");
            _maxCoolDown = character.AttackSpeed;
            _anime = anime;
        }

        private Damage CreateDmage()
        {
            Damage damage = new GamePlay.Damage();
            damage.DamageType = DamageType.Physic;
            damage.During = 0;
            damage.Effects = new SkillEffect[0];
            damage.HitDamage = UnityEngine.Random.Range(_character.MinAttack, _character.MaxAttack);
            return damage;
        }

        private UnityEngine.GameObject CreateBullet()
        {
            return (GameObject)UnityEngine.Object.Instantiate(_character.BulletPrefab, _muzzle.transform.position, _muzzle.transform.rotation);
        }

        public bool PassAttacking(float daltaTime)
        {
            _anime.SetBool("Attacking", true);
            return true;
        }

        public bool ReleaseAttack(float daltaTime)
        {
            _anime.SetBool("Attacking", false);
            return true;
        }

        public void OnHit(int combo)
        {
            //throw new NotImplementedException();
            var bullet = CreateBullet();
            IProjection projection;

            if (!bullet.TryGetComponent<IProjection>(out projection))
                Debug.LogWarning("Can't get projection for bullet prefab");

            var damage = CreateDmage();
            projection.SetDamage(damage);
            projection.SetOwner(_character);
        }
    }
}
