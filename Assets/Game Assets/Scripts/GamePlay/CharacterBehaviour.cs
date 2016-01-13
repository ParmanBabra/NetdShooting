using UnityEngine;
using System.Collections;

namespace NetdShooting.GamePlay
{
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

        public void Awake()
        {
            // Set up references.
            _agent = GetComponent<NavMeshAgent>();
            _characterManager = GameHelper.GetCharacterManager();
            _character = this.gameObject.GetComponent<Character>();
        }

        public void Start()
        {
            _agent.speed = _character.Speed;
            _startUpLocation = this.gameObject.transform.position;
        }

        public void Update()
        {
            Detact();
        }

        public void RandomMove()
        {
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

                if (insideFOV(otherCharacter.gameObject, this.gameObject, this.gameObject.transform.forward, FOV, Range))
                {
                    _foundedEnemy = true;
                    _foundEnemy = otherCharacter.gameObject;
                    return;
                }
            }

            _foundedEnemy = false;
            _foundEnemy = null;
        }

        private bool insideFOV(GameObject targetTemp, GameObject goTemp, Vector3 direction, float angleTemp, float distanceTemp)
        {
            Vector3 distanceToPlayer = targetTemp.transform.position - (goTemp.transform.transform.position - (goTemp.transform.transform.forward * 0.5f));
            float angleToPlayer = Vector3.Angle(distanceToPlayer, direction.normalized);
            float finalDistanceToPlayer = distanceToPlayer.magnitude;

            if (angleToPlayer <= angleTemp / 2 & finalDistanceToPlayer <= distanceTemp)
                return true;

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            var op = this.gameObject.transform.position - (this.gameObject.transform.forward * 0.5f);
            var rotate = this.gameObject.transform.eulerAngles;
            var direction = Quaternion.Euler(rotate) * Vector3.forward;

            Gizmos.color = Color.red;
            float arrowHeadLength = 0.25f;
            float arrowHeadAngle = 20.0f;

            Gizmos.DrawRay(op, direction.normalized * Range);

            Gizmos.color = Color.yellow;

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(op + direction.normalized * Range, right * arrowHeadLength);
            Gizmos.DrawRay(op + direction.normalized * Range, left * arrowHeadLength);


            Gizmos.color = Color.yellow;

            var leftRayRotation = Quaternion.AngleAxis(-FOV / 2, Vector3.up);
            var rightRayRotation = Quaternion.AngleAxis(FOV / 2, Vector3.up);

            Vector3 leftRayDirection = leftRayRotation * direction;
            Vector3 rightRayDirection = rightRayRotation * direction;
            Gizmos.DrawRay(op, leftRayDirection * Range);
            Gizmos.DrawRay(op, rightRayDirection * Range);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.gameObject.transform.position, GetAttackDistance());
        }

        public void Following(GameObject target)
        {
            var distance = Mathf.Max(GetAttackDistance() / 2, _agent.radius * 4);

            var moveingPosition = Vector3.MoveTowards(this.gameObject.transform.position, target.transform.position, distance);

            _agent.SetDestination(moveingPosition);
        }

        public void LookAt(GameObject target)
        {
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

        public void Attack()
        {
            _character.Attack();
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
    }
}