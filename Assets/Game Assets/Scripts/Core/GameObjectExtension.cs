using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        public static List<GameObject> FindGameObjectsInChildWithTag(this GameObject parent, string tag)
        {
            List<GameObject> list = new List<GameObject>();
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

        public static List<GameObject> FindGameObjectsInHierarchyWithTag(this GameObject parent, string tag)
        {

            List<GameObject> list = new List<GameObject>();
            findGameObjectsInHierarchyWithTag(parent, tag, ref list);
            return list;
        }

        private static void findGameObjectsInHierarchyWithTag(GameObject parent, string tag, ref List<GameObject> list)
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    list.Add(tr.gameObject);
                }

                findGameObjectsInHierarchyWithTag(tr.gameObject, tag, ref list);
            }
        }

        public static GameObject FindMuzzle(this GameObject parent, string name)
        {
            var muzzlies = parent.FindGameObjectsInHierarchyWithTag("Muzzle");

            foreach (var go in muzzlies)
            {
                if (go.name == name)
                    return go;
            }

            return null;
        }

        public static List<GameObject> FindMuzzlies(this GameObject parent, string name)
        {
            List<GameObject> list = new List<GameObject>();
            var muzzlies = parent.FindGameObjectsInHierarchyWithTag("Muzzle");

            foreach (var go in muzzlies)
            {
                if (go.name == name)
                    list.Add(go);
            }

            return list;
        }

        public static bool TryGetComponent<T>(this GameObject go, out T component)
        {
            component = go.GetComponent<T>();
            if (component == null)
                return false;
            return true;
        }
    }
}
