using UnityEngine;
using System.Collections;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Skill Instance")]
    public class SkillInstance : BaseSkill
    {
        [Header("Skill Infomation")]
        public bool AddCharacterDamage = false;
        public bool SingleTarget = true;
        public float FOV = 90.0f;
        public float Range = 5.0f;
        public float LowDistance = 2.0f;
        public int Damage = 1;
        public DamageType DamageType = DamageType.Physic;
        public float During = 0;

        private CharacterManager _characterManager;

        public SkillInstance()
        {
            this.SkillType = SkillType.Instance;
        }

        protected override void OnStart()
        {
            _characterManager = GameHelper.GetCharacterManager();
        }

        protected override void ProcessUseSkill(float daltaTime)
        {
            //Play animation

            //Check Deal Damage
            bool dealed = false;
            var rotate = this.gameObject.transform.eulerAngles;
            var direction = Quaternion.Euler(rotate) * Vector3.forward;

            foreach (Character character in _characterManager.Characters)
            {
                if (character.Team == OwnerSkill.Team)
                    continue;

                var objInsideFOV = insideFOV(character.gameObject, this.gameObject, direction, FOV, Range);
                var objPassDistance = checkDistance(character.gameObject, this.gameObject);

                if (objInsideFOV || objPassDistance)
                {
                    if ((SingleTarget && !dealed) || !SingleTarget)
                    {
                        dealDamage(character);
                        dealed = true;
                    }
                }
            }
        }

        protected override void OnDrawActionGizmos()
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
        }

        private void dealDamage(Character target)
        {
            var maxDamage = Damage;
            var minDamage = Damage;

            if (AddCharacterDamage)
            {
                maxDamage += OwnerSkill.MaxAttack;
                minDamage += OwnerSkill.MinAttack;
            }

            var hitDamage = Random.Range(minDamage, maxDamage);
            Damage damage = new GamePlay.Damage();
            damage.HitDamage = hitDamage;
            damage.DamageType = DamageType;
            damage.Effects = Effects;
            damage.During = During;

            target.DealDamage(damage);
        }

        private bool checkDistance(GameObject targetTemp, GameObject goTemp)
        {
            return Vector3.Distance(targetTemp.transform.position, goTemp.transform.position) < LowDistance;
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
    }
}
