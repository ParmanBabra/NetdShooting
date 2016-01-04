using UnityEngine;
using System.Collections;
using NetdShooting.Core;

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

        public void Start()
        {
            _characterManager = GameHelper.GetCharacterManager();

            _rigidbody = this.GetComponent<Rigidbody>();
            var force = this.transform.forward * Speed;
            _rigidbody.AddForce(force, ForceMode.Impulse);
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

        public void OnCollisionEnter(Collision collision)
        {
            Character character;
            Debug.Break();

            if (!collision.gameObject.TryGetComponent<Character>(out character))
                return;

            character.DealDamage(_damage);

            if (_areaDamage)
            {
                foreach (var otherCharacter in _characterManager.Characters)
                {
                    if (character.Team == otherCharacter.Team)
                        continue;

                    var colliders = otherCharacter.GetComponents<Collider>();

                    foreach (var otherCollider in colliders)
                    {
                        if (areaCollider.bounds.Intersects(otherCollider.bounds))
                        {
                            otherCharacter.DealDamage(_damage);
                            break;
                        }
                    }
                }
            }

            GameObject.Destroy(this);
        }
    }
}
