using UnityEngine;
using System.Collections;

namespace NetdShooting.GamePlay
{
    public interface IAttack
    {
        bool Attacking(float daltaTime);
    }
}
