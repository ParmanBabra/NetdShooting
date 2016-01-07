using UnityEngine;
using System.Collections;

namespace NetdShooting.GamePlay
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("Game Play/Character Behaviour")]
    public class CharacterBehaviour : MonoBehaviour
    {
        private Character _character;
        private float _speed;

        private Vector3 _movement;                   // The vector to store the direction of the player's movement.
        private Animator _anim;                      // Reference to the animator component.
        private Rigidbody _playerRigidbody;          // Reference to the player's rigidbody.


        void Awake()
        {
            // Set up references.
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        public void Start()
        {
            _character = this.gameObject.GetComponent<Character>();
            _speed = _character.Speed;
        }

        public void Move(float h, float v)
        {
            // Set the movement vector based on the axis input.
            _movement.Set(h, 0f, v);

            // Normalise the movement vector and make it proportional to the speed per second.
            _movement = _movement.normalized * _speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            _playerRigidbody.MovePosition(transform.position + _movement);
        }

        public void Rotate(Vector3 direction)
        {
            Vector3 turnDir = new Vector3(direction.x, 0f, direction.z);

            if (turnDir != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                _playerRigidbody.MoveRotation(newRotatation);
            }
        }

        public void Attack()
        {
            _character.Attack();
        }
    }
}