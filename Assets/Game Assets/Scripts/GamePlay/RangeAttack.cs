﻿using UnityEngine;
using System.Collections;
using NetdShooting.Core;

namespace NetdShooting.GamePlay
{
    public class RangeAttack : IAttack
    {
        private Character _character;
        private GameObject _muzzle;

        private float _coolDown;
        private float _maxCoolDown;


        public RangeAttack(Character character)
        {
            _character = character;
            _muzzle = character.gameObject.FindMuzzle("Spawn Bullet");
            _maxCoolDown = character.AttackSpeed;
        }

        public bool Attacking(float daltaTime)
        {
            _coolDown -= daltaTime;

            if (!canAttack())
                return false;

            //Play Animation

            //Create Bullet
            var bullet = CreateBullet();
            IProjection projection;

            if (!bullet.TryGetComponent<IProjection>(out projection))
                Debug.LogWarning("Can't get projection for bullet prefab");

            var damage = CreateDmage();
            projection.SetDamage(damage);

            //Update Cooldown
            if (_coolDown <= 0)
                _coolDown = _maxCoolDown;

            return true;
        }

        private Damage CreateDmage()
        {
            Damage damage = new GamePlay.Damage();
            damage.DamageType = DamageType.Physic;
            damage.During = 0;
            damage.Effects = new SkillEffect[0];
            damage.HitDamage = Random.Range(_character.MinAttack, _character.MaxAttack);
            return damage;
        }

        private UnityEngine.GameObject CreateBullet()
        {
            return (GameObject)UnityEngine.Object.Instantiate(_character.BulletPrefab, _muzzle.transform.position, _muzzle.transform.rotation);
        }

        private bool canAttack()
        {
            return _coolDown <= 0;
        }
    }
}