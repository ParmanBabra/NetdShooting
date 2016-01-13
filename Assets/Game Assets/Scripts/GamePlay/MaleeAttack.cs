using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    public class MaleeAttack : IAttack
    {
        private Character _character;

        private float _coolDown;
        private float _maxCoolDown;

        public MaleeAttack(Character character)
        {
            _character = character;
            _maxCoolDown = character.AttackSpeed;
        }

        public bool Attacking(float daltaTime)
        {
            _coolDown -= daltaTime;

            if (!canAttack())
                return false;

            Debug.Log("Attacking");

            //Update Cooldown
            if (_coolDown <= 0)
                _coolDown = _maxCoolDown;
            return true;
        }

        private bool canAttack()
        {
            return _coolDown <= 0;
        }
    }
}
