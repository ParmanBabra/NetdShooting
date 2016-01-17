using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    public class NoneAttack : IAttack
    {
        private Character _character;

        public NoneAttack(Character character)
        {
            _character = character;
        }

        public bool PassAttacking(float daltaTime)
        {
            return true;
        }

        public bool ReleaseAttack(float daltaTime)
        {
            return true;
        }
    }
}

