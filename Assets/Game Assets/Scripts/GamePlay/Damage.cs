using UnityEngine;
using System.Collections;
namespace NetdShooting.GamePlay
{
    public class Damage
    {
        public DamageType DamageType { get; set; }
        public int HitDamage { get; set; }
        public float During { get; set; }
        public SkillEffect[] Effects { get; set; }

        public Damage()
        {
            Effects = new SkillEffect[0];
        }
    }
}
