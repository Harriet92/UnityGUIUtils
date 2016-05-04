using UnityEngine;

namespace EmiGd.GUI.Core
{
    public class ItemsContainer : MonoBehaviour
    {
        public RectTransform ItemsPanel;
        public virtual void AttachItem(GameObject item) { }
    }
}
