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
        private Queue<BaseSkill> _usingSkill = new Queue<BaseSkill>();


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
            var currentSkill = _usingSkill.Peek();
            currentSkill.ActionSkill();
        }

        public void EndUseSkill()
        {
            var currentSkill = _usingSkill.Dequeue();
            currentSkill.EndUseSkill();
        }

        private void UseGameObjectSkill(GameObject go)
        {
            var skill = go.GetComponent<BaseSkill>();
            if (skill == null)
            {
                Debug.LogWarning("Skill dosen't have base skill component.");
                return;
            }
            var successed = skill.Use();

            if (successed)
            {
                _usingSkill.Enqueue(skill);
            }
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
