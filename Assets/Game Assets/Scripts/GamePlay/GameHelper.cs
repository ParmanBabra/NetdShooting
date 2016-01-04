using UnityEngine;
using System.Collections;

namespace NetdShooting.GamePlay
{
    public static class GameHelper
    {
        public static CharacterManager GetCharacterManager()
        {
            var goCharacterManager = GameObject.FindGameObjectWithTag("Character Manager");

            if (goCharacterManager == null)
                throw new System.Exception("Can't find character manager");

            return goCharacterManager.GetComponent<CharacterManager>();
        }
    }
}
