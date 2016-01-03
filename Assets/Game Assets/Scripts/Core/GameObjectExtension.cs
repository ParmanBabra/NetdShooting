using UnityEngine;
using System.Collections;
namespace NetdShooting.Core
{
    public static class GameObjectExtension
    {
        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.GetComponent<T>();
                }
            }

            return null;
        }

        public static GameObject FindGameObjectInChildWithTag(this GameObject parent, string tag)
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.gameObject;
                }
            }

            return null;
        }

        public static ArrayList FindGameObjectsInChildWithTag(this GameObject parent, string tag)
        {
            ArrayList list = new ArrayList();
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    list.Add(tr.gameObject);
                }
            }

            return list;
        }

        public static GameObject FindGameObjectInHierarchyWithTag(this GameObject parent, string tag)
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.gameObject;
                }

                var child = tr.gameObject.FindGameObjectInHierarchyWithTag(tag);

                if (child != null)
                    return child;
            }

            return null;
        }
    }
}
