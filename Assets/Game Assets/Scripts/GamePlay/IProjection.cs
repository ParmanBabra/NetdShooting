using UnityEngine;
using System.Collections;

namespace NetdShooting.GamePlay
{
    public interface IProjection
    {
        float MovingSpeed { get; set; }
        void SetDamage(Damage damage, bool areaDamage = false, float radius = 0.0f);
    }
}
