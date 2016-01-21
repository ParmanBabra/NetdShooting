using UnityEngine;
using NetdShooting.Core;
using System.Collections;
using System;

namespace NetdShooting.GamePlay
{
    public class MaleeAttack : IAttack
    {
        CharacterManager _characterManager;
        Character _character;
        Animator _anime;


        public MaleeAttack(Character character, Animator anime)
        {
            _characterManager = GameHelper.GetCharacterManager();
            _character = character;
            _anime = anime;
        }

        private Damage CreateDmage()
        {
            Damage damage = new Damage();
            damage.DamageType = DamageType.Physic;
            damage.HitDamage = UnityEngine.Random.Range(_character.MinAttack, _character.MaxAttack);
            return damage;
        }


        public void OnHit(int combo)
        {
            foreach (Character other in _characterManager.Characters)
            {
                if (other.Team == _character.Team)
                    continue;

                if (_character.gameObject.InsideFOV(other.gameObject,
                                     _character.transform.forward,
                                     _character.FOV,
                                     _character.Range))
                {
                    Damage damage = CreateDmage();
                    other.DealDamage(damage);
                }
            }
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
