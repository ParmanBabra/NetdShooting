using System;
using UnityEngine;

namespace NetdShooting.GamePlay
{
    public class PlayerDeathable : IDeathable
    {
        private Character _character;
        private Animator _anime;

        public PlayerDeathable(Character character, Animator anime)
        {
            _character = character;
            _anime = anime;
        }

        public void Death()
        {
            if (_anime != null)
            {
                _anime.SetLayerWeight(1, 0.0f);
                _anime.SetBool("Die", true);
            }
            else
            {
                _character.Die();
            }

        }
    }
}
