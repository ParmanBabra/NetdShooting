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

        public bool Attacking(float daltaTime)
        {
            return true;
        }
    }
}

