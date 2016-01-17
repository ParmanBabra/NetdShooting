using UnityEngine;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    public class MaleeAttack : IAttack
    {
        private Character _character;
        private Animator _anime;

        public MaleeAttack(Character character, Animator anime)
        {
            _character = character;
            _anime = anime;
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
    }
}
