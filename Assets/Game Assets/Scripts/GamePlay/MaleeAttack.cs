using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    public class MaleeAttack : IAttack
    {
        private Character _character;

        public MaleeAttack(Character character)
        {
            _character = character;
        }

        public bool Attacking(float daltaTime)
        {
            return true;
        }
    }
}
