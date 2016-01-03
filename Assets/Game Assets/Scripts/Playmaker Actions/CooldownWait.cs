using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
    public class CooldownWait : Wait
    {
        public FsmFloat coolDown;

        public override void OnEnter()
        {
            base.OnEnter();
            coolDown.Value = time.Value;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            coolDown.Value -= Time.deltaTime;
        }
    }
}
