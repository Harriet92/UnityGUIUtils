using UnityEngine;

namespace EmiGd.GUI.Core
{
    public abstract class ScrolledListElement<T> : MonoBehaviour
    {
        public T            DataCache { get; protected set; }
        private Transform   _transform;

        protected virtual void Awake()
        {
            _transform = transform;
        }

        public virtual void SetData(T data, Transform parentTransform)
        {
            _transform.SetParent(parentTransform, false);
            UpdateData(data);
        }

        public virtual void UpdateData(T data)
        {
            DataCache = data;
        }

        public abstract void Click();
    }
}
