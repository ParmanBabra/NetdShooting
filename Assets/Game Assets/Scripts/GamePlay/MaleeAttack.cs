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
        int _team;
        float _fov;
        float _range;

        int _minAttack;
        int _maxAttack;

        public MaleeAttack(Character character, Animator anime)
        {
            _characterManager = GameHelper.GetCharacterManager();
            _character = character;
            _anime = anime;
            _team = _character.Team;

            _fov = _character.FOV;
            _range = _character.Range;
            _minAttack = _character.MinAttack;
            _maxAttack = _character.MaxAttack;
        }


        public void OnHit(int combo)
        {
            foreach (Character other in _characterManager.Characters)
            {
                if (other.Team == _team)
                    continue;

                if (_character.gameObject.InsideFOV(other.gameObject,
                                     _character.transform.forward,
                                     _fov,
                                     _range))
                {
                    Damage damage = new Damage();
                    damage.DamageType = DamageType.Physic;
                    damage.HitDamage = UnityEngine.Random.Range(_minAttack, _maxAttack);
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
