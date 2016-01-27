using UnityEngine;
using System.Collections;
using NetdShooting.Core;
using System.Collections.Generic;
using System;

namespace NetdShooting.GamePlay
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    [AddComponentMenu("Game Play/Projection")]
    public class Projection : MonoBehaviour, IProjection
    {
        [Header("Infomation")]
        public float Speed = 100.0f;

        private Rigidbody _rigidbody;
        private SphereCollider areaCollider;
        private Damage _damage;
        private bool _areaDamage;

        private CharacterManager _characterManager;
        private List<Character> _inAreaCharacter;
        private Character _owner;

        public void Start()
        {
            _characterManager = GameHelper.GetCharacterManager();

            _rigidbody = this.GetComponent<Rigidbody>();
            var force = this.transform.forward * Speed;
            _rigidbody.AddForce(force, ForceMode.Impulse);

            _inAreaCharacter = new List<Character>();
        }

        public void SetDamage(Damage damage, bool areaDamage = false, float radius = 0.0f)
        {
            _damage = damage;
            _areaDamage = areaDamage;

            var spheres = this.GetComponents<SphereCollider>();
            foreach (var sphere in spheres)
            {
                if (sphere.isTrigger)
                    areaCollider = sphere;
            }

            if (areaCollider != null)
                areaCollider.radius = radius;
        }

        public void OnTriggerEnter(Collider other)
        {
            Character character;

            if (!other.gameObject.TryGetComponent<Character>(out character))
                return;

            _inAreaCharacter.Add(character);
        }

        public void OnTriggerExit(Collider other)
        {
            Character character;

            if (!other.gameObject.TryGetComponent<Character>(out character))
                return;

            _inAreaCharacter.Remove(character);
        }

        public void OnCollisionEnter(Collision collision)
        {
            Character character;

            if (!collision.gameObject.TryGetComponent<Character>(out character))
                return;

            if (_owner.Team == character.Team)
                return;

            character.DealDamage(_damage);

            if (_areaDamage)
            {
                foreach (var otherCharacter in _inAreaCharacter)
                {
                    if (_owner.Team == otherCharacter.Team)
                        continue;

                    if (character == otherCharacter)
                        continue;

                    otherCharacter.DealDamage(_damage);
                }
            }

            GameObject.DestroyObject(this.gameObject);
            //GameObject.DestroyImmediate(this);
        }

        public void SetOwner(Character owner)
        {
            _owner = owner;
        }

        public void SetEffect(GameObject effect)
        {
            effect.transform.SetParent(this.transform);
        }

        public float MovingSpeed
        {
            get { return Speed; }
            set { Speed = value; }
        }
    }
}
