using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Character Manager")]
    public class CharacterManager : MonoBehaviour
    {
        private List<Character> _characters;
        public List<Character> Characters
        {
            get
            {
                if (_characters == null)
                {
                    return new List<Character>();
                }
                return _characters.ToList();
            }
        }

        // Use this for initialization
        public void Start()
        {
            this.gameObject.tag = "Character Manager";

            if (_characters == null)
                _characters = new List<Character>();

            _characters.Clear();

            var players = GameObject.FindGameObjectsWithTag("Player");
            var Enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var go in players)
            {
                var character = go.GetComponent<Character>();
                if (character != null)
                    _characters.Add(character);
            }

            foreach (var go in Enemies)
            {
                var character = go.GetComponent<Character>();
                if (character != null)
                    _characters.Add(character);
            }
        }

        public void DestoryCharacter(Character character)
        {
            _characters.Remove(character);
            GameObject.DestroyObject(character.gameObject);
        }
    }
}