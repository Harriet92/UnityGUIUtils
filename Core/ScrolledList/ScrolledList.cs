using System;
using System.Collections.Generic;
using UnityEngine;

namespace EmiGd.GUI.Core
{
    public class ScrolledList<TDataType, TComponentType> : MonoBehaviour
       where TComponentType : Component
    {
        public event Action OnViewLoaded = delegate { };
        public bool         IsInitialized;

        [SerializeField]
        protected ScrolledPanel     _container;
        protected RectTransform     _containerTransform;
        protected TComponentType    _listElementCache;
        protected List<GameObject>  _children;
        protected GameObject        _gameObjectCache;
        protected Transform         _transformCache;


        public virtual void LoadView(IEnumerable<TDataType> elements)
        {
            if (!IsInitialized) {
                Init();
            }
            UpdateList(elements);
            _gameObjectCache.SetActive(true);
            OnViewLoaded();
        }

        public virtual void Init()
        {
            if (IsInitialized)
                return;
            if (_container == null)
                _container = GetComponentInChildren<ScrolledPanel>(true);
            _gameObjectCache = gameObject;
            _containerTransform = _container.GetComponent<RectTransform>();
            _listElementCache = _container.GetComponentInChildren<TComponentType>(true);
            _listElementCache.gameObject.SetActive(false);
            _transformCache = transform;
            _children = new List<GameObject>();
            IsInitialized = true;
        }

        public virtual void UpdateList(IEnumerable<TDataType> elements)
        {
            ClearPanel();
            if (elements == null) {
                return;
            }
            foreach (var element in elements)
                AddElement(element);
        }

        public virtual void ClearPanel()
        {
            RefreshChildren();
            if (_children.Count == 0)
                return;
            _children.RemoveAt(0);
            _children.ForEach(c => ObjectPoolManager.DestroyPooled(c.gameObject));
        }

        protected void RefreshChildren()
        {
            _children.Clear();
            foreach (Transform child in _containerTransform) {
                _children.Add(child.gameObject);
            }
        }

        protected virtual void AddElement(TDataType element)
        {
            var childObject = SetUpChild(element);
            _children.Add(childObject);
        }

        protected GameObject SetUpChild(TDataType element)
        {
            var newElement = ObjectPoolManager.CreatePooled(_listElementCache.gameObject, Vector3.zero, Quaternion.identity);
            newElement.SetActive(true);
            var elemScript = newElement.GetComponent<ScrolledListElement<TDataType>>();
            elemScript.SetData(element, _containerTransform);
            return newElement;
        }
    }
}
