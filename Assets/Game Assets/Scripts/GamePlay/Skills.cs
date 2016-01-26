using UnityEngine;
using System.Collections;
using NetdShooting.Core;
using System.Collections.Generic;

namespace NetdShooting.GamePlay
{
    [AddComponentMenu("Game Play/Skills")]
    public class Skills : MonoBehaviour
    {
        public FindMethod FindMethod;
        public int Count;
        private List<GameObject> _skills;
        private Queue<BaseSkill> _usingSkills = new Queue<BaseSkill>();
        private BaseSkill _currentSkill;


        // Use this for initialization
        public void Start()
        {
            switch (FindMethod)
            {
                case FindMethod.InChild:
                    _skills = this.gameObject.FindGameObjectsInChildWithTag("Skill");
                    break;
                case FindMethod.Restful:
                    //Mockup
                    _skills = this.gameObject.FindGameObjectsInChildWithTag("Skill");
                    break;
            }

            Count = _skills.Count;
        }

        public void UseSkill(int index)
        {
            if (index >= _skills.Count)
            {
                Debug.LogWarning("Skill out of range");
                return;
            }
            var goSkill = (GameObject)_skills[index];
            UseGameObjectSkill(goSkill);
        }

        public void UseSkill(string skillName)
        {
            foreach (GameObject goSkill in _skills)
            {
                if (GetSkillName(goSkill) == skillName)
                    UseGameObjectSkill(goSkill);
            }
        }

        public void ActionSkill()
        {
            //var currentSkill = _usingSkills.Peek();
            _currentSkill.ActionSkill();
        }

        public void EndUseSkill()
        {
            //var currentSkill = _usingSkills.Dequeue();
            _currentSkill.EndUseSkill();
            _currentSkill = null;

            if (_usingSkills.Count != 0)
            {
                _currentSkill = _usingSkills.Dequeue();
                _currentSkill.Use();
            }
        }

        private void UseGameObjectSkill(GameObject go)
        {
            var skill = go.GetComponent<BaseSkill>();
            if (skill == null)
            {
                Debug.LogWarning("Skill dosen't have base skill component.");
                return;
            }

            if (!skill.CanUse)
                return;

            if (!skill.CoolDowning())
                return;

            if (!skill.HaveMP())
                return;

            if (_currentSkill == null)
            {
                _currentSkill = skill;
                _currentSkill.Use();
            }
            else if (_usingSkills.Count >= 1 && _usingSkills.Peek() != skill)
                _usingSkills.Enqueue(skill);
        }

        private string GetSkillName(GameObject go)
        {
            var skill = go.GetComponent<BaseSkill>();
            if (skill == null)
            {
                Debug.LogWarning("Skill dosen't have base skill component.");
                return string.Empty;
            }

            return skill.SkillName;
        }
    }
}
