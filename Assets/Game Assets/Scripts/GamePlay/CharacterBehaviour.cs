using UnityEngine;
using System.Collections;
using NetdShooting.Core;

namespace NetdShooting.GamePlay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("Game Play/Character Behaviour")]
    public class CharacterBehaviour : MonoBehaviour
    {
        public float RadiusRandomMove;
        public float FOV = 90.0f;
        public float Range = 5.0f;

        private Character _character;

        private Animator _anim;                      // Reference to the animator component.
        private NavMeshAgent _agent;

        private Quaternion _lastRotation;
        private Quaternion _desiredRotation;

        private Vector3 _startUpLocation;
        private bool _foundedEnemy;

        private GameObject _foundEnemy;
        private CharacterManager _characterManager;

        private bool _death;

        public void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _characterManager = GameHelper.GetCharacterManager();
            _character = this.gameObject.GetComponent<Character>();

            _agent.speed = _character.Speed;
            _startUpLocation = this.gameObject.transform.position;
        }

        public void Update()
        {
            Detact();
            
            Animating(_agent.velocity.x, _agent.velocity.z);
        }

        private void Animating(float h, float v)
        {
            if (_death)
                return;

            var moveDirection = new Vector3(h, 0.0f, v);
            var animationDirection = transform.InverseTransformDirection(moveDirection);
            animationDirection.Normalize();

            // Tell the animator whether or not the player is walking.
            _anim.SetFloat("ForwadMovement", animationDirection.z);
            _anim.SetFloat("SideMovement", animationDirection.x);
        }

        public void RandomMove()
        {
            if (_death)
                return;

            if (CheckAreadyToRandomTarget())
            {
                var location = CalculateRandomLocation();
                _agent.SetDestination(location);
            }
        }

        private Vector3 CalculateRandomLocation()
        {
            var x = Random.Range(-RadiusRandomMove, RadiusRandomMove);
            var z = Random.Range(-RadiusRandomMove, RadiusRandomMove);

            var randomLocation = _startUpLocation + new Vector3(x, 0.0f, z); ;
            return randomLocation;
        }

        private bool CheckAreadyToRandomTarget()
        {
            float dist = _agent.remainingDistance;
            return (dist != Mathf.Infinity &&
                    _agent.pathStatus == NavMeshPathStatus.PathComplete &&
                    _agent.remainingDistance <= _agent.stoppingDistance);
        }

        public void Detact()
        {
            foreach (var otherCharacter in _characterManager.Characters)
            {
                if (_character.Team == otherCharacter.Team)
                    return;

                if (gameObject.InsideFOV(otherCharacter.gameObject, this.gameObject.transform.forward, FOV, Range))
                {
                    _foundedEnemy = true;
                    _foundEnemy = otherCharacter.gameObject;
                    return;
                }
            }

            _foundedEnemy = false;
            _foundEnemy = null;
        }

        private void OnDrawGizmosSelected()
        {
            var op = this.gameObject.transform.position - (this.gameObject.transform.forward * 0.5f);
            var rotate = this.gameObject.transform.eulerAngles;
            var direction = Quaternion.Euler(rotate) * Vector3.forward;

            Gizmos.color = Color.green;
            MyGizmos.DrawFOV(op, direction, FOV, Range);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.gameObject.transform.position, GetAttackDistance());
        }

        public void Following(GameObject target)
        {
            if (_death)
                return;

            var distance = Mathf.Max(GetAttackDistance() / 2, _agent.radius * 4);

            var moveingPosition = Vector3.MoveTowards(this.gameObject.transform.position, target.transform.position, distance);

            _agent.SetDestination(moveingPosition);
        }

        public void LookAt(GameObject target)
        {
            if (_death)
                return;

            Vector3 lookAtPos;

            lookAtPos = target.transform.position;

            lookAtPos.y = this.gameObject.transform.position.y;

            var diff = lookAtPos - this.gameObject.transform.position;
            if (diff != Vector3.zero && diff.sqrMagnitude > 0)
            {
                _desiredRotation = Quaternion.LookRotation(diff, Vector3.up);
            }

            _lastRotation = Quaternion.Slerp(_lastRotation, _desiredRotation, _agent.angularSpeed * Time.deltaTime);
            this.gameObject.transform.rotation = _lastRotation;
        }

        public void PassAttack()
        {
            if (_death)
                return;

            _character.PassAttack();
        }

        public void ReleaseAttack()
        {
            _character.ReleaseAttack();
        }

        public bool GetFoundedEnemy()
        {
            return _foundEnemy;
        }

        public GameObject GetFoundEnemy()
        {
            return _foundEnemy;
        }

        public bool GetIsAgentToMoveTarget()
        {
            return CheckAreadyToRandomTarget();
        }

        public float GetAttackDistance()
        {
            if (_character == null)
                return 0.0f;

            return _character.AttackDistance;
        }

        public void Death()
        {
            _agent.Stop();
            _death = true;
        }
    }
}