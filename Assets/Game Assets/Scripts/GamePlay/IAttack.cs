using UnityEngine;
using System.Collections;

namespace NetdShooting.GamePlay
{
    public interface IAttack
    {
        bool PassAttacking(float daltaTime);

        bool ReleaseAttack(float daltaTime);
    }
}
