using UnityEngine;
using System.Collections;
using System;
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
            _muzzle = character.gameObject.FindGameObjectInHierarchyWithTag("Spawn Bullet Location");
            _maxCoolDown = character.AttackSpeed;
        }

        public bool Attacking(float daltaTime)
        {
            _coolDown -= daltaTime;

            if (!canAttack())
                return false;

            //Play Animation

            //Create Bullet
            CreateBullet();

            //Update Cooldown
            if (_coolDown <= 0)
                _coolDown = _maxCoolDown;

            return true;
        }

        private UnityEngine.Object CreateBullet()
        {
            return UnityEngine.Object.Instantiate(_character.BulletPrefab, _muzzle.transform.position, _muzzle.transform.rotation);
        }

        private bool canAttack()
        {
            return _coolDown <= 0;
        }
    }
}
