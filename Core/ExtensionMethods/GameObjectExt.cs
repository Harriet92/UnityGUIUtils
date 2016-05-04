using UnityEngine;

namespace EmiGd.GUI.Core
{
    public static class GameObjectExt
    {
        public static T FindInParents<T>(this GameObject go) where T : Component
        {
            if (go == null) return null;
            var comp = go.GetComponent<T>();

            if (comp != null)
                return comp;

            Transform t = go.transform.parent;
            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
            return comp;
        }

        public static void Hide(this GameObject go)
        {
            go.SetActive(false);
        }

        public static void Show(this GameObject go)
        {
            go.SetActive(true);
        }

        public static void AppendChild(this Transform parent, GameObject obj)
        {
            obj.transform.SetParent(parent);
            obj.transform.SetAsLastSibling();
        }
    }
}
