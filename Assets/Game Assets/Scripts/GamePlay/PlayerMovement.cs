﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace NetdShooting.GamePlay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Character))]
    public class PlayerMovement : MonoBehaviour
    {

        float speed = 6f;            // The speed that the player will move at.


        Vector3 _movement;                   // The vector to store the direction of the player's movement.
        Animator _anim;                      // Reference to the animator component.
        Rigidbody _playerRigidbody;          // Reference to the player's rigidbody.
        Character _character;
#if !MOBILE_INPUT
        int _floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        float _camRayLength = 100f;          // The length of the ray from the camera into the scene.
#endif

        void Awake()
        {
#if !MOBILE_INPUT
            // Create a layer mask for the floor layer.
            _floorMask = LayerMask.GetMask("Floor");
#endif

            // Set up references.
            _anim = GetComponent<Animator>();
            _playerRigidbody = GetComponent<Rigidbody>();
            _character = GetComponent<Character>();
            speed = _character.Speed;
        }


        void FixedUpdate()
        {
            // Store the input axes.
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // Move the player around the scene.
            Move(h, v);

            // Turn the player to face the mouse cursor.
            Turning();

            // Animate the player.
            Animating(h, v);
        }


        void Move(float h, float v)
        {
            // Set the movement vector based on the axis input.
            _movement.Set(h, 0f, v);

            // Normalise the movement vector and make it proportional to the speed per second.
            _movement = _movement.normalized * speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            _playerRigidbody.MovePosition(transform.position + _movement);
        }


        void Turning()
        {
#if !MOBILE_INPUT
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, _camRayLength, _floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                _playerRigidbody.MoveRotation(newRotatation);
            }
#else

            Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Mouse X") , 0f , CrossPlatformInputManager.GetAxisRaw("Mouse Y"));

            if (turnDir != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotatation);
            }
#endif
        }
        void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawRay(this.gameObject.transform.position, this.gameObject.transform.rotation.eulerAngles * 20.0f);
        }

        void Animating(float h, float v)
        {
            var moveDirection = new Vector3(h, 0.0f, v);
            var animationDirection = transform.InverseTransformDirection(moveDirection);
            animationDirection.Normalize();

            // Tell the animator whether or not the player is walking.
            _anim.SetFloat("ForwadMovement", animationDirection.z);
            _anim.SetFloat("SideMovement", animationDirection.x);
        }
    }
}