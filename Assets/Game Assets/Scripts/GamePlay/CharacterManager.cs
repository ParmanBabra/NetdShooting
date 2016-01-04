using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Character Manager")]
    public class CharacterManager : MonoBehaviour
    {
        public List<Character> Characters { get; set; }

        // Use this for initialization
        public void Start()
        {
            this.gameObject.tag = "Character Manager";

            if (Characters == null)
                Characters = new List<Character>();

            Characters.Clear();

            var players = GameObject.FindGameObjectsWithTag("Player");
            var Enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var go in players)
            {
                var character = go.GetComponent<Character>();
                if (character != null)
                    Characters.Add(character);
            }

            foreach (var go in Enemies)
            {
                var character = go.GetComponent<Character>();
                if (character != null)
                    Characters.Add(character);
            }
        }
    }
}