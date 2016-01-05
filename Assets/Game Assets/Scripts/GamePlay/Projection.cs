using UnityEngine;
using System.Collections;
using NetdShooting.Core;
using System.Collections.Generic;

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
        private List<Character> inAreaCharacter;

        public void Start()
        {
            _characterManager = GameHelper.GetCharacterManager();

            _rigidbody = this.GetComponent<Rigidbody>();
            var force = this.transform.forward * Speed;
            _rigidbody.AddForce(force, ForceMode.Impulse);

            inAreaCharacter = new List<Character>();
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

            areaCollider.radius = radius;
        }

        public void OnTriggerEnter(Collider other)
        {
            Character character;

            if (!other.gameObject.TryGetComponent<Character>(out character))
                return;

            inAreaCharacter.Add(character);
        }

        public void OnTriggerExit(Collider other)
        {
            Character character;

            if (!other.gameObject.TryGetComponent<Character>(out character))
                return;

            inAreaCharacter.Remove(character);
        }

        public void OnCollisionEnter(Collision collision)
        {
            Character character;

            if (!collision.gameObject.TryGetComponent<Character>(out character))
                return;

            character.DealDamage(_damage);

            if (_areaDamage)
            {
                foreach (var otherCharacter in inAreaCharacter)
                {
                    if (character.Team == otherCharacter.Team)
                        continue;

                    otherCharacter.DealDamage(_damage);
                }
            }


            GameObject.DestroyObject(this.gameObject);
        }

        public float MovingSpeed
        {
            get { return Speed; }
            set { Speed = value; }
        }
    }
}
